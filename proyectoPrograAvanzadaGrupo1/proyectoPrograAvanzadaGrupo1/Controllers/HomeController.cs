using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using proyectoPrograAvanzadaGrupo1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace proyectoPrograAvanzadaGrupo1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

       // private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
       // {
        //    _logger = logger;
        //}

        public IActionResult Index()
        {

            var username = HttpContext.User.Identity.Name;
            

            ViewData["Username"] = username;

            List<Producto> Productos = _context.Productos.ToList();
            return View(Productos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Ofertas()
        {

            var username = HttpContext.User.Identity.Name;
            ViewData["Username"] = username;

            List<Producto> Productos = _context.Productos.ToList();
            return View(Productos);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
