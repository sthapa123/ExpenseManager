using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SebJones.Models
{
    public class Receipt
    {
        public Receipt() { }

        [Key]
        [Display(Name = "Receipt ID")]
        public int ReceiptID { get; set; }
        [Display(Name = "Claim ID")]
        public virtual Claim ClaimID { get; set; }
        //[Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //[Required]
        [Display(Name = "Category")]
        public Category Category { get; set; }
        //[Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}