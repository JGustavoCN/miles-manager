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

    public void Adicionar(ProgramaFidelidade programa)
    {
        _context.ProgramasFidelidade.Add(programa);
        _context.SaveChanges();
    }

    public void Atualizar(ProgramaFidelidade programa)
    {
        _context.ProgramasFidelidade.Update(programa);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var programa = _context.ProgramasFidelidade.Find(id);
        if (programa != null)
        {
            _context.ProgramasFidelidade.Remove(programa);
            _context.SaveChanges();
        }
    }

    public ProgramaFidelidade? ObterPorId(int id)
    {
        return _context.ProgramasFidelidade
            .Include(p => p.Usuario)
            .FirstOrDefault(p => p.Id == id);
    }

    public List<ProgramaFidelidade> ObterPorUsuario(int userId)
    {
        return _context.ProgramasFidelidade
            .Where(p => p.UsuarioId == userId)
            .OrderBy(p => p.Nome) // Ordenação alfabética facilita a visualização na lista
            .ToList();
    }

    public bool ExistePeloNome(string nome, int usuarioId)
    {
        // Verifica duplicidade apenas dentro dos programas do usuário (Multi-tenant)
        return _context.ProgramasFidelidade
            .Any(p => p.UsuarioId == usuarioId && p.Nome == nome);
    }

    public bool PossuiCartoesVinculados(int id)
    {
        // Verifica integridade referencial para o FE-02
        return _context.Cartoes.Any(c => c.ProgramaId == id);
    }
}
