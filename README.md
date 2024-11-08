# Rota-Oeste-BackEnd

## Visão Geral

Este projeto é o backend da solução integrada de comunicação para a empresa Rota Oeste. Desenvolvido em **.NET Core**, o sistema oferece uma API robusta para gerenciamento de checklists e comunicação via WhatsApp, permitindo que a Rota Oeste interaja com clientes e parceiros de forma eficiente e centralizada. Integrado com a **API WhatsApp do Twilio** e conectado ao banco de dados **SQL Server**, o sistema garante alta escalabilidade, segurança e confiabilidade.

## Proposta do Projeto

O backend foi desenvolvido para atender às necessidades de comunicação e controle da Rota Oeste. As principais funcionalidades incluem:

- **Gestão de checklists e interações**: Possibilita a criação, edição e exclusão de checklists, permitindo monitoramento de atividades.
- **Integração com WhatsApp via Twilio API**: Facilita o envio e recebimento de mensagens para comunicação direta com clientes.
- **Geração de relatórios e insights**: Oferece dados detalhados sobre interações, ajudando na análise de desempenho e na tomada de decisões.

## Tecnologias Utilizadas

| Tecnologia      | Descrição                                                                                       |
|-----------------|-------------------------------------------------------------------------------------------------|
| .NET Core       | Framework de backend utilizado para criar uma API RESTful segura e de alto desempenho.          |
| SQL Server      | Sistema de Gerenciamento de Banco de Dados Relacional, garantindo segurança e integridade dos dados. |
| API Twilio WhatsApp | Protocolo de comunicação para envio e recebimento de mensagens via WhatsApp.                  |

## Arquitetura do Sistema

O projeto adota o padrão de arquitetura **Model-View-Controller (MVC)**, que facilita a manutenção e escalabilidade, separando a lógica de negócios da interface de usuário. A arquitetura é reforçada com o uso de **OAuth 2.0** para autenticação e **CORS** para segurança da API, garantindo que somente usuários autorizados tenham acesso.

## Funcionalidades Principais

- **Gestão de Checklists**: Criação, edição e exclusão de checklists para monitoramento e controle de atividades.
- **Gerenciamento de Interações**: Registro de todas as interações via WhatsApp, permitindo o acompanhamento do status de cada checklist.
- **Geração de Relatórios**: Relatórios detalhados para análise das interações, identificando tendências e padrões para suporte em decisões estratégicas.

## Regras de Negócio e Requisitos

1. **Controle de Acesso**: Sistema com níveis de acesso (Administrador e Usuário Geral), onde o Administrador tem acesso completo e o Usuário Geral possui permissões restritas.
2. **Gerenciamento de Interações e Checklists**: Todas as interações com os checklists são registradas para controle de status e continuidade, evitando duplicidade e garantindo histórico.
3. **Segurança e Auditoria**: Logs de auditoria ativados no SQL Server para rastrear operações realizadas e garantir a segurança e integridade das informações.

## Estrutura do Banco de Dados

O banco de dados SQL Server foi projetado para suportar as operações do sistema de forma eficiente e segura. As principais tabelas incluem:

1. **Usuário**: Gerencia informações dos administradores e usuários.
2. **Cliente**: Representa entidades que interagem com o sistema via WhatsApp.
3. **Checklist**: Documentos de controle criados pelos usuários para monitoramento de atividades.
4. **Interação**: Registro das comunicações e status das respostas dos clientes aos checklists.
5. **Questão e Resposta**: Representam as perguntas e respostas associadas aos checklists, permitindo um sistema de coleta de dados detalhado.

Para mais detalhes sobre a estrutura do banco de dados e os relacionamentos entre as tabelas, consulte o [Dicionário de Dados](https://pt.overleaf.com/read/vdwdjvqvtwwr#5cba88).

## DIAGRAMA ENTIDADE RELACIONAMENTO (DER)

[der](assets/der.jpg)

## Repositório Público

O repositório público [Rota-Oeste-BackEnd](https://github.com/pereiraR3/Rota-Oeste-BackEnd) inclui:

- **Código-fonte da API**: Código organizado por módulos, com camadas de controle, serviços e repositórios para manipulação de dados.
- **Scripts de Banco de Dados**: Scripts SQL para criação das tabelas e estruturas necessárias no SQL Server.
- **Configurações do Docker**: Arquivo Dockerfile e docker-compose configurados para facilitar o deploy e execução do sistema em contêineres, incluindo configuração do SQL Server e Twilio API.
- **Exemplos de Uso da API**: Documentação sobre como utilizar as principais rotas da API para operações de CRUD nos checklists, interações e geração de relatórios.
- **Testes Automatizados**: Testes unitários e de integração para validar a funcionalidade e a segurança da API, garantindo uma cobertura de código consistente.

## Metodologia de Desenvolvimento

O projeto segue a metodologia **Agile Scrum**, com entregas iterativas e incrementais. Cada sprint inclui desenvolvimento, testes e revisão das funcionalidades para garantir que o projeto atenda aos requisitos e mantenha a qualidade esperada.

## Testes e Homologação

Para garantir a qualidade e a confiabilidade do backend, foram implementados diversos testes:

- **Testes Unitários**: Validam funções e métodos individualmente.
- **Testes de Integração**: Verificam a interação entre diferentes partes do sistema e o banco de dados.
- **Testes de Performance**: Avaliam a resposta da API sob condições de carga, garantindo que o sistema seja escalável.
- **Testes de API**: Testam os endpoints para garantir que a API funcione conforme esperado.

### Ferramentas de Teste

- **Postman**: Utilizado para testes manuais de endpoints da API.
- **xUnit**: Framework para testes automatizados no .NET, garantindo a consistência das funcionalidades.

## Equipe de Desenvolvimento

A equipe de backend foi responsável pela implementação do núcleo do sistema, incluindo lógica de negócios, integração com o banco de dados e configuração de segurança. Cada membro contribuiu para garantir a funcionalidade e confiabilidade do sistema.

| Membro        | Função                | Contribuições                                           | Foto                                  |
|---------------|-----------------------|---------------------------------------------------------|----------------------------------------|
| André         | Back-End + DBA        | Estrutura do banco de dados, implementação da lógica de negócios e configuração de segurança do sistema. | ![André](equipe/andre.png)           |
| Anthony       | Product Owner + Back-End | Coordenação do projeto, implementação da lógica de negócios central da API, Implementação de integrações com o Twilio e estrutura do banco de dados. | ![Anthony](equipe/anthony.png)       |
| Vinícius      | Back-End + DBA        | Implementação de integrações com o Twilio, implementação da lógica de negócios e estrutura do banco de dados. | ![Vinícius](equipe/vinicius.png)     |

## Links Úteis

### Documentações

- [Engenharia de Requisitos](https://pt.overleaf.com/read/frtcrbrscwgs#5915a5)
- [Modelagem de Banco de Dados](https://pt.overleaf.com/read/vdwdjvqvtwwr#5cba88)
- [Figma - Prototipagem](https://www.figma.com/design/nwaVccYxXjauVKnK2g10S5/Prototipagem---Desafio-da-Rota-Oeste?node-id=0-1&t=mXpimYtfWCtENctq-1)

---

Esse README oferece uma visão completa e detalhada do backend, com informações sobre as contribuições específicas de cada membro da equipe, além dos recursos disponíveis no repositório público. Essa estrutura facilita o entendimento do sistema e da participação de cada colaborador no projeto.
