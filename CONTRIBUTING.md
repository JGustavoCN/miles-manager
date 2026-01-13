# Guia de Contribuição - Projeto Milhas

## 📂 Organização de Pastas
Seguimos a Arquitetura Limpa. Não coloque lógica de banco no Frontend!
- **Core:** Só classes puras C# (Ex: `Usuario`, `CalculadoraPontos`). Nada de SQL aqui.
- **Infrastructure:** Só aqui entra o Entity Framework e SQL.
- **Web:** Aqui ficam as páginas `.razor` e componentes MudBlazor.

## 🤝 Padrão de Commits
Usamos **Conventional Commits**. Mensagens claras!
- `feat:` Nova funcionalidade (Ex: `feat: cria tela de login`)
- `fix:` Correção de erro (Ex: `fix: erro no calculo de pontos`)
- `docs:` Apenas documentação
- `style:` Formatação, ponto e vírgula, espaços (sem mudar código)

## 🎨 Linter e Estilo
O projeto tem um arquivo `.editorconfig`.
- **NÃO** ignore os avisos amarelos do Visual Studio.
- Se o código estiver sublinhado, corrija antes de subir.

## 🚀 Fluxo de Trabalho (Obrigatório)

### 1. Iniciando uma Tarefa
1. Vá no Board e escolha uma Issue da coluna **Todo**.
2. Arraste para **In Progress** e se adicione como *Assignee* (dono).
3. **Importante:** Verifique se a Issue está na Milestone correta (Swimlane).

### 2. Criando a Branch
Crie a branch sempre a partir da `main` atualizada.
Padrão de nome: `tipo/numero-issue-breve-descricao`
*Exemplos:*
- `feat/42-tela-login` (Para a Issue #42)
- `fix/15-erro-calculo` (Para a Issue #15)

### 3. Finalizando (Pull Request)
1. Abra um **Pull Request (PR)** para a `main`.
2. **Automação:** Na descrição do PR, você DEVE escrever: `Closes #numero-da-issue`.
   - *Isso fará o card mover sozinho para Done quando aprovado.*
3. O card moverá automaticamente para **Review/Testing**.
4. Avise no grupo para alguém revisar. Só após **1 aprovação** o merge será liberado.
