# 📘 Manual de Desenvolvimento e Fluxo de Trabalho

Bem-vindo ao repositório! Este documento explica como nossa equipe trabalha, como as automações funcionam e as regras para contribuir com código de qualidade.

---

## 🛡️ Regras de Governança e Segurança

Para garantir a estabilidade do projeto, configuramos algumas travas no GitHub ("Branch Protection Rules"). **É importante que você saiba como elas afetam seu dia a dia:**

1. **A `main` é Sagrada:**
   - A branch `main` está **bloqueada** para pushes diretos.
   - _O que acontece se tentar:_ O Git retornará um erro (`GH006: Protected branch update failed`).
   - _Solução:_ Todo código deve vir via **Pull Request (PR)**.

2. **Code Review Obrigatório:**
   - Nenhum PR pode ser mergeado sem aprovação.
   - **Regra:** É necessário **1 aprovação** de outro membro da equipe.
   - _Nota:_ Você não pode aprovar seu próprio PR.

3. **Segurança de Aprovação:**
   - Se você receber uma aprovação ✅ e depois fizer um novo _commit_ na mesma branch, a aprovação **será descartada automaticamente**.
   - _Motivo:_ Garantir que o código mergeado seja exatamente o que foi revisado.

---

## 🤖 Automação do Board (Project)

Não precisamos arrastar cards manualmente o tempo todo. O GitHub Projects foi configurado com **Workflows** automáticos.

Para que a "mágica" aconteça, siga estas regras:

| Ação do Dev      | Reação do Board                   | Onde acontece                                                             |
| :--------------- | :-------------------------------- | :------------------------------------------------------------------------ |
| **Criar Issue**  | Aparece na coluna **Todo**        | Automaticamente                                                           |
| **Abrir PR**     | Move card para **Review/Testing** | **IMPORTANTE:** Só funciona se usar a palavra-chave (Closes #NumberIssue) |
| **Merge PR**     | Move card para **Done**           | Automaticamente                                                           |
| **Fechar Issue** | Move card para **Done**           | Se fechada manualmente sem PR                                             |

---

## 🚀 Fluxo de Trabalho (Passo a Passo)

### 1. Pegando uma Tarefa

1. Vá na aba **Projects** e escolha uma Issue da coluna **Todo**.
2. Arraste para **In Progress**.
3. **Assignees:** Clique no seu nome para saberem que você está trabalhando nela.
4. **Milestone:** Verifique na lateral direita se a Issue pertence à Milestone (Swimlane) correta.

### 2. Criando a Branch

Crie a branch sempre a partir da `main` atualizada. Use o padrão:
`tipo/numero-issue-breve-descricao`

**Exemplos:**

- `feat/42-tela-login` (Para a Issue #42)
- `fix/15-erro-calculo` (Para a Issue #15)
- `docs/20-atualiza-readme`

Este guia explica como gerenciamos nosso código. O objetivo não é apenas decorar comandos, mas entender o **ciclo de vida** do desenvolvimento no nosso projeto.

## 🌳 O Conceito de Branch (Ramificação)

Imagine que a branch `main` é a linha do tempo "sagrada" do nosso projeto. Tudo que está lá funciona.
Para criar uma nova funcionalidade (ex: Cadastro de Cartão) sem quebrar o que já existe, criamos um "universo paralelo" chamado **Branch**.

Você trabalha nesse universo paralelo. Se quebrar algo, a `main` continua intacta. Só quando tudo estiver perfeito, nós unimos (**Merge**) sua branch de volta à `main`.

## 🛠️ Passo a Passo: Do Início ao Fim

Existem duas formas de criar esse "universo paralelo". A **Opção A** é a nossa favorita pois já liga a tarefa ao código automaticamente.

### Opção A: Criando via GitHub (Recomendado 🌟)

1. Abra a **Issue** que você vai trabalhar no Board.
2. Na barra lateral direita, procure por **Development** e clique em **Create a branch**.
3. Deixe o nome sugerido (o GitHub já padroniza) e clique em Create.
4. Vá no seu terminal e digite:

```bash
   # "Baixa" as informações das novas branches da nuvem
   git fetch origin

   # Entra na branch que você acabou de criar no site
   git checkout nome-da-branch-que-voce-copiou
```

### Opção B: Criando via Terminal (Manual)

Se você não estiver no site, pode criar na mão. **Atenção à nomenclatura:** use `feat/` para funcionalidades, `fix/` para correções e `docs/` para documentação.

```bash
# 1. Garanta que você está saindo da base atualizada
git checkout main
git pull

# 2. Cria (-b) uma nova branch e entra nela
git checkout -b feat/15-logica-calculo-milhas
```

## 💾 O Ciclo de Vida dos Arquivos (Como salvar)

Entender os "estados" de um arquivo evita que você perca código ou suba coisas erradas.

1. **Untracked (Não rastreado):** Você criou um arquivo novo. O Git nem sabe que ele existe.
2. **Modified (Modificado):** O arquivo já existia, você alterou o código, mas ainda não "avisou" o Git que quer salvar essa mudança.
3. **Staged (Preparado):** Você disse "Git, inclua esse arquivo no próximo pacote".
4. **Committed (Salvo):** O pacote está fechado e salvo no seu histórico local.

### Comandos Essenciais

- **`git status` (O GPS):**
  Use sempre! Ele te diz o que está modificado (vermelho) e o que está preparado para salvar (verde).
- **`git add` (Preparar):**
- `git add .` (Ponto): Adiciona **TUDO** que mudou. É rápido, mas perigoso. Use `git status` antes para garantir que não vai subir lixo.
- `git add NomeDoArquivo.cs`: Adiciona apenas um arquivo específico. É a forma mais profissional.

- **`git commit` (Salvar):**
  Fecha o pacote. A mensagem **DEVE** seguir nosso padrão:
- ✅ `feat: cria tela de login`
- ✅ `fix: corrige erro no calculo de juros`
- ❌ `up`, `alterações`, `arrumando`

## ⚠️ Zona de Perigo: Corrigindo erros e o "Force Push"

Às vezes, você faz um commit e percebe que escreveu a mensagem errada ou esqueceu um arquivo.

**1. Corrigindo o último commit (git commit --amend)**
Se você ainda **não** enviou para o GitHub (não deu push), você pode refazer o último pacote:

```bash
git add arquivo-esquecido.cs
git commit --amend -m "mensagem corrigida"
```

_Isso reescreve a história localmente._

**2. O Perigo do `git push --force`**
Se você usou o `--amend` mas já tinha enviado o código para o GitHub antes, o Git vai bloquear seu próximo envio. Ele dirá que os históricos não batem.

Para resolver, você precisa "forçar" a sua versão:

```bash
git push -f origin nome-da-sua-branch
```

🚨 **REGRA DE OURO:**

- **Pode usar:** Na sua branch de feature (`feat/minha-task`) se só você estiver mexendo nela.
- **NUNCA USE:** Na branch `main`. Isso apaga o trabalho dos outros colegas e quebra o projeto de todo mundo.

## 🏁 Finalizando o Dia

Terminou o código?

1. `git push` (Envie para a nuvem).
2. Abra o **Pull Request** no GitHub.
3. Arraste o card no Board para **Review/Testing**.

---

### 3. Codando (Padrões Técnicos)

Seguimos a **Arquitetura Limpa** e **Conventional Commits**:

**Estrutura de Pastas:**

- **Core:** Só classes puras C# (Regras de Negócio). _Proibido referenciar Banco de Dados aqui._
- **Infrastructure:** Implementação de EF Core, SQL e serviços externos.
- **Web:** Páginas `.razor` e componentes visuais (MudBlazor).

**Mensagens de Commit:**

- `feat: adiciona botão de login`
- `fix: corrige erro de soma na classe X`
- `style: formatação de código`

### 4. Abrindo o Pull Request (O Pulo do Gato 🐱)

Quando terminar, abra o PR para a `main`.

**⚠️ Regra de Ouro para Automação:**
Na descrição do PR, você **DEVE** escrever uma das palavras-chave de linkagem seguida do número da Issue.

> Exemplo: _"Essa implementação finaliza a tela de login. **Closes #42**"_

_Se você não escrever `Closes #numero` ou `Fixes #numero`, o card não moverá sozinho e a Issue não fechará automaticamente após o merge._

### 5. Revisão e Merge

1. O card moveu para **Review/Testing**? Avise no grupo.
2. Um colega deve entrar, revisar o código e marcar **Approve**.
3. Se houver comentários ("Changes requested"), discuta e corrija.
   - _Nota:_ O GitHub bloqueia o merge se houver conversas não resolvidas. Clique em "Resolve conversation" após responder.
4. Ficou verde? Clique em **Squash and Merge** (ou Merge padrão) para finalizar.

---

## ❓ FAQ Rápido

**P: O botão de Merge está cinza/bloqueado. Por quê?**

- **R:** Ou falta aprovação de 1 colega, ou alguém pediu mudanças, ou há conversas (comentários) em aberto que precisam ser resolvidas.

**P: Fiz o push mas esqueci de criar a branch (estava na main). E agora?**

- **R:** O GitHub recusou seu push. Não se desespere.
  1. Crie a branch agora: `git checkout -b feat/minha-branch`
  2. Faça o push da nova branch: `git push origin feat/minha-branch`
  3. Volte a main para o estado original: `git checkout main` e `git reset --hard origin/main`

**P: O Linter está reclamando (sublinhado amarelo). Posso ignorar?**

- **R:** Não. O arquivo `.editorconfig` define nosso estilo. Corrija os avisos antes de subir o código para manter o projeto limpo.
