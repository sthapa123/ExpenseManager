using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SebJones.Models
{
    interface IExpenseRepository
    {
        //
        // Claim Repository
        //
        IEnumerable<Claim> SelectAll();
        IEnumerable<Claim> SelectOwn(string userID);
        Claim SelectByID(int id);
        void Create(Claim obj);
        void Update(Claim obj);
        void Delete(int id);
        void Save();
        
        // Status
        ClaimStatus GetStatus(string description);

        //
        // Receipt Repository
        //
        Receipt SelectReceiptByID(int id);
        IEnumerable<Receipt> SelectReceiptByClaimID(int claimID);
        void CreateReceipt(Receipt obj);
        void UpdateReceipt(Receipt obj);
        void DeleteReceipt(int id);
        void SaveReceipt();

        // Categories
        IEnumerable<Category> GetCategories();
        Category GetCategoryByID(int categoryID);

        // App User
        ApplicationUser GetUser(string userID);
    }
}
