using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Miles.WebApp.Auth;

/// Provedor de estado de autenticação customizado para Blazor Server (UC-01)
/// Gerencia a sessão do usuário autenticado
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    /// Marca o usuário como autenticado e armazena suas informações
    public void MarkUserAsAuthenticated(int userId, string nome, string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, nome),
            new Claim(ClaimTypes.Email, email)
        };

        var identity = new ClaimsIdentity(claims, "CustomAuth");
        _currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    /// Marca o usuário como deslogado
    public void MarkUserAsLoggedOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
}
