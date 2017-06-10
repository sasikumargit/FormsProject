using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Models
{
    [Table("InsuredDropDown", Schema = "pic")]
    public class InsuredDropDown
    {
        [Key]
        public int id { get; set; }

        [MaxLength(100)]
        public string BusinessName { get; set; }

        [MaxLength(14)]
        public string ABN { get; set; }

        [MaxLength(255)]
        public string BusinessAddress { get; set; }
    }
}