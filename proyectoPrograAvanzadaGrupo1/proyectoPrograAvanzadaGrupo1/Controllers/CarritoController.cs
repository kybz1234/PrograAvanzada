using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoPrograAvanzadaGrupo1.Models;
using System.Linq;

namespace proyectoPrograAvanzadaGrupo1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly DatabaseContext _context;
        public CarritoController(DatabaseContext context)
        {
            _context = context;
        }
    }
}
