using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Serilog;
using System.Security.Claims;

namespace Miles.WebApp.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Tenta ler os dados do navegador
            var userSessionResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionResult.Success ? userSessionResult.Value : null;

            // Se não tiver sessão salva, retorna deslogado
            if (userSession == null)
            {
                return await Task.FromResult(new AuthenticationState(_currentUser));
            }

            // Se achou e leu com sucesso, recria o usuário
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
            // --- A CORREÇÃO ESTÁ AQUI ---
            // Se der erro ao ler (ex: chave de criptografia mudou), apagamos o dado corrompido
            // Isso permite que o usuário faça login novamente sem ficar travado
            Log.Error($"Erro ao recuperar sessão: {ex.Message}");
            await _sessionStorage.DeleteAsync("UserSession");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }
    }


    // Adicione este método na classe CustomAuthenticationStateProvider
    public async Task NotificarPerfilAtualizadoAsync(string novoNome)
    {
        // Recupera os dados atuais que não mudaram (ID e Email) das claims atuais
        var idStr = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = _currentUser.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(idStr) || string.IsNullOrEmpty(email) || !int.TryParse(idStr, out int userId))
        {
            return; // Segurança: Se não estiver logado corretamente, não faz nada
        }

        // 1. Cria o objeto de sessão atualizado
        var userSession = new UserSession
        {
            UserId = userId,
            Nome = novoNome,
            Email = email
        };

        // 2. Atualiza no SessionStorage (para persistir ao recarregar a página)
        await _sessionStorage.SetAsync("UserSession", userSession);

        // 3. Atualiza o estado em memória para a renderização atual
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, idStr),
            new Claim(ClaimTypes.Name, novoNome),
            new Claim(ClaimTypes.Email, email)
        };

        var identity = new ClaimsIdentity(claims, "CustomAuth");
        _currentUser = new ClaimsPrincipal(identity);

        // 4. Notifica os componentes (Header)
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    public async Task MarkUserAsAuthenticatedAsync(int userId, string nome, string email)
    {
        var userSession = new UserSession { UserId = userId, Nome = nome, Email = email };

        // Salva os dados no navegador
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

    public async Task MarkUserAsLoggedOutAsync()
    {
        try
        {
            // Tenta limpar o armazenamento persistente
            await _sessionStorage.DeleteAsync("UserSession");

            // Log de Auditoria (Útil para rastrear quem saiu e quando)
            Log.Information("Usuário realizou logout do sistema.");
        }
        catch (Exception ex)
        {
            // Se falhar ao limpar o disco, apenas logamos o aviso,
            // mas NÃO impedimos o fluxo de continuar para limpar a memória.
            Log.Warning($"Falha ao limpar sessão do navegador durante logout: {ex.Message}");
        }
        finally
        {
            // GARANTIA: Remove o usuário da memória independentemente de erros no storage
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

public class UserSession
{
    public int UserId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
