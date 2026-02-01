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

    public void Adicionar(Transacao transacao)
    {
        _context.Transacoes.Add(transacao);
        _context.SaveChanges();
    }

    public List<Transacao> ObterExtrato(int cartaoId)
    {
        return _context.Transacoes
            .AsNoTracking()
            .Where(t => t.CartaoId == cartaoId)
            .OrderByDescending(t => t.Data)
            .ToList();
    }

    public List<Transacao> ObterPorUsuario(int userId)
    {
        // Inclui o Cartão para filtrar pelo ID do Usuário dono do cartão
        return _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Cartao)
            .Where(t => t.Cartao.UsuarioId == userId)
            .OrderByDescending(t => t.Data)
            .ToList();
    }
}
