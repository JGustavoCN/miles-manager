using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Data;

namespace Miles.Infrastructure.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Transacao transacao, CancellationToken ct = default)
    {
        await _context.Transacoes.AddAsync(transacao, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Transacao?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Cartao) // Importante para exibir o nome do cartão na edição
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<IEnumerable<Transacao>> ObterPorUsuarioAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Cartao)
            .Where(t => t.Cartao.UsuarioId == userId)
            .OrderByDescending(t => t.Data)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Transacao>> ObterExtratoAsync(int cartaoId, CancellationToken ct = default)
    {
        return await _context.Transacoes
            .AsNoTracking()
            .Where(t => t.CartaoId == cartaoId)
            .OrderByDescending(t => t.Data)
            .ToListAsync(ct);
    }

    public async Task AtualizarAsync(Transacao transacao, CancellationToken ct = default)
    {
        // Proteção contra Tracking Duplicado no Blazor
        var local = _context.Transacoes.Local.FirstOrDefault(t => t.Id == transacao.Id);
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Transacoes.Update(transacao);
        await _context.SaveChangesAsync(ct);

        // Limpeza pós-update
        _context.Entry(transacao).State = EntityState.Detached;
    }

    public async Task RemoverAsync(int id, CancellationToken ct = default)
    {
        var transacao = await _context.Transacoes.FindAsync(new object[] { id }, ct);
        if (transacao != null)
        {
            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync(ct);
        }
    }
}
