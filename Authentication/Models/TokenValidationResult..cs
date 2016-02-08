using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.Models
{
    /// <summary>
    /// Modelo que se devuelve cuando un token es válido
    /// </summary>
    public class TokenValidationResult
    {
        public bool valid { get; set; }
    }
}