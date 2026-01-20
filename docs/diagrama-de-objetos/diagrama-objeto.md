# Diagrama de Objetos - Miles Manager

## Visão Geral

Este diretório contém o **Diagrama de Objetos** do sistema Miles Manager, representando um snapshot (instantâneo) do sistema em execução.

## Arquivos

- **`diagramaObjetos.puml`** - Código-fonte PlantUML do diagrama
- **`diagramaObjetos.svg`** - Imagem SVG gerada (usada na documentação)
- **`plantuml.jar`** - Ferramenta PlantUML para geração de diagramas

## Cenário Representado

**"O usuário José usa seu cartão Nubank (vinculado à Livelo) para comprar um Livro na Amazon"**

### Objetos Instanciados

1. **u1: Usuario**
   - Nome: "José"
   - Email: "jose@email.com"

2. **p1: ProgramaFidelidade**
   - Nome: "Livelo"
   - Banco: "Bradesco"
   - FatorConversao: 1.0

3. **c1: Cartao**
   - Nome: "Nubank Ultravioleta"
   - Bandeira: "Mastercard"
   - Limite: R$ 5.000,00
   - FatorPontos: 1.0
   - DiaVencimento: 15

4. **t1: Transacao**
   - Descricao: "Livro - Amazon"
   - Valor: R$ 100,00
   - Categoria: "Educação"
   - Data: "2024-01-15"
   - PontosGerados: 100.0

### Relacionamentos

- **u1 → c1**: Usuário possui cartão (1 para 1..*)
- **c1 → p1**: Cartão pontua em programa (1 para 1)
- **c1 → t1**: Cartão registra transações (1 para 0..*)

## Como Regenerar o Diagrama

Se você precisar modificar o diagrama e regenerar o SVG:

```bash
# Navegue até o diretório
cd docs/assets

# Execute o PlantUML
java -jar plantuml.jar -tsvg diagramaObjetos.puml
```

## Propósito

Este diagrama serve para:

1. **Validar o modelo de classes** - Confirma que as classes definidas conseguem representar cenários reais
2. **Seed Data de referência** - Fornece dados de exemplo para popular o banco de dados de desenvolvimento
3. **Documentação visual** - Ilustra como os objetos se relacionam em tempo de execução
4. **Casos de teste** - Base para criar cenários de teste automatizados

## Integração com a Documentação

Este diagrama está referenciado no documento principal de modelagem UML:
- `docs/modelagem-uml.md` - Seção "Diagrama de Objetos"

---

**Nota:** Este diagrama foi criado seguindo as especificações do Issue #21 - Diagrama de Objetos.
