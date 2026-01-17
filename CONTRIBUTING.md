# 👋 Bem-vindo ao Projeto Milhas

Ficamos felizes com seu interesse em contribuir. Para garantir que nosso time de 3 pessoas trabalhe de forma ágil e sem conflitos, seguimos alguns processos de governança e arquitetura.

## 📚 Leitura Obrigatória

Antes de abrir seu primeiro Pull Request, por favor leia nosso manual completo:

👉 **[CLIQUE AQUI PARA LER O MANUAL DE DESENVOLVIMENTO E FLUXO DE TRABALHO](docs/DESENVOLVIMENTO.md)**

Lá você encontrará os detalhes sobre:

1. **Git Flow:** Como nomear branches (`feat/`, `fix/`) e commits.
2. **Automação:** Palavras-chave obrigatórias (`Closes #123`) para mover cards no Kanban.
3. **Arquitetura:** Onde colocar lógica de negócio, EF Core e Views (Clean Arch).
4. **Regras de PR:** Por que a `main` é bloqueada e como aprovar código.

## ⚡ Resumo Rápido (Checklist)

Se você já leu o manual e precisa apenas relembrar as regras básicas:

- [ ] A branch sai da `main`: `git checkout -b tipo/numero-issue-nome`
- [ ] O código segue o `.editorconfig` (sem avisos amarelos)?
- [ ] O PR tem `Closes #numero-da-issue` na descrição?
- [ ] Você moveu o card no Board para **In Progress** antes de começar?

Dúvidas? Chame no grupo da equipe!

---

### 🧱 Milestone 0: O Esqueleto Funcional (Walking Skeleton)

**Prazo:** Até 19/01 (Segunda-feira)
**Lema:** _"Tudo conecta, nada trafega."_

Nesta etapa, o foco é **Infraestrutura e Design**. O sistema existe, compila e roda, mas o usuário final não consegue _fazer_ nada útil (não cadastra, não vê saldo).

✅ **O que você TERÁ no dia 19/01 (Entregáveis):**

1. **Arquitetura Viva:** Solution criada, camadas (`Core`, `Infra`, `Web`) conversando e `git` padronizado.
2. **Infraestrutura de Dados:** O `DbContext` conecta no SQL Server. A conexão funciona, mesmo que o banco esteja vazio.
3. **Casca Visual (Shell):** O site abre. O menu lateral (MudBlazor) existe. Você clica em "Cartões" e vai para uma página em branco (ou com um título). A navegação funciona.
4. **Cérebro Matemático (POC):** Você tem uma classe no Core (e um teste) que prova que sua lógica de `(Valor / Dolar) * Fator` funciona.
5. **Mapa do Tesouro (Docs):** Os Diagramas de Classe e Banco estão prontos (no papel/PDF). O time sabe _exatamente_ quais tabelas criar na segunda-feira.

❌ **O que NÃO ENTRA aqui (Anti-Escopo):**

- Criar formulários de cadastro (`<EditForm>`).
- Botões de "Salvar" funcionando.
- Telas bonitas com gráficos.
- Login funcionando.

---

### ⚙️ Milestone 1: O MVP da Nota (Core Features)

**Prazo:** 20/01 a 26/01
**Lema:** _"O sistema funciona (CRUD)."_

Aqui é onde o trabalho pesado de codificação acontece. O foco é atender os requisitos de **2,5 pontos** do professor (Formulários, Máscaras, CRUD).

✅ **O que você TERÁ no dia 26/01 (Entregáveis):**

1. **Banco Materializado:** As tabelas (`Usuarios`, `Cartoes`, `Compras`) existem no SQL Server.
2. **Gestão de Dados (CRUD):**
   - Eu consigo cadastrar um Cartão.
   - Eu consigo editar um Cartão.
   - Eu consigo listar as Compras numa tabela (`MudTable`).

3. **Prevenção de Erros:** Os campos têm máscaras (CPF, Data, Moeda) e validação (não deixa salvar vazio).
4. **Integração Real:** O botão "Salvar" na tela realmente grava no banco e o dado persiste.

❌ **O que NÃO ENTRA aqui (Anti-Escopo):**

- Dashboard com gráficos coloridos.
- Exportar para PDF/Excel.
- Log e tratamento de erro _sofisticado_ (faz o básico, se der erro 500, paciência).
- Documentação final (relatório).

---

### ✨ Milestone 2: Polimento e "Fator Uau"

**Prazo:** 27/01 a 02/02
**Lema:** _"Parece um produto profissional."_

Aqui transformamos um "trabalho de faculdade" em um "produto de portfólio". Se a M1 atrasar, a M2 serve de margem de segurança (buffer).

✅ **O que você TERÁ no dia 02/02 (Entregáveis):**

1. **Dashboard Inteligente:** A tela inicial mostra cards com "Total de Pontos", "Próxima Expiração" (usando a lógica da POC da M0).
2. **Relatórios:** Botão para exportar dados (PDF ou CSV).
3. **Refinamento Visual:** Ajuste de cores, ícones, mensagens de sucesso ("Salvo com sucesso!") mais bonitas.
4. **O Documento Final:** O PDF impresso com os prints do sistema pronto e os diagramas atualizados.

---

### 🧪 O "Teste de Fogo" para suas Issues Atuais

Olhe para as 13 issues abertas na **Milestone 0**. Faça essas perguntas para cada uma. Se a resposta for "Sim", ela fica. Se for "Não", mova para a Milestone 1.

1. _"Essa tarefa envolve criar um formulário para o usuário digitar dados?"_
   - Sim? -> **Mova para Milestone 1**. (Exceção: Tela de Login simples se for essencial para entrar no sistema).
   - Não (é só configuração ou menu)? -> Fica na M0.

2. _"Essa tarefa é sobre conectar peças (Front com Back, Back com Banco)?"_
   - Sim? -> **Fica na M0**. (Isso é fundação).

3. _"Essa tarefa é sobre criar tabelas finais no banco?"_
   - Sim? -> **Cuidado.** Se for _apenas_ criar a classe C# e a Migration vazia, é M0. Se for popular com dados reais do usuário, é M1.

### Resumo Visual

| Característica      | **Milestone 0 (19/01)**              | **Milestone 1 (26/01)**        | **Milestone 2 (02/02)**     |
| ------------------- | ------------------------------------ | ------------------------------ | --------------------------- |
| **Estado do Banco** | Conectado (pode ter Seed Data teste) | Tabelas Reais e Dados salvos   | Dados complexos p/ Gráficos |
| **Telas**           | Menu + Páginas em Branco             | Formulários + Tabelas de Dados | Dashboard + Gráficos        |
| **Foco do Time**    | Configuração e Arquitetura           | Lógica de CRUD e Validação     | Visual e Documentação       |
| **Risco**           | "Não conectar"                       | "Não salvar"                   | "Não entregar o PDF"        |
