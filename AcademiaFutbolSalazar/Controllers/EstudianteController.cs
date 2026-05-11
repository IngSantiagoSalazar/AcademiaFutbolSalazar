using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Models;

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
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Entrenadores = _context.Entrenadores.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Entrenadores = _context.Entrenadores.ToList(); 
                return View(estudiante);
            }
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
        public IActionResult Edit(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Entrenadores = _context.Entrenadores.ToList(); 
                return View(estudiante);
            }
            _context.Estudiantes.Update(estudiante);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var estudiante = _context.Estudiantes.Find(id);
            _context.Estudiantes.Remove(estudiante);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}