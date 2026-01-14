# 📘 Manual de Desenvolvimento e Fluxo de Trabalho

Bem-vindo ao repositório! Este documento explica como nossa equipe trabalha, como as automações funcionam e as regras para contribuir com código de qualidade.

---

## 🛡️ Regras de Governança e Segurança
Para garantir a estabilidade do projeto, configuramos algumas travas no GitHub ("Branch Protection Rules"). **É importante que você saiba como elas afetam seu dia a dia:**

1.  **A `main` é Sagrada:**
    * A branch `main` está **bloqueada** para pushes diretos.
    * *O que acontece se tentar:* O Git retornará um erro (`GH006: Protected branch update failed`).
    * *Solução:* Todo código deve vir via **Pull Request (PR)**.

2.  **Code Review Obrigatório:**
    * Nenhum PR pode ser mergeado sem aprovação.
    * **Regra:** É necessário **1 aprovação** de outro membro da equipe.
    * *Nota:* Você não pode aprovar seu próprio PR.

3.  **Segurança de Aprovação:**
    * Se você receber uma aprovação ✅ e depois fizer um novo *commit* na mesma branch, a aprovação **será descartada automaticamente**.
    * *Motivo:* Garantir que o código mergeado seja exatamente o que foi revisado.

---

## 🤖 Automação do Board (Project)
Não precisamos arrastar cards manualmente o tempo todo. O GitHub Projects foi configurado com **Workflows** automáticos.

Para que a "mágica" aconteça, siga estas regras:

| Ação do Dev | Reação do Board | Onde acontece |
| :--- | :--- | :--- |
| **Criar Issue** | Aparece na coluna **Todo** | Automaticamente |
| **Abrir PR** | Move card para **Review/Testing** | **IMPORTANTE:** Só funciona se usar a palavra-chave (veja abaixo) |
| **Merge PR** | Move card para **Done** | Automaticamente |
| **Fechar Issue** | Move card para **Done** | Se fechada manualmente sem PR |

---

## 🚀 Fluxo de Trabalho (Passo a Passo)

### 1. Pegando uma Tarefa
1.  Vá na aba **Projects** e escolha uma Issue da coluna **Todo**.
2.  Arraste para **In Progress**.
3.  **Assignees:** Clique no seu nome para saberem que você está trabalhando nela.
4.  **Milestone:** Verifique na lateral direita se a Issue pertence à Milestone (Swimlane) correta.

### 2. Criando a Branch
Crie a branch sempre a partir da `main` atualizada. Use o padrão:
`tipo/numero-issue-breve-descricao`

**Exemplos:**
* `feat/42-tela-login` (Para a Issue #42)
* `fix/15-erro-calculo` (Para a Issue #15)
* `docs/20-atualiza-readme`

### 3. Codando (Padrões Técnicos)
Seguimos a **Arquitetura Limpa** e **Conventional Commits**:

**Estrutura de Pastas:**
* **Core:** Só classes puras C# (Regras de Negócio). *Proibido referenciar Banco de Dados aqui.*
* **Infrastructure:** Implementação de EF Core, SQL e serviços externos.
* **Web:** Páginas `.razor` e componentes visuais (MudBlazor).

**Mensagens de Commit:**
* `feat: adiciona botão de login`
* `fix: corrige erro de soma na classe X`
* `style: formatação de código`

### 4. Abrindo o Pull Request (O Pulo do Gato 🐱)
Quando terminar, abra o PR para a `main`.

**⚠️ Regra de Ouro para Automação:**
Na descrição do PR, você **DEVE** escrever uma das palavras-chave de linkagem seguida do número da Issue.
> Exemplo: *"Essa implementação finaliza a tela de login. **Closes #42**"*

*Se você não escrever `Closes #numero` ou `Fixes #numero`, o card não moverá sozinho e a Issue não fechará automaticamente após o merge.*

### 5. Revisão e Merge
1.  O card moveu para **Review/Testing**? Avise no grupo.
2.  Um colega deve entrar, revisar o código e marcar **Approve**.
3.  Se houver comentários ("Changes requested"), discuta e corrija.
    * *Nota:* O GitHub bloqueia o merge se houver conversas não resolvidas. Clique em "Resolve conversation" após responder.
4.  Ficou verde? Clique em **Squash and Merge** (ou Merge padrão) para finalizar.

---

## ❓ FAQ Rápido

**P: O botão de Merge está cinza/bloqueado. Por quê?**
* **R:** Ou falta aprovação de 1 colega, ou alguém pediu mudanças, ou há conversas (comentários) em aberto que precisam ser resolvidas.

**P: Fiz o push mas esqueci de criar a branch (estava na main). E agora?**
* **R:** O GitHub recusou seu push. Não se desespere.
    1. Crie a branch agora: `git checkout -b feat/minha-branch`
    2. Faça o push da nova branch: `git push origin feat/minha-branch`
    3. Volte a main para o estado original: `git checkout main` e `git reset --hard origin/main`

**P: O Linter está reclamando (sublinhado amarelo). Posso ignorar?**
* **R:** Não. O arquivo `.editorconfig` define nosso estilo. Corrija os avisos antes de subir o código para manter o projeto limpo.
