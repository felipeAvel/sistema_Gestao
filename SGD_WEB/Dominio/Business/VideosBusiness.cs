using SGD.Dominio.Entidade.Video;
using SGD.Dominio.Entidade.Autenticacao;
using SGD.Dominio.Factories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SGD.Dominio.Entidade.Gerais;

namespace Dominio.Business
{
    public class VideosBusiness
    {
        public async Task<Video> ObterVideo(int id)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.videos.AsNoTracking()
                        .FirstOrDefaultAsync(v => v.Id == id);
            }
        }

        public async Task<Tuple<IList<Video>, int>> ListarVideos(Paginacao paginacao, string categoria)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                var query = ctx.videos
                    .OrderBy(t => t.Nome == paginacao.Ordenacao)
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(paginacao.TermoBusca))
                {
                    query = query.Where(t =>
                        t.Categoria.Contains(paginacao.TermoBusca));
                }

                if (!string.IsNullOrEmpty(categoria))
                {
                    query = query.Where(t =>
                        t.Categoria.Contains(categoria));
                }

                var total = await query.CountAsync();
                var itens = await query.Skip((paginacao.PaginaAtual - 1) * paginacao.TamanhoPagina)
                    .Take(paginacao.TamanhoPagina)
                    .ToListAsync();

                return new Tuple<IList<Video>, int>(itens, total);
            }
        }

        public async Task<IList<Video>> ListarVideos(string categoria)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                return await ctx.videos.Where(v => v.Categoria
                                        .Contains(categoria))
                                        .Take(20)
                                        .ToListAsync();
            }
        }

        public async Task<bool> InserirVideo(Video video)
        {
            var contextFactory = new DbConnectionFactory();

            using (var ctx = contextFactory.CreateDbContext())
            {
                ctx.videos.Add(video);

                return await ctx.SaveChangesAsync() > 0;
            }
        }
    }
}