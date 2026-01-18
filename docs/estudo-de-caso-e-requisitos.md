### 1. Levantamento de Requisitos e Estudo de Caso

Este capítulo fundamenta o desenvolvimento do sistema **Miles Manager**, iniciando pela definição do problema de negócio e evoluindo para a especificação técnica das funcionalidades. Conforme preconizado por Brooks na Engenharia de Software, a compreensão clara do problema precede a codificação, garantindo que a solução desenvolvida atenda às necessidades reais de organização e integridade dos dados do usuário.

#### 1.1 Descrição do Estudo de Caso

**O Cenário Atual**
No contexto financeiro contemporâneo, os cartões de crédito transcenderam sua função original de meio de pagamento para se tornarem ferramentas de investimento pessoal através de Programas de Fidelidade (Milhas e Pontos). Consumidores utilizam múltiplas bandeiras e participam de diversos programas de companhias aéreas para maximizar seus retornos financeiros.

**O Problema de Negócio**
O problema central identificado é a **fragmentação da informação**. Atualmente, para obter uma visão consolidada de seus ativos, o usuário necessita acessar múltiplas plataformas bancárias e sites de companhias aéreas, realizando cálculos manuais e propensos a erros para estimar conversões de moeda e saldos totais.

Essa descentralização gera ineficiência operacional e riscos financeiros, uma vez que a falta de um registro unificado dificulta o acompanhamento de metas e o controle sobre quais compras já foram devidamente contabilizadas.

**A Solução: Miles Manager**
O projeto **Miles Manager** propõe-se a solucionar essas dores através de um **Sistema de Informação Gerencial** centralizado. O sistema atua como uma plataforma de registro e controle (CRUD), onde o usuário pode consolidar dados de diferentes cartões e programas em uma única base de dados confiável.

O foco principal da solução é garantir a **integridade e a organização dos dados**. Diferente de anotações em planilhas não padronizadas, o Miles Manager oferece interfaces validadas que impedem erros de digitação (máscaras e validações de tipos de dados), padronizam o registro de transações e automatizam o cálculo básico de conversão de pontos.

Desta forma, o sistema transforma dados dispersos em informação estruturada, permitindo ao usuário:

1. Manter um histórico seguro e editável de suas transações;
2. Visualizar saldos consolidados por programa de fidelidade;
3. Eliminar a incerteza no cálculo de conversão de gastos para pontos.

#### 1.1.1 Identificação dos Atores

Para fins de modelagem do sistema, foi identificado o ator principal que interage diretamente com as fronteiras do **Miles Manager**:

- **Usuário:** Representa o consumidor final, portador dos cartões de crédito e participante dos programas de fidelidade. Este ator é responsável por iniciar todos os processos de gestão (CRUD), como cadastrar cartões, registrar compras e consultar o dashboard de saldos.

### 1.2 Especificação de Requisitos de Software

A especificação a seguir utiliza o padrão de identificação único para garantir a rastreabilidade, classificando os requisitos em **Funcionais (RF)** — descrevendo os comportamentos e funções do sistema — e **Não Funcionais (RNF)** — descrevendo restrições tecnológicas e padrões de qualidade.

A priorização foi definida com base na metodologia **MoSCoW** (Must have, Should have, Could have, Won't have), focando no que é essencial para a avaliação da disciplina (CRUDs, Validações e Navegabilidade).

#### 1.2.1 Requisitos Funcionais (RF)

| ID         | Título do Requisito            | Descrição Detalhada                                                                                                                                                                                                      | Prioridade | Critério de Avaliação Associado    |
| ---------- | ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ---------- | ---------------------------------- |
| **RF-001** | Autenticação de Usuário        | O sistema deve prover um mecanismo de controle de acesso (Login) exigindo e-mail e senha, garantindo que apenas usuários autenticados acessem as áreas internas de gerenciamento.                                        | **Alta**   | Navegabilidade / Segurança         |
| **RF-002** | Manter Usuário (CRUD)          | O sistema deve permitir o cadastro de novos usuários, bem como a visualização e edição dos dados de perfil (Nome e Senha).                                                                                               | **Alta**   | CRUD / Formulários                 |
| **RF-003** | Manter Cartões (CRUD)          | O sistema deve permitir o registro, edição, visualização e exclusão de cartões de crédito, armazenando dados como Nome do Cartão, Bandeira (Visa/Master), Limite e Dia de Vencimento.                                    | **Alta**   | CRUD / Formulários                 |
| **RF-004** | Manter Programas de Fidelidade | O sistema deve permitir cadastrar e gerenciar os programas de milhas (ex: Smiles, Latam Pass, Livelo) vinculando-os aos cartões cadastrados.                                                                             | **Alta**   | CRUD / Relacionamento de Dados     |
| **RF-005** | Registrar Transação de Compra  | O sistema deve disponibilizar um formulário para lançamento de compras realizadas, capturando: Data, Valor (R$), Descrição, Categoria e Cartão utilizado.                                                                | **Alta**   | Formulários / Validações           |
| **RF-006** | Cálculo Automático de Pontos   | Ao registrar uma transação (RF-005), o sistema deve calcular automaticamente a estimativa de pontos gerados com base em uma regra de conversão (considerando cotação do dólar e fator do cartão) e persistir esse valor. | **Média**  | Regra de Negócio                   |
| **RF-007** | Dashboard de Saldos            | O sistema deve apresentar uma tela inicial (Dashboard) que exiba o somatório total de pontos acumulados agrupados por Programa de Fidelidade.                                                                            | **Média**  | Navegabilidade / Visualização      |
| **RF-008** | Validação de Dados de Entrada  | O sistema deve validar todos os formulários de entrada, impedindo o envio de campos vazios, datas futuras inválidas ou valores monetários negativos, exibindo mensagens de erro claras ao usuário.                       | **Alta**   | **Máscaras e Validações de Telas** |
| **RF-009** | Logout                         | O sistema deve permitir que o usuário encerre sua sessão de forma segura, redirecionando-o para a tela de login.                                                                                                         | **Alta**   | Navegabilidade                     |

#### 1.2.2 Requisitos Não Funcionais (RNF)

Estes requisitos definem a infraestrutura técnica necessária para suportar o sistema em ambiente .NET, substituindo a stack Java original para adequação ao ambiente de desenvolvimento da equipe.

| ID          | Título do Requisito               | Descrição Técnica                                                                                                                                                                 | Categoria      |
| ----------- | --------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------- |
| **RNF-001** | **Plataforma de Desenvolvimento** | O backend do sistema deve ser desenvolvido utilizando a linguagem **C#** e o framework **ASP.NET Core Blazor Server**, garantindo uma arquitetura moderna e unificada.            | Tecnologia     |
| **RNF-002** | **Persistência de Dados**         | O sistema deve utilizar um banco de dados relacional (SQL Server) gerenciado através do ORM **Entity Framework Core**.                                                            | Dados          |
| **RNF-003** | **Interface e Usabilidade**       | A interface deve ser responsiva e utilizar uma **Biblioteca de Componentes de UI** compatível com Blazor para garantir a padronização visual e a presença de máscaras de entrada. | Usabilidade    |
| **RNF-004** | **Formatação Regional (L10n)**    | O sistema deve respeitar as normas de formatação brasileira (pt-BR) para exibição de moeda (R$), datas e separadores decimais.                                                    | Padrão         |
| **RNF-005** | **Confiabilidade e Logs**         | O sistema deve possuir mecanismo de **registro de logs (em arquivo ou console)** para auditoria de erros e não deve expor falhas de código (_stack trace_) ao usuário final.      | Confiabilidade |

### Nota Explicativa sobre as Decisões de Requisitos

Para fins de avaliação acadêmica, conforme detalhado no documento "Trabalho para segunda unidade", optou-se por focar na robustez das operações **CRUD (Create, Read, Update, Delete)** e na integridade das validações de formulário.

Funcionalidades de alta complexidade técnica e baixo valor para os critérios de avaliação atuais, como "Notificações Push" e "Upload de Imagens", foram removidas do escopo desta versão (MVP 1.0) para garantir que os requisitos **RF-005** e **RF-008** (que correspondem a 50% da nota prática) sejam implementados com excelência utilizando a tecnologia Microsoft .NET.
