namespace sampleTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MedicalRegistration")]
    public partial class MedicalRegistration
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string RegMedicalName_VC { get; set; }

        [StringLength(10)]
        public string RegCompanyID_VC { get; set; }

        [StringLength(50)]
        public string RegMedicalPhone_VC { get; set; }

        public int MedicalRep_ID { get; set; }

        [StringLength(50)]
        public string RegMedicalCompanyCode_VC { get; set; }

        public int? BranchID { get; set; }

        public int? RecordStatus_NB { get; set; }

        public DateTime? CreatedDate_DT { get; set; }

        public int? CreatedUser_NB { get; set; }

        public DateTime? ModifiedDate_DT { get; set; }

        public int? ModifiedUser_NB { get; set; }

        public int? RegionID_NB { get; set; }

        [StringLength(50)]
        public string Grade_CH { get; set; }

        public int? LocationId_NB { get; set; }
    }
}
