using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Data;

namespace Miles.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Usuario usuario, CancellationToken ct = default)
    {
        await _context.Usuarios.AddAsync(usuario, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AtualizarAsync(Usuario usuario, CancellationToken ct = default)
    {
        // BLINDAGEM BLAZOR: Verifica se já existe na memória
        var local = _context.Usuarios.Local.FirstOrDefault(u => u.Id == usuario.Id);
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync(ct);

        // Limpeza
        _context.Entry(usuario).State = EntityState.Detached;
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Usuarios
            .AsNoTracking() // Login é leitura apenas, muito mais rápido assim
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }
}
