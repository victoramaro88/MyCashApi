using Microsoft.Owin.Security.OAuth;
using MyCashApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MyCashApi
{
    public class ProviderDeTokensDeAcesso : OAuthAuthorizationServerProvider
    {
        private readonly UsuarioRepository _usuarioRepo = new UsuarioRepository();

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // ->Realiza a conexão com o banco de dados para validar um usuário.
            var usuario = _usuarioRepo.AutenticarUsuario(context.UserName, context.Password);

            //var usuario = BaseUsuarios.Usuarios().FirstOrDefault(x => x.Nome == context.UserName && x.Senha == context.Password);

            if (usuario == null)
            {
                context.SetError("invalid_grant", "Usuário não encontrado ou senha incorreta.");
                return;
            }

            var identidadeUsuario = new ClaimsIdentity(context.Options.AuthenticationType);
            context.Validated(identidadeUsuario);
        }
    }
}