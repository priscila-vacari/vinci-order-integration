# 📦 Order Integration Solution
Sistema de integração para consolidar pedidos de múltiplas fontes, com API em .NET 8, mensageria Kafka, SQL Server, cache opcional e interface em Angular.

## 📁 Estrutura de Pastas
```
vinci-order-integration/
├── src/            
    ├── backend/            # Projeto ASP.NET 8 Web API
    ├── frontend/           # Projeto Angular
├── docker-compose.yml      # Orquestração dos containers
├── README.md
```

## 🚀 Como executar a aplicação
### Pré-requisitos
- Docker

- Docker Compose

## 🔧 Passos para subir a aplicação
1. Clone o repositório:
```
git clone https://github.com/priscila-vacari/vinci-order-integration.git
cd vinci-order-integration
```

2. Construa os containers:
```
docker-compose build
```

3. Suba a aplicação:
```
docker-compose up
```

## 🌐 Endpoints
- Swagger API: http://localhost:5000/swagger

- Angular Frontend: http://localhost:3000

## 📡 Funcionalidades
### API .NET 8
- POST /orders
Cadastra um novo pedido e envia para o Kafka.

- GET /orders/{id}
Busca um pedido processado no banco SQL Server.

### Angular
- Formulário de criação de pedido

- Consulta de pedidos pelo ID

## 🧱 Infraestrutura via Docker
Serviço	        Porta Local	    Tecnologia
API	            5000	        ASP.NET 8
Frontend	    3000	        Angular + NGINX
SQL Server	    1433	        SQL Server 2022
Kafka	        9092	        Apache Kafka
Zookeeper	    2181	        Apache Zookeeper

## ⚙️ Configurações importantes
### 🔐 Conexão com SQL Server (API appsettings.json)
```
"ConnectionStrings": {
  "DefaultConnection": "Server=sqlserver;Database=OrderDb;User=sa;Password=Your_password123;TrustServerCertificate=True;"
}
```

### 📤 Kafka
```
"Kafka": {
  "BootstrapServers": "kafka:9092",
  "Topic": "orders-topic"
}
```

## 🐳 Comandos úteis
- Ver containers ativos:
docker ps

- Parar os containers:
docker-compose down

- Logs de um container específico:
docker logs nome_do_container

## 🛠️ Tecnologias utilizadas
- ASP.NET 8 (Web API)

- Angular 17+

- Apache Kafka

- SQL Server 2022

- Docker e Docker Compose

- Swagger (OpenAPI)

- NGINX (para servir o Angular)