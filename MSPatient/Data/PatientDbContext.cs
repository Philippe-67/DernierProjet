using Microsoft.EntityFrameworkCore;
using MSPatient.Models;
using System.Collections.Generic;

namespace MSPatient.Data
{
        
        public class PatientDbContext : DbContext
        {
            public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }

            public DbSet<Patient> Patients { get; set; }

        }
    }
