using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SebJones.Models
{
    public class ClaimStatus
    {
        [Key]
        public int ClaimStatusID { get; set; }
        public string Description { get; set; }
    }
}