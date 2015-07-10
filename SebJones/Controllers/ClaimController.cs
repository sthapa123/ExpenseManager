using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SebJones.Models;
using Microsoft.AspNet.Identity;

namespace SebJones.Controllers
{
    public class ClaimController : Controller
    {
        private IExpenseRepository repository = null;

        public ClaimController()
        {
            this.repository = new ExpenseRepository();
        }

        //public ClaimController(IClaimRepository claimRepo/*, IApplicationUser appUserRepo, IReceiptRepository receiptRepo*/)
        //{
        //    this.claimRepo = claimRepo;
        //    //this.appUserRepo = appUserRepo;
        //    //this.receiptRepo = receiptRepo;
        //}

        [Authorize]
        public ActionResult Index()
        {
            List<Claim> claims = null;
            if (User.IsInRole("admin"))
            {
                claims = (List<Claim>)repository.SelectAll();
            }
            else
            {
                string userID = User.Identity.GetUserId();
                claims = (List<Claim>)repository.SelectOwn(userID);
            }
            return View(claims);
        }

        [Authorize]
        public ActionResult Create()
        {
            string userID = User.Identity.GetUserId();
            ApplicationUser user = repository.GetUser(userID);

            Claim claim = new Claim();
            claim.CreatedBy = user;
            claim.CreatedDate = DateTime.Now;
            claim.LastUpdatedDate = DateTime.Now;
            claim.Status = repository.GetStatus("Draft");

            repository.Create(claim);
            repository.Save();
            return RedirectToAction("Index", "Receipt", new { id = claim.ClaimID });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Claim claim = repository.SelectByID(id);
            if (claim.Receipts != null)
            {
                List<Receipt> receipts = claim.Receipts.ToList();
                foreach (Receipt r in receipts)
                {
                    repository.DeleteReceipt(r.ReceiptID);
                }
                repository.Save();
            }
            repository.Delete(claim.ClaimID);
            repository.Save();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Reject(int id)
        {
            Claim claim = repository.SelectByID(id);
            var claimStatus = repository.GetStatus("Rejected");
            if (claimStatus != null &&
                claim.Status.Description == "Submitted")
            {
                claim.Status = claimStatus;
                repository.Update(claim);
                repository.Save();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Approve(int id)
        {
            Claim claim = repository.SelectByID(id);
            var claimStatus = repository.GetStatus("Approved");
            if (claimStatus != null &&
                claim.Status.Description == "Submitted")
            {
                claim.Status = claimStatus;
                repository.Update(claim);
                repository.Save();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Submit(int id)
        {
            Claim claim = repository.SelectByID(id);
            var claimStatus = repository.GetStatus("Submitted");
            if (claimStatus != null &&
                claim.Status.Description == "Draft")
            {
                claim.Status = claimStatus;
                repository.Update(claim);
                repository.Save();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Withdraw(int id)
        {
            Claim claim = repository.SelectByID(id);
            var claimStatus = repository.GetStatus("Draft");
            if (claimStatus != null &&
                claim.Status.Description == "Submitted")
            {
                claim.Status = claimStatus;
                repository.Update(claim);
                repository.Save();
            }
            return RedirectToAction("Index");
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
