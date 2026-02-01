using Miles.Core.Entities;

namespace Miles.Core.Interfaces;

/// Contrato de persistência para a entidade Usuario (RF-002)
/// Define operações de acesso a dados seguindo o padrão Repository
public interface IUsuarioRepository
{

    void Adicionar(Usuario usuario);

    void Atualizar(Usuario usuario);
    Usuario? ObterPorEmail(string email);


    Usuario? ObterPorId(int id);
}
