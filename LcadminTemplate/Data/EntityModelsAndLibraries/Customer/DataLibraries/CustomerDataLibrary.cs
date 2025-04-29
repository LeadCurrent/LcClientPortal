
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class CustomerDataLibrary
    {
        public DataContext context { get; }
        static readonly char[] padding = { '=' };

        public CustomerDataLibrary(DataContext Context)
        {
            context = Context;
        }

        public async Task<List<Customer>> GetCustomers(int CompanyId)
        {
            List<Customer> customers = await context.Customer
              .Include(x => x.CustomerDocuments)
              .Include(x => x.CustomerNotes)
              .Where(x => x.CompanyId == CompanyId && !x.IsDeleted)
              .OrderByDescending(x => x.Name)
              .ToListAsync();
            foreach (var company in customers)
            {
                company.CustomerNotes = company.CustomerNotes.OrderBy(x => x.CreateDate).ToList();
            }
            return customers;
        }
        /*  For get filer data on Customers Leads   */
        public async Task<List<Customer>> GetFilterdCustomers(string Name, string city)
        {
            List<Customer> Customers = await context.Customer.
                Where(x => !x.IsDeleted && (Name == null || (x.Name.ToUpper().Contains(Name.ToUpper()))) && (city == null || x.City == city))
                .Include(x => x.CustomerNotes)
                .OrderBy(x => x.Name)
                .ToListAsync();
            foreach (var company in Customers)
            {
                company.CustomerNotes = company.CustomerNotes.OrderBy(x => x.CreateDate).ToList();
            }
            return Customers;
        }
        public async Task<List<Customer>> GetActiveCustomers(int CompanyId)
        {
            List<Customer> Customers = await context.Customer
                .Where(x => x.Status == GeneralEnums.Status.Active && x.CompanyId == CompanyId)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return Customers;
        }
        public async Task<Customer> GetSimpleCustomer(int CustomerId)
        {
            var Customer = await context.Customer
                 .Where(x => x.Id == CustomerId).FirstOrDefaultAsync();
            return Customer;
        }

        public async Task<Customer> GetCustomer(int CustomerId)
        {
            var Customer = await context.Customer
                .Include(x => x.CustomerNotes)
                .Include(x => x.Company)
                 .Where(x => x.Id == CustomerId).FirstOrDefaultAsync();
            Customer.CustomerNotes = Customer.CustomerNotes.OrderBy(x => x.CreateDate).ToList();
            return Customer;
        }

        public async Task<Customer> GetCustomerByGUID(string Guid)
        {
            var Customer = await context.Customer
                .Include(x => x.CustomerNotes)
                .Include(x => x.Company)
                 .Where(x => x.Guid == Guid).FirstOrDefaultAsync();
            Customer.CustomerNotes = Customer.CustomerNotes.OrderBy(x => x.CreateDate).ToList();
            return Customer;
        }
        public async Task<Customer> GetCustomerForApi(string Guid)
        {
            var Customer = await context.Customer

                .Include(x => x.Company)
                 .Where(x => x.Guid == Guid).FirstOrDefaultAsync();

            return Customer;
        }

        public async Task<Customer> GetCustomerByPhone(string PhoneNumber)
        {
            var Customer = await context.Customer
                 .Where(x => x.PhoneNumber == PhoneNumber).FirstOrDefaultAsync();
            if (Customer != null)
                return Customer;
            else
            {
                return null;
            }
        }


        public async Task<int> CreateCustomer(Customer Customer, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            if (CurrentUser == "Created By API")
            {
                Customer.UpdatedBy = "Creted By API";
                Customer.CreatedBy = "Creted By API";
            }
            else
            {
                var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
                if (user != null)
                {
                    Customer.UpdatedBy = user.FullName;
                    Customer.CreatedBy = user.FullName;
                }
            }

            Customer.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Customer.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Customer.Guid = System.Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd(padding).Replace('+', '-').Replace('/', '_').Substring(0, 10);
            context.Customer.Add(Customer);
            await context.SaveChangesAsync();
            return Customer.Id;
        }
        public async Task<int> CreateCustomer(Customer Customer)
        {
            context.ChangeTracker.Clear();
            Customer.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Customer.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Customer.Guid = System.Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd(padding).Replace('+', '-').Replace('/', '_').Substring(0, 10);

            context.Customer.Add(Customer);
            await context.SaveChangesAsync();
            return Customer.Id;
        }
        public async Task UpdateCustomer(Customer Customer, string CurrentUser)
        {
            context.ChangeTracker.Clear();

            var CurrentCustomer = await context.Customer.Where(s => s.Id == Customer.Id).FirstOrDefaultAsync();
            context.Entry(CurrentCustomer).State = EntityState.Detached;
            Customer.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            if (CurrentUser != null)
            {
                var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
                Customer.UpdatedBy = user.Id;
            }
            Customer.CreateDate = CurrentCustomer.CreateDate;
            Customer.CreatedBy = CurrentCustomer.CreatedBy;
            context.Entry(Customer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(Customer Customer, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentCustomer = await context.Customer.Where(s => s.Id == Customer.Id).FirstOrDefaultAsync();
            context.Entry(CurrentCustomer).State = EntityState.Detached;
            Customer.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Customer.UpdatedBy = user.Id;
            Customer.CreateDate = CurrentCustomer.CreateDate;
            Customer.CreatedBy = CurrentCustomer.CreatedBy;
            Customer.IsDeleted = true;
            context.Entry(Customer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task UpdateCustomerFromCSR(Customer Customer, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentCustomer = await context.Customer.Where(s => s.Id == Customer.Id).FirstOrDefaultAsync();
            CurrentCustomer.Name = Customer.Name;
            CurrentCustomer.PhoneNumber = Customer.PhoneNumber;
            CurrentCustomer.Website = Customer.Website;
            CurrentCustomer.Address = Customer.Address;
            CurrentCustomer.City = Customer.City;
            CurrentCustomer.State = Customer.State;
            CurrentCustomer.ZipCode = Customer.ZipCode;
            CurrentCustomer.Email = Customer.Email;
            context.Entry(CurrentCustomer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteCustomer(int CustomerId)
        {
            context.ChangeTracker.Clear();
            var Customer = await context.Customer
                .Where(s => s.Id == CustomerId).FirstOrDefaultAsync();
            context.Customer.Remove(Customer);
            await context.SaveChangesAsync();
        }

        public async Task<List<CustomerNote>> GetCustomerNotes(int CustomerId)
        {
            List<CustomerNote> CustomerNotes = await context.CustomerNote
                .Where(x => x.CustomerId == CustomerId)
                   .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
            return CustomerNotes;
        }
        public async Task<CustomerNote> GetCustomerNote(int CustomerNoteId)
        {
            return await context.CustomerNote
                .Where(x => x.Id == CustomerNoteId)
                .FirstOrDefaultAsync();
        }
        public async Task CreateCustomerNote(CustomerNote CustomerNote, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            CustomerNote.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CustomerNote.UpdatedBy = user.FullName;
            CustomerNote.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CustomerNote.CreatedBy = user.FullName;
            context.CustomerNote.Add(CustomerNote);
            await context.SaveChangesAsync();
        }
        public async Task UpdateCustomerNote(CustomerNote CustomerNote, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentCustomerNote = await context.CustomerNote.Where(s => s.Id == CustomerNote.Id).FirstOrDefaultAsync();
            context.Entry(CurrentCustomerNote).State = EntityState.Detached;
            CustomerNote.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CustomerNote.UpdatedBy = user.FullName;
            CustomerNote.CreateDate = CurrentCustomerNote.CreateDate;
            CustomerNote.CreatedBy = CurrentCustomerNote.CreatedBy;
            context.Entry(CustomerNote).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }


        public async Task DeleteCustomerNote(int CustomerNoteId)
        {
            context.ChangeTracker.Clear();
            var CustomerNote = await context.CustomerNote.Where(s => s.Id == CustomerNoteId).FirstOrDefaultAsync();
            context.CustomerNote.Remove(CustomerNote);
            await context.SaveChangesAsync();
        }

        public async Task<List<Document>> GetCustomerDocuments(int CustomerId)
        {
            List<Document> Documents = await context.Document
               .Where(x => x.CustomerId == CustomerId)
                   .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
            return Documents;
        }
        public async Task<Document> GetCustomerDocument(int CustomerDocumentId)
        {
            return await context.Document
                .Where(x => x.Id == CustomerDocumentId)
                .FirstOrDefaultAsync();
        }
        public async Task CreateCustomerDocument(Document Document, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            Document.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Document.UpdatedBy = user.FullName;
            Document.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Document.CreatedBy = user.FullName;
            context.Document.Add(Document);
            await context.SaveChangesAsync();
        }
        public async Task UpdateCustomerDocument(Document Document, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = await context.User.Where(u => u.UserName == CurrentUser).FirstOrDefaultAsync();
            var CurrentDocument = await context.Document.Where(s => s.Id == Document.Id).FirstOrDefaultAsync();
            context.Entry(CurrentDocument).State = EntityState.Detached;
            Document.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Document.UpdatedBy = user.FullName;
            Document.CreateDate = CurrentDocument.CreateDate;
            Document.CreatedBy = CurrentDocument.CreatedBy;
            context.Entry(Document).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task DeleteCustomerDocument(int DocumentId)
        {
            context.ChangeTracker.Clear();
            var Document = await context.Document.Where(s => s.Id == DocumentId).FirstOrDefaultAsync();
            context.Document.Remove(Document);
            await context.SaveChangesAsync();
        }

    }
}