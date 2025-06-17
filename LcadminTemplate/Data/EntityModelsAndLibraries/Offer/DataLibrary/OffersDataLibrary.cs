using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Data
{
    public class OffersDataLibrary
    {
        public DataContext context { get; }

        public OffersDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<Offer> GetOfferById(int Id)
        {
            return await context.Offers
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Client>> GetClients()
        {
            return await context.Clients
                .OrderByDescending(x => x.Active)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetOffers(int? companyId, int recordPerPage, string schoolName, int page, int? status)
        {
            var query = context.Offers
                .Include(x => x.School)
                .Include(x => x.Client)
                .Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrEmpty(schoolName))
            {
                string upperSchoolName = schoolName.ToUpper();
                query = query.Where(x => x.School != null &&
                                         x.School.Name != null &&
                                         x.School.Name.ToUpper().Contains(upperSchoolName));
            }

            if (status.HasValue)
                query = query.Where(x => x.Active == (status == 1));

            query = query.OrderBy(x => x.Id);

            if (recordPerPage != -1)
                query = query.Skip((page - 1) * recordPerPage).Take(recordPerPage);

            var offers = await query.ToListAsync();

            if (!offers.Any())
                return offers;

            // =====================
            // === Date Ranges ====
            // =====================
            DateTime todayStart = DateOnly.FromDateTime(DateTime.Today).ToDateTime(TimeOnly.MinValue);
            DateTime yesterdayStart = todayStart.AddDays(-1);
            DateTime tomorrowStart = todayStart.AddDays(1);
            DateTime thisWeekStart = todayStart.AddDays(-(int)todayStart.DayOfWeek);
            DateTime lastWeekStart = thisWeekStart.AddDays(-7);
            DateTime thisMonthStart = new DateTime(todayStart.Year, todayStart.Month, 1);
            DateTime lastMonthStart = thisMonthStart.AddMonths(-1);

            var offerIds = offers.Select(x => x.Id).ToList();

            var submissions = await context.VwAllSubmissions
                .Where(s => offerIds.Contains(s.Offerid))
                .Select(s => new { s.Offerid, s.Submissiontype, s.Submisiondate })
                .ToListAsync();

            // Group by offerId + range
            foreach (var offer in offers)
            {
                string GetHtml(DateTime start, DateTime end, string urlScope)
                {
                    var startDate = DateOnly.FromDateTime(start);
                    var endDate = DateOnly.FromDateTime(end);
                    var rangeSubs = submissions
                        .Where(s => s.Offerid == offer.Id && s.Submisiondate >= startDate && s.Submisiondate < endDate)
                        .GroupBy(s => s.Submissiontype)
                        .Select(g => new { Type = g.Key, Count = g.Count() })
                        .ToList();

                    int cnt = 0, postcnt = 0;
                    foreach (var g in rangeSubs)
                    {
                        if (g.Type == "l")
                            cnt = g.Count;
                        else
                            postcnt = g.Count;
                    }

                    return $"<a href='/admin/viewreport.aspx?type=sub&scope={urlScope}&offerid={offer.Id}&clientid=-1&sourceid=-1&schoolid=-1' target='_blank'>{Math.Max(cnt, postcnt)}</a>";
                }

                // === DayHtml: Yesterday / Today ===
                offer.DayHtml = $"{GetHtml(yesterdayStart, todayStart, "yest")} / {GetHtml(todayStart, tomorrowStart, "today")}";

                // === WeekHtml: Last Week / This Week ===
                offer.WeekHtml = $"{GetHtml(lastWeekStart, thisWeekStart, "lastw")} / {GetHtml(thisWeekStart, DateTime.Now, "wtd")}";

                // === MonthHtml: Last Month / This Month ===
                offer.MonthHtml = $"{GetHtml(lastMonthStart, thisMonthStart, "lastm")} / {GetHtml(thisMonthStart, DateTime.Now, "mtd")}";
            }

            return offers;
        }

        public async Task<int> GetOffersCount(int? companyId, string schoolName, int? status)
        {
            var query = context.Offers.Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrEmpty(schoolName))
            {
                string upperSchoolName = schoolName.ToUpper();
                query = query.Where(x => x.School != null &&
                                         x.School.Name != null &&
                                         x.School.Name.ToUpper().Contains(upperSchoolName));
            }

            if (status.HasValue)
                query = query.Where(x => x.Active == (status == 1));

            return await query.CountAsync();
        }

        public async Task ChangeOfferStatus(bool Status,int OfferId)
        {
            context.ChangeTracker.Clear();
            var Offer = await context.Offers.Where(s => s.Id == OfferId).FirstOrDefaultAsync();
            context.Entry(Offer).State = EntityState.Detached;
            Offer.Active = Status;
            context.Entry(Offer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<int> GetSubmissionCountByOffer(int offerId, DateTime date1, DateTime date2)
        {
            var submissionGroups = await context.VwAllSubmissions
                .Where(s => s.Offerid == offerId &&
                            s.Submisiondate >= DateOnly.FromDateTime(date1) &&
                            s.Submisiondate < DateOnly.FromDateTime(date2))
                .GroupBy(s => s.Submissiontype)
                .Select(g => new
                {
                    SubmissionType = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            int cnt = 0, postcnt = 0;

            foreach (var group in submissionGroups)
            {
                if (group.SubmissionType == "l")
                    cnt = group.Total;
                else
                    postcnt = group.Total;
            }

            return Math.Max(cnt, postcnt);
        }

        public async Task<Offer> CreateOffer(Offer offer)
        {

            var Targeting = offer.Targeting;

            Targeting.MondayStart = new DateTime(2001, 1, 1, Targeting.MondayStartHour, Targeting.MondayStartMinute, 0);
            Targeting.MondayEnd = new DateTime(2001, 1, 1, Targeting.MondayEndHour, Targeting.MondayEndMinute, 0);

            Targeting.TuesdayStart = new DateTime(2001, 1, 1, Targeting.TuesdayStartHour, Targeting.TuesdayStartMinute, 0);
            Targeting.TuesdayEnd = new DateTime(2001, 1, 1, Targeting.TuesdayEndHour, Targeting.TuesdayEndMinute, 0);

            Targeting.WednesdayStart = new DateTime(2001, 1, 1, Targeting.WednesdayStartHour, Targeting.WednesdayStartMinute, 0);
            Targeting.WednesdayEnd = new DateTime(2001, 1, 1, Targeting.WednesdayEndHour, Targeting.WednesdayEndMinute, 0);

            Targeting.ThursdayStart = new DateTime(2001, 1, 1, Targeting.ThursdayStartHour, Targeting.ThursdayStartMinute, 0);
            Targeting.ThursdayEnd = new DateTime(2001, 1, 1, Targeting.ThursdayEndHour, Targeting.ThursdayEndMinute, 0);

            Targeting.FridayStart = new DateTime(2001, 1, 1, Targeting.FridayStartHour, Targeting.FridayStartMinute, 0);
            Targeting.FridayEnd = new DateTime(2001, 1, 1, Targeting.FridayEndHour, Targeting.FridayEndMinute, 0);

            Targeting.SaturdayStart = new DateTime(2001, 1, 1, Targeting.SaturdayStartHour, Targeting.SaturdayStartMinute, 0);
            Targeting.SaturdayEnd = new DateTime(2001, 1, 1, Targeting.SaturdayEndHour, Targeting.SaturdayEndMinute, 0);

            Targeting.SundayStart = new DateTime(2001, 1, 1, Targeting.SundayStartHour, Targeting.SundayStartMinute, 0);
            Targeting.SundayEnd = new DateTime(2001, 1, 1, Targeting.SundayEndHour, Targeting.SundayEndMinute, 0);


            //context.Offertargeting.Add(Targeting);
            //await context.SaveChangesAsync();

            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            return offer;
        }


    }
}
