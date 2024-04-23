using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoPrograAvanzadaGrupo1.Models;
using System.Linq;

namespace proyectoPrograAvanzadaGrupo1.Controllers
{
    public class PedidoController : Controller
    {
        private readonly DatabaseContext _context;

        public PedidoController(DatabaseContext context)
        {
            _context = context;
        }

        // Acción para mostrar todos los pedidos
        public IActionResult Index()
        {
            // Obtener todos los pedidos de la base de datos incluyendo los detalles de usuario
            List<Pedido> pedidos = _context.Pedidos.Include(p => p.User).ToList();

            // Pasar la lista de pedidos a la vista
            return View(pedidos);
        }

        // Acción para mostrar los detalles de un pedido específico
        public IActionResult PedidoDetail(int id)
        {
            // Obtener el pedido con sus detalles asociados
            var pedido = _context.Pedidos
                .Include(p => p.User)
                .Include(p => p.DetallesPedido)
                .FirstOrDefault(p => p.orden_id == id);

            // Verificar si se encontró el pedido
            if (pedido == null)
            {
                return NotFound();
            }

            // Pasar el pedido y sus detalles a la vista
            return View(pedido);
        }
    }
}
