using System.ComponentModel.DataAnnotations;
using AcademiaFutbolSalazar.Models;

namespace AcademiaFutbolSalazar.Models
{
    public class Entrenador
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [RegularExpression(@"^3\d{9}$",
            ErrorMessage = "El celular debe estar entre 3000000000 y 3999999999")]
        public string Celular { get; set; }

        [StringLength(100)]
        public string Especialidad { get; set; }

        public string Categoria { get; set; }

        public DateTime FechaContratacion { get; set; }

        public List<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

        [Required]
        public bool Activo { get; set; } = true;
    }
}