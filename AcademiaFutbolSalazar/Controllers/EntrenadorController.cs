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
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var entrenadores = _context.Entrenadores
                .Include(e => e.Estudiantes)
                .ToList();
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
        public IActionResult Create(Entrenador entrenador)
        {
            if (!ModelState.IsValid)
            {
                return View(entrenador);
            }
            _context.Entrenadores.Add(entrenador);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var entrenador = _context.Entrenadores.Find(id);
            ViewBag.Estudiantes = _context.Estudiantes.ToList();
            return View(entrenador);
        }

        [HttpPost]
        public IActionResult Edit(Entrenador entrenador)
        {
            if (!ModelState.IsValid)
            {
                return View(entrenador);
            }
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