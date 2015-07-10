using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SebJones.Models
{
    public class ExpenseRepository : IExpenseRepository
    {
        private ApplicationDbContext db = null;

        public ExpenseRepository()
        {
            this.db = new ApplicationDbContext();
        }

        public ExpenseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        //
        // Claim Repository
        //

        public IEnumerable<Claim> SelectAll()
        {
            var claims = db.Claims.Include(x => x.CreatedBy)
                                .Include(x => x.Status)
                                .OrderByDescending(x => x.CreatedDate).ToList();
            return claims;
        }

        public IEnumerable<Claim> SelectOwn(string userID)
        {
            var claims = db.Claims.Where(x => x.CreatedBy.Id == userID)
                                .Include(x => x.CreatedBy)
                                .Include(x => x.Status)
                                .OrderByDescending(x => x.CreatedDate).ToList();
            return claims;
        }

        public Claim SelectByID(int id)
        {
            var claim = db.Claims.Where(x => x.ClaimID == id)
                .Include(x => x.CreatedBy)
                .Include(x => x.Receipts)
                .Include(x => x.Status)
                .FirstOrDefault();
            return claim;
        }

        public void Create(Claim obj)
        {
            db.Claims.Add(obj);
        }

        public void Update(Claim obj)
        {
            obj.LastUpdatedDate = DateTime.Now;
            db.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Claim claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public ClaimStatus GetStatus(string description)
        {
            var status = db.ClaimStatus
                .Where(x => x.Description == description)
                .FirstOrDefault();
            return status;
        }

        //
        // Receipt Repository
        //
        public Receipt SelectReceiptByID(int id)
        {
            var receipt = db.Receipts.Where(x => x.ReceiptID == id)
                .Include(x => x.ClaimID)
                .Include(x => x.Category)
                .FirstOrDefault();
            return receipt;
        }

        public IEnumerable<Receipt> SelectReceiptByClaimID(int claimID)
        {
            var receipts = db.Receipts.Where(x => x.ClaimID.ClaimID == claimID)
                .Include(x => x.Category)
                .OrderByDescending(x => x.ReceiptID).ToList();

            return receipts;
        }

        public void CreateReceipt(Receipt obj)
        {
            db.Receipts.Add(obj);
        }

        public void UpdateReceipt(Receipt obj)
        {
            db.Entry(obj).State = EntityState.Modified;
        }

        public void DeleteReceipt(int id)
        {
            Receipt receipt = db.Receipts.Find(id);
            db.Receipts.Remove(receipt);
        }

        public void SaveReceipt()
        {
            db.SaveChanges();
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = from c in db.Categories
                             select c;

            return categories;
        }

        public Category GetCategoryByID(int categoryID)
        {
            var category = (from c in db.Categories
                            where c.CategoryID == categoryID
                            select c).FirstOrDefault();
            return category;
        }

        //
        // AppUser Repository
        //
        public ApplicationUser GetUser(string userID)
        {
            ApplicationUser user = db.Users
                .FirstOrDefault(x => x.Id == userID);
            return user;
        }
    }
}