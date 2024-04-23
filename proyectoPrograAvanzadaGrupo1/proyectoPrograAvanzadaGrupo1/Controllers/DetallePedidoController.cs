using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoPrograAvanzadaGrupo1.Models;
using System.Linq;

public class DetallePedidoController : Controller
{
    private readonly DatabaseContext _context;

    public DetallePedidoController(DatabaseContext context)
    {
        _context = context;
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

