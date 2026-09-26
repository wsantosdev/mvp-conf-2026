# MVP Conf 2026 — Ports, Adapters & Vertical Slices

Este repositório contém o projeto de demonstração da palestra **Ports, Adapters & Vertical Slices**, apresentada no **MVP Conf 2026**. A solução implementa uma pequena lista de tarefas para mostrar, de forma incremental, como as decisões de organização do código influenciam acoplamento, coesão, testabilidade e evolução de uma aplicação.

O domínio é intencionalmente simples: uma tarefa pode ser criada, concluída ou cancelada. A simplicidade do problema permite concentrar a discussão nos padrões arquiteturais, sem esconder as decisões importantes atrás de regras de negócio complexas ou de uma infraestrutura extensa.

## O que a solução demonstra

O mesmo caso de uso é implementado por duas abordagens:

- **`TodoList.Layered`** — uma organização tradicional em camadas, com endpoints, serviço, repositório e domínio.
- **`TodoList.Modern`** — uma organização por funcionalidades, combinando **Ports & Adapters** com **Vertical Slices**.

As duas aplicações expõem os mesmos endpoints:

| Operação | Método | Rota |
| --- | --- | --- |
| Criar tarefa | `POST` | `/api/v1/todos` |
| Cancelar tarefa | `POST` | `/api/v1/todos/{id}/cancel` |
| Concluir tarefa | `POST` | `/api/v1/todos/{id}/complete` |

O armazenamento utilizado nas duas versões é em memória, por meio de `ConcurrentDictionary`. Ele funciona como um detalhe de infraestrutura substituível e mantém o foco da demonstração nos limites arquiteturais.

## Abordagem em camadas

O projeto `TodoList.Layered` separa o código por responsabilidades técnicas:

```text
TodoList.Layered/
├── Domain/
├── Endpoints/
├── Service/
└── Infrastructure/
    └── Database/
```

O fluxo parte dos endpoints, passa pelo `TodoService` e chega ao `ITodoRepository`. Essa estrutura é familiar e pode ser adequada para sistemas pequenos, mas tende a concentrar funcionalidades diferentes nas mesmas camadas. Conforme a aplicação cresce, uma alteração em um caso de uso pode exigir navegar por vários diretórios e tipos relacionados apenas indiretamente.

Nesta versão, parte das regras de negócio fica no serviço: validação do título, busca da tarefa e validação das transições de estado. O `TodoItem` é um modelo com propriedades públicas, e o repositório abstrai o armazenamento.

## Ports & Adapters

No projeto `TodoList.Modern`, a aplicação depende de contratos — as **ports** — e não conhece a implementação concreta do armazenamento:

```text
Features/CreateTask/ICreateTaskPort.cs
Features/CompleteTask/ICompleteTaskPort.cs
Features/CancelTask/ICancelTaskPort.cs
Infrastructure/TodoStorageAdapter.cs
```

As interfaces definem o que cada caso de uso precisa para persistir ou recuperar dados. `TodoStorageAdapter` é um **adapter de saída** que implementa essas interfaces usando armazenamento em memória. A composição das dependências acontece no `Program.cs`.

Esse limite permite trocar o adapter por um banco de dados, uma API externa ou um fake de teste sem alterar os handlers. Os testes unitários aproveitam exatamente essa fronteira: cada handler recebe uma implementação de port específica, sem depender da infraestrutura real.

O domínio também protege suas próprias invariantes. `TodoItem` controla a criação e expõe os comportamentos `Complete()` e `Cancel()`, impedindo, por exemplo, concluir uma tarefa cancelada ou cancelar uma tarefa já concluída.

## Vertical Slices

Em vez de agrupar todos os endpoints, serviços e contratos por camada, a versão moderna organiza o código por capacidade de negócio:

```text
TodoList.Modern/
├── Domain/
├── Features/
│   ├── CreateTask/
│   │   ├── CreateTaskCommand.cs
│   │   ├── CreateTaskEndpoint.cs
│   │   ├── CreateTaskHandler.cs
│   │   └── ICreateTaskPort.cs
│   ├── CompleteTask/
│   │   ├── CompleteTaskCommand.cs
│   │   ├── CompleteTaskEndpoint.cs
│   │   ├── CompleteTaskHandler.cs
│   │   └── ICompleteTaskPort.cs
│   └── CancelTask/
│       ├── CancelTaskCommand.cs
│       ├── CancelTaskEndpoint.cs
│       ├── CancelTaskHandler.cs
│       └── ICancelTaskPort.cs
└── Infrastructure/
    └── TodoStorageAdapter.cs
```

Cada **vertical slice** reúne os elementos necessários para um fluxo completo:

1. O endpoint traduz a requisição HTTP e define a resposta.
2. O command representa a intenção de negócio.
3. O handler coordena o caso de uso.
4. A port expressa a dependência externa necessária pela funcionalidade.
5. O domínio executa e protege as regras da entidade.

Essa organização favorece alta coesão dentro de cada funcionalidade e torna mais direto localizar, testar e alterar um fluxo. Ela não significa ausência de abstrações: as abstrações continuam existindo, mas são colocadas junto do caso de uso que depende delas, em vez de serem reunidas em uma camada técnica global.

## Testes

Os projetos de teste acompanham as duas abordagens:

- `TodoList.Layered.Tests` testa o serviço e os fluxos HTTP da versão em camadas.
- `TodoList.Modern.Tests` testa cada slice (`CreateTask`, `CompleteTask` e `CancelTask`) e seus fluxos HTTP.

Além dos caminhos felizes, os testes cobrem títulos vazios, tarefas inexistentes e transições de estado inválidas. A versão moderna evidencia o isolamento proporcionado pelas ports, usando implementações de teste dos contratos de cada funcionalidade.

## Tecnologias

- .NET 10
- ASP.NET Core Minimal APIs
- C#
- OpenAPI em ambiente de desenvolvimento
- xUnit

## Como executar

Na raiz do repositório, restaure, compile e execute os testes:

```bash
dotnet restore
dotnet build
dotnet test
```

Para iniciar uma das APIs:

```bash
dotnet run --project src/TodoList.Layered
dotnet run --project src/TodoList.Modern
```

Durante o desenvolvimento, a especificação OpenAPI é disponibilizada pela aplicação. Também há um arquivo `.http` no projeto em camadas com exemplos de chamadas para os endpoints.

## Estrutura da solução

```text
mvp-conf-2026.slnx
└── src/
    ├── TodoList.Layered/
    ├── TodoList.Layered.Tests/
    ├── TodoList.Modern/
    └── TodoList.Modern.Tests/
```

O objetivo deste código não é indicar que uma organização é universalmente superior à outra. A demonstração mostra como partir de uma estrutura em camadas e, quando a complexidade do produto exigir, aproximar cada caso de uso de seus contratos, regras e testes — mantendo dependências externas nas bordas por meio de ports e adapters.
