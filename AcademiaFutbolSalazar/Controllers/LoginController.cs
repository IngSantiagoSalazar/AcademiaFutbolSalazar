using Microsoft.AspNetCore.Mvc;
using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Models;

namespace AcademiaFutbolSalazar.Controllers
{
    public class LoginController : Controller
    {
        private readonly AcademiaContext _context;

        public LoginController(AcademiaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string nombre, string celular)
        {
            var entrenador = _context.Entrenadores
                .FirstOrDefault(e => e.Nombre == nombre && e.Celular == celular);

            if (entrenador != null)
            {
                HttpContext.Session.SetString("Entrenador", entrenador.Nombre);
                HttpContext.Session.SetString("Especialidad", entrenador.Especialidad);
                return RedirectToAction("Index", "Estudiante");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}