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

## 🚀 Fluxo de Trabalho
1. Pegue uma Issue no board e mova para **Doing**.
2. Crie uma branch: `feat/nome-da-tarefa`.
3. Terminou? Abra um **Pull Request** para a `main`.
4. Avise no grupo para alguém revisar.
