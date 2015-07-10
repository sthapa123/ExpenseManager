using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SebJones.Models
{
    public class ClaimComments
    {
        [Key]
        public int CommentID { get; set; }
        public virtual Claim ClaimID { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string Text { get; set; }
    }
}