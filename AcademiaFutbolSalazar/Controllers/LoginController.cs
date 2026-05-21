using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Helpers;
using AcademiaFutbolSalazar.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index(string nombre, string clave)
        {
            string claveHash = HashHelper.obtenerHash(clave);

            var entrenador = _context.Entrenadores.ToList()
                .FirstOrDefault(e => e.Nombre.Trim() == nombre.Trim() && e.clave == claveHash);

            if (entrenador != null)
            {
                HttpContext.Session.SetString("Usuario", entrenador.Nombre);
                HttpContext.Session.SetString("Rol", "Entrenador");
                HttpContext.Session.SetString("Especialidad", entrenador.Especialidad ?? ""); // ✅
                return RedirectToAction("Index", "Home");
            }

            var estudiante = _context.Estudiantes.ToList()
                .FirstOrDefault(e => e.Nombre.Trim() == nombre.Trim() && e.clave == claveHash);

            if (estudiante != null)
            {
                HttpContext.Session.SetString("Usuario", estudiante.Nombre);
                HttpContext.Session.SetString("Rol", "Estudiante");
                return RedirectToAction("Index", "Home");
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