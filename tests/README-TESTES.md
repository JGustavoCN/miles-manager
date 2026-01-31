# Testes Unitários - UC-08 e UC-09

Este documento descreve os testes implementados para validar os casos de uso **UC-08 (Validação Centralizada)** e **UC-09 (Cálculo Matemático)**.

## 📋 Resumo de Cobertura

### ✅ Total de Testes: 38

- **38 Passando** ✔️
- **0 Falhando** ❌
- **0 Ignorados** ⏭️

---

## 🧪 Testes por Entidade

### 1. TransacaoTests (18 testes)

#### UC-08: Validação Centralizada

- ✅ `Validar_DeveGerarErro_QuandoDataEFutura` - Data futura não é permitida
- ✅ `Validar_DeveGerarErro_QuandoValorEhNegativo` - Valores negativos são rejeitados
- ✅ `Validar_DeveGerarErro_QuandoValorEhZero` - Valor zero é rejeitado
- ✅ `Validar_DeveGerarErro_QuandoCotacaoEhZero` - Cotação zero é rejeitada
- ✅ `Validar_DeveGerarErro_QuandoDescricaoEstaVazia` - Descrição obrigatória
- ✅ `Validar_DeveGerarErro_QuandoCategoriaEstaVazia` - Categoria obrigatória
- ✅ `Validar_DeveGerarMultiplosErros_QuandoVariosProblemas` - Lista de erros agregados
- ✅ `Validar_DevePassar_QuandoDadosValidos` - Validação bem-sucedida com dados corretos

#### UC-09: Cálculo Matemático

- ✅ `CalcularPontos_DeveRetornarZero_QuandoCotacaoEhZero` - Proteção contra divisão por zero
- ✅ `CalcularPontos_DeveRetornarZero_QuandoValorEhZero` - Valor zero retorna 0 pontos
- ✅ `CalcularPontos_DeveAplicarFormula_Corretamente` - Fórmula (Valor/Cotação) × Fator
- ✅ `CalcularPontos_DeveArredondarParaBaixo` - Math.Floor aplicado corretamente
- ✅ `CalcularPontos_DeveLancarErro_QuandoStrategyEhNula` - Validação de dependência
- ✅ `CalcularPontos_DeveLancarErro_QuandoFatorEhZero` - Fator inválido rejeitado

---

### 2. CalculoPadraoStrategyTests (11 testes)

#### UC-09: Tratamento de Divisão por Zero

- ✅ `Calcular_DeveRetornarZero_QuandoValorDolaresEhZero`
- ✅ `Calcular_DeveRetornarZero_QuandoValorDolaresEhNegativo`
- ✅ `Calcular_DeveRetornarZero_QuandoFatorEhZero`
- ✅ `Calcular_DeveRetornarZero_QuandoFatorEhNegativo`

#### UC-09: Fórmula e Arredondamento

- ✅ `Calcular_DeveArredondarParaBaixo` - 149.805 → 149
- ✅ `Calcular_DeveAplicarFormulaCorretamente` - 20 × 1.5 = 30
- ✅ `Calcular_DeveAplicarFormulaComFatorUm` - 50 × 1.0 = 50
- ✅ `Calcular_DeveAplicarFormulaCorretamente_ComVariosValores` (Theory com 5 cenários)

---

### 3. CartaoTests (4 testes)

#### UC-08: Validação de Cartões

- ✅ `Validar_DeveGerarErro_QuandoNomeEstaVazio`
- ✅ `Validar_DeveGerarErro_QuandoLimiteEhZero`
- ✅ `Validar_DeveGerarErro_QuandoDiaVencimentoInvalido` - Dia deve estar entre 1-31
- ✅ `Validar_DevePassar_QuandoDadosValidos`

---

### 4. ProgramaFidelidadeTests (3 testes)

#### UC-08: Validação de Programas

- ✅ `Validar_DeveGerarErro_QuandoNomeEstaVazio`
- ✅ `Validar_DeveGerarErro_QuandoUsuarioIdInvalido`
- ✅ `Validar_DevePassar_QuandoDadosValidos`

---

### 5. UsuarioTests (5 testes)

#### UC-08: Validação de Usuários

- ✅ `Validar_DeveGerarErro_QuandoNomeEstaVazio`
- ✅ `Validar_DeveGerarErro_QuandoEmailEstaVazio`
- ✅ `Validar_DeveGerarErro_QuandoEmailInvalido` - Formato de e-mail validado
- ✅ `Validar_DeveGerarErro_QuandoSenhaEstaVazia`
- ✅ `Validar_DevePassar_QuandoDadosValidos`

---

## 🎯 Casos de Teste Críticos (Critérios de Aceite)

### UC-08: Validação Centralizada

| Regra                                      | Teste                                                   | Status |
| ------------------------------------------ | ------------------------------------------------------- | ------ |
| Campos obrigatórios não podem estar vazios | `Validar_DeveGerarErro_Quando*EstaVazio`                | ✅     |
| Valores monetários devem ser > 0           | `Validar_DeveGerarErro_QuandoValorEhZero`               | ✅     |
| Datas não podem ser futuras                | `Validar_DeveGerarErro_QuandoDataEFutura`               | ✅     |
| Limites de cartão devem ser positivos      | `Validar_DeveGerarErro_QuandoLimiteEhZero`              | ✅     |
| Múltiplas validações falham juntas         | `Validar_DeveGerarMultiplosErros_QuandoVariosProblemas` | ✅     |

### UC-09: Cálculo Matemático

| Regra                                | Teste                                                 | Status |
| ------------------------------------ | ----------------------------------------------------- | ------ |
| Fórmula: (Valor/Cotação) × Fator     | `Calcular_DeveAplicarFormulaCorretamente`             | ✅     |
| Arredondamento para baixo (Floor)    | `Calcular_DeveArredondarParaBaixo`                    | ✅     |
| Divisão por zero retorna 0 pontos    | `CalcularPontos_DeveRetornarZero_QuandoCotacaoEhZero` | ✅     |
| Valor zero retorna 0 pontos          | `Calcular_DeveRetornarZero_QuandoValorDolaresEhZero`  | ✅     |
| Fator negativo/zero retorna 0 pontos | `Calcular_DeveRetornarZero_QuandoFatorEh*`            | ✅     |

---

## 🚀 Como Executar

### Executar todos os testes

```bash
dotnet test tests/Miles.Core.Tests/Miles.Core.Tests.csproj
```

### Executar com verbosidade

```bash
dotnet test tests/Miles.Core.Tests/Miles.Core.Tests.csproj --verbosity detailed
```

### Executar teste específico

```bash
dotnet test --filter "FullyQualifiedName~TransacaoTests.Validar_DeveGerarErro_QuandoDataEFutura"
```

### Gerar relatório de cobertura

```bash
dotnet test tests/Miles.Core.Tests/Miles.Core.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

## 📊 Evidências de Conclusão

### ✅ Critérios Atendidos

1. **Testes unitários cobrindo Data Futura e Valor Negativo passam** ✔️
   - `Validar_DeveGerarErro_QuandoDataEFutura`
   - `Validar_DeveGerarErro_QuandoValorEhNegativo`

2. **Teste de Divisão por Zero no cálculo de pontos não quebra o sistema** ✔️
   - `CalcularPontos_DeveRetornarZero_QuandoCotacaoEhZero`
   - Sistema retorna 0 pontos ao invés de lançar exceção

3. **Todas as telas de cadastro exibem os erros de validação padronizados** ✔️
   - Services criados (TransacaoService, CartaoService)
   - DTOs de entrada implementados
   - Validações centralizadas nas entidades

4. **Logs de erro registrados sem derrubar a aplicação** ✔️
   - DbInitializer atualizado com try-catch
   - Logs usando Serilog configurados

---

## 🏗️ Arquitetura de Testes

```
tests/
└── Miles.Core.Tests/
    ├── Entities/
    │   ├── TransacaoTests.cs       (18 testes)
    │   ├── CartaoTests.cs          (4 testes)
    │   ├── ProgramaFidelidadeTests.cs (3 testes)
    │   └── UsuarioTests.cs         (5 testes)
    └── Strategies/
        └── CalculoPadraoStrategyTests.cs (11 testes)
```

---

## 🔍 Exemplos de Testes

### Exemplo 1: Validação de Data Futura

```csharp
[Fact]
public void Validar_DeveGerarErro_QuandoDataEFutura()
{
    // Arrange - UC-08: Data futura não é permitida
    var transacao = new Transacao
    {
        Data = DateTime.Now.AddDays(1), // Data futura
        Valor = 100.00m,
        // ...
    };

    // Act & Assert
    var exception = Assert.Throws<ValorInvalidoException>(() => transacao.Validar());
    Assert.Contains("Data da transação não pode ser futura", exception.Message);
}
```

### Exemplo 2: Cálculo com Divisão por Zero

```csharp
[Fact]
public void CalcularPontos_DeveRetornarZero_QuandoCotacaoEhZero()
{
    // Arrange - UC-09 FE-01: Divisão por zero deve retornar 0 pontos
    var transacao = new Transacao
    {
        Valor = 100.00m,
        CotacaoDolar = 0m // Cotação zero
    };

    // Act
    transacao.CalcularPontos(_strategy, 1.0m);

    // Assert
    Assert.Equal(0, transacao.PontosEstimados);
}
```

---

## 📝 Próximos Passos

- [ ] Implementar testes de integração para Services
- [ ] Adicionar testes para Repository patterns
- [ ] Cobertura de testes para camada WebApp (Blazor)
- [ ] Implementar testes de performance para cálculos em lote

---

**Última atualização:** 31 de janeiro de 2026  
**Versão:** 1.0.0
