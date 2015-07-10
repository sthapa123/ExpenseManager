using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SebJones.Models
{
    public class Claim
    {
        public Claim()
        {
            List<Receipt> ReceiptsList = new List<Receipt>();
            List<ClaimComments> CommentsList = new List<ClaimComments>();
        }

        [Key]
        [Display(Name = "Claim ID")]
        public int ClaimID { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Created By")]
        public ApplicationUser CreatedBy { get; set; }
        [Display(Name = "Last Updated By")]
        public DateTime LastUpdatedDate { get; set; }
        [Display(Name = "Status")]
        public ClaimStatus Status { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<ClaimComments> Comments { get; set; }
    }
}