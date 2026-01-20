# Modelagem UML

Esta seção apresenta os diagramas da **Unified Modeling Language (UML)** desenvolvidos para o projeto.

## Diagrama de Casos de Uso

O diagrama de casos de uso ilustra as interações entre os atores e o sistema, refletindo o escopo funcional definido anteriormente.

![Diagrama de Caso de Uso](assets/diagramaCasoDeUso.svg)

<details><summary>Mostrar código PlantUML</summary>

```UML
@startuml
left to right direction
skinparam packageStyle rectangle

' Definição do Ator conforme documento
actor "Usuário" as U

package "Miles Manager (Sistema de Gestão)" {
    ' Autenticação e Segurança
    usecase "Autenticar Usuário (Login)\n(RF-001)" as RF001
    usecase "Fazer Logout\n(RF-009)" as RF009

    ' CRUDs (Manter)
    usecase "Manter Usuário\n(RF-002)" as RF002
    usecase "Manter Cartões\n(RF-003)" as RF003
    usecase "Manter Programas\n(RF-004)" as RF004

    ' Processos de Negócio
    usecase "Registrar Transação\n(RF-005)" as RF005
    usecase "Visualizar Dashboard\n(RF-007)" as RF007

    ' Funcionalidades Internas (Reutilizáveis)
    usecase "Calcular Pontos Auto.\n(RF-006)" as RF006
    usecase "Validar Dados\n(RF-008)" as RF008
}

' Interações do Ator (Usuário inicia estes processos)
U --> RF001
U --> RF009
U --> RF002
U --> RF003
U --> RF004
U --> RF005
U --> RF007

' Relacionamentos de Inclusão (Obrigatórios)
' O cálculo é parte obrigatória do registro da transação [RF-006]
RF005 ..> RF006 : <<include>>

' A validação é obrigatória em todos os formulários [RF-008]
RF002 ..> RF008 : <<include>>
RF003 ..> RF008 : <<include>>
RF004 ..> RF008 : <<include>>
RF005 ..> RF008 : <<include>>

@enduml
```

</details><br/>

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

## Diagrama Conceitual

O Diagrama Conceitual ilustra os conceitos fundamentais do domínio do problema sob a ótica do analista de negócios.

![Diagrama de Caso de Uso](assets/diagramaConceitual.svg)

<details><summary>Mostrar código PlantUML</summary>

```UML
@startuml

' Configurações visuais para parecer um Modelo Conceitual (sem ícones de visibilidade ou métodos)
hide methods
hide circle
skinparam classAttributeIconSize 0
skinparam linetype ortho

title Modelo Conceitual - Miles Manager

class Usuario {
    nome : String
    email : String
    senha : String
}

class Cartao {
    nome : String
    bandeira : String
    limite : Double
    diaVencimento : Integer
    fatorConversao : Double
}

class ProgramaFidelidade {
    nome : String
    bancoResponsavel : String
}

class Transacao {
    data : Date
    valor : Double
    descricao : String
    categoria : String
    cotacaoDolar : Double
    pontosEstimados : Integer
}

' Relacionamentos (Associações)

' 1 Usuário possui N Cartões
Usuario "1" -- "0..*" Cartao : possui >

' 1 Cartão pertence a 1 Programa de Fidelidade (considerando que o cartão pontua em um programa específico)
Cartao "0..*" -- "1" ProgramaFidelidade : vinculado a >

' 1 Cartão possui N Transações
Cartao "1" -- "0..*" Transacao : gera >

note right of Cartao::fatorConversao
  Simplificação MVP:
  Regra de conversão (Pontos por Dólar)
  fica no Cartão.
end note

note right of Transacao::pontosEstimados
  Valor calculado e persistido
  (RF-006)
end note

note right of Transacao::cotacaoDolar
  Dólar do dia (Auditoria)
end note

@enduml
```

</details><br/>
