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
    public void Atualizar(Usuario usuario)
    {
        var local = _context.Usuarios.Local.FirstOrDefault(u => u.Id == usuario.Id);

        // Se encontrou, "esquece" a antiga para poder anexar a nova sem conflito
        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }
        _context.Usuarios.Update(usuario);
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
