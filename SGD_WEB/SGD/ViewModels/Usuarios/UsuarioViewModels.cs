using SGD.Dominio.Entidade.Autenticacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SGD.ViewModels.Videos
{
    public class UsuarioViewModels
    {
        public string Nome { get; set; }
        public string Racf { get; set; }
        public string Equipe { get; set; }
        public string Email { get; set; }
        public int EquipeId { get; set; }
        public string Funcional { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string IsAtivo { get; set; }
        public HttpPostedFileBase Foto { get; set; }
        public DateTime DataRegistro { get; set; }

        public void LoadFromModel(Usuario usuario)
        {
            Nome = usuario.Nome;
            Racf = usuario.Racf;
            Equipe = usuario.Equipe.Nome;
            EquipeId = usuario.Equipe.Id;
            Funcional = usuario.Funcional;
            Email = usuario.Email;
            Login = usuario.Login;
            IsAtivo = (usuario.IsAtivo) != false ? "Ativo" : "Desativado";
            DataRegistro = usuario.DataRegistro;
        }
    }
}