namespace sampleTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        public int LogID_ID { get; set; }

        public int? Log_UserID_NB { get; set; }

        public string Log_ModuleName_VC { get; set; }

        public string Log_ClassName_VC { get; set; }

        public string Log_ProcedureName_VC { get; set; }

        public string Log_ErrorCode_VC { get; set; }

        public string Log_Remarks_VC { get; set; }

        public DateTime? Log_LogDate_DT { get; set; }

        public string Log_Parameter_VC { get; set; }
    }
}
