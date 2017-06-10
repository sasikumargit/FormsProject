using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Models
{
    [Table("Claim", Schema = "pic")]
    public class Claim
    {
        [Key]
        public int id { get; set; }

        [MaxLength(255)]
        public string PropertyDescription { get; set; }

        [MaxLength(15)]
        public string ItemAge { get; set; }

        public double OriginalCost { get; set; }

        public double ReplacementValue { get; set; }

        public double AmountClaimed  { get; set; }

        public int ClaimFormId { get; set; }

        [ForeignKey("ClaimFormId")]
        public ClaimAdvice ClaimAdvice { get; set; }
    }
}