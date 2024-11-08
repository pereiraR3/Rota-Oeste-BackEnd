# Rota-Oeste-Desafio

## Visão Geral

Este projeto visa desenvolver uma solução integrada para facilitar a comunicação e gestão de informações via WhatsApp para a Rota Oeste. Utilizando a **API Wpp do Twillio**, o sistema permite o envio e recebimento de mensagens, criação de checklists, gerenciamento de interações e geração de relatórios. Com uma arquitetura robusta, a plataforma proporciona uma comunicação eficiente e escalável para a Rota Oeste e seus parceiros.

## Proposta do Projeto

Este sistema foi criado para resolver a necessidade de comunicação ágil e centralizada entre a Rota Oeste e seus parceiros comerciais. A solução inclui:

- **Frontend** em Flutter, permitindo uma interface de usuário moderna e responsiva.
- **Backend** em .NET Core, garantindo escalabilidade e segurança.
- **Banco de dados** no SQL Server para gerenciamento eficiente de dados e transações.
- Integração com a **API Wpp do Twillio** para comunicação direta com os clientes e parceiros.

## Tecnologias Utilizadas

| Tecnologia    | Descrição                                                                                       |
|---------------|-------------------------------------------------------------------------------------------------|
| Flutter       | Framework para o desenvolvimento do frontend, garantindo uma experiência de usuário multiplataforma. |
| .NET Core     | Framework de backend utilizado para criar uma API RESTful segura e de alto desempenho.          |
| SQL Server    | Sistema de Gerenciamento de Banco de Dados Relacional para garantir a segurança e a integridade dos dados. |
| API Wpp do Twillio | Protocolo de comunicação para envio e recebimento de mensagens via WhatsApp.                  |

## Arquitetura do Sistema

O projeto adota o padrão de arquitetura **Model-View-Controller (MVC)**, que facilita a manutenção e escalabilidade ao separar a lógica de negócios da interface de usuário. A arquitetura é complementada pela integração com o **OAuth 2.0** para autenticação e a configuração de **CORS** para segurança da API.

## Requisitos e Funcionalidades

### Principais Funcionalidades

- **Gestão de Checklists**: Criação, edição e exclusão de checklists para monitoramento e controle de atividades.
- **Gerenciamento de Interações**: Registra todas as interações via WhatsApp, permitindo o acompanhamento do status de cada checklist.
- **Geração de Relatórios**: Relatórios detalhados para análise das interações e respostas, identificando tendências e padrões.
  
### Requisitos do Usuário

O sistema possui diferentes níveis de acesso, incluindo:

- **Administrador**: Acesso completo ao sistema, incluindo relatórios e todas as interações.
- **Usuário Geral**: Acesso limitado, apenas para as interações e checklists designados.

### Regras de Negócio

1. **Gestão de Checklists**: A aplicação deve permitir a criação, edição e exclusão de checklists para garantir uma gestão completa.
2. **Gerenciamento de Interações**: Todas as interações com os checklists são registradas para controle de status e continuidade.
3. **Relatórios e Análise de Dados**: Relatórios destacando respostas frequentes e tendências devem ser gerados para análise de desempenho.

## Autenticação e Segurança

O sistema utiliza o **OAuth 2.0** para autenticação, garantindo que apenas usuários autorizados acessem a aplicação. A configuração de **CORS** é implementada para proteger a API de acessos não autorizados. Além disso, logs de auditoria são ativados no SQL Server para garantir a rastreabilidade das operações.

## Estrutura do Banco de Dados

O banco de dados foi projetado com as seguintes entidades principais:

1. **Usuário**: Gerencia informações dos administradores e usuários do sistema.
2. **Cliente**: Entidades que recebem os checklists e interagem com o sistema via WhatsApp.
3. **Checklist**: Documento de controle criado pelos usuários.
4. **Interação**: Registro das comunicações e status das respostas dos clientes aos checklists.
5. **Questão**: Perguntas contidas nos checklists.
6. **Resposta**: Respostas fornecidas pelos clientes.

> Consulte o [Dicionário de Dados](https://pt.overleaf.com/read/vdwdjvqvtwwr#5cba88) para detalhes sobre os atributos e tipos de dados utilizados.

## Metodologia de Desenvolvimento

O projeto segue a metodologia **Agile Scrum** para desenvolvimento iterativo. As funcionalidades são distribuídas em sprints, com entregas parciais ao final de cada uma. As sprints principais incluem:

1. Configuração do ambiente e modelagem do banco de dados.
2. Implementação do backend e integração inicial com o banco de dados.
3. Desenvolvimento do frontend e APIs REST.
4. Testes e otimização de segurança.
5. Validação final e deploy.

## Testes para Homologação

### Tipos de Testes

- **Testes Unitários**: Validação de cada função e componente individualmente.
- **Testes de Integração**: Validação da comunicação entre backend e frontend.
- **Testes de Interface de Usuário**: Verificação da responsividade e usabilidade.
- **Testes de API**: Análise da funcionalidade, performance e segurança da API.
- **Testes de Performance**: Avaliação da resposta do sistema em condições de carga.

### Ferramentas

- Postman para testes de API.
- Ferramentas de monitoramento de performance para garantir escalabilidade.

## Equipe de Desenvolvimento

| Membro        | Função      | Foto                                  |
|---------------|-------------|----------------------------------------|
| André         | Back-End + DBA  | ![André](equipe/andre.png)           |
| Anthony       | Product Owner + Back-End + DBA | ![Anthony](equipe/anthony.png)       |
| Vinícius      | Back-End + DBA  | ![Vinícius](equipe/vinicius.png)     |

## Links Úteis

### Documentações

- [Engenharia de Requisitos](https://pt.overleaf.com/read/frtcrbrscwgs#5915a5)
- [Modelagem de Banco de Dados](https://pt.overleaf.com/read/vdwdjvqvtwwr#5cba88)
- [Figma - Prototipagem](https://www.figma.com/design/nwaVccYxXjauVKnK2g10S5/Prototipagem---Desafio-da-Rota-Oeste?node-id=0-1&t=mXpimYtfWCtENctq-1)

---
