using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SebJones.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Display(Name = "Category")]
        public string Description { get; set; }
        [Display(Name = "Claim VAT?")]
        public bool ClaimVAT { get; set; }
    }
}