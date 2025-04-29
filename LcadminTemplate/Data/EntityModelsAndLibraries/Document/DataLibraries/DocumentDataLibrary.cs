using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Data.GeneralEnums;

namespace Data
{
    public class DocumentDataLibrary
    {
        public DataContext context { get; }

        public DocumentDataLibrary(DataContext Context)
        {
            context = Context;
        }

      

        public async Task<List<Document>> GetControlDocuments()
        {
            List<Document> Documents = await context.Document.Include(x=>x.CompanyFolder)
            .ToListAsync();

            return Documents;
        }

        public async Task<List<CompanyFolder>> GetParentFolders()
        {
            List<CompanyFolder> folders = await context.CompanyFolder
                .Where(x => x.ParentFolderId == 0)
                .ToListAsync();
            
            foreach (var folder in folders)
            {
                folder.FolderAccesses = await context.FolderAccess
                    .Where(fa => fa.CompanyFolderId == folder.Id)
                    .Include(fa => fa.Role) 
                    .ThenInclude(x=>x.RolePermissions)
                    .ToListAsync();
            }
            return folders;
        }

        public async Task<List<CompanyFolder>> GetParentFolders(string userName, string filterName, int companyId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await context.CompanyFolder
                    .Where(x => x.ParentFolderId == 0 && x.CompanyId == companyId)
                    .Include(x => x.FolderAccesses)
                        .ThenInclude(fa => fa.Role)
                            .ThenInclude(r => r.RolePermissions)
                    .ToListAsync();
            }

            var userRoleIds = await context.CompayUserRole
                .Where(cur => cur.CompanyUser.User.UserName == userName)
                .Select(cur => cur.RoleId)
                .ToListAsync();

            string normalizedFilterName = filterName?.ToLower();
            var folders = await context.CompanyFolder
                .Include(x => x.FolderAccesses)
                    .ThenInclude(fa => fa.Role)
                        .ThenInclude(r => r.RolePermissions)
                .Where(x => x.ParentFolderId == 0 &&
                            (x.RestrictAccess || x.AllUserAccess) &&
                            x.CompanyId == companyId &&
                            (string.IsNullOrEmpty(normalizedFilterName) || x.Name.ToLower() == normalizedFilterName))
                .ToListAsync();

            var filteredFolders = folders
                .Where(folder => folder.AllUserAccess ||
                                 folder.FolderAccesses.Any(fa => userRoleIds.Contains(fa.RoleId)))
                .ToList();

            return filteredFolders;
        }


        public async Task<List<Document>> GetRestrictFolderDocumentsByRole(int RoleId, int CompanyId)
        {
            List<CompanyFolder> folders = await context.FolderAccess
                .Where(x => x.RoleId == RoleId)
                .Select(x => x.CompanyFolder)
                .ToListAsync();

            List<Document> allDocuments = new List<Document>();

            foreach (var folder in folders)
            {
                var restrictedDocuments = await context.Document
                    .Where(x => x.CompanySubFolderId == folder.Id && x.CompanyId == CompanyId)
                    .ToListAsync();

                allDocuments.AddRange(restrictedDocuments);
            }

            var unRestrictedDocuments = await context.Document
                    .Where(x => x.CompanySubFolderId == null && x.CompanyId == CompanyId)
                    .ToListAsync();

            allDocuments.AddRange(unRestrictedDocuments);

            return allDocuments;
        }



        public async Task<List<CompanyFolder>> GetSubFolders(int ParentFolderId)
        {
            List<CompanyFolder> Documents = await context.CompanyFolder.Where(x=>x.ParentFolderId== ParentFolderId)
            .ToListAsync();

            foreach (var folder in Documents)
            {
                folder.FolderAccesses = await context.FolderAccess
                    .Where(fa => fa.CompanyFolderId == folder.Id)
                    .Include(fa => fa.Role)
                    .ThenInclude(x => x.RolePermissions)
                    .ToListAsync();
            }
            return Documents;
        }


        public async Task<Document> GetDocument(int DocumentId)
        {
            var Document = await context.Document         
            .Include(x => x.CompanyFolder)
            .Include(x => x.DocumentVersions)         
            .Where(x => x.Id == DocumentId).FirstOrDefaultAsync();

            if(Document != null)
            {
                Document.DocumentVersions = Document.DocumentVersions.OrderBy(x => x.VersionNumber).ToList();
            }

            return Document;
        }

        public async Task<Document> GetDocumentByName(string Name)
        {
            var Document = await context.Document
            .Where(x => x.DocumentName == Name).FirstOrDefaultAsync();
            return Document;
        }

        public async Task<List<Document>> GetActiveDocuments(int CompanyId, string FilterName)
        {
            List<Document> Documents = await context.Document
            .Where(x => x.Status == Status.Active && x.CompanySubFolderId==null && x.CompanyId == CompanyId
            && FilterName == null || FilterName != null && x.DocumentName.ToLower() == FilterName.ToLower())
            .ToListAsync();

            return Documents;
        }

        public async Task<List<Document>> GetAllDocuments(int CompanyId)
        {
            List<Document> Documents = await context.Document
            .Where(x => x.Status == Status.Active && x.CompanyId == CompanyId)
            .ToListAsync();

            return Documents;
        }

        public async Task<int> CreateDocument(Document Document, string CurrentUser)
        {
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            Document.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Document.UpdatedBy = user.UserName;
            Document.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Document.CreatedBy = user.Id;
            await context.Document.AddAsync(Document);
            var success = await context.SaveChangesAsync() > 0;
            return Document.Id;
        }

        public async Task<bool> UpdateDocument(Document Document, string CurrentUser)
        {
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            var CurrentDocument = context.Document.Where(s => s.Id == Document.Id).FirstOrDefault();
            context.Entry(CurrentDocument).State = EntityState.Detached;
            Document.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Document.UpdatedBy = user.Id;
            Document.CreateDate = CurrentDocument.CreateDate;
            Document.CreatedBy = CurrentDocument.CreatedBy;
            context.Entry(Document).State = EntityState.Modified;
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }

        public async Task<bool> DeleteDocument(int DocumentId)
        {
            var Document = context.Document.Where(s => s.Id == DocumentId).FirstOrDefault();
            context.Document.Remove(Document);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }

        public async Task<List<CompanyFolder>> GetCompanySubFolders(int ParentFolderId)
        {
            List<CompanyFolder> CompanySubFolders = await context.CompanyFolder
                .Where(x => x.ParentFolderId == ParentFolderId)
                .OrderBy(x => x.Order)
                .Include(x => x.Documents)
            .ToListAsync();

            return CompanySubFolders;
        }

        public async Task<List<CompanyFolder>> GetAllCompanySubFoldersAsync(int parentFolderId)
        {

            var trail = new List<CompanyFolder>();
            while (parentFolderId != 0)
            {
                var folder = await context.CompanyFolder
                    .Include(x => x.Documents)
                    .FirstOrDefaultAsync(x => x.Id == parentFolderId);

                if (folder == null)
                {
                    break;
                }

                trail.Insert(0, folder); // Insert at the start of the list to maintain the order
                parentFolderId = folder.ParentFolderId;

                
            }
            return trail;
        }




        public async Task<List<FolderAccess>> GetFolderAcccess(int FolderId)
        {
            List<FolderAccess> CompanySubFolders = await context.FolderAccess
                .Include(x=>x.Role)
                .Where(x => x.CompanyFolderId == FolderId)           
            .ToListAsync();



            return CompanySubFolders;
        }

        public async Task<CompanyFolder> GetCompanySubFolder(int CompanySubFolderId)
        {
            var CompanySubFolder = await context.CompanyFolder
            .Where(x => x.Id == CompanySubFolderId).FirstOrDefaultAsync();

            return CompanySubFolder;
        }
        public async Task<List<Document>> GetFolderDocuments(int CompanySubFolderId)
        {
            var Documents = await context.Document
            .Where(x => x.CompanySubFolderId == CompanySubFolderId).ToListAsync();

            return Documents;
        }

       

        public async Task<List<CompanyFolder>> GetActiveCompanySubFolders()
        {
            List<CompanyFolder> CompanySubFolders = await context.CompanyFolder
            .Where(x => x.Status == Status.Active)
            .ToListAsync();

            return CompanySubFolders;
        }

        public async Task<int> CreateCompanySubFolder(CompanyFolder CompanySubFolder, string CurrentUser, int ParentFolderId=0)
        {
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            CompanySubFolder.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CompanySubFolder.UpdatedBy = user.Id;
            CompanySubFolder.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            CompanySubFolder.CreatedBy = user.Id;
            var Count= await context.CompanyFolder.CountAsync();
            if(Count == 0) {
            CompanySubFolder.ParentFolderId = ParentFolderId;
            }
            await context.CompanyFolder.AddAsync(CompanySubFolder);
            await context.SaveChangesAsync();
            return CompanySubFolder.Id;
        }
        public async Task<int> CreateFolderAcccess(FolderAccess FolderAccess, string CurrentUser, int CompanySubFolderId)
        {
            context.ChangeTracker.Clear();
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            FolderAccess.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            FolderAccess.UpdatedBy = user.Id;
            FolderAccess.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            FolderAccess.CreatedBy = user.Id;
            FolderAccess.CompanyFolderId = CompanySubFolderId;
            await context.FolderAccess.AddAsync(FolderAccess);
            var success = await context.SaveChangesAsync();
            return FolderAccess.Id;
        }
        public async Task<bool> UpdateCompanySubFolder(int CompanySubFolderId, string Name, bool AllUserAccess, bool RestrictAccess)
        {
            var CurrentCompanySubFolder = context.CompanyFolder.Where(s => s.Id == CompanySubFolderId).FirstOrDefault();
            CurrentCompanySubFolder.Name = Name;
            CurrentCompanySubFolder.AllUserAccess = AllUserAccess;
            CurrentCompanySubFolder.RestrictAccess = RestrictAccess;
            context.Entry(CurrentCompanySubFolder).State = EntityState.Modified;
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }

      

       

        public async Task<bool> DeleteCompanySubFolder(int CompanySubFolderId)
        {
            var CompanySubFolder = context.CompanyFolder.Where(s => s.Id == CompanySubFolderId).FirstOrDefault();
            context.CompanyFolder.Remove(CompanySubFolder);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }
        public async Task<bool> DeleteFolderAccess(int FolderAccessId)
        {
            var FolderAccess = context.FolderAccess.Where(s => s.Id == FolderAccessId).FirstOrDefault();
            context.FolderAccess.Remove(FolderAccess);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }


        public async Task<List<DocumentVersion>> GetDocumentVersions()
        {
            List<DocumentVersion> DocumentVersions = await context.DocumentVersion
            .ToListAsync();

            return DocumentVersions;
        }

        public async Task<DocumentVersion> GetDocumentVersion(int DocumentVersionId)
        {
            var DocumentVersion = await context.DocumentVersion
            .Where(x => x.Id == DocumentVersionId).FirstOrDefaultAsync();

            return DocumentVersion;
        }
        public async Task<List<DocumentVersion>> GetDocumentVersionsByDocumentId(int DocumentId)
        {
            var DocumentVersion = await context.DocumentVersion
            .Where(x => x.DocumentId == DocumentId).ToListAsync();

            return DocumentVersion;
        }

        public async Task<List<DocumentVersion>> GetActiveDocumentVersions()
        {
            List<DocumentVersion> DocumentVersions = await context.DocumentVersion
            .Where(x => x.Status == Status.Active)
            .ToListAsync();

            return DocumentVersions;
        }

        public async Task<bool> CreateDocumentVersion(DocumentVersion DocumentVersion, string CurrentUser)
        {
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            DocumentVersion.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            DocumentVersion.UpdatedBy = user.UserName;
            DocumentVersion.CreateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            DocumentVersion.CreatedBy = user.Id;
            await context.DocumentVersion.AddAsync(DocumentVersion);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }

        public async Task<bool> UpdateDocumentVersion(DocumentVersion DocumentVersion, string CurrentUser)
        {
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            var CurrentDocumentVersion = context.DocumentVersion.Where(s => s.Id == DocumentVersion.Id).FirstOrDefault();
            context.Entry(CurrentDocumentVersion).State = EntityState.Detached;
            DocumentVersion.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            DocumentVersion.UpdatedBy = user.UserName;
            DocumentVersion.CreateDate = CurrentDocumentVersion.CreateDate;
            DocumentVersion.CreatedBy = CurrentDocumentVersion.CreatedBy;
            context.Entry(DocumentVersion).State = EntityState.Modified;
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }

        public async Task<bool> DeleteDocumentVersion(int DocumentVersionId)
        {
            var DocumentVersion = context.DocumentVersion.Where(s => s.Id == DocumentVersionId).FirstOrDefault();
            context.DocumentVersion.Remove(DocumentVersion);
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }
        public async Task<bool> UpdateFolder(CompanyFolder Folder, string CurrentUser)
        {
            context.ChangeTracker.Clear();
            var user = context.User.Where(u => u.UserName == CurrentUser).FirstOrDefault();
            var CurrentFolder = context.CompanyFolder.Where(s => s.Id == Folder.Id).FirstOrDefault();
            context.Entry(CurrentFolder).State = EntityState.Detached;
            Folder.UpdateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", "Central Standard Time");
            Folder.UpdatedBy = user.UserName;
            context.Entry(Folder).State = EntityState.Modified;
            var success = await context.SaveChangesAsync() > 0;
            return success;
        }


    }
}
