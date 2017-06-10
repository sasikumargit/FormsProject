using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Models
{
    [Table("SupportingFileUpload", Schema = "pic")]
    public class SupportingFileUpload
    {
        [Key]
        public int id { get; set; }

        [MaxLength(255)]
        public string UploadDescription { get; set; }

        [MaxLength(255)]
        public string UploadFilename { get; set; }

        public int ClaimFormId { get; set; }
        [ForeignKey("ClaimFormId")]
        public ClaimAdvice ClaimAdvice { get; set; }
    }
}