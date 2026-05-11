using System.ComponentModel.DataAnnotations;

namespace AcademiaFutbolSalazar.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        public int Edad => DateTime.Now.Year - FechaNacimiento.Year;

        [Required]
        [RegularExpression(@"^3\d{9}$",
            ErrorMessage = "El celular debe estar entre 3000000000 y 3999999999")]
        public string Celular { get; set; }

        [StringLength(50)]
        public string Categoria { get; set; }

        [StringLength(50)]
        public string Posicion { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public int EntrenadorId { get; set; }
        public Entrenador? Entrenador { get; set; }

        [Required]
        public bool Activo { get; set; } = true;
    }
}