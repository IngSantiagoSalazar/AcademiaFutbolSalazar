using Microsoft.EntityFrameworkCore;    
using AcademiaFutbolSalazar.Models;




namespace AcademiaFutbolSalazar.Data
{
    public class AcademiaContext : DbContext
    {
        public AcademiaContext(DbContextOptions<AcademiaContext>options) 
               : base(options)
        { 

        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public  DbSet<Entrenador> Entrenadores { get; set; }
         public DbSet<Pago> Pagos { get; set; }



    }
}
   