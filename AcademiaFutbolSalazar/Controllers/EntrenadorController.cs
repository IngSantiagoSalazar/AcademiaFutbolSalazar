using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Helpers;
using AcademiaFutbolSalazar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AcademiaFutbolSalazar.Controllers
{
    public class EntrenadorController : Controller
    {
        private readonly AcademiaContext _context;

        public EntrenadorController(AcademiaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var entrenadores = _context.Entrenadores.ToList(); // ✅ sin Include
            return View(entrenadores);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Entrenadores = _context.Entrenadores.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Entrenador entrenador, IFormFile imagen)
        {
            if (entrenador.FechaContratacion > DateTime.Now)
            {
                ModelState.AddModelError("FechaContratacion", "La fecha de contratación no puede ser mayor a hoy");
                return View(entrenador);
            }

            if (imagen != null)
            {
                var ruta = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/images", imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                entrenador.ImagenUrl = "/images/" + imagen.FileName;
            }

            if (!string.IsNullOrEmpty(entrenador.clave))
            {
                entrenador.clave = HashHelper.obtenerHash(entrenador.clave);
            }

            _context.Entrenadores.Add(entrenador);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            
            var entrenador = _context.Entrenadores.Find(id);
            ViewBag.Estudiantes = _context.Estudiantes.ToList();
            return View(entrenador);
        }

        // ACTUALIZAR ENTRENADOR
        [HttpPost]
        public IActionResult Edit(Entrenador entrenador, IFormFile imagen)
        {
            if (entrenador.FechaContratacion > DateTime.Now)
            {
                ModelState.AddModelError("FechaContratacion", "La fecha de contratación no puede ser mayor a hoy");
                return View(entrenador);
            }

            var entrenadorBD = _context.Entrenadores.Find(entrenador.Id);
            if (entrenadorBD == null)
                return NotFound();

            entrenadorBD.Nombre = entrenador.Nombre;
            entrenadorBD.Apellido = entrenador.Apellido;
            entrenadorBD.Celular = entrenador.Celular;
            entrenadorBD.Especialidad = entrenador.Especialidad;
            entrenadorBD.Categoria = entrenador.Categoria;
            entrenadorBD.FechaContratacion = entrenador.FechaContratacion;
            entrenadorBD.Activo = entrenador.Activo;

            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var ruta = Path.Combine(carpeta, imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                entrenadorBD.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var entrenador = _context.Entrenadores.Find(id);
            _context.Entrenadores.Remove(entrenador);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}