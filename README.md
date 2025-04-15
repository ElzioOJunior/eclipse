# ECLIPSE WORKS
## Informações sobre o projeto
Foi utilizado o banco de dados PostgreSQL e versão do .Net Core 8.0

Usar o comando docker compose up para rodar a aplicação e o banco de dados via docker.

Caso o projeto seja executado via Dokcer, alterar a connection string para --> Host=db;Database=EclipseDb;Username=postgres;Password=admin
Caso rode via Visual Studio, manter --> Host=localhost;Database=EclipseDb;Username=postgres;Password=admin

A migration para montagem do banco é executada automaticamente ao rodar o projeto via classe Program.cs.
Como não temos crud para usuários, foi criado um Seeder dentro de Program.cs, que inclui 2 usuários para utilização como teste no projeto

Ids dos usuários para testes: 
	650a8ec4-a12b-409d-8118-5ea31bb5a778
	dd51de23-ff87-4258-aaf2-5ef506624cd5



## Fase 2
- Segue algumas perguntas para o próximo refinamento ao PO
1 - Teremos algum tipo de controle de horas por tarefa no futuro?
2 - Existe uma necessidade de termos filtros futuramente por tarefas e projetos?
3 - Teremos suporte a múltiplos idiomas e regiões?
4 - Os usuários serão notificados futuramente quando forem atribuidas novas tarefas?
5 - Teremos outros relatórios, como por exemplo, cálculo de horas gastas por usuário, por projeto, % de diferença de horas entre o previsto e executado por cada tarefa, por usuário ou por projeto?

## Fase 3

1 - Adicionar validações em requests, para prevenir de código malioso e possíveis problemas no projeto
2 - Adidionar padrão UnitOfWork para gerenciar as transações com banco de dados.
3 - Monitorar o desempenho e saúde da API via kibana/grafana
4 - Implementação caching para dados frequentemente acessados (ex.: listas de projetos e tarefas)
5 - Implementação pipelines de CI/CD para integração e deploy.
6 - Implementação de logs detalhados, para facilitar a análise e troubleshooting.
7 - Planejar a evolução para mcroserviços, para maior escabilidade e flexibilidade.




