using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Helpers;
using AcademiaFutbolSalazar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AcademiaFutbolSalazar.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly AcademiaContext _context;

        public EstudianteController(AcademiaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var estudiantes = _context.Estudiantes
                .Include(e => e.Entrenador)
                .ToList();
            return View(estudiantes);
        }

        public IActionResult Create()
        {
            ViewBag.Entrenadores = _context.Entrenadores.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Estudiante estudiante, IFormFile imagen)
        {
            if (imagen != null)
            {
                var ruta = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/images", imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                estudiante.ImagenUrl = "/images/" + imagen.FileName;
            }

            estudiante.clave = HashHelper.obtenerHash(estudiante.clave);

            _context.Estudiantes.Add(estudiante);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var estudiante = _context.Estudiantes.Find(id);
            ViewBag.Entrenadores = _context.Entrenadores.ToList();
            return View(estudiante);
        }

        [HttpPost]
        public IActionResult Edit(Estudiante estudiante, IFormFile imagen)
        {
            var estudianteBD = _context.Estudiantes.Find(estudiante.Id);
            if (estudianteBD == null)
                return NotFound();

            estudianteBD.Nombre = estudiante.Nombre;
            estudianteBD.Apellido = estudiante.Apellido;
            estudianteBD.FechaNacimiento = estudiante.FechaNacimiento;
            estudianteBD.Celular = estudiante.Celular;
            estudianteBD.Categoria = estudiante.Categoria;
            estudianteBD.EntrenadorId = estudiante.EntrenadorId;
            estudianteBD.Activo = estudiante.Activo;

            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var ruta = Path.Combine(carpeta, imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                estudianteBD.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var estudiante = _context.Estudiantes.Find(id);
            _context.Estudiantes.Remove(estudiante);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AgregarPago(int id, double monto, string mes)
        {
            // VALIDAR QUE NO HAYA PAGADO ESE MES YA EN LA BD
            var pagoExiste = _context.Pagos
                .Any(p => p.EstudianteId == id && p.Descripcion == mes && p.Pagado == true);

            if (pagoExiste)
            {
                TempData["Error"] = "Este estudiante ya pagó " + mes;
                return RedirectToAction("Index");
            }

            var pagoJson = HttpContext.Session.GetString("Pagos");
            List<PagoItem> pagos;

            if (pagoJson == null)
            {
                pagos = new List<PagoItem>();
            }
            else
            {
                pagos = JsonSerializer.Deserialize<List<PagoItem>>(pagoJson);
            }

            var item = pagos.FirstOrDefault(p => p.EstudianteId == id);

            if (item != null)
            {
                item.Monto = monto;
                item.Mes = mes;
            }
            else
            {
                pagos.Add(new PagoItem
                {
                    EstudianteId = id,
                    Monto = monto,
                    Mes = mes
                });
            }

            HttpContext.Session.SetString("Pagos", JsonSerializer.Serialize(pagos));

            TempData["Mensaje"] = "Pago de " + mes + " agregado correctamente";
            return RedirectToAction("Index");
        }

        public IActionResult VerPagos()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var pagoJson = HttpContext.Session.GetString("Pagos");
            List<(Estudiante estudiante, double monto, string mes)> lista = new();

            if (pagoJson != null)
            {
                var pagos = JsonSerializer.Deserialize<List<PagoItem>>(pagoJson);

                foreach (var item in pagos)
                {
                    var estudiante = _context.Estudiantes.Find(item.EstudianteId);
                    if (estudiante != null)
                    {
                        lista.Add((estudiante, item.Monto, item.Mes));
                    }
                }
            }

            return View(lista);
        }

        public IActionResult ConfirmarPagos()
        {
            var pagoJson = HttpContext.Session.GetString("Pagos");

            if (pagoJson == null)
                return RedirectToAction("Index");

            var pagos = JsonSerializer.Deserialize<List<PagoItem>>(pagoJson);

            foreach (var item in pagos)
            {
                var pago = new Pago
                {
                    EstudianteId = item.EstudianteId,
                    Monto = item.Monto,
                    FechaPago = DateTime.Now,
                    Pagado = true,
                    Descripcion = item.Mes,
                    MetodoPago = "Efectivo"
                };

                _context.Pagos.Add(pago);
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("Pagos");
            return RedirectToAction("Index");
        }
    }
}