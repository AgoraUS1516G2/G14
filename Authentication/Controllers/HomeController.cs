using Authentication.Models;
using Authentication.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Authentication.Controllers
{
    /// <summary>
    /// Este es el controlador principal de la aplicación, es el que se encarga de orquestar las peticiones http a la home del nuestro subsistema.
    /// Tiene los métodos: login, para logear y registrar usuarios.
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        UserRepository ur = new UserRepository();

        /// <summary>
        /// Función de logeo que devuelve la vista de logeo.
        /// </summary>
        /// <param name="returnurl"></param>
        /// <returns></returns>
        public ActionResult Login(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }
        /// <summary>
        /// Función de logeo post en el controlador.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnurl"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(UserLoginModel model, string returnurl)
        {
            ViewBag.returnUrl = returnurl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ur.ValidLogin(model.Username, UserRepository.GetSHA512(model.Password, model.Username)))
            {
                var user = ur.FindByUsername(model.Username);
                if (user.Confirmed)
                {
                    ModelState.AddModelError("", "Login correcto."); //Para testeo


                    if (String.IsNullOrEmpty(returnurl))
                        return View(model);
                    else
                    {

                        // Aquí generamos el token y el usuario para que otros subsistemas lo usen.

                        var uriBuilder = new UriBuilder(returnurl);
                        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                        query["token"] = ur.GetToken(model.Username, UserRepository.GetSHA512(model.Password, model.Username));
                        uriBuilder.Query = query.ToString();
                        returnurl = uriBuilder.ToString();


                        returnurl = returnurl.Replace("%3a", ":");

                        return Redirect(returnurl);
                    }
                }
                else
                {
                    sendConfirmationEmail(user);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                return View(model);
            }
        }
        /// <summary>
        /// Función de registro post en el controlador.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Registro correcto.");
                User user = new Models.User();

                user.Age = (int)model.Age;
                user.Autonomous_community = model.Autonomous_community;
                user.Email = model.Email;
                user.Genre = model.Genre;
                user.Is_admin = false;
                user.Password = UserRepository.GetSHA512(model.Password, model.Username);
                user.UserName = model.Username;
                user.Confirmed = false;
                ur.Add(user);
                sendConfirmationEmail(user);

                return RedirectToAction("login", "home");
            }
        }
        /// <summary>
        ///  Función get de la vista de registro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            UserRegisterModel model = new UserRegisterModel();


            return View(model);


        }

        private void sendConfirmationEmail(User user)
        {
            var provider = new MachineKeyProtectionProvider();
            UserManager<User, int> um = new UserManager<User, int>(ur);
            um.UserTokenProvider = new DataProtectorTokenProvider<User, int>(provider.Create("EmailConfirmation"));
            um.EmailService = new EmailService();
            TempData.Add("confirmEmail", "Le hemos enviado un correo electrónico para confirmar su cuenta, comprube la carpeta spam");

            if (Url != null)
            {
                var code = um.GenerateEmailConfirmationToken(user.U_id);
           
                var callbackUrl = Url.Action(
                "ConfirmEmail", "Home",
                new { userId = user.Id, code = code },
                protocol: "http");
                um.SendEmail(user.Id,
                           "Confirma tu correo",
                           "Por favor confirme su correo haciendo click en este <a href=\""
                                                           + callbackUrl + "\">link</a>");
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || code == null)
            {
                TempData.Add("confirmEmail", "No se ha podido confirmar el email");
                return View("Login");
            }
            IdentityResult result;
            try
            {
                var provider = new MachineKeyProtectionProvider();
                UserManager<User, int> um = new UserManager<User, int>(ur);
                um.UserTokenProvider = new DataProtectorTokenProvider<User, int>(provider.Create("EmailConfirmation"));
                result = await um.ConfirmEmailAsync(userId, code);
            }
            catch (ArgumentNullException)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                TempData.Add("confirmEmail", "Usuario no encontrado");
                return View("Login");
            }
            catch(ArgumentException)
            {
                TempData.Add("confirmEmail", "El email ya ha sido validado anteriormente");
                return View("Login");
            }

            if (result.Succeeded)
            {
                TempData.Add("confirmEmail", "Se ha confirmado su email correctamente");
                return View("Login");
            }

            TempData.Add("confirmEmail", result);
            return View("Login");
        }
    }
}
