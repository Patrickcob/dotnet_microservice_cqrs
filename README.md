[![.NET](https://github.com/Patrickcob/dotnet_microservice_cqrs/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Patrickcob/dotnet_microservice_cqrs/actions/workflows/dotnet.yml)

# Social Media Microservices with CQRS

This project is a tentative exploration of microservices architecture and Command Query Responsibility Segregation (CQRS) implemented in C# using .NET Core 6.0. The goal of this project is to gain a better understanding of microservices and CQRS concepts by building a social media REST apis.

## Project Structure

The project is organized as a collection of microservices, each responsible for a specific domain or functionality. The microservices communicate with each other through defined APIs and follow the CQRS pattern to separate the command (write) and query (read) responsibilities.

The following microservices are included in this project:

- Social Media post Command Service with a MongoDb database (event store/write database)
- Social Media Post Query Service with a MS SQL (read database)

Each microservice has its own database and can be independently deployed and scaled. Communication between microservices is typically done through lightweight protocols like HTTP or messaging systems.

## Technologies Used

The project utilizes the following technologies and frameworks:

- C# programming language
- .NET Core 6.0
- ASP.NET Core Web API
- Docker for containerization and deployment
- Apache Kafka

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository: `git clone git@github.com:Patrickcob/dotnet_microservice_cqrs.git`
2. Install .NET Core 6.0 SDK (https://dotnet.microsoft.com/download)
3. Build the solution: `dotnet build`
4. Run the microservices individually or using containers (if Docker is installed).
5. Access the microservices through their defined APIs and interact with the social media functionalities.

## Contributing

This project was created as a learning exercise and may not be actively maintained. However, contributions, suggestions, and improvements are welcome. If you find a bug or have an idea for enhancement, please open an issue or submit a pull request.

## License

This project is licensed under the [Apache 2.0 License](LICENSE).

