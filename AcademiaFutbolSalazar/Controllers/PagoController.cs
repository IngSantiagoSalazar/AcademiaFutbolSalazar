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
            var pagos = _context.Pagos
                .Include(p => p.Estudiante)
                .ToList();
            return View(pagos);
        }

        public IActionResult Create()
        {
            ViewBag.Estudiantes = _context.Estudiantes.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pago pago)
        {
            _context.Pagos.Add(pago);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var pago = _context.Pagos.Find(id);
            ViewBag.Estudiantes = _context.Estudiantes.ToList();
            return View(pago);
        }

        [HttpPost]
        public IActionResult Edit(Pago pago)
        {
            _context.Pagos.Update(pago);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var pago = _context.Pagos.Find(id);
            _context.Pagos.Remove(pago);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}