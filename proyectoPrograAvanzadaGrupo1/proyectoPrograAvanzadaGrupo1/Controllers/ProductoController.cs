using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoPrograAvanzadaGrupo1.Models;
using System.Linq;

public class ProductoController : Controller
{
    private readonly DatabaseContext _context;

    public ProductoController(DatabaseContext context)
    {
        _context = context;
    }

    public IActionResult ProductoDetail(int id)
    {
        var producto = _context.Productos.Find(id);

        if (TempData.ContainsKey("IsAdmin"))
        {
            ViewData["IsAdmin"] = TempData["IsAdmin"];
        }
        TempData["IsAdmin"] = TempData["IsAdmin"];

        if (producto == null)
        {
            return NotFound();
        }

        var randomProductos = _context.Productos
            .Where(p => p.producto_id != id && p.estado_id == 1) // Excluir el producto actual y solo activos
            .OrderBy(x => Guid.NewGuid())
            .Take(4)
            .ToList();
            

        ViewBag.RelatedProducts = randomProductos;

        return View(producto);
    }

}
