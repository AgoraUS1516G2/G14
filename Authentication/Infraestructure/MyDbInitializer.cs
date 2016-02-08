using Authentication.Models;
using Authentication.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Authentication.Infraestructure
{

    /// <summary>
    /// En esta clase, se generan los datos de prueba de nuestra api.
    /// </summary>
    public class MyDbInitializer : CreateDatabaseIfNotExists<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {
            // hash md5(test) = f6f4061a1bddc1c04d8109b39f581270
            context.Users.Add(new User
            { UserName = "test0", Password = UserRepository.GetSHA512("test0", "test0"), Email = "jorgeron1993@hotmail.com", Genre = "Masculino", Autonomous_community = "Andalucia", Age = 21, Is_admin = true, Confirmed = true });
            context.Users.Add(new User
            { UserName = "test1", Password = UserRepository.GetSHA512("test1", "test1"), Email = "jorgeron1993@hotmail.com", Genre = "Masculino", Autonomous_community = "Andalucia", Age = 21, Is_admin = true, Confirmed = true });
            context.Users.Add(new User
            { UserName = "test2", Password = UserRepository.GetSHA512("test2", "test2"), Email = "jorgeron1993@hotmail.com", Genre = "Masculino", Autonomous_community = "Andalucia", Age = 21, Is_admin = false, Confirmed = true });

            base.Seed(context);
        }
    }
}