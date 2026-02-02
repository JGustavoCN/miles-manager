using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Data;

namespace Miles.Infrastructure.Repositories;

public class ProgramaRepository : IProgramaRepository
{
    private readonly AppDbContext _context;

    public ProgramaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(ProgramaFidelidade programa, CancellationToken ct = default)
    {
        await _context.ProgramasFidelidade.AddAsync(programa, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AtualizarAsync(ProgramaFidelidade programa, CancellationToken ct = default)
    {
        // 1. Verifica se já existe um programa com este ID na memória local do EF
        var local = _context.ProgramasFidelidade.Local.FirstOrDefault(p => p.Id == programa.Id);

        if (local != null)
        {
            // 2. Se existir, desanexa para evitar o erro de "Same Key"
            _context.Entry(local).State = EntityState.Detached;
        }

        // 3. Atualiza
        _context.ProgramasFidelidade.Update(programa);
        await _context.SaveChangesAsync(ct);

        // 4. Limpeza (Boa prática para Blazor Server)
        _context.Entry(programa).State = EntityState.Detached;
    }

    public async Task RemoverAsync(int id, CancellationToken ct = default)
    {
        var programa = await _context.ProgramasFidelidade.FindAsync(new object[] { id }, ct);
        if (programa != null)
        {
            _context.ProgramasFidelidade.Remove(programa);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<ProgramaFidelidade?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.ProgramasFidelidade
            .AsNoTracking() // Performance: Leitura rápida sem tracking
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<ProgramaFidelidade>> ObterPorUsuarioAsync(int userId, CancellationToken ct = default)
    {
        return await _context.ProgramasFidelidade
            .AsNoTracking()
            .Where(p => p.UsuarioId == userId)
            .OrderBy(p => p.Nome)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistePeloNomeAsync(string nome, int usuarioId, CancellationToken ct = default)
    {
        // AnyAsync é otimizado: faz "SELECT TOP 1 1 FROM..."
        return await _context.ProgramasFidelidade
            .AnyAsync(p => p.UsuarioId == usuarioId && p.Nome == nome, ct);
    }

    public async Task<bool> PossuiCartoesVinculadosAsync(int id, CancellationToken ct = default)
    {
        return await _context.Cartoes
            .AnyAsync(c => c.ProgramaId == id, ct);
    }
}
