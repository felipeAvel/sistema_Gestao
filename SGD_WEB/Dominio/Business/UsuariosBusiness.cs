using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Entidade.Gerais;
using SGD.Dominio.Entidade.Video;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Dominio.Business
{
    public class UsuariosBusiness
    {
        public async Task<IList<Usuario>> ListarUsuarios(int? equipeId = null, bool? isAtivo = null)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.usuarios.AsNoTracking()
                        .ToListAsync();
            }
        }

        public async Task<Tuple<IList<Usuario>, int>> ListarUsuarios(Paginacao paginacao)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.usuarios
                    .Include(u => u.Equipe)
                    .OrderBy(t => t.Nome == paginacao.Ordenacao)
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(paginacao.TermoBusca))
                {
                    query = query.Where(t =>
                        t.Nome.Contains(paginacao.TermoBusca));
                }

                var total = await query.CountAsync();
                var itens = await query.Skip((paginacao.PaginaAtual - 1) * paginacao.TamanhoPagina)
                    .Take(paginacao.TamanhoPagina)
                    .ToListAsync();

                return new Tuple<IList<Usuario>, int>(itens, total);
            }
        }

        public async Task<bool> InserirUsuario(Usuario usuario)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.usuarios.Add(usuario);

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> ObterUsuario(Usuario usuario)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.usuarios
                    .Where(u => u.Nome == usuario.Nome ||
                                u.Login == usuario.Login ||
                                u.Racf == usuario.Racf ||
                                u.Funcional == usuario.Funcional)
                    .FirstOrDefault();

                if (query != null)
                    return true;
                else
                    return false;
            }
        }

        public async Task<Usuario> ObterUsuario(string login)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.usuarios
                    .Where(u => u.Login == login)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
