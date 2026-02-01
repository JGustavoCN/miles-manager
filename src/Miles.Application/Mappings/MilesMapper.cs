using Miles.Application.DTOs;
using Miles.Core.Entities;
using Miles.Core.ValueObjects;
namespace Miles.Application;

public static class MilesMapper
{
    public static SessaoResultDTO ToResult(Usuario usuario)
    {
        return new SessaoResultDTO
        {
            Sucesso = true,
            UsuarioId = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            MensagemErro = null
        };
    }
    public static SessaoResultDTO ToErrorResult(string mensagem)
    {
        return new SessaoResultDTO
        {
            Sucesso = false,
            MensagemErro = mensagem,
            UsuarioId = null,
            Nome = null,
            Email = null
        };
    }


    public static ProgramaFidelidade ToEntity(ProgramaInputDTO dto)
    {
        return new ProgramaFidelidade
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Banco = dto.Banco,
            UsuarioId = dto.UsuarioId
        };
    }

    // Método auxiliar inverso, útil para carregar o formulário de edição
    public static ProgramaInputDTO ToDTO(ProgramaFidelidade entity)
    {
        return new ProgramaInputDTO
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Banco = entity.Banco,
            UsuarioId = entity.UsuarioId
        };
    }


    public static Cartao ToEntity(CartaoInputDTO dto)
    {
        return new Cartao
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Bandeira = dto.Bandeira,
            Limite = dto.Limite,
            DiaVencimento = dto.DiaVencimento,
            FatorConversao = dto.FatorConversao,
            UsuarioId = dto.UsuarioId,
            ProgramaId = dto.ProgramaId
        };
    }

    public static CartaoInputDTO ToDTO(Cartao entity)
    {
        return new CartaoInputDTO
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Bandeira = entity.Bandeira,
            Limite = entity.Limite,
            DiaVencimento = entity.DiaVencimento,
            FatorConversao = entity.FatorConversao,
            UsuarioId = entity.UsuarioId,
            ProgramaId = entity.ProgramaId
        };
    }

    public static DadosTransacao ToDadosTransacao(TransacaoInputDTO dto)
    {
        return new DadosTransacao
        {
            Valor = dto.Valor,
            Data = dto.Data,
            Descricao = dto.Descricao,
            Categoria = dto.Categoria,
            CotacaoDolar = dto.CotacaoDolar,
            CartaoId = dto.CartaoId
        };
    }
}
