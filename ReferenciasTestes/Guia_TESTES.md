# Guia de testes automatizados — eAgenda Web

## 1. Casos de teste

Os casos e níveis de cobertura são definidos na planilha `Casos de Teste e-Agenda 2026.xlsx`, nesta mesma pasta.

| Módulo | Casos | Domínio | Casos de uso | Integração | E2E |
|---|---:|---:|---:|---:|---:|
| Contatos | 21 | 14 | 12 | 11 | 7 |
| Compromissos | 25 | 21 | 17 | 16 | 11 |
| Categorias | 13 | 8 | 9 | 8 | 7 |
| Despesas | 18 | 15 | 11 | 8 | 9 |
| Tarefas | 18 | 12 | 15 | 13 | 10 |
| Itens de Tarefa | 12 | 11 | 10 | 8 | 5 |
| **Total** | **107** | **81** | **74** | **64** | **49** |

As quatro últimas colunas representam marcações de cobertura. Um caso pode ser exercitado em mais de um nível, portanto essas quantidades não devem ser somadas como casos funcionais diferentes.

Testes extras podem ser adicionados para limites, regressões e comportamentos auxiliares, desde que não substituam os casos indicados na planilha.

## 2. Estrutura esperada

```text
Tests/
├── eAgenda.Testes.Unidade/
│   ├── Modulos/
│   └── MSTestSettings.cs
├── eAgenda.Testes.Integracao/
│   ├── Compartilhado/
│   ├── Modulos/
│   └── MSTestSettings.cs
└── eAgenda.Testes.E2E/
    ├── Compartilhado/
    ├── Modulos/
    ├── MSTestSettings.cs
    └── playwright.runsettings
```

O E-Agenda usa Dapper e SQL Server. Os testes de integração devem executar os repositórios e scripts de `eAgendaWeb.DataBase` em um banco separado para testes.

## 3. Responsabilidade de cada nível

### Unidade — domínio

- Entidades isoladas.
- Validações de campos e limites.
- Cálculos e transições de estado.
- Sem acesso ao banco ou mocks de infraestrutura.

### Unidade — casos de uso

- Serviços de aplicação.
- Repositórios substituídos por mocks ou fakes.
- Regras que dependem de dados previamente cadastrados.
- Verificação do resultado e das interações relevantes com os repositórios.

### Integração

- Repositórios Dapper executados contra SQL Server de teste.
- Scripts reais de criação das tabelas.
- Consultas, filtros, relacionamentos e restrições do banco.
- Isolamento e limpeza dos dados entre testes.

### E2E

- Aplicação iniciada em ambiente de testes.
- Playwright e Page Objects.
- Jornadas completas marcadas na planilha.

## 4. Organização dos testes

Os arquivos devem ficar agrupados por módulo:

```text
Modulos/
├── ModuloCategoria/
│   ├── CategoriaTests.cs
│   └── ServicoCategoriaTests.cs
├── ModuloContato/
│   ├── ContatoTests.cs
│   └── ServicoContatoTests.cs
└── ModuloTarefa/
    ├── TarefaTests.cs
    └── ServicoTarefaTests.cs
```

Os demais módulos devem seguir a mesma organização.
## 5. Convenções

### Nomenclatura

```csharp
Metodo_Condicao_ResultadoEsperado
```

Exemplos:

- `Validar_ComTituloVazio_DeveRetornarErros`
- `Cadastrar_ComTituloDuplicado_DeveRetornarFalha`
- `Excluir_ComVinculos_DeveRetornarFalha`

### Padrão AAA

```csharp
[TestMethod]
public void Validar_ComTituloVazio_DeveRetornarErros()
{
    // Arranjo
    Categoria categoria = new(string.Empty);

    // Ação
    List<string> erros = categoria.Validar();

    // Asserção
    Assert.HasCount(2, erros);
    CollectionAssert.Contains(erros, "O título da categoria é obrigatório.");
}
```

As asserções devem conferir o comportamento esperado, incluindo mensagens, valores alterados e interações relevantes quando aplicável.

## 6. Divisão dos módulos

### Dev A

- Contatos
- Categorias
- Tarefas

### Dev B

- Compromissos
- Despesas
- Itens de Tarefa

Cada módulo deve considerar os níveis indicados na planilha: domínio, casos de uso, integração e E2E.
