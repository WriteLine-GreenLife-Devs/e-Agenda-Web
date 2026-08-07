# Casos de Teste — eAgenda Web 2026

> **Total geral:** 132 testes (85 domínio + 47 serviço)
> **Dev A:** 64 testes (34 domínio + 30 serviço)
> **Dev B:** 68 testes (51 domínio + 17 serviço)

---

## 📋 Sumário Geral

### Domínio

| Módulo         | Planilha     | Extras       | Total        |
| --------------- | ------------ | ------------ | ------------ |
| Categorias      | 7            | +1           | 8            |
| Contatos        | 13           | +1           | 14           |
| Tarefas         | 11           | +1           | 12           |
| Compromissos    | 21           | +1           | 22           |
| Despesas        | 15           | +1           | 16           |
| Itens de Tarefa | 11           | +2           | 13           |
| **Total** | **78** | **+7** | **85** |

### Serviço

| Módulo         | Planilha     | Extras AP     | Total        |
| --------------- | ------------ | ------------- | ------------ |
| Categorias      | 6            | +3            | 9            |
| Contatos        | 8            | +3            | 11           |
| Tarefas         | 7            | +3            | 10           |
| Compromissos    | 4            | +3            | 7            |
| Despesas        | 3            | +3            | 6            |
| Itens de Tarefa | 1            | +3            | 4            |
| **Total** | **29** | **+18** | **47** |

### Por Dev

| Dev             | Módulos                                | Domínio     | Serviço     | Total         |
| --------------- | --------------------------------------- | ------------ | ------------ | ------------- |
| A               | Categorias, Contatos, Tarefas           | 34           | 30           | 64            |
| B               | Compromissos, Despesas, Itens de Tarefa | 51           | 17           | 68            |
| **Geral** | —                                      | **85** | **47** | **132** |

---

## 🅰️ Dev A

### Domínio — Categorias (8 testes)

| # | Origem   | Caso de Teste                                                                                      | Método       |
| - | -------- | -------------------------------------------------------------------------------------------------- | ------------- |
| 1 | Planilha | Cadastrar categoria com dados válidos                                                             | `Validar`   |
| 2 | Planilha | Cadastrar categoria sem informar o título                                                         | `Validar`   |
| 3 | Planilha | Cadastrar categoria com título abaixo do mínimo (1 caractere)                                    | `Validar`   |
| 4 | Planilha | Cadastrar categoria com título no limite mínimo (2 caracteres)                                   | `Validar`   |
| 5 | Planilha | Cadastrar categoria com título no limite máximo (100 caracteres)                                 | `Validar`   |
| 6 | Planilha | Cadastrar categoria com título acima do máximo (101 caracteres)                                  | `Validar`   |
| 7 | Planilha | Editar categoria com dados válidos                                                                | `Atualizar` |
| 8 | Extra    | Cadastrar categoria com título apenas com espaços (valida`IsNullOrWhiteSpace` ≠ string vazia) | `Validar`   |

### Serviço — Categorias (9 testes)

| # | Origem   | Caso de Teste                                    | Método do Serviço |
| - | -------- | ------------------------------------------------ | ------------------- |
| 1 | Planilha | Cadastrar categoria com título duplicado        | `Cadastrar`       |
| 2 | Planilha | Editar categoria para título já existente      | `Editar`          |
| 3 | Planilha | Visualizar despesas de uma categoria específica | `SelecionarPorId` |
| 4 | Planilha | Listar todas as categorias cadastradas           | `SelecionarTodos` |
| 5 | Planilha | Excluir categoria sem despesas vinculadas        | `Excluir`         |
| 6 | Planilha | Excluir categoria vinculada a despesas           | `Excluir`         |
| 7 | Extra AP | Editar categoria inexistente                     | `Editar`          |
| 8 | Extra AP | Excluir categoria inexistente                    | `Excluir`         |
| 9 | Extra AP | SelecionarPorId de categoria inexistente         | `SelecionarPorId` |

---

### Domínio — Contatos (14 testes)

| #  | Origem   | Caso de Teste                                                                  | Método               |
| -- | -------- | ------------------------------------------------------------------------------ | --------------------- |
| 1  | Planilha | Cadastrar contato com todos os campos preenchidos                              | `Validar`           |
| 2  | Planilha | Cadastrar contato apenas com os campos obrigatórios                           | `Validar`           |
| 3  | Planilha | Cadastrar contato com campos obrigatórios em branco                           | `Validar`           |
| 4  | Planilha | Cadastrar contato com nome abaixo do mínimo (1 caractere)                     | `Validar`           |
| 5  | Planilha | Cadastrar contato com nome no limite mínimo (2 caracteres)                    | `Validar`           |
| 6  | Planilha | Cadastrar contato com nome no limite máximo (100 caracteres)                  | `Validar`           |
| 7  | Planilha | Cadastrar contato com nome acima do máximo (101 caracteres)                   | `Validar`           |
| 8  | Planilha | Cadastrar contato com e-mail em formato inválido                              | `Validar`           |
| 9  | Planilha | Cadastrar contato com e-mail sem domínio                                      | `Validar`           |
| 10 | Planilha | Cadastrar contato com telefone fixo em formato válido (10 dígitos)           | `VerificarTelefone` |
| 11 | Planilha | Cadastrar contato com telefone celular em formato válido (11 dígitos)        | `VerificarTelefone` |
| 12 | Planilha | Cadastrar contato com telefone em formato inválido                            | `Validar`           |
| 13 | Planilha | Editar contato com dados válidos                                              | `Atualizar`         |
| 14 | Extra    | Verificar e-mail inválido diretamente (testa regex sem passar por`Validar`) | `VerificarEmail`    |

### Serviço — Contatos (11 testes)

| #  | Origem   | Caso de Teste                                              | Método do Serviço |
| -- | -------- | ---------------------------------------------------------- | ------------------- |
| 1  | Planilha | Cadastrar contato com e-mail duplicado                     | `Cadastrar`       |
| 2  | Planilha | Cadastrar contato com telefone duplicado                   | `Cadastrar`       |
| 3  | Planilha | Editar contato para e-mail já utilizado por outro contato | `Editar`          |
| 4  | Planilha | Editar contato mantendo o próprio e-mail e telefone       | `Editar`          |
| 5  | Planilha | Visualizar dados de um contato                             | `SelecionarPorId` |
| 6  | Planilha | Listar todos os contatos cadastrados                       | `SelecionarTodos` |
| 7  | Planilha | Excluir contato sem compromissos vinculados                | `Excluir`         |
| 8  | Planilha | Excluir contato com compromissos vinculados                | `Excluir`         |
| 9  | Extra AP | Editar contato inexistente                                 | `Editar`          |
| 10 | Extra AP | Excluir contato inexistente                                | `Excluir`         |
| 11 | Extra AP | SelecionarPorId de contato inexistente                     | `SelecionarPorId` |

---

### Domínio — Tarefas (13 testes)

| #  | Origem   | Caso de Teste                                                                             | Método                       |
| -- | -------- | ----------------------------------------------------------------------------------------- | ----------------------------- |
| 1  | Planilha | Cadastrar tarefa com dados válidos                                                       | `Validar`                   |
| 2  | Planilha | Tarefa recém-cadastrada nasce pendente, com 0% e sem data de conclusão                  | Construtor                    |
| 3  | Planilha | Cadastrar tarefa sem itens                                                                | Construtor                    |
| 4  | Planilha | Cadastrar tarefa já com itens                                                            | Construtor /`AdicionarItem` |
| 5  | Planilha | Cadastrar tarefa com campos obrigatórios em branco                                       | `Validar`                   |
| 6  | Planilha | Cadastrar tarefa com título abaixo do mínimo (1 caractere)                              | `Validar`                   |
| 7  | Planilha | Cadastrar tarefa com título no limite máximo (100 caracteres)                           | `Validar`                   |
| 8  | Planilha | Cadastrar tarefa com título acima do máximo (101 caracteres)                            | `Validar`                   |
| 9  | Planilha | Editar tarefa com dados válidos                                                          | `Atualizar`                 |
| 10 | Planilha | Concluir tarefa registra data de conclusão e status                                      | `Concluir`                  |
| 11 | Planilha | Reabrir tarefa concluída                                                                 | `Reabrir`                   |
| 12 | Extra    | Cadastrar tarefa com título no limite mínimo (2 caracteres) — planilha só tem máximo | `Validar`                   |

### Serviço — Tarefas (9 testes)

| #   | Origem   | Caso de Teste                               | Método do Serviço               |
| --- | -------- | ------------------------------------------- | --------------------------------- |
| 1   | Planilha | Listar todas as tarefas cadastradas         | `SelecionarTodos`               |
| 2   | Planilha | Listar apenas as tarefas pendentes          | `SelecionarTodos` (filtro)      |
| 3   | Planilha | Listar apenas as tarefas concluídas        | `SelecionarTodos` (filtro)      |
| 4   | Planilha | Visualizar tarefas agrupadas por prioridade | `SelecionarTodos` (agrupamento) |
| 5   | Planilha | Visualizar dados de uma tarefa e seus itens | `SelecionarPorId`               |
| 6   | Planilha | Excluir tarefa e seus itens vinculados      | `Excluir`                       |
| 7   | Planilha | Informar prioridade fora da lista permitida | `Cadastrar`                     |
| 8   | Extra AP | Editar tarefa inexistente                   | `Editar`                        |
| 9   | Extra AP | Excluir tarefa inexistente                  | `Excluir`                       |
| 10  | Extra AP | SelecionarPorId de tarefa inexistente       | `SelecionarPorId`               |

---

## 🅱️ Dev B

### Domínio — Compromissos (22 testes)

| #  | Origem   | Caso de Teste                                                                                  | Método               |
| -- | -------- | ---------------------------------------------------------------------------------------------- | --------------------- |
| 1  | Planilha | Cadastrar compromisso presencial com dados válidos                                            | `Validar`           |
| 2  | Planilha | Cadastrar compromisso remoto com dados válidos                                                | `Validar`           |
| 3  | Planilha | Cadastrar compromisso sem vincular contato                                                     | `Validar`           |
| 4  | Planilha | Cadastrar compromisso vinculado a um contato                                                   | `Validar`           |
| 5  | Planilha | Cadastrar compromisso com campos obrigatórios em branco                                       | `Validar`           |
| 6  | Planilha | Cadastrar compromisso com assunto abaixo do mínimo (1 caractere)                              | `Validar`           |
| 7  | Planilha | Cadastrar compromisso com assunto no limite máximo (100 caracteres)                           | `Validar`           |
| 8  | Planilha | Cadastrar compromisso com assunto acima do máximo (101 caracteres)                            | `Validar`           |
| 9  | Planilha | Cadastrar compromisso presencial sem informar o local                                          | `Validar`           |
| 10 | Planilha | Cadastrar compromisso remoto sem informar o link                                               | `Validar`           |
| 11 | Planilha | Cadastrar compromisso remoto com link em formato inválido                                     | `Validar`           |
| 12 | Planilha | Cadastrar compromisso com hora de término anterior à de início                              | `Validar`           |
| 13 | Planilha | Cadastrar compromisso com hora de término igual à de início                                 | `Validar`           |
| 14 | Planilha | Cadastrar compromisso com sobreposição parcial de horário                                   | `VerificarConflito` |
| 15 | Planilha | Cadastrar compromisso totalmente contido em outro existente                                    | `VerificarConflito` |
| 16 | Planilha | Cadastrar compromisso que engloba outro existente                                              | `VerificarConflito` |
| 17 | Planilha | Cadastrar compromisso imediatamente após outro (limite sem sobreposição)                    | `VerificarConflito` |
| 18 | Planilha | Cadastrar compromisso no mesmo horário em data diferente                                      | `VerificarConflito` |
| 19 | Planilha | Editar compromisso com dados válidos                                                          | `Atualizar`         |
| 20 | Planilha | Alterar tipo de Presencial para Remoto                                                         | `Atualizar`         |
| 21 | Planilha | Excluir compromisso cadastrado                                                                 | Comportamento         |
| 22 | Extra    | Cadastrar compromisso com assunto no limite mínimo (2 caracteres) — planilha só tem máximo | `Validar`           |

### Serviço — Compromissos (7 testes)

| # | Origem   | Caso de Teste                                           | Método do Serviço |
| - | -------- | ------------------------------------------------------- | ------------------- |
| 1 | Planilha | Editar compromisso gerando conflito com outro existente | `Editar`          |
| 2 | Planilha | Editar compromisso mantendo o próprio horário         | `Editar`          |
| 3 | Planilha | Visualizar dados de um compromisso                      | `SelecionarPorId` |
| 4 | Planilha | Listar todos os compromissos cadastrados                | `SelecionarTodos` |
| 5 | Extra AP | Editar compromisso inexistente                          | `Editar`          |
| 6 | Extra AP | Excluir compromisso inexistente                         | `Excluir`         |
| 7 | Extra AP | SelecionarPorId de compromisso inexistente              | `SelecionarPorId` |

---

### Domínio — Despesas (16 testes)

| #  | Origem   | Caso de Teste                                                                                  | Método       |
| -- | -------- | ---------------------------------------------------------------------------------------------- | ------------- |
| 1  | Planilha | Cadastrar despesa com dados válidos e uma categoria                                           | `Validar`   |
| 2  | Planilha | Cadastrar despesa vinculada a múltiplas categorias                                            | `Validar`   |
| 3  | Planilha | Cadastrar despesa sem informar a data de ocorrência (default = hoje)                          | Construtor    |
| 4  | Planilha | Cadastrar despesa com campos obrigatórios em branco                                           | `Validar`   |
| 5  | Planilha | Cadastrar despesa com descrição abaixo do mínimo (1 caractere)                              | `Validar`   |
| 6  | Planilha | Cadastrar despesa com descrição no limite máximo (100 caracteres)                           | `Validar`   |
| 7  | Planilha | Cadastrar despesa com descrição acima do máximo (101 caracteres)                            | `Validar`   |
| 8  | Planilha | Cadastrar despesa com valor igual a zero                                                       | `Validar`   |
| 9  | Planilha | Cadastrar despesa com valor negativo                                                           | `Validar`   |
| 10 | Planilha | Cadastrar despesa com valor decimal (duas casas)                                               | `Validar`   |
| 11 | Planilha | Cadastrar despesa sem selecionar forma de pagamento                                            | `Validar`   |
| 12 | Planilha | Informar forma de pagamento fora da lista permitida                                            | `Validar`   |
| 13 | Planilha | Cadastrar despesa sem selecionar nenhuma categoria                                             | `Validar`   |
| 14 | Planilha | Editar despesa alterando valor e categorias                                                    | `Atualizar` |
| 15 | Planilha | Excluir despesa cadastrada                                                                     | Comportamento |
| 16 | Extra    | Cadastrar despesa com descrição no limite mínimo (2 caracteres) — planilha só tem máximo | `Validar`   |

### Serviço — Despesas (6 testes)

| # | Origem   | Caso de Teste                                          | Método do Serviço |
| - | -------- | ------------------------------------------------------ | ------------------- |
| 1 | Planilha | Remover todas as categorias de uma despesa na edição | `Editar`          |
| 2 | Planilha | Visualizar dados de uma despesa                        | `SelecionarPorId` |
| 3 | Planilha | Listar todas as despesas cadastradas                   | `SelecionarTodos` |
| 4 | Extra AP | Editar despesa inexistente                             | `Editar`          |
| 5 | Extra AP | Excluir despesa inexistente                            | `Excluir`         |
| 6 | Extra AP | SelecionarPorId de despesa inexistente                 | `SelecionarPorId` |

---

### Domínio — Itens de Tarefa (13 testes)

| #  | Origem   | Caso de Teste                                                                                  | Método                                |
| -- | -------- | ---------------------------------------------------------------------------------------------- | -------------------------------------- |
| 1  | Planilha | Adicionar item sem informar o título                                                          | `Validar`                            |
| 2  | Planilha | Adicionar item com título abaixo do mínimo (1 caractere)                                     | `Validar`                            |
| 3  | Planilha | Adicionar item com título acima do máximo (101 caracteres)                                   | `Validar`                            |
| 4  | Planilha | Concluir um item atualiza o percentual da tarefa                                               | `Concluir` / `AtualizarPercentual` |
| 5  | Planilha | Concluir todos os itens leva a tarefa a 100%                                                   | `Concluir` / `AtualizarPercentual` |
| 6  | Planilha | Reabrir um item concluído reduz o percentual                                                  | `Reabrir` / `AtualizarPercentual`  |
| 7  | Planilha | Remover um item recalcula o percentual da tarefa                                               | `Remover` / `AtualizarPercentual`  |
| 8  | Planilha | Remover o último item de uma tarefa                                                           | `Remover` / `AtualizarPercentual`  |
| 9  | Planilha | Editar o título de um item existente                                                          | `Atualizar`                          |
| 10 | Planilha | Adicionar item a uma tarefa existente                                                          | `AdicionarItem`                      |
| 11 | Planilha | Adicionar item sem vínculo com uma tarefa                                                     | Construtor /`Validar`                |
| 12 | Extra    | Adicionar item com título no limite mínimo (2 caracteres) — planilha só tem abaixo/acima   | `Validar`                            |
| 13 | Extra    | Adicionar item com título no limite máximo (100 caracteres) — planilha só tem abaixo/acima | `Validar`                            |

### Serviço — Itens de Tarefa (4 testes)

| # | Origem   | Caso de Teste                       | Método do Serviço              |
| - | -------- | ----------------------------------- | -------------------------------- |
| 1 | Planilha | Listar os itens de uma tarefa       | `SelecionarTodos` (por tarefa) |
| 2 | Extra AP | Editar item inexistente             | `Editar`                       |
| 3 | Extra AP | Excluir item inexistente            | `Excluir`                      |
| 4 | Extra AP | SelecionarPorId de item inexistente | `SelecionarPorId`              |

---

## 📐 Padrões e Convenções

### Nomenclatura

```csharp
{Metodo}{Condicao}{ResultadoEsperado}
```

Exemplos:

- `Validar_ComTituloVazio_DeveRetornarErros`
- `Cadastrar_ComTituloDuplicado_DeveRetornarFalha`
- `Atualizar_DeveAtualizarTodosCampos`

### Estrutura de Arquivos

tests/eAgenda.Testes.Unidade/
├── Modulos/
│   ├── ModuloCategorias/
│   │   ├── CategoriaTests.cs          (domínio)
│   │   └── ServicoCategoriaTests.cs   (serviço)
│   ├── ModuloContatos/
│   │   ├── ContatoTests.cs
│   │   └── ServicoContatoTests.cs
│   ├── ModuloTarefas/
│   │   ├── TarefaTests.cs
│   │   └── ServicoTarefaTests.cs
│   ├── ModuloCompromissos/
│   │   ├── CompromissoTests.cs
│   │   └── ServicoCompromissoTests.cs
│   ├── ModuloDespesas/
│   │   ├── DespesaTests.cs
│   │   └── ServicoDespesaTests.cs
│   └── ModuloItensTarefa/
│       ├── ItemTarefaTests.cs
│       └── ServicoItemTarefaTests.cs
├── eAgenda.Testes.Unidade.csproj
└── MSTestSettings.cs

### Padrão AAA (Arranjo / Ação / Asserção)

```csharp
[TestMethod]
public void Validar_ComTituloVazio_DeveRetornarErros()
{
    // Arranjo
    Categoria categoria = new Categoria(string.Empty);

    // Ação
    List<string> erros = categoria.Validar();

    // Asserção
    Assert.HasCount(2, erros);
    CollectionAssert.Contains(erros, "O título da categoria é obrigatório.");
}
```

### Os 3 extras obrigatórios por módulo no nível de serviço:

- Editar inexistente — serviço deve retornar falha quando id não existe no repositório
- Excluir inexistente — serviço deve retornar falha quando id não existe no repositório
- SelecionarPorId inexistente — serviço deve retornar falha quando id não existe no repositório

### Casos extras onde há "abaixo do mínimo" e "acima do máximo" mas não testa os limites exatos (mínimo e máximo válidos). Todo campo com validação de tamanho precisa de ambos:

|     Módulo     |    Campo    |                  Limite Ausente                  |
| :-------------: | :---------: | :----------------------------------------------: |
|   Categorias   |   Título   |  Apenas espaços (IsNullOrWhiteSpace ≠ vazio)  |
|    Contatos    |            | VerificarEmail testado diretamente (sem Validar) |
|     Tarefas     |   Título   |          Limite mínimo (2 caracteres)          |
|  Compromissos  |   Assunto   |          Limite mínimo (2 caracteres)          |
|    Despesas    | Descrição |          Limite mínimo (2 caracteres)          |
| Itens de Tarefa |   Título   |    Limite mínimo (2) + limite máximo (100)    |
