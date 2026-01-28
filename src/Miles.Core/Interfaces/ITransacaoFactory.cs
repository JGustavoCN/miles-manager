using Miles.Core.Entities;
using Miles.Core.ValueObjects;

namespace Miles.Core.Interfaces;

/// Factory para criação de transações válidas (Factory Pattern - GoF)
/// Garante que objetos Transacao sempre nasçam em estado consistente
/// Benefícios:
/// - Centraliza a lógica de criação complexa
/// - Valida dependências (ex: Cartao deve existir)
/// - Permite mockar criação de objetos em testes
/// - Evita construtores com muitos parâmetros (Code Smell: Long Parameter List)
public interface ITransacaoFactory
{

    Transacao CriarNova(DadosTransacao dados);
}
