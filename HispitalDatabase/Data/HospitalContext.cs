using P01_HospitalDatabase.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext:DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> Prescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(Config.connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigPatientModel(modelBuilder);

            ConfigVisitationModel(modelBuilder);

            ConfigDiagnoseModel(modelBuilder);

            ConfigMedicamentModel(modelBuilder);

            ConfigPrescriptionsModel(modelBuilder);
        }

        private void ConfigPrescriptionsModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(x => new
                {
                    x.Patient.PatientId,
                    x.Medicament.MedicamentId
                });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(p => p.Patient)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(m => m.Medicament)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.MedicamentId);
        }

        private void ConfigMedicamentModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Medicament>()
                .HasKey(x => x.MedicamentId);

            modelBuilder
                .Entity<Medicament>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode(true);


        }

        private void ConfigDiagnoseModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Diagnose>()
                .HasKey(x => x.DiagnoseId);

            modelBuilder
                .Entity<Diagnose>()
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode(true);

            modelBuilder
                .Entity<Diagnose>()
                .Property(x => x.Comments)
                .HasMaxLength(250)
                .IsUnicode(true);

            modelBuilder
                .Entity<Diagnose>()
                .HasOne(p => p.Patient)
                .WithMany(d => d.Diagnoses);
        }

        private void ConfigVisitationModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Visitation>()
                .HasKey(x => x.VisitationId);

            modelBuilder
                .Entity<Visitation>()
                .Property(x => x.VisitationId)
                .HasMaxLength(250)
                .IsUnicode(true);
        }

        private void ConfigPatientModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Patient>()
                .HasKey(x => x.PatientId);

            modelBuilder
                .Entity<Patient>()
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsUnicode(true);

            modelBuilder
                .Entity<Patient>()
                .Property(x => x.LastName)
                .HasMaxLength(50)
                .IsUnicode(true);

            modelBuilder
                .Entity<Patient>()
                .Property(x => x.Address)
                .HasMaxLength(250)
                .IsUnicode(true);

            modelBuilder
                .Entity<Patient>()
                .Property(x => x.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

            modelBuilder
                .Entity<Patient>()
                .HasMany(v => v.Visitations)
                .WithOne(p => p.Patient);

            modelBuilder
                .Entity<Patient>()
                .HasMany(d => d.Diagnoses)
                .WithOne(p => p.Patient);
        }
    }
}
