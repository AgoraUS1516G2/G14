using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication.Models
{
    /// <summary>
    /// Modelo que representa una comunidad autónoma, junto con su respectiva función de validación.
    /// </summary>
    public class AutonomousCommunity : ValidationAttribute
    {
        private static List<String> AutonomusCommunity = new List<string> { "Andalucía", "Murcia", "Extremadura", "Castilla la Mancha", "Comunidad Valenciana", "Madrid", "Castilla y León", "Aragón", "Cataluña", "La Rioja", "Galicia", "Asturias", "Cantabria", "País Vasco", "Navarra", "Canarias", "Islas Baleares", "Ceuta", "Melilla" };

        public override bool IsValid(object value)
        {
            bool res = false;
            if (value is string)
            {
                string com = value.ToString();
                if (AutonomusCommunity.Contains(com, StringComparer.OrdinalIgnoreCase))
                    res = true;
            }
            return res;
        }
    }
}