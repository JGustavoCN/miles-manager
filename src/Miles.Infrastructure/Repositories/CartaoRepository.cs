using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Data;

namespace Miles.Infrastructure.Repositories;

public class CartaoRepository : ICartaoRepository
{
    private readonly AppDbContext _context;

    public CartaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cartao>> ObterTodosAsync()
    {
        return await _context.Cartoes
            .AsNoTracking() // Já estava correto aqui
            .Include(c => c.Programa)
            .Include(c => c.Usuario)
            .ToListAsync();
    }

    // --- CORREÇÃO AQUI ---
    public async Task<Cartao?> ObterPorIdAsync(int id)
    {
        return await _context.Cartoes
            .AsNoTracking() // Adicionado AsNoTracking() para evitar conflito no Blazor
            .Include(c => c.Programa)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    // ---------------------

    public async Task<List<Cartao>> ObterPorUsuarioAsync(int userId)
    {
        return await _context.Cartoes
            .AsNoTracking() // Já estava correto aqui
            .Include(c => c.Programa)
            .Where(c => c.UsuarioId == userId)
            .ToListAsync();
    }

    public void Adicionar(Cartao cartao)
    {
        _context.Cartoes.Add(cartao);
        _context.SaveChanges();
    }

    public void Atualizar(Cartao cartao)
    {
        // Verifica se o EF já está rastreando uma entidade com este ID
        var local = _context.Cartoes
            .Local
            .FirstOrDefault(entry => entry.Id.Equals(cartao.Id));

        // Se encontrou, desanexa para permitir que o novo objeto seja anexado/atualizado
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Cartoes.Update(cartao);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        // O Find continua buscando localmente ou no banco, o que é correto para Exclusão
        var cartao = _context.Cartoes.Find(id);
        if (cartao != null)
        {
            _context.Cartoes.Remove(cartao);
            _context.SaveChanges();
        }
    }

    public async Task<bool> PossuiTransacoesAsync(int id)
    {
        return await _context.Set<Transacao>().AnyAsync(t => t.CartaoId == id);
    }
}
