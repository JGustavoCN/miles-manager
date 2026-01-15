<h1 align="center">✈️ Miles Manager: Gestão Inteligente de Milhas</h1>

<p align="center">
  <strong>Instituto Federal de Sergipe (IFS) - Campus Lagarto</strong><br>
  <em>Disciplina: Programação WEB II — Prof. MSc. Arquimedes S. L. de Medeiros</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/Blazor-MudBlazor-7722FF?style=for-the-badge&logo=blazor&logoColor=white"/>
  <img src="https://img.shields.io/badge/Entity_Framework-Core-6DB33F?style=for-the-badge&logo=nuget&logoColor=white"/>
  <img src="https://img.shields.io/badge/Clean_Architecture-Onion-000000?style=for-the-badge&logo=csharp&logoColor=white"/>
</p>

---

> 🎯 **Projeto Acadêmico** focado no gerenciamento de cartões de crédito e programas de milhas.
> Desenvolvido sobre os pilares da **Clean Architecture**, este sistema visa resolver o problema de dispersão de pontos entre múltiplos programas de fidelidade, oferecendo um dashboard unificado e cálculo automático de pontuação.

---

## 🚀 Tecnologias e Arquitetura

Utilizamos uma abordagem de **crescimento orgânico** baseada em camadas para garantir manutenibilidade.

| 💻 Camada / Tech   | 📘 Descrição Técnica                                                                            |
| :----------------- | :---------------------------------------------------------------------------------------------- |
| **Frontend (UI)**  | **Blazor Server** com **MudBlazor** para componentes ricos, máscaras e responsividade.          |
| **Core (Domínio)** | Regras de negócio puras (C#), dissociadas de frameworks externos (Lógica de cálculo de milhas). |
| **Infrastructure** | **Entity Framework Core** com **SQL Server** para persistência e Repositories.                  |
| **Padronização**   | Uso de `.editorconfig` e **Conventional Commits** para governança do código.                    |

---

## 📁 Estrutura do Projeto (Clean Architecture)

A organização reflete a separação de responsabilidades exigida para projetos de alta escalabilidade:

```bash
📦 miles-manager-csharp
├── 📂 src/
│   ├── 📂 Miles.Core/           → O "Coração". Entidades e Interfaces (Sem dependência externa).
│   ├── 📂 Miles.Infrastructure/ → O "Mecanismo". Banco de dados, EF Core e Migrations.
│   ├── 📂 Miles.WebApp/         → A "Pele". Páginas .razor, Controllers e Layout MudBlazor.
│
├── 📂 docs/                     → Documentação, UML e Prints para o relatório.
├── 📄 CONTRIBUTING.md           → Guia de padronização de commits e código.
└── 📄 README.md                 → Este arquivo.

```

---

## 🤝 Como Contribuir

Para manter a qualidade e a padronização do código entre VS Code e Visual Studio, siga estes passos rápidos:

1. **Ambiente:** Ao abrir o projeto no VS Code, aceite a instalação das **Extensões Recomendadas** (janela pop-up no canto inferior direito). Isso garante que o C# Dev Kit e os formatadores funcionem automaticamente.
2. **Padronização:** Respeite as regras do `.editorconfig` (C# com 4 espaços, Web com 2 espaços).
3. **Commits:** Use o padrão de **Conventional Commits** (ex: `feat: novo dashboard`, `fix: erro no login`).

Para detalhes completos sobre o fluxo de trabalho (Git Flow, Clean Arch), leia nosso guia oficial:

## 📄 **[Manual Completo de Contribuição (CONTRIBUTING.md)](./CONTRIBUTING.md)**

## ✨ Funcionalidades (Requisitos WEB 2)

O sistema atende aos critérios avaliativos da 2ª Unidade:

- [x] **Gestão de Dados (CRUD):** Cadastro completo de Usuários, Cartões e Compras.
- [x] **Prevenção de Erros:** Uso de máscaras (CPF, Data, Moeda) e validações robustas (Data Annotations).
- [x] **Navegabilidade:** Menu lateral responsivo e fluxo de usuário intuitivo.
- [x] **Inteligência de Negócio:** Dashboard com estatísticas (Média de pontos, Total acumulado).
- [x] **Feedback Visual:** Alertas de sucesso/erro (Snackbars) e indicadores de carregamento.

---

## 👨‍💻 Equipe de Desenvolvimento

<div align="center">

| Desenvolvedor                                                                                                                                                                               | Descrição                                                                                                                  | Contato                                                                                                                                                                                                                                                                                                                     |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| <img src="https://avatars.githubusercontent.com/u/142621578?v=4" width="90" height="90" style="border-radius:50%"><br><b>José Gustavo C. Nascimento</b><br><sub>Matrícula: 2023004247</sub> | Desenvolvedor Full Stack • Java • .NET • Go (Golang) • Flutter • React • Foco em Engenharia de Software & Alta Performance | [![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/josé-gustavo-correia-nascimento-2100b2377) [![GitHub](https://img.shields.io/badge/GitHub-24292F?style=for-the-badge&logo=github&logoColor=white)](https://github.com/JGustavoCN) |
| <img src="https://avatars.githubusercontent.com/u/38109358?v=4" width="90" height="90" style="border-radius:50%"><br><b>Jeferson de Souza Andrade</b><br><sub>Matrícula: 2023001405</sub>   | Desenvolvedor Web • FullStack • Node • React • Firebase • IA • Java • PHP • Técnico de Segurança do Trabalho               | [![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/dev-jeferson-andrade/?locale=pt_BR) [![GitHub](https://img.shields.io/badge/GitHub-24292F?style=for-the-badge&logo=github&logoColor=white)](https://github.com/jefersonae)        |
| <img src="https://avatars.githubusercontent.com/u/210615743?v=4" width="90" height="90" style="border-radius:50%"><br><b>Mariano Nascimento Santos</b><br><sub>Matrícula: 2023004069</sub>  | Back-End Developer • Java • Spring Boot • React • Redes de Computadores                                                    | [![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/mariano-santos-892871272/) [![GitHub](https://img.shields.io/badge/GitHub-24292F?style=for-the-badge&logo=github&logoColor=white)](https://github.com/marianosantts)              |

</div>

---

## ⚙️ Como Executar o Projeto

```bash
# 1. Clone o repositório
git clone [https://github.com/JGustavoCN/miles-manager.git](https://github.com/JGustavoCN/miles-manager.git)

# 2. Configure a ConnectionString
# Edite o arquivo appsettings.json no projeto WebApp com seu SQL Server local.

# 3. Aplique as Migrations (Criação do Banco)
cd src/Miles.Infrastructure
dotnet ef database update --startup-project ../Miles.WebApp

# 4. Execute a aplicação
cd ../Miles.WebApp
dotnet run

```

---

## 🧾 Licença e Contexto

> 📜 Este projeto foi desenvolvido para avaliação na disciplina de **Programação WEB II** do **IFS - Campus Lagarto**.
> O código segue padrões de mercado para demonstrar proficiência em desenvolvimento .NET Moderno.

---

<div align="center">
<sub>Feito com 💚 e C# pela equipe Miles Manager.</sub>
</div>
