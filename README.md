# Navigation Project

A microservices-based navigation system that provides journey tracking, navigation services, and administrative capabilities.

## Project Overview

This project is built using a microservices architecture with .NET Core. It consists of multiple services that communicate with each other through both synchronous HTTP calls and asynchronous messaging.

### Key Features

- Journey tracking and management
- User status monitoring
- Administrative dashboard
- Statistics and reporting
- Real-time notifications using SignalR
- Public and private journey sharing

## Architecture

The project follows a clean architecture pattern with the following components:

```
├── ApiGateways
│   └── YarpApiGateway          # API Gateway using YARP
├── BuildingBlocks
│   ├── BuildingBlocks          # Shared components (CQRS, Exceptions, etc.)
│   └── BuildingBlocks.Messaging # Messaging infrastructure
└── Services
    ├── Navigation              # Navigation service
    │   ├── Navigation.Api      # API layer
    │   ├── Navigation.Application # Application layer
    │   ├── Navigation.Domain   # Domain layer
    │   └── Navigation.Infrastructure # Infrastructure layer
    └── Administration          # Administration service
        └── Administration.Api  # API layer
```

### Communication Patterns

- **Synchronous**: REST APIs for direct service-to-service communication
- **Asynchronous**: Message-based communication using RabbitMQ and MassTransit
- **Real-time**: SignalR for real-time notifications

## Services

### Navigation Service

The core service responsible for journey tracking, route planning, and user status management. It publishes events when significant actions occur, such as daily goal achievements.

### Administration Service

Handles administrative tasks, user management, and statistics. It consumes events from the Navigation service to update user statistics and badges.

### API Gateway

Uses YARP (Yet Another Reverse Proxy) to route requests to the appropriate microservices. It provides a unified entry point for clients and handles cross-cutting concerns.

## Docker Setup

The project is containerized using Docker and can be run using Docker Compose.

### Services and Containers

- **navigation.api**: The Navigation service
- **administration.api**: The Administration service
- **navigation.db**: SQL Server database
- **messagebroker**: RabbitMQ message broker
- **yarpapigateway**: YARP API Gateway

## Port Configuration

| Service | HTTP Port | HTTPS Port | Container Port |
|---------|-----------|------------|----------------|
| Navigation API | 6003 | 6063 | 8080/8081 |
| Administration API | 6001 | 6061 | 8080/8081 |
| SQL Server | 1433 | - | 1433 |
| RabbitMQ | 5672 | - | 5672 |
| RabbitMQ Management | 15672 | - | 15672 |
| API Gateway | 6004 | 6064 | 8080/8081 |

## Running the Project

### Prerequisites

- Docker and Docker Compose
- .NET 8.0 SDK (for development)

### Using Docker Compose

1. Clone the repository:
   ```bash
   git clone https://github.com/eduarthajko/NavigationProject.git
   cd NavigationProject
   ```

2. Build and run the containers:
   
  docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

3. Access the services:
   - API Gateway: http://localhost:6004
   - Navigation API: http://localhost:6003
   - Administration API: http://localhost:6001
   - RabbitMQ Management: http://localhost:15672 (username: guest, password: guest)
   - The full URL to access Jaeger is: http://localhost:16686
  
     
4  - Import the 3 json files to postman the collection and the two env files so you can test in docker and in localy by seting as startup project the api gateway and the two other microservices
   - . In docker only set docker compose as startup and select docker env in postman.

5  - Go to Get Authenticated api get the berar token and pas it to an api after you are authenticated , ypu have attached and the postman collection 
### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or Visual Studio Code
- SQL Server (or use the Docker container)
- RabbitMQ (or use the Docker container)

### Running Locally

1. Set up the database:
   - Use the Docker container or install SQL Server locally
   - Update connection strings in appsettings.json if needed

2. Set up RabbitMQ:
   - Use the Docker container or install RabbitMQ locally
   - Update message broker settings in appsettings.json if needed

3. Run the services:
   - Open the solution in Visual Studio
   - Set multiple startup projects (Navigation.Api, Administration.Api, YarpApiGateway)
   - Start debugging

## Messaging Infrastructure

The project uses MassTransit with RabbitMQ for asynchronous messaging between services. The `AddMessageBroker` extension method in `BuildingBlocks.Messaging` configures the message broker:


## API Routes

The API Gateway routes requests to the appropriate services:

- `/administration-service/*`: Routes to the Administration API
- `/navigation-service/*`: Routes to the Navigation API

## License

This project is licensed under the MIT License - see the LICENSE file for details.
