using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Models
{
    [Table("SceneWitness", Schema = "pic")]
    public class SceneWitness
    {
        [Key]
        public int id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(7)]
        public string Registration { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public int ClaimFormId { get; set; }

        [ForeignKey("ClaimFormId")]
        public ClaimAdvice ClaimAdvice { get; set; }
    }
}