using SGD.Dominio.Entidade.Video;
using SGD.Dominio.Entidade.Autenticacao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Dominio.Entidade.Autenticacao;

namespace SGD.Dominio
{
    public class AppContext : DbContext
    {
        public AppContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        // DbSets para suas entidades
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Equipe> equipes { get; set; }
        public DbSet<Video> videos { get; set; }
        public DbSet<Role> roles { get; set; }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries().Where(t => t.Entity.GetType().GetProperty("DataRegistro") != null))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("DataRegistro").CurrentValue = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    { 
                        entry.Property("DataRegistro").IsModified = false;
                        if (entry.Entity.GetType().GetProperty("DataModificacao") != null)
                        {
                            entry.Property("DataModificacao").CurrentValue = DateTime.Now;
                        }
                    }
                }
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var erros = ex.EntityValidationErrors;
                throw;
            }
            catch (Exception ex)
            { 
                var msg = ex.Message;
                throw;
            }
        }
    }
}



