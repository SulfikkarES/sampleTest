namespace sampleTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("MedicalRepMaster")]
    public partial class MedicalRepMaster
    {
        [Key]
        public int MedicalRep_ID { get; set; }



        [StringLength(100)]
        public string MedicalRepName_VC { get; set; }
        public string MedicalLeadAgentMobilePhone_VC { get; set; }
        public string MedicalLeadAgentEmail_VC { get; set; }
        public string MedicalLeadAgentStatus_VC { get; set; }
        public string MedicalLeadAgentRemarks_VC { get; set; }


        public byte? RecordStatus_NB { get; set; }

        public DateTime? CreatedDate_DT { get; set; }

        public int? CreatedUser_NB { get; set; }

        public DateTime? ModifiedDate_DT { get; set; }

        public int? ModifiedUser_NB { get; set; }

        public string ImageTitle_VC { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public string ImageTitle2_VC { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile2 { get; set; }
        public string Location_VC { get; set; }
        

    }

    public class UMedicaLeadAgentArgs
    {
        public int LeadAgentID { get; set; }
        public string LeadAgentName_VC { get; set; }
        public string LeadAgentMobilePhone_VC { get; set; }
        public string LeadAgentEmail_VC { get; set; }
        public string LeadAgentRemarks_VC { get; set; }
        public string LeadAgentStatus_VC { get; set; }
    }

    public class DMedicaLeadAgentArgs
    {
        public int LeadAgentID { get; set; }

    }

    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }

    public class UpdateResponse
    {
        public string Message { get; set; }
    }
}
