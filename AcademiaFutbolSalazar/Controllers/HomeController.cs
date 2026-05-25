using AcademiaFutbolSalazar.Data;
using AcademiaFutbolSalazar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AcademiaFutbolSalazar.Controllers
{
    public class HomeController : Controller
    {
        private readonly AcademiaContext _context;

        public HomeController(AcademiaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.TotalEntrenadores = _context.Entrenadores.Count();
            ViewBag.TotalEstudiantes = _context.Estudiantes.Count();
            ViewBag.TotalPagos = _context.Pagos.Where(p => p.Pagado == true).Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}