using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using proyectoPrograAvanzadaGrupo1.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace proyectoPrograAvanzadaGrupo1.Controllers
{
    [Authorize]
    public class AdminController : Controller

    {

        //conexion a la base de datos por medio del context
        private readonly DatabaseContext _context;
        public AdminController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;

            return View();
        }

        //Acciones de Agregar

        public IActionResult AgregarProducto()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;

            return View();
        }


        [HttpPost]
        public ActionResult AgregarProducto(Producto oProducto)
        {
            try
            {
                byte[] bytes;
                if (oProducto.File != null && oProducto.nombre_producto != null)
                {




                    using (Stream fs = oProducto.File.OpenReadStream())
                    {
                        using (BinaryReader br = new(fs))
                        {
                            bytes = br.ReadBytes((int)fs.Length);
                            oProducto.foto = Convert.ToBase64String(bytes, 0, bytes.Length);

                            using (SqlConnection cn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                            {
                                using (SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", cn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@nombre_producto", SqlDbType.VarChar).Value = oProducto.nombre_producto;
                                    cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = oProducto.precio;
                                    cmd.Parameters.Add("@fecha_salida", SqlDbType.DateTime).Value = oProducto.fecha_salida;
                                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = oProducto.cantidad;
                                    cmd.Parameters.Add("@video_url", SqlDbType.VarChar).Value = oProducto.video_url;
                                    cmd.Parameters.Add("@estado_id", SqlDbType.Int).Value = oProducto.estado_id;
                                    cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = oProducto.descripcion;
                                    cmd.Parameters.Add("@foto", SqlDbType.VarChar).Value = oProducto.foto;
                                    cmd.Parameters.Add("@precio_descuento", SqlDbType.Decimal).Value = oProducto.precio_descuento;
                                    cmd.Parameters.Add("@en_descuento", SqlDbType.Bit).Value = oProducto.en_descuento;


                                    cn.Open();
                                    cmd.ExecuteNonQuery();
                                    cn.Close();
                                }
                            }
                        }
                    }

                }

            }
            catch (System.Exception e)
            {

                ViewBag.error = e.Message;
                return View();

            }

            return RedirectToAction("Home", "Admin");

        }

        public IActionResult AgregarUsuario()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;

            return View();
        }

        [HttpPost]
        public IActionResult AgregarUsuario(User oUser)
        {

            try
            {

                if (oUser.usuario != null && oUser.contraseña_hash != null)
                {

                    string contraseñaHash = HashPassword(oUser.contraseña_hash);

                    using (SqlConnection cn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_RegistrarUsuarioAdmin", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@nombre_usuario", SqlDbType.VarChar).Value = oUser.usuario;
                            cmd.Parameters.Add("@contraseña_hash", SqlDbType.VarChar).Value = contraseñaHash;
                            cmd.Parameters.Add("@estado_id", SqlDbType.VarChar).Value = oUser.estado_id;

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();
                        }
                    }

                }

            }
            catch (System.Exception e)
            {

                ViewBag.error = e.Message;
                return View();

            }

            return RedirectToAction("Home", "Admin");
        }

        // Acciones de Mostrar Tabla

        public IActionResult ProductoTable()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;


            List<Producto> Productos = _context.Productos.ToList();
            return View(Productos);


        }

        public IActionResult UsuariosTable()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;


            List<User> Usuarios = _context.Usuarios.ToList();
            return View(Usuarios);


        }


        // Acciones de Eliminar

        public IActionResult EliminarUsuario(int user_id)
        {

            User usuarioAEliminar = _context.Usuarios.Find(user_id);
            if (usuarioAEliminar != null)
            {

                _context.Usuarios.Remove(usuarioAEliminar);
                _context.SaveChanges();

            }
            

            return RedirectToAction("UsuariosTable", "Admin");
        }

        public IActionResult EliminarProducto(int producto_id)
        {

            Producto productoAEliminar = _context.Productos.Find(producto_id);
            if (productoAEliminar != null)
            {

                _context.Productos.Remove(productoAEliminar);
                _context.SaveChanges();

            }


            return RedirectToAction("ProductoTable", "Admin");
        }

        //Acciones de Editar

        public IActionResult EditarUsuario()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;

            return View();
        }

        [HttpGet]
        public IActionResult EditarUsuario(int user_id)
        {
            User usuarioAEditar = _context.Usuarios.Find(user_id);
            return View();

        }

        [HttpPost]
        public IActionResult EditarUsuario(User ModifiedData)
        {

            string contraseñaHash = HashPassword(ModifiedData.contraseña_hash);
            ModifiedData.contraseña_hash = contraseñaHash;

            _context.Update(ModifiedData);
            _context.SaveChanges();
            return RedirectToAction("UsuariosTable", "Admin");

        }

        public IActionResult EditarProducto()
        {

            var username = HttpContext.User.Identity.Name;

            ViewData["Username"] = username;

            return View();
        }

        [HttpGet]
        public IActionResult EditarProducto(int producto_id)
        {
            Producto productoAEditar = _context.Productos.Find(producto_id);
            return View();

        }

        [HttpPost]
        public IActionResult EditarProducto(Producto ModifiedData)
        {
            byte[] bytes;
            using (Stream fs = ModifiedData.File.OpenReadStream())
            {
                using (BinaryReader br = new(fs))
                {
                    bytes = br.ReadBytes((int)fs.Length);
                    ModifiedData.foto = Convert.ToBase64String(bytes, 0, bytes.Length);



                    _context.Update(ModifiedData);
                    _context.SaveChanges();
                    return RedirectToAction("ProductoTable", "Admin");

                }

            }

        }


        //encriptar el password

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        


    }
    
}
