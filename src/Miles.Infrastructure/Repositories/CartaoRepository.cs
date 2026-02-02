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

    public async Task<IEnumerable<Cartao>> ObterTodosAsync(CancellationToken ct = default)
    {
        return await _context.Cartoes
            .AsNoTracking() // Performance: Não rastreia na memória
            .Include(c => c.Programa)
            .Include(c => c.Usuario)
            .OrderBy(c => c.Nome) // Boa prática: Ordenar no banco
            .ToListAsync(ct);
    }

    public async Task<Cartao?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        // Nota: Aqui NÃO usamos AsNoTracking se a intenção for editar logo em seguida na mesma requisição.
        // Mas como no Blazor a edição acontece em outro "tempo" (outro postback),
        // usamos AsNoTracking para economizar memória e o Update trata de reanexar.
        return await _context.Cartoes
            .AsNoTracking()
            .Include(c => c.Programa)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<IEnumerable<Cartao>> ObterPorUsuarioAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Cartoes
            .AsNoTracking()
            .Include(c => c.Programa)
            .Where(c => c.UsuarioId == userId)
            .ToListAsync(ct);
    }

    public async Task AdicionarAsync(Cartao cartao, CancellationToken ct = default)
    {
        await _context.Cartoes.AddAsync(cartao, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AtualizarAsync(Cartao cartao, CancellationToken ct = default)
    {
        // 1. Verifica se o EF Core já está rastreando um cartão com este ID na memória local
        var local = _context.Cartoes.Local.FirstOrDefault(c => c.Id == cartao.Id);

        if (local != null)
        {
            // 2. Se encontrou, "desanexa" (Detach) o objeto antigo da memória
            // Isso evita o erro "The instance of entity type... cannot be tracked"
            _context.Entry(local).State = EntityState.Detached;
        }

        // 3. Agora podemos anexar e atualizar o novo objeto sem conflitos
        _context.Cartoes.Update(cartao);
        await _context.SaveChangesAsync(ct);

        // 4. (Opcional mas recomendado para Blazor) Desanexa o objeto que acabamos de salvar
        // para deixar o contexto limpo para a próxima operação.
        _context.Entry(cartao).State = EntityState.Detached;
    }

    public async Task RemoverAsync(int id, CancellationToken ct = default)
    {
        // Busca apenas pelo ID (FindAsync é otimizado para chave primária)
        var cartao = await _context.Cartoes.FindAsync(new object[] { id }, ct);

        if (cartao != null)
        {
            _context.Cartoes.Remove(cartao);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> PossuiTransacoesAsync(int id, CancellationToken ct = default)
    {
        // AnyAsync é muito mais rápido que Count() > 0 pois para no primeiro registro
        return await _context.Set<Transacao>()
            .AnyAsync(t => t.CartaoId == id, ct);
    }
}
