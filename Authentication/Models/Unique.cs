using Authentication.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Authentication.Models
{
    /// <summary>
    /// Modelo que representa nuestra anotación personalizada unique, para que un campo de una fila de nuestra tabla de la base de datos sea única.
    /// </summary>
    public class Unique : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            UserRepository ur = new UserRepository();
            bool res = false;
            if (value is string)
            {
                string com = value.ToString();
                res = ur.Unique(com);
            }
            return res;
        }
    }
}