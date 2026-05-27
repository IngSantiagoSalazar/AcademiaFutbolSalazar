using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Models;

namespace AcademiaFutbolSalazar.Controllers
{
    public class PagoController : Controller
    {
        private readonly AcademiaContext _context;

        public PagoController(AcademiaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var pagos = _context.Pagos
                .Include(p => p.Estudiante)
                .ToList();
            return View(pagos);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Estudiantes = _context.Estudiantes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pago pago)
        {
            if (pago.FechaPago > DateTime.Now)
            {
                ModelState.AddModelError("FechaPago", "La fecha de pago no puede ser mayor a hoy");
                ViewBag.Estudiantes = _context.Estudiantes.ToList();
                return View(pago);
            }

            // ✅ Validar mes duplicado
            var pagoExiste = _context.Pagos
                .Any(p => p.EstudianteId == pago.EstudianteId
                       && p.Descripcion == pago.Descripcion
                       && p.Pagado == true);

            if (pagoExiste)
            {
                ModelState.AddModelError("Descripcion", "Este estudiante ya pagó ese mes");
                ViewBag.Estudiantes = _context.Estudiantes.ToList();
                return View(pago);
            }

            _context.Pagos.Add(pago);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Pago pago)
        {
            if (pago.FechaPago > DateTime.Now)
            {
                ModelState.AddModelError("FechaPago", "La fecha de pago no puede ser mayor a hoy");
                ViewBag.Estudiantes = _context.Estudiantes.ToList();
                return View(pago);
            }

            // ✅ Validar mes duplicado (excluyendo el pago actual)
            var pagoExiste = _context.Pagos
                .Any(p => p.EstudianteId == pago.EstudianteId
                       && p.Descripcion == pago.Descripcion
                       && p.Pagado == true
                       && p.Id != pago.Id); // ✅ excluye el pago que estamos editando

            if (pagoExiste)
            {
                ModelState.AddModelError("Descripcion", "Este estudiante ya pagó ese mes");
                ViewBag.Estudiantes = _context.Estudiantes.ToList();
                return View(pago);
            }

            _context.Pagos.Update(pago);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}