using Microsoft.EntityFrameworkCore;
using Miles.Core.Entities;
using Miles.Core.Interfaces;
using Miles.Infrastructure.Data;

namespace Miles.Infrastructure.Repositories;

/// Implementação do repositório de usuários usando EF Core (UC-01)
public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public Usuario? ObterPorEmail(string email)
    {
        return _context.Usuarios
            .AsNoTracking()
            .FirstOrDefault(u => u.Email == email);
    }

    public Usuario? ObterPorId(int id)
    {
        return _context.Usuarios
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == id);
    }
}
