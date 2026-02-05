# 🛡️ Guia de Verificação de Propriedade e Segurança (Google Safe Browsing)

Este documento explica o processo de verificação de propriedade do domínio junto ao Google para resolver alertas de "Site Perigoso" ou "Phishing" em ambientes de teste na Azure.

## 1. Por que o Google bloqueia sites acadêmicos?

O Google utiliza um sistema chamado **Safe Browsing**. Sites novos hospedados em domínios gratuitos ou compartilhados (como `.azurewebsites.net`) que possuem telas de login ou formulários podem ser marcados preventivamente como **Phishing**. Isso ocorre porque:

- **Reputação do Domínio:** O sufixo da Azure é visado por atacantes para criar sites falsos.
- **Palavras-Chave:** Nomes como "manager", "login" ou "app" aumentam o rigor da análise automatizada.

## 2. Métodos de Verificação Disponíveis

O Google Search Console oferece diversas formas de provar que você é o dono do site:

| Método               | Como funciona                                                | Recomendação                                                    |
| -------------------- | ------------------------------------------------------------ | --------------------------------------------------------------- |
| **Arquivo HTML**     | Você sobe um arquivo `.html` específico na raiz do servidor. | Difícil na Azure sem acesso FTP configurado.                    |
| **Tag HTML**         | Uma metatag é adicionada ao `<head>` da página inicial.      | **Escolhida** (Mais simples para Blazor/WebApps).               |
| **Provedor de DNS**  | Adiciona-se um registro TXT na configuração do domínio.      | Exige domínio próprio (não disponível no `.azurewebsites.net`). |
| **Google Analytics** | Usa o código de rastreamento já existente.                   | Exige conta no Analytics configurada.                           |

## 3. Implementação Escolhida: Tag HTML

Para o projeto **Miles Manager**, optamos pela **Tag HTML** devido à facilidade de integração com o ciclo de vida de componentes do Blazor.

### Passo a Passo da Implementação

1. **Obtenção da Tag:** No [Google Search Console](https://search.google.com/search-console/), ao adicionar a propriedade `https://miles-manager-app...`, selecionamos "Tag HTML".
2. **Edição do Código:** A tag foi inserida no arquivo principal de estrutura do projeto: `src/Miles.WebApp/Components/App.razor`.

   ```razor
   <head>
       <meta charset="utf-8" />
       <meta name="viewport" content="width=device-width, initial-scale=1.0" />
       <meta name="google-site-verification" content="oOO1sdAC21xv88eOw9WURT5I4UawHjBZmm_J76QwmI4" />
       <base href="/" />
       ...
   </head>

   ```

3. **Deploy:** O código foi enviado ao repositório e o deploy automático via **GitHub Actions** (`deploy.yml`) atualizou o site na Azure.
4. **Validação:** Após o deploy, clicamos em "Verificar" no painel do Google.

## 4. Como remover o aviso de "Site Perigoso"

Após verificar a propriedade, o aviso vermelho não some instantaneamente. É necessário:

1. Acessar o menu **Segurança e Ações Manuais** > **Problemas de Segurança**.
2. Clicar em **Solicitar Revisão**.
3. **Argumentação sugerida:**
   > "O site é um projeto estritamente acadêmico para a disciplina de [Nome da Disciplina]. Não há coleta de dados reais, apenas simulados para fins de avaliação. O sistema é seguro e o código-fonte está disponível no GitHub para auditoria."

## 5. Resumo do Fluxo de Trabalho

1. **Google** identifica comportamento suspeito (login em domínio compartilhado).
2. **Desenvolvedor** prova a posse do site via **Tag HTML**.
3. **Google** valida a tag e libera o acesso aos relatórios de segurança.
4. **Desenvolvedor** solicita a reanálise humana/automatizada.
5. **Aviso é removido** (geralmente entre 24h e 72h).
