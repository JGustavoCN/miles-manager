using Miles.Core.Entities;
using Miles.Core.Exceptions;
using Miles.Core.Strategies;
using Microsoft.EntityFrameworkCore;

namespace Miles.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context, Serilog.ILogger Log)
    {
        try
        {
            // Garante que o banco de dados existe e aplica migrations pendentes
            context.Database.Migrate();

            // Verifica se já existem usuários (idempotência)
            if (context.Usuarios.Any())
            {
                Log.Information("Seed Data: Banco já contém dados. Operação ignorada.");
                return; // Banco já foi populado
            }

            Log.Information("Iniciando população do banco de dados (Seed Data)...");


            var usuario = new Usuario
            {
                Nome = "Admin Teste",
                Email = "admin@milesmanager.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            Log.Debug("Usuário '{Nome}' criado (Id: {Id}) - Senha: Admin@123", usuario.Nome, usuario.Id);

            var programas = new[]
            {
                new ProgramaFidelidade
                {
                    Nome = "Livelo",
                    Banco = "Banco Bradesco",
                    UsuarioId = usuario.Id
                },
                new ProgramaFidelidade
                {
                    Nome = "Smiles",
                    Banco = "Gol Linhas Aéreas",
                    UsuarioId = usuario.Id
                },
                new ProgramaFidelidade
                {
                    Nome = "Latam Pass",
                    Banco = "Banco Itaú",
                    UsuarioId = usuario.Id
                },
                new ProgramaFidelidade
                {
                    Nome = "Esfera",
                    Banco = "Banco do Brasil",
                    UsuarioId = usuario.Id
                }
            };

            // Valida antes de inserir
            foreach (var programa in programas)
            {
                try
                {
                    programa.Validar(); // UC-08
                }
                catch (ValorInvalidoException ex)
                {
                    Log.Error(ex, "Programa '{Nome}' possui dados inválidos: {Erro}", programa.Nome, ex.Message);
                    throw;
                }
            }

            context.ProgramasFidelidade.AddRange(programas);
            context.SaveChanges();

            Log.Debug(" {Count} programa(s) de fidelidade criado(s)", programas.Length);

            var cartoes = new[]
            {
                new Cartao
                {
                    Nome = "Nubank Ultraviolet",
                    Bandeira = "Mastercard",
                    Limite = 15000.00m,
                    DiaVencimento = 10,
                    FatorConversao = 1.0m, // 1 ponto por dólar
                    UsuarioId = usuario.Id,
                    ProgramaId = programas[0].Id // Livelo
                },
                new Cartao
                {
                    Nome = "XP Visa Infinite",
                    Bandeira = "Visa",
                    Limite = 25000.00m,
                    DiaVencimento = 15,
                    FatorConversao = 2.0m, // 2 pontos por dólar (melhor conversão)
                    UsuarioId = usuario.Id,
                    ProgramaId = programas[1].Id // Smiles
                },
                new Cartao
                {
                    Nome = "Itaú Personnalité Black",
                    Bandeira = "Visa",
                    Limite = 30000.00m,
                    DiaVencimento = 20,
                    FatorConversao = 1.5m, // 1.5 pontos por dólar
                    UsuarioId = usuario.Id,
                    ProgramaId = programas[2].Id // Latam Pass
                },
                new Cartao
                {
                    Nome = "BB Ourocard Estilo",
                    Bandeira = "Mastercard",
                    Limite = 10000.00m,
                    DiaVencimento = 5,
                    FatorConversao = 1.2m, // 1.2 pontos por dólar
                    UsuarioId = usuario.Id,
                    ProgramaId = programas[3].Id // Esfera
                }
            };

            // Valida antes de inserir (RF-008)
            foreach (var cartao in cartoes)
            {
                try
                {
                    cartao.Validar(); // UC-08
                }
                catch (ValorInvalidoException ex)
                {
                    Log.Error(ex, "Cartão '{Nome}' possui dados inválidos: {Erro}", cartao.Nome, ex.Message);
                    throw;
                }
            }

            context.Cartoes.AddRange(cartoes);
            context.SaveChanges();

            Log.Debug(" {Count} cartão(ões) criado(s)", cartoes.Length);

            var strategy = new CalculoPadraoStrategy(); // Instância da Strategy real

            var transacoesSeed = new[]
            {
                // Transação 1 - Passado recente (Mês anterior)
                new {
                    Data = DateTime.Now.AddMonths(-1).Date,
                    Valor = 500.00m,
                    Descricao = "Amazon - Livros Técnicos",
                    Categoria = "Educação",
                    CotacaoDolar = 5.00m,
                    CartaoId = cartoes[0].Id,
                    FatorConversao = cartoes[0].FatorConversao
                },

                // Transação 2 - Passado (3 semanas atrás)
                new {
                    Data = DateTime.Now.AddDays(-21).Date,
                    Valor = 1200.00m,
                    Descricao = "Passagem Aérea - Congonhas/Santos Dumont",
                    Categoria = "Viagem",
                    CotacaoDolar = 5.10m,
                    CartaoId = cartoes[1].Id,
                    FatorConversao = cartoes[1].FatorConversao
                },

                // Transação 3 - Presente (Hoje)
                new {
                    Data = DateTime.Now.Date,
                    Valor = 350.00m,
                    Descricao = "Supermercado Extra - Compra Mensal",
                    Categoria = "Alimentação",
                    CotacaoDolar = 5.05m,
                    CartaoId = cartoes[2].Id,
                    FatorConversao = cartoes[2].FatorConversao
                },

                // Transação 4 - Futuro (Compra parcelada agendada)
                new {
                    Data = DateTime.Now.AddDays(10).Date,
                    Valor = 800.00m,
                    Descricao = "Hotel Copacabana Palace - Reserva",
                    Categoria = "Viagem",
                    CotacaoDolar = 5.00m,
                    CartaoId = cartoes[0].Id,
                    FatorConversao = cartoes[0].FatorConversao
                },

                // Transação 5 - Histórico (Ano anterior)
                new {
                    Data = new DateTime(DateTime.Now.Year - 1, 12, 25),
                    Valor = 2500.00m,
                    Descricao = "Notebook Dell Inspiron - Black Friday",
                    Categoria = "Tecnologia",
                    CotacaoDolar = 4.95m,
                    CartaoId = cartoes[1].Id,
                    FatorConversao = cartoes[1].FatorConversao
                },

                // Transação 6 - Recente (Semana passada)
                new {
                    Data = DateTime.Now.AddDays(-7).Date,
                    Valor = 180.00m,
                    Descricao = "Restaurante Fasano - Jantar",
                    Categoria = "Alimentação",
                    CotacaoDolar = 5.05m,
                    CartaoId = cartoes[3].Id,
                    FatorConversao = cartoes[3].FatorConversao
                },

                // Transação 7 - Presente (Ontem)
                new {
                    Data = DateTime.Now.AddDays(-1).Date,
                    Valor = 95.00m,
                    Descricao = "Uber - Corridas do Mês",
                    Categoria = "Transporte",
                    CotacaoDolar = 5.05m,
                    CartaoId = cartoes[2].Id,
                    FatorConversao = cartoes[2].FatorConversao
                }
            };

            var transacoes = new List<Transacao>();

            foreach (var seed in transacoesSeed)
            {
                try
                {
                    var transacao = new Transacao
                    {
                        Data = seed.Data,
                        Valor = seed.Valor,
                        Descricao = seed.Descricao,
                        Categoria = seed.Categoria,
                        CotacaoDolar = seed.CotacaoDolar,
                        CartaoId = seed.CartaoId
                    };

                    // UC-09: Cálculo de Pontos com Tratamento de Erro
                    try
                    {
                        transacao.CalcularPontos(strategy, seed.FatorConversao);
                    }
                    catch (Exception ex)
                    {
                        // UC-09 FE-01: Log de Erro de Cálculo (não quebra o seed)
                        Log.Warning(ex, "Erro ao calcular pontos para transação '{Descricao}'. Atribuindo 0 pontos.", seed.Descricao);
                        transacao.PontosEstimados = 0;
                    }

                    // UC-08: Validação antes de adicionar
                    transacao.Validar();

                    transacoes.Add(transacao);
                }
                catch (ValorInvalidoException ex)
                {
                    // UC-08 FE-01: Log de Erro de Validação
                    Log.Error(ex, "Transação '{Descricao}' possui dados inválidos: {Erro}", seed.Descricao, ex.Message);
                    // Continua com as outras transações ao invés de quebrar tudo
                }
            }

            context.Transacoes.AddRange(transacoes);
            context.SaveChanges();

            Log.Debug("✅ {Count} transação(ões) criada(s)", transacoes.Count);

            var totalPontos = transacoes.Sum(t => t.PontosEstimados);

            Log.Information(
                " Seed Data inserido com sucesso!\n" +
                "    Estatísticas:\n" +
                "   - {Usuarios} usuário(s)\n" +
                "   - {Programas} programa(s) de fidelidade\n" +
                "   - {Cartoes} cartão(ões)\n" +
                "   - {Transacoes} transação(ões)\n" +
                "   - {TotalPontos} pontos totais estimados",
                context.Usuarios.Count(),
                context.ProgramasFidelidade.Count(),
                context.Cartoes.Count(),
                context.Transacoes.Count(),
                totalPontos
            );
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao executar Seed Data");
            throw;
        }
    }
}
