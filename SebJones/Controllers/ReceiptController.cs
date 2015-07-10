using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SebJones.Models;

namespace SebJones.Controllers
{
    public class ReceiptController : Controller
    {
        private IExpenseRepository repository = null;

        public ReceiptController()
        {
            this.repository = new ExpenseRepository();
        }

        //public ReceiptController(IClaimRepository claimRepo/*, IApplicationUser appUserRepo, IReceiptRepository receiptRepo*/)
        //{
        //    this.claimRepo = claimRepo;
        //    //this.appUserRepo = appUserRepo;
        //    //this.receiptRepo = receiptRepo;
        //}

        [Authorize]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int claimID = (int)id;
            var receipts = repository.SelectReceiptByClaimID(claimID);

            ViewBag.Claim = repository.SelectByID(claimID);

            var categories = from c in repository.GetCategories()
                             select new
                             {
                                 CategoryID = c.CategoryID,
                                 Description = c.Description
                             };
            SelectList list = new SelectList(categories, "CategoryID", "Description");
            ViewBag.Categories = list;

            return View(receipts);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int claimID, string newDescription, int newCategory, string newAmount)
        {
            if (ModelState.IsValid)
            {
                Receipt receipt = new Receipt();
                receipt.ClaimID = repository.SelectByID(claimID);
                receipt.Description = newDescription;
                receipt.Category = repository.GetCategoryByID(newCategory);
                receipt.Amount = Convert.ToDecimal(newAmount);

                repository.Update(receipt.ClaimID);
                repository.CreateReceipt(receipt);
                repository.SaveReceipt();
                repository.Save();

                return RedirectToAction("Index", new { id = claimID });
            }
            return RedirectToAction("Index", new { id = claimID });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Receipt receipt = repository.SelectReceiptByID(id);
            int claimID = receipt.ClaimID.ClaimID;
            repository.Update(receipt.ClaimID);
            repository.DeleteReceipt(id);
            repository.SaveReceipt();
            repository.Save();
            return RedirectToAction("Index", new { id = claimID });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int ReceiptID, string Description, int? Category, decimal Amount)
        {
            Receipt receipt = repository.SelectReceiptByID(ReceiptID);
            receipt.Description = Description;
            receipt.Amount = Amount;
            if (Category != null)
            {
                int intCategory = (int)Category;
                receipt.Category = repository.GetCategoryByID(intCategory);
            }
            repository.Update(receipt.ClaimID);
            repository.UpdateReceipt(receipt);
            repository.SaveReceipt();
            repository.Save();

            return Json(new { result = "success" });
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
