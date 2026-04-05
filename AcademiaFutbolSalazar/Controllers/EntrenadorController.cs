using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Models;

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
            var entrenadores = _context.Entrenadores
                .Include(e => e.Estudiantes)
                .ToList();
            return View(entrenadores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Entrenador entrenador)
        {
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

        [HttpPost]
        public IActionResult Edit(Entrenador entrenador)
        {
            _context.Entrenadores.Update(entrenador);
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