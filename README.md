# London Stock Exchange Solution

The MVP of the solution is based on utilizing Azure's serverless offering in the form of Azure Functions to create the required endpoints to fulfill the requirements.

The solution has been structured to allow for abstraction on the service and repository layers which enabled creating unit tests and mocking. Resisted the temptation to use SQL output binding for the MVP because it wouldn't have allowed me to unit test different aspects of the solution (would have broken SRP).

Implemented the repository pattern and used Dapper as an ORM. Autofac has also been used to allow for dependency injection.

Unit tests have been created to cover the data access and API layer.


# API Endpoints

The all API have been created to meet the OpenAPI specification and this has done to allow for the endpoints to be registered in Azure API Manager to allow us to centralize monitoring, usage, throttling and managing policies.

To access Swagger documentation please go to: http://localhost:7071/api/swagger/ui
