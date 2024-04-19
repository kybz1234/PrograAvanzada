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

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }
}
