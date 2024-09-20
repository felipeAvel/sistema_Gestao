using Dominio.Business;
using SGD.Dominio.Entidade.Gerais;
using PagedList;
using SGD.Dominio.Business;
using SGD.Dominio.Entidade.Video;
using SGD.ViewModels.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using SGD.Dominio.Entidade.Autenticacao;
using System.IO;
using System.Text;

namespace SGD.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuariosBusiness usuariosBusiness;
        private readonly EquipesBusiness equipesBusiness;

        public UsuarioController()
        {
            this.usuariosBusiness = new UsuariosBusiness();
            this.equipesBusiness = new EquipesBusiness();
        }

        [HttpGet]
        public async Task<ActionResult> Index(Paginacao paginacao, string categoria)
        {
            var termo = paginacao.TermoBusca;
            var usuarios = await usuariosBusiness.ListarUsuarios(paginacao);

            ViewBag.ListaUsuarios = new StaticPagedList<Usuario>(
                usuarios.Item1, paginacao.PaginaAtual,
                paginacao.TamanhoPagina, usuarios.Item2);

            return View(paginacao);
        }

        [HttpGet]
        public async Task<ActionResult> InserirUsuario()
        {
            await PreencherDados();

            return View();
        }

        private async Task PreencherDados(int? equipeId = null)
        {
            var equipes = await equipesBusiness.ListarEquipes();

            ViewBag.Equipes = new SelectList(equipes, "Id", "Nome", equipeId);

            ViewBag.ItemSelect = equipeId;
        }

        [HttpPost]
        public async Task<JsonResult> InserirUsuario(UsuarioViewModels vm)
        {

            var binaryReader = new BinaryReader(vm.Foto.InputStream);

            byte[] bytesSenha = Encoding.UTF8.GetBytes(vm.Senha);
            byte[] fileBytes = binaryReader.ReadBytes(vm.Foto.ContentLength);
            string base64String = Convert.ToBase64String(fileBytes);

            var usuario = new Usuario();

            usuario.Nome = vm.Nome;
            usuario.Login = vm.Login;
            usuario.Racf = vm.Racf;
            usuario.Funcional = vm.Funcional;
            usuario.Email = vm.Email;
            usuario.EquipeId = vm.EquipeId;
            usuario.Login = vm.Login;
            usuario.Foto = base64String;
            usuario.PassWord = Convert.ToBase64String(bytesSenha);
            usuario.IsAtivo = true;

            //Valida se o User ja existe
            if (await usuariosBusiness.ObterUsuario(usuario))
                return Json("Usuário ja Existente!", JsonRequestBehavior.AllowGet);
            else
            {
                if (await usuariosBusiness.InserirUsuario(usuario))
                    return Json("Sucesso", JsonRequestBehavior.AllowGet);
                else
                    return Json("Erro ao Inserir Usuário!", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> BuscarFoto()
        {
            var dados = await usuariosBusiness.ObterUsuario(User.Identity.Name);
            return Json(dados.Foto, JsonRequestBehavior.AllowGet);
        }
    }
}