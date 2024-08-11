namespace sampleTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("MedicalLeadAgent")]
    public partial class MedicalLeadAgent
    {
        public int MedicalLeadAgentID { get; set; }

        [StringLength(100)]
        public string MedicalLeadAgentName_VC { get; set; }

        [StringLength(100)]
        public string MedicalLeadAgentMobilePhone_VC { get; set; }

        [StringLength(100)]
        public string MedicalLeadAgentEmail_VC { get; set; }

        public int? RecordStatus_NB { get; set; }

        [StringLength(1000)]
        public string MedicalLeadAgentLocation_VC { get; set; }

        public DateTime CreatedDate_DT { get; set; }

        public int CreatedUser_NB { get; set; }

        public DateTime? ModifiedDate_DT { get; set; }

        public int? ModifiedUser_NB { get; set; }

        public int? MedicalRepID { get; set; }

        [StringLength(100)]
        public string MedicalLeadAgentRemarks_VC { get; set; }

        [StringLength(100)]
        public string MedicalLeadAgentStatus_VC { get; set; }

        [StringLength(1000)]
        public string ImageTitle_VC { get; set; }

        [StringLength(1000)]
        public string ImageTitle2_VC { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile2 { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [NotMapped]
        public List<MedicalLeadAgent> MedicalLeadAgentList { get; set; }

       
    }
    public class MedicalLeadAgentReport
    {
        public string MedicalLeadAgentName_VC { get; set; }


        public string MedicalLeadAgentMobilePhone_VC { get; set; }


        public string MedicalLeadAgentEmail_VC { get; set; }


        public string MedicalLeadAgentLocation_VC { get; set; }

      
        public string MedicalLeadAgentRemarks_VC { get; set; }

        public string MedicalLeadAgentStatus_VC { get; set; }

    }
}
