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
            .Include(c => c.Programa)
            .Include(c => c.Usuario)
            .ToListAsync();
    }

    public void Adicionar(Cartao cartao)
    {
        _context.Cartoes.Add(cartao);
        _context.SaveChanges();
    }

    public List<Cartao> ObterPorUsuario(int userId)
    {
        return _context.Cartoes
            .Include(c => c.Programa)
            .Where(c => c.UsuarioId == userId)
            .ToList();
    }

    public Cartao? ObterPorId(int id)
    {
        return _context.Cartoes
            .Include(c => c.Programa)
            .Include(c => c.Usuario)
            .FirstOrDefault(c => c.Id == id);
    }

    public void Atualizar(Cartao cartao)
    {
        _context.Cartoes.Update(cartao);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var cartao = _context.Cartoes.Find(id);
        if (cartao != null)
        {
            _context.Cartoes.Remove(cartao);
            _context.SaveChanges();
        }
    }
}
