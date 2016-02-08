using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Authentication.Models;
using Authentication.Repositories;
using System;

namespace Authentication.Controllers
{
    /// <summary>
    /// Este controlador, es el encargado de la api del subsistema, aquí se indican los 4 métodos que contiene nuestra api y se comenta cada funcionalidad de estos. El resultado es una documentación autogenerada en inglés.
    /// </summary>
    public class UsersController : ApiController
    {
        //private AuthContext db = new AuthContext();
        private UserRepository ur = new UserRepository();

        // GET: api/getUsers
        /// <summary>
        ///Obtiene todos los usuarios del sistema, incluyendo sus datos.
        /// </summary>
        /// <returns>JSON con un array con los datos de cada usuario. Los datos de cada usuario son los siguientes: "username", "password" y "email", "genre" y "autonomous_community".</returns>
        [Route("api/getUsers")]
        public IEnumerable<User> GetUsers()
        {
            return ur.GetUsers();
        }

        // GET: api/Users/username
        /// <summary>
        /// Devuelve un usuario
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [ResponseType(typeof(User))]
        [Route("api/getUser")]
        public IHttpActionResult GetUser(string username)
        {
            User user = ur.FindByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        // GET: api/checkToken
        /// <summary>
        /// Comprueba si un token es válido. Para ello, se obtiene el usuario correspondiente al token (indicado al comienzo del token), se genera el token del usuario y se comprueba si es igual que el pasado como parámetro.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>JSON con el campo "valid" indicando la validez del token (true/false).</returns>

        [Route("api/checkToken")]
        [HttpGet]
        [ResponseType(typeof(TokenValidationResult))]
        public IHttpActionResult checkToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Bad Request. This method doesn't exists or the necessary parameters weren't provided");
            }
            TokenValidationResult tokenresult = new TokenValidationResult();
            try
            {
                tokenresult.valid = ur.tokenIsCorrect(token);
            }
            catch (Exception)
            {
                tokenresult.valid = false;
            }
            return Ok(tokenresult);
        }


        /// <summary>
        /// Comprueba si un token es válido para un usuario. Para ello, se obtiene el usuario pasado como parámetro, se genera el token del usuario y se comprueba si es igual que el pasado como parámetro.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns>JSON con el campo "valid" indicando la validez del token (true/false).</returns>
        [Route("api/checkTokenUser")]
        [HttpGet]
        [ResponseType(typeof(TokenValidationResult))]
        public IHttpActionResult checkTokenUser(string token, string user)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Bad Request. This method doesn't exists or the necessary parameters weren't provided");
            }
            TokenValidationResult tokenresult = new TokenValidationResult();
            try
            { 
                tokenresult.valid = ur.CheckUserToken(token, user);
            }
            catch (Exception)
            {
                tokenresult.valid = false;
            }
            return Ok(tokenresult);
        }
    }
}