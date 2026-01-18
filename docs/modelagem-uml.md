# Modelagem UML

Esta seção apresenta os diagramas da **Unified Modeling Language (UML)** desenvolvidos para o projeto.

## Diagrama de Casos de Uso

O diagrama de casos de uso ilustra as interações entre os atores e o sistema, refletindo o escopo funcional definido anteriormente.

![Diagrama de Caso de Uso](assets/diagramaCasoDeUso.svg)

A modelagem adotou as seguintes definições para a representação visual:

### Ator Principal (Primary Actor)

O **Usuário** é representado como o único ator do sistema.
Conforme a identificação de atores, ele é o responsável por alimentar a base de dados (transações e programas) e iniciar todos os fluxos de eventos presentes nos requisitos **RF-001** a **RF-009**.

#### Exclusão de Atores Secundários

O diagrama **não apresenta atores secundários** (como sistemas externos de envio de e-mail ou timers).
[cite_start]Esta decisão de modelagem deve-se à remoção de funcionalidades de automação passiva, como _“Notificações Push”_, do escopo deste **MVP**[cite: 55].
Dessa forma, não há interações ativas que justifiquem a representação de outros atores além do usuário final.

## Especificação de Casos de Uso (Fluxos de Eventos)

A seguir são detalhados os fluxos de interação para os casos de uso críticos do sistema, descrevendo o comportamento esperado nos cenários de sucesso (Caminho Feliz) e exceção.

### Especificação de Caso de Uso: Autenticar Usuário (Login)

**[CLIQUE AQUI PARA VER O UC-01](casos-de-uso/UC-01.md)**

### Especificação de Caso de Uso: Registrar Transação

**[CLIQUE AQUI PARA VER O UC-02](casos-de-uso/UC-02.md)**

### Especificação de Caso de Uso: Manter Cartões

**[CLIQUE AQUI PARA VER O UC-03](casos-de-uso/UC-03.md)**

### Especificação de Caso de Uso: Manter Programas

**[CLIQUE AQUI PARA VER O UC-04](casos-de-uso/UC-04.md)**

### Especificação de Caso de Uso: Manter Usuário (Perfil)

**[CLIQUE AQUI PARA VER O UC-05](casos-de-uso/UC-05.md)**

### Especificação de Caso de Uso: Visualizar Dashboard

**[CLIQUE AQUI PARA VER O UC-06](casos-de-uso/UC-06.md)**

### Especificação de Caso de Uso: Fazer Logout

**[CLIQUE AQUI PARA VER O UC-07](casos-de-uso/UC-07.md)**

### Casos de Uso Internos (Regras de Negócio)

_Estes casos descrevem processos internos reutilizáveis (Includes)._

**[CLIQUE AQUI PARA VER O UC-08 (Validar Dados)](casos-de-uso/UC-08.md)**
**[CLIQUE AQUI PARA VER O UC-09 (Calcular Pontos)](casos-de-uso/UC-09.md)**
