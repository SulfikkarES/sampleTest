namespace sampleTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatusMaster")]
    public partial class StatusMaster
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string StatusType { get; set; }

        public int? RecordStatus_NB { get; set; }
    }
}
