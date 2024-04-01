using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using proyectoPrograAvanzadaGrupo1.Models;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace proyectoPrograAvanzadaGrupo1.Controllers
{
    public class AccesoController : Controller

    {

        //conexion a la base de datos
        static string chain = "Data Source=DESKTOP-3P5MFNR;Initial Catalog=PrograAvanzadaGrupo1;Integrated Security=true; TrustServerCertificate=True";


        //iniciar sesion

        public ActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(User u)
        {
            try
            {
                string contraseñaHash = HashPassword(u.contraseña_hash);

                using (SqlConnection cn = new SqlConnection(chain))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@nombre_usuario", SqlDbType.VarChar).Value = u.usuario;
                        cmd.Parameters.Add("@contraseña_hash", SqlDbType.VarChar).Value = contraseñaHash;
                        cn.Open();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            if (dr["usuario"] != null && u.usuario != null)
                            {
                                List<Claim> c = new List<Claim>()
                                {
                                    new Claim (ClaimTypes.Name, u.usuario)
                                };

                                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties p = new();

                                p.AllowRefresh = true;
                                p.IsPersistent = u.MantenerActivo;

                                if (!u.MantenerActivo)
                                {
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);
                                }
                                else
                                {
                                    p.ExpiresUtc = DateTimeOffset.MaxValue;
                                }
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.Error = "Credenciales incorrectas || la Cuenta no esta registrada";
                            }
                        }
                        cn.Close();
                    }

                    return View();
                }
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }


        //registro

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User oUser)
        {
            try
            {
                
                if (oUser.usuario != null && oUser.contraseña_hash != null)
                {

                    string contraseñaHash = HashPassword(oUser.contraseña_hash);

                    using (SqlConnection cn = new SqlConnection(chain))
                  {
                      using (SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn))
                      {
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.Add("@nombre_usuario", SqlDbType.VarChar).Value = oUser.usuario;
                         cmd.Parameters.Add("@contraseña_hash", SqlDbType.VarChar).Value = contraseñaHash;
                         cmd.Parameters.Add("@estado_nombre", SqlDbType.VarChar).Value = "Activo";

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

            return RedirectToAction("Login", "Acceso");

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
