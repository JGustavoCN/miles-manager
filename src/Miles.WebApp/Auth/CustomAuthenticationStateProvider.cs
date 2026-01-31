using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Miles.WebApp.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    // 1. Injetamos o Storage do navegador no construtor
    public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    // 2. Ao iniciar, tentamos ler do navegador para recuperar a sessão
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Tenta ler os dados salvos
            var userSessionResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionResult.Success ? userSessionResult.Value : null;

            if (userSession == null)
            {
                return await Task.FromResult(new AuthenticationState(_currentUser));
            }

            // Se achou, recria o usuário logado
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userSession.UserId.ToString()),
                new Claim(ClaimTypes.Name, userSession.Nome),
                new Claim(ClaimTypes.Email, userSession.Email)
            };

            var identity = new ClaimsIdentity(claims, "CustomAuth");
            _currentUser = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(_currentUser));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao recuperar sessão: {ex.Message}");

            // Se deu erro (ex: chave mudou), apaga o dado inválido para permitir novo login limpo
            await _sessionStorage.DeleteAsync("UserSession");
            return await Task.FromResult(new AuthenticationState(_currentUser));
        }
    }

    // 3. Login agora é ASSÍNCRONO e salva no navegador
    public async Task MarkUserAsAuthenticatedAsync(int userId, string nome, string email)
    {
        var userSession = new UserSession { UserId = userId, Nome = nome, Email = email };

        // Salva de verdade!
        await _sessionStorage.SetAsync("UserSession", userSession);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, nome),
            new Claim(ClaimTypes.Email, email)
        };

        var identity = new ClaimsIdentity(claims, "CustomAuth");
        _currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // 4. Logout apaga do navegador
    public async Task MarkUserAsLoggedOutAsync()
    {
        await _sessionStorage.DeleteAsync("UserSession");

        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

// Classe simples para guardar os dados
public class UserSession
{
    public int UserId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
