using System.ComponentModel.DataAnnotations;
using AcademiaFutbolSalazar.Models;

namespace AcademiaFutbolSalazar.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [Required]
        public double Monto { get; set; }

        [Required]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string MetodoPago { get; set; }  

        [StringLength(200)]
        public string Descripcion { get; set; } 

        public bool Pagado { get; set; } = false;

       
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
    }
}