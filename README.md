# 📅 e-Agenda Web

[![Academia do Programador](https://img.shields.io/badge/Academia_do_Programador-0056b3?style=for-the-badge&logo=book&logoColor=white)](https://academiadoprogramador.net/inicio)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap_5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![Azure](https://img.shields.io/badge/Azure-0089D6?style=for-the-badge&logo=microsoft-azure&logoColor=white)

> O **e-Agenda Web** é uma aplicação completa para gestão pessoal e financeira. Desenvolvido com foco em alta coesão, baixo acoplamento e excelente experiência do usuário, o sistema permite o controle centralizado de tarefas, contatos, compromissos e despesas.

👨‍💻 **Desenvolvido por:** [Alec Luí](https://github.com/DrElucidator) e [Gustavo Tessaro](https://github.com/GustavoTessaro) (Equipe WriteLine("GreenLife Devs");)

---

## 🚀 Funcionalidades e Módulos

O sistema foi construído sob a arquitetura de **Monolito Modular**, garantindo que cada contexto de negócio seja isolado e independente:

### ✅ Módulo de Tarefas
- Criação de tarefas com definição de prioridade (Baixa, Normal, Alta).
- Gestão de **Itens de Tarefa (Subtarefas)** com relação 1:N.
- **Cálculo automático de progresso:** O percentual de conclusão é calculado dinamicamente em tempo real conforme os itens são marcados.
- Fechamento automático de status e data de conclusão ao atingir 100%.

### 👥 Módulo de Contatos
- Cadastro completo de contatos (Nome, Telefone, E-mail, Empresa e Cargo).
- Validações de formato e integridade de dados.
- Visualização em formato de cartões interativos.

### 🗓️ Módulo de Compromissos
- Agendamento de compromissos com data, hora de início e término.
- Vínculo opcional com Contatos cadastrados.
- **Prevenção de Conflitos:** O sistema impede o agendamento de compromissos no mesmo período de tempo.

### 💰 Módulo de Despesas e Categorias
- Gestão financeira com registro de despesas e formas de pagamento (Dinheiro, PIX, Cartão).
- **Lógica de Parcelamento:** Geração automática de múltiplas despesas futuras ao selecionar pagamento no Crédito.
- Categorização múltipla (N:N) para organização detalhada de gastos.

---

## 🏗️ Arquitetura e Padrões de Projeto

Este projeto foge do tradicional padrão anêmico e adota práticas avançadas de Engenharia de Software:

*   **Domain-Driven Design (DDD):** Entidades ricas com construtores blindados, validações internas e comportamentos encapsulados.
*   **Padrão MVC (Model-View-Controller):** Separação clara entre a interface do usuário, o roteamento e a lógica de apresentação.
*   **Camada de Aplicação (Serviços e DTOs):** Orquestração de regras de negócio utilizando `FluentResults` para tratamento elegante de erros, eliminando o uso excessivo de *Exceptions*.
*   **Padrão Repository:** Abstração do acesso a dados com interfaces genéricas (`IRepositorio<T>`).
*   **Dapper & SQL Transactions:** Consultas otimizadas e transações atômicas para garantir a integridade de dados em operações complexas (ex: salvar Tarefa + Itens).
*   **AutoMapper:** Mapeamento automático e seguro entre DTOs, ViewModels e Entidades.

---

## 🛠️ Tecnologias Utilizadas

*   **Backend:** C# 12, .NET 8, ASP.NET Core MVC
*   **Banco de Dados:** SQL Server (Projeto `.sqlproj` integrado)
*   **Micro-ORM:** Dapper
*   **Bibliotecas:** FluentResults, AutoMapper
*   **Frontend:** HTML5, CSS3, Bootstrap 5, Razor Pages
*   **CI/CD:** GitHub Actions com deploy automatizado para o Microsoft Azure.

---

## 📄 Licença
* Este projeto foi desenvolvido para fins acadêmicos e de portfólio. Todos os direitos reservados aos autores Alec Luí e Gustavo Tessaro.