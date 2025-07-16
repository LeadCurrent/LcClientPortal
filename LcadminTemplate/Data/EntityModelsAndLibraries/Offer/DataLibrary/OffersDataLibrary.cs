using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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
            return await context.Offers.
                Include(x => x.Offertargetings)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
        }
        public async Task<Allocation> GetAllocationByOfferId(int OfferId)
        {
            return await context.Allocations
                .Include(x =>x.Source)
                .Where(x => x.Offerid == OfferId)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Client>> GetClients()
        {
            return await context.Clients
                .OrderByDescending(x => x.Active)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<Offertargeting> GetTargetingByOfferId(int offerId)
        {
            var targeting = await context.Offertargetings.FirstOrDefaultAsync(x => x.Offerid == offerId);
            if (targeting != null)
            {
                // Monday
                targeting.MondayStartHour = targeting.MondayStart.Hour;
                targeting.MondayStartMinute = targeting.MondayStart.Minute;
                targeting.MondayEndHour = targeting.MondayEnd.Hour;
                targeting.MondayEndMinute = targeting.MondayEnd.Minute;

                // Tuesday
                targeting.TuesdayStartHour = targeting.TuesdayStart.Hour;
                targeting.TuesdayStartMinute = targeting.TuesdayStart.Minute;
                targeting.TuesdayEndHour = targeting.TuesdayEnd.Hour;
                targeting.TuesdayEndMinute = targeting.TuesdayEnd.Minute;

                // Wednesday
                targeting.WednesdayStartHour = targeting.WednesdayStart.Hour;
                targeting.WednesdayStartMinute = targeting.WednesdayStart.Minute;
                targeting.WednesdayEndHour = targeting.WednesdayEnd.Hour;
                targeting.WednesdayEndMinute = targeting.WednesdayEnd.Minute;

                // Thursday
                targeting.ThursdayStartHour = targeting.ThursdayStart.Hour;
                targeting.ThursdayStartMinute = targeting.ThursdayStart.Minute;
                targeting.ThursdayEndHour = targeting.ThursdayEnd.Hour;
                targeting.ThursdayEndMinute = targeting.ThursdayEnd.Minute;

                // Friday
                targeting.FridayStartHour = targeting.FridayStart.Hour;
                targeting.FridayStartMinute = targeting.FridayStart.Minute;
                targeting.FridayEndHour = targeting.FridayEnd.Hour;
                targeting.FridayEndMinute = targeting.FridayEnd.Minute;

                // Saturday
                targeting.SaturdayStartHour = targeting.SaturdayStart.Hour;
                targeting.SaturdayStartMinute = targeting.SaturdayStart.Minute;
                targeting.SaturdayEndHour = targeting.SaturdayEnd.Hour;
                targeting.SaturdayEndMinute = targeting.SaturdayEnd.Minute;

                // Sunday
                targeting.SundayStartHour = targeting.SundayStart.Hour;
                targeting.SundayStartMinute = targeting.SundayStart.Minute;
                targeting.SundayEndHour = targeting.SundayEnd.Hour;
                targeting.SundayEndMinute = targeting.SundayEnd.Minute;
            }

            return targeting;
        }
        public async Task<List<Offer>> GetOffers(int? companyId, int recordPerPage, string schoolName, int page, int? status)
        {
            var query = context.Offers
                .Include(x => x.School)
                .Include(x => x.Client)
                .Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(schoolName))
            {
                string upperSchoolName = schoolName.ToUpper();
                query = query.Where(x => x.School != null &&
                                         x.School.Name != null &&
                                         x.School.Name.ToUpper().Contains(upperSchoolName));
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.Active == (status == 1));
            }

            // ✅ Active offers first, then sort by Id
            query = query.OrderByDescending(x => x.Active)
                         .ThenBy(x => x.Id);

            if (recordPerPage != -1)
            {
                query = query.Skip((page - 1) * recordPerPage).Take(recordPerPage);
            }

            var offers = await query.ToListAsync();

            if (!offers.Any())
                return offers;

            // === Date Ranges ===
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
                .Select(s => new { s.Offerid, s.Submissiontype, s.Submissiondate })
                .ToListAsync();

            foreach (var offer in offers)
            {
                string GetHtml(DateTime start, DateTime end, string scope)
                {
                    var startDate = DateOnly.FromDateTime(start);
                    var endDate = DateOnly.FromDateTime(end);

                    var rangeSubs = submissions
                        .Where(s => s.Offerid == offer.Id &&
                                    s.Submissiondate >= startDate &&
                                    s.Submissiondate < endDate)
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

                    return $"<a href='/admin/viewreport.aspx?type=sub&scope={scope}&offerid={offer.Id}&clientid=-1&sourceid=-1&schoolid=-1' target='_blank'>{Math.Max(cnt, postcnt)}</a>";
                }

                offer.DayHtml = $"{GetHtml(yesterdayStart, todayStart, "yest")} / {GetHtml(todayStart, tomorrowStart, "today")}";
                offer.WeekHtml = $"{GetHtml(lastWeekStart, thisWeekStart, "lastw")} / {GetHtml(thisWeekStart, DateTime.Now, "wtd")}";
                offer.MonthHtml = $"{GetHtml(lastMonthStart, thisMonthStart, "lastm")} / {GetHtml(thisMonthStart, DateTime.Now, "mtd")}";
            }

            var allocationsMap = await GetAllocationsForOfferIds(offerIds);

            foreach (var offer in offers)
            {
                if (allocationsMap.TryGetValue(offer.Id, out var allocations))
                {
                    offer.Allocations = allocations;
                }
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
                            s.Submissiondate >= DateOnly.FromDateTime(date1) &&
                            s.Submissiondate < DateOnly.FromDateTime(date2))
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
            var targeting = offer.Targeting;

            // Set Active flags
            targeting.MondayActive = offer.Targeting.MondayActive;
            targeting.TuesdayActive = offer.Targeting.TuesdayActive;
            targeting.WednesdayActive = offer.Targeting.WednesdayActive;
            targeting.ThursdayActive = offer.Targeting.ThursdayActive;
            targeting.FridayActive = offer.Targeting.FridayActive;
            targeting.SaturdayActive = offer.Targeting.SaturdayActive;
            targeting.SundayActive = offer.Targeting.SundayActive;

            // Set Start and End Times
            targeting.MondayStart = new DateTime(2001, 1, 1, targeting.MondayStartHour, targeting.MondayStartMinute, 0);
            targeting.MondayEnd = new DateTime(2001, 1, 1, targeting.MondayEndHour, targeting.MondayEndMinute, 0);

            targeting.TuesdayStart = new DateTime(2001, 1, 1, targeting.TuesdayStartHour, targeting.TuesdayStartMinute, 0);
            targeting.TuesdayEnd = new DateTime(2001, 1, 1, targeting.TuesdayEndHour, targeting.TuesdayEndMinute, 0);

            targeting.WednesdayStart = new DateTime(2001, 1, 1, targeting.WednesdayStartHour, targeting.WednesdayStartMinute, 0);
            targeting.WednesdayEnd = new DateTime(2001, 1, 1, targeting.WednesdayEndHour, targeting.WednesdayEndMinute, 0);

            targeting.ThursdayStart = new DateTime(2001, 1, 1, targeting.ThursdayStartHour, targeting.ThursdayStartMinute, 0);
            targeting.ThursdayEnd = new DateTime(2001, 1, 1, targeting.ThursdayEndHour, targeting.ThursdayEndMinute, 0);

            targeting.FridayStart = new DateTime(2001, 1, 1, targeting.FridayStartHour, targeting.FridayStartMinute, 0);
            targeting.FridayEnd = new DateTime(2001, 1, 1, targeting.FridayEndHour, targeting.FridayEndMinute, 0);

            targeting.SaturdayStart = new DateTime(2001, 1, 1, targeting.SaturdayStartHour, targeting.SaturdayStartMinute, 0);
            targeting.SaturdayEnd = new DateTime(2001, 1, 1, targeting.SaturdayEndHour, targeting.SaturdayEndMinute, 0);

            targeting.SundayStart = new DateTime(2001, 1, 1, targeting.SundayStartHour, targeting.SundayStartMinute, 0);
            targeting.SundayEnd = new DateTime(2001, 1, 1, targeting.SundayEndHour, targeting.SundayEndMinute, 0);

            // Save Offer
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            // Assign Offer ID to targeting and save targeting
            targeting.Offerid = offer.Id;
            context.Offertargetings.Add(targeting);
            await context.SaveChangesAsync();

            return offer;
        }
        public async Task<Offer> EditOffer(Offer updatedOffer)
        {
            var existingOffer = await context.Offers
                .Include(o => o.Offertargetings)
                .FirstOrDefaultAsync(o => o.Id == updatedOffer.Id);

            if (existingOffer == null)
                throw new Exception("Offer not found");

            // Update main offer fields
            existingOffer.Schoolid = updatedOffer.Schoolid;
            existingOffer.Clientid = updatedOffer.Clientid;
            existingOffer.Url = updatedOffer.Url;
            existingOffer.Active = updatedOffer.Active;
            existingOffer.Rpl = updatedOffer.Rpl;
            existingOffer.Dcap = updatedOffer.Dcap;
            existingOffer.Dcapamt = updatedOffer.Dcapamt;
            existingOffer.Mcap = updatedOffer.Mcap;
            existingOffer.Mcapamt = updatedOffer.Mcapamt;
            existingOffer.Wcap = updatedOffer.Wcap;
            existingOffer.Wcapamt = updatedOffer.Wcapamt;
            existingOffer.Type = updatedOffer.Type;
            existingOffer.Militaryonly = updatedOffer.Militaryonly;
            existingOffer.Nomilitary = updatedOffer.Nomilitary;
            existingOffer.Transferphone = updatedOffer.Transferphone;
            existingOffer.Lccampaignid = updatedOffer.Lccampaignid;
            existingOffer.Archive = updatedOffer.Archive;
            existingOffer.EndClient = updatedOffer.EndClient;
            existingOffer.DeliveryIdentifier = updatedOffer.DeliveryIdentifier;
            existingOffer.DeliveryName = updatedOffer.DeliveryName;

            // Update new CEC RPL fields
            existingOffer.CecRplA = updatedOffer.CecRplA;
            existingOffer.CecRplB = updatedOffer.CecRplB;
            existingOffer.CecRplC = updatedOffer.CecRplC;
            existingOffer.CecRplD = updatedOffer.CecRplD;
            existingOffer.CecRplE = updatedOffer.CecRplE;
            existingOffer.CecRplF = updatedOffer.CecRplF;
            existingOffer.CecRplG = updatedOffer.CecRplG;

            // Update Offertargeting
            var targeting = updatedOffer.Targeting;

            if (existingOffer.Offertargetings.Any())
            {
                var existingTargeting = existingOffer.Offertargetings.First();

                // ✅ Update CEC checkboxes
                existingTargeting.CecIncludeA = targeting.CecIncludeA;
                existingTargeting.CecIncludeB = targeting.CecIncludeB;
                existingTargeting.CecIncludeC = targeting.CecIncludeC;
                existingTargeting.CecIncludeD = targeting.CecIncludeD;
                existingTargeting.CecIncludeE = targeting.CecIncludeE;
                existingTargeting.CecIncludeF = targeting.CecIncludeF;
                existingTargeting.CecIncludeG = targeting.CecIncludeG;

                // ✅ Update CEC CPLs
                existingTargeting.CecCplA = targeting.CecCplA;
                existingTargeting.CecCplB = targeting.CecCplB;
                existingTargeting.CecCplC = targeting.CecCplC;
                existingTargeting.CecCplD = targeting.CecCplD;
                existingTargeting.CecCplE = targeting.CecCplE;
                existingTargeting.CecCplF = targeting.CecCplF;
                existingTargeting.CecCplG = targeting.CecCplG;

                // Set Active flags
                existingTargeting.MondayActive = targeting.MondayActive;
                existingTargeting.TuesdayActive = targeting.TuesdayActive;
                existingTargeting.WednesdayActive = targeting.WednesdayActive;
                existingTargeting.ThursdayActive = targeting.ThursdayActive;
                existingTargeting.FridayActive = targeting.FridayActive;
                existingTargeting.SaturdayActive = targeting.SaturdayActive;
                existingTargeting.SundayActive = targeting.SundayActive;

                // ✅ Update day/time targeting
                existingTargeting.MondayStart = new DateTime(2001, 1, 1, targeting.MondayStartHour, targeting.MondayStartMinute, 0);
                existingTargeting.MondayEnd = new DateTime(2001, 1, 1, targeting.MondayEndHour, targeting.MondayEndMinute, 0);

                existingTargeting.TuesdayStart = new DateTime(2001, 1, 1, targeting.TuesdayStartHour, targeting.TuesdayStartMinute, 0);
                existingTargeting.TuesdayEnd = new DateTime(2001, 1, 1, targeting.TuesdayEndHour, targeting.TuesdayEndMinute, 0);

                existingTargeting.WednesdayStart = new DateTime(2001, 1, 1, targeting.WednesdayStartHour, targeting.WednesdayStartMinute, 0);
                existingTargeting.WednesdayEnd = new DateTime(2001, 1, 1, targeting.WednesdayEndHour, targeting.WednesdayEndMinute, 0);

                existingTargeting.ThursdayStart = new DateTime(2001, 1, 1, targeting.ThursdayStartHour, targeting.ThursdayStartMinute, 0);
                existingTargeting.ThursdayEnd = new DateTime(2001, 1, 1, targeting.ThursdayEndHour, targeting.ThursdayEndMinute, 0);

                existingTargeting.FridayStart = new DateTime(2001, 1, 1, targeting.FridayStartHour, targeting.FridayStartMinute, 0);
                existingTargeting.FridayEnd = new DateTime(2001, 1, 1, targeting.FridayEndHour, targeting.FridayEndMinute, 0);

                existingTargeting.SaturdayStart = new DateTime(2001, 1, 1, targeting.SaturdayStartHour, targeting.SaturdayStartMinute, 0);
                existingTargeting.SaturdayEnd = new DateTime(2001, 1, 1, targeting.SaturdayEndHour, targeting.SaturdayEndMinute, 0);

                existingTargeting.SundayStart = new DateTime(2001, 1, 1, targeting.SundayStartHour, targeting.SundayStartMinute, 0);
                existingTargeting.SundayEnd = new DateTime(2001, 1, 1, targeting.SundayEndHour, targeting.SundayEndMinute, 0);
            }

            await context.SaveChangesAsync();
            return existingOffer;
        }
        public async Task<List<Allocation>> GetAllocationsForOfferId(int offerId)
        {
            var result = new List<Allocation>();

            var connectionString = context.Database.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string sql = @"
            SELECT a.id, a.offerid, a.sourceid, a.identifier, a.active, s.name as sourcename
            FROM allocations a
            INNER JOIN sources s ON a.sourceid = s.id
            WHERE a.offerid = @offerId
            ORDER BY a.active DESC, s.name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@offerId", offerId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Allocation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Offerid = reader.GetInt32(reader.GetOrdinal("offerid")),
                                Sourceid = reader.GetInt32(reader.GetOrdinal("sourceid")),
                                Identifier = reader["identifier"]?.ToString(),
                                Active = reader.GetBoolean(reader.GetOrdinal("active")),
                                Source = new Source
                                {
                                    Name = reader["sourcename"]?.ToString()
                                }
                            });
                        }
                    }
                }
            }

            return result;
        }
        public async Task<Dictionary<int, List<Allocation>>> GetAllocationsForOfferIds(List<int> offerIds)
        {
            var result = new Dictionary<int, List<Allocation>>();

            if (offerIds == null || offerIds.Count == 0)
                return result;

            var connectionString = context.Database.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // Build dynamic parameterized IN clause
                var parameters = new List<string>();
                for (int i = 0; i < offerIds.Count; i++)
                {
                    parameters.Add($"@id{i}");
                }

                string inClause = string.Join(",", parameters);

                string sql = $@"
            SELECT a.id, a.offerid, a.sourceid, a.identifier, a.active, s.name as sourcename
            FROM allocations a
            INNER JOIN sources s ON a.sourceid = s.id
            WHERE a.offerid IN ({inClause})
            ORDER BY a.offerid, a.active DESC, s.name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    for (int i = 0; i < offerIds.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@id{i}", offerIds[i]);
                    }

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var offerId = reader.GetInt32(reader.GetOrdinal("offerid"));

                            if (!result.ContainsKey(offerId))
                                result[offerId] = new List<Allocation>();

                            result[offerId].Add(new Allocation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Offerid = offerId,
                                Sourceid = reader.GetInt32(reader.GetOrdinal("sourceid")),
                                Identifier = reader["identifier"]?.ToString(),
                                Active = reader.GetBoolean(reader.GetOrdinal("active")),
                                Source = new Source
                                {
                                    Name = reader["sourcename"]?.ToString()
                                }
                            });
                        }
                    }
                }
            }

            return result;
        }
        public async Task<bool> UpdateAllocation(Allocation updatedAllocation)
        {
            var existingAllocation = await context.Allocations.FindAsync(updatedAllocation.Id);

            if (existingAllocation != null)
            {
                // Basic fields
                existingAllocation.Active = updatedAllocation.Active;
                existingAllocation.Sourceid = updatedAllocation.Sourceid;
                existingAllocation.Identifier = updatedAllocation.Identifier;
                existingAllocation.Transferphone = updatedAllocation.Transferphone;
                existingAllocation.Cpl = updatedAllocation.Cpl;

                // Cap fields
                existingAllocation.Dcap = updatedAllocation.Dcap;
                existingAllocation.Dcapamt = updatedAllocation.Dcapamt;
                existingAllocation.Wcap = updatedAllocation.Wcap;
                existingAllocation.Wcapamt = updatedAllocation.Wcapamt;
                existingAllocation.Mcap = updatedAllocation.Mcap;
                existingAllocation.Mcapamt = updatedAllocation.Mcapamt;

                // CEC Score checkboxes
                existingAllocation.CecIncludeA = updatedAllocation.CecIncludeA;
                existingAllocation.CecIncludeB = updatedAllocation.CecIncludeB;
                existingAllocation.CecIncludeC = updatedAllocation.CecIncludeC;
                existingAllocation.CecIncludeD = updatedAllocation.CecIncludeD;
                existingAllocation.CecIncludeE = updatedAllocation.CecIncludeE;
                existingAllocation.CecIncludeF = updatedAllocation.CecIncludeF;
                existingAllocation.CecIncludeG = updatedAllocation.CecIncludeG;

                // CEC CPL values
                existingAllocation.CecCplA = updatedAllocation.CecCplA;
                existingAllocation.CecCplB = updatedAllocation.CecCplB;
                existingAllocation.CecCplC = updatedAllocation.CecCplC;
                existingAllocation.CecCplD = updatedAllocation.CecCplD;
                existingAllocation.CecCplE = updatedAllocation.CecCplE;
                existingAllocation.CecCplF = updatedAllocation.CecCplF;
                existingAllocation.CecCplG = updatedAllocation.CecCplG;

                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<List<TblConfigEducationLevel>> GetEducationLevelsAsync()
        {
            return await context.TblConfigEducationLevels
                .OrderBy(e => e.Label)
                .ToListAsync();
        }
    }
}
