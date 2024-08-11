namespace sampleTest.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminLogin")]
    public partial class AdminLogin
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        public int? RecordStatus_NB { get; set; }

        public DateTime? CreatedDate_DT { get; set; }

        public int? CreatedUser_NB { get; set; }

        public DateTime? ModifiedDate_DT { get; set; }

        public int? ModifiedUser_NB { get; set; }
    }

    public class Response1
    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }
}
