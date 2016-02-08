using Authentication.Infraestructure;
using Authentication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace Authentication.Repositories
{
    /// <summary>
    /// Esta clase representa el repositorio, aquí están implementados los metodos de guardar, editar, borrar usuarios. A su vez la lógica de la aplicación está implementada en esta clase.
    /// </summary>
    public class UserRepository : IDisposable, IUserStore<User, int>, IUserEmailStore<User, int>
    {
        AuthContext context = new AuthContext();
        public bool disposed = false;
        /// <summary>
        /// Añadir un usuario al sistema.
        /// </summary>
        /// <param name="b"></param>
        public void Add(User b)
        {
            context.Users.Add(b);
            context.SaveChanges();
        }
        /// <summary>
        /// Editar un usuario del sistema.
        /// </summary>
        /// <param name="b"></param>
        public void Edit(User b)
        {
            context.Entry(b).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
        /// <summary>
        /// Borrar un usuario del sistema.
        /// </summary>
        /// <param name="b"></param>
        public void Remove(User b)
        {
            context.Users.Remove(b);
            context.SaveChanges();
        }
        /// <summary>
        /// Obtener todos los usuarios del sistema.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            return context.Users;
        }
        /// <summary>
        /// Buscar un usuario según su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User FindById(int id)
        {
            /*var c = (from r in context.Users where r.U_id == Id select r).FirstOrDefault();*/
            var c = context.Users.Find(id);
            return c;
        }
        public User FindByUsername(string username)
        {
            /*var c = (from r in context.Users where r.U_id == Id select r).FirstOrDefault();*/
            var c = (from r in context.Users where r.UserName == username select r).FirstOrDefault();
            return c;
        }
        /// <summary>
        /// Comprobar si un usuario existe según su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UserExists(int id)
        {
            return context.Users.Count(e => e.U_id == id) > 0;
        }
        /// <summary>
        /// Esta función comprueba si el token recibido es el asignado a un usuario.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="username"></param>
        /// <returns>Un bool si el token es válido</returns>
        public bool CheckUserToken(string token, string username)
        {
            var dbUser = FindByUsername(username);
            //GetSHA512(dbUser.username + dbUser.password, dbUser.username) == token)

            return (string.IsNullOrEmpty(token) == false && GetToken(dbUser.UserName, dbUser.Password) == username + ":" + token);
        }
        /// <summary>
        /// Esta función genera el token que se usará en otros subsistemas.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="hashedpassword"></param>
        /// <returns></returns>
        public string GetToken(string user, string hashedpassword)
        {

            return user + ':' + GetSHA512(user + hashedpassword, user);
        }

        /// <summary>
        /// Esta método comprueba si el usuario introducido en el lógin es válido
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidLogin(string username, string password)
        {
            var c = (from r in context.Users where r.UserName == username && r.Password == password select r).FirstOrDefault();
            return c != null ? true : false;
        }
        /// <summary>
        /// Esta función es la encargada de comprobar si un token es válido
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Un boolean en el cuál se indica si un token es válido o no</returns>
        public bool tokenIsCorrect(string token)
        {
            String[] split = token.Split(':');
            token = split[1].Trim();
            var username = split[0].Trim();

            var result = CheckUserToken(token, username);

            return result;
        }
        /// <summary>
        /// Comprueba si el usuario es único
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Unique(string data)
        {
            var c = (from r in context.Users where r.UserName == data || r.Email == data select r).FirstOrDefault();
            return c != null ? false : true;
        }
        /// <summary>
        /// Genera el hash de la familia SHA2, SHA512 de la password en todo proceso de registro o logeo. El parámetro salt se suele dejar vacio, pues no se usa sal en esta aplicación.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetSHA512(string password, string salt)
        {
            System.Security.Cryptography.SHA512Managed HashTool = new System.Security.Cryptography.SHA512Managed();
            Byte[] PasswordAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(password, salt));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PasswordAsByte);
            HashTool.Clear();
            return BitConverter.ToString(EncryptedBytes);
        }
        /// <summary>
        /// Genera el MD5 de la string recibida, se mantiene en el código pues es parte del código original heredado, pero se ha eliminado su uso del sistema por ser inseguro.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            Edit(user);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(int userid)
        {
            return Task.FromResult<User>(FindById(userid));
        }

        public Task<User> FindByNameAsync(string username)
        {
            return Task.FromResult<User>(FindByUsername(username));
        }

        public Task SetEmailAsync(User user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult<string>(FindByUsername(user.UserName).Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if(user.Confirmed)
            {
                throw new ArgumentException("ya validado");
            }
            user.Confirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}