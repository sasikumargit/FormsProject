using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Models
{
    [Table("ClaimAdvice", Schema = "pic")]
    public class ClaimAdvice
    {
        public ClaimAdvice()
        {
            PolicyNumber = "FSP120000479";
            CreationDate = DateTime.Now;
        }

        [Key]
        public int id { get; set; }

        public DateTime CreationDate { get; private set; }

        [MaxLength(10)]
        public string ReferenceNumber { get; set; }

        [MaxLength(15)]
        public string IPAddress { get; set; }

        [MaxLength(100)]
        public string InsuredBusinessName { get; set; }

        [MaxLength(14)]
        public string InsuredABN { get; set; }

        [MaxLength(255)]
        public string InsuredBusinessAddress { get; set; }

        [MaxLength(20)]
        public string PolicyNumber { get; set; }

        [MaxLength(150)]
        public string ContactName { get; set; }

        [MaxLength(255)]
        public string ContactEmail { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateofEvent { get; set; }

        [MaxLength(8)]
        public string TimeofDay { get; set; }

        [MaxLength(255)]
        public string EventAddress { get; set; }

        public string HowLossDescription { get; set; }

        [MaxLength(3)]
        public string ThirdPartyBlame { get; set; }

        [MaxLength(3)]
        public string ReceiveNoticeThirdParty { get; set; }

        public string ReceiveNoticeThirdPartyDetails { get; set; }

        [MaxLength(3)]
        public string WitnessesatScene { get; set; }

        [MaxLength(3)]
        public string BurglaryorTheft { get; set; }

        [MaxLength(150)]
        public string PoliceStation { get; set; }

        [MaxLength(150)]
        public string PoliceOfficerName { get; set; }

        [MaxLength(30)]
        public string PoliceReportNumber { get; set; }

        public double TotalAmountClaimed { get; set; }

        [MaxLength(3)]
        public string ThirdPartyLiabilityClaim { get; set; }

        [MaxLength(150)]
        public string ThirdPartyName { get; set; }

        [MaxLength(255)]
        public string ThirdPartyPermanentAddress  { get; set; }

        public string InjuriesDamageDescription { get; set; }

        [MaxLength(3)]
        public string ThirdPartyCorrespondence { get; set; }

        [MaxLength(3)]
        public string AdmissionofLiability { get; set; }

        public string AdmissionofLiabilityDetails { get; set; }

        public string OtherComments { get; set; }

        [MaxLength(3)]
        public string DeclarationChecked  { get; set; }

        public List<ThirdPartyBlame> ThirdPartyBlames { get; set; }
        public List<SceneWitness> SceneWitnesses { get; set; }
        public List<Claim> Claims { get; set; }
        public List<SupportingFileUpload> SupportingFileUploads { get; set; }
        public List<ThirdPartyFileUpload> ThirdPartyFileUploads { get; set; }
    }
}