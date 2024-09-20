using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Business
{
    public class LoginBusiness
    {
        public async Task<Usuario> Login(string login, string senha)
        {
            byte[] textoEmBytes = Encoding.UTF8.GetBytes(senha);

            // Converte os bytes para Base64
            string textoBase64 = Convert.ToBase64String(textoEmBytes);

            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.usuarios.AsNoTracking()
                            .Include(r => r.Roles);

                query = query.Where(u => u.Login == login && u.PassWord == textoBase64);

                return await query.FirstOrDefaultAsync();
            }
        }
    }
}