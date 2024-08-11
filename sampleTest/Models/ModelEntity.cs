using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace sampleTest.Models
{
    public partial class ModelEntity : DbContext
    {
        public ModelEntity()
            : base("name=ModelEntity")
        {
        }

        public virtual DbSet<AdminLogin> AdminLogins { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<MedicalLeadAgent> MedicalLeadAgents { get; set; }
        public virtual DbSet<MedicalRegistration> MedicalRegistrations { get; set; }
        public virtual DbSet<MedicalRepMaster> MedicalRepMasters { get; set; }
        public virtual DbSet<StatusMaster> StatusMasters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalRegistration>()
                .Property(e => e.RegMedicalName_VC)
                .IsUnicode(false);

            modelBuilder.Entity<MedicalRegistration>()
                .Property(e => e.RegMedicalPhone_VC)
                .IsUnicode(false);

            modelBuilder.Entity<MedicalRegistration>()
                .Property(e => e.RegMedicalCompanyCode_VC)
                .IsUnicode(false);





            modelBuilder.Entity<MedicalRepMaster>()
                .Property(e => e.MedicalRepName_VC)
                .IsUnicode(false);
        }
    }
}
