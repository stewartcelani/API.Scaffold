# API Scaffold

A reusable starting point for building APIs with .NET, featuring basic API key authentication and MediatR for request handling.

This project is primarily designed for **learning and testing purposes**. It serves as a boilerplate I created for my own personal projects, offering a quick setup with essential features pre-configured so I can dive straight into building out business logic. While it's tailored to my needs, I'm making it public in case it's useful to others as a foundation for their own projects or experiments.

## Features

- **Basic API Key Authentication**: Simple authentication via an `X-API-Key` header.
- **MediatR Integration**: Pre-configured with pipelines for logging, validation, and authorization.
- **Serilog Logging**: Structured logging to the console with configurable sinks.
- **Exception Handling Middleware**: Formats unhandled exceptions as problem details.
- **CORS Enabled**: Allows cross-origin requests by default.
- **OpenAPI/Swagger Support**: API documentation available in development mode.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- An IDE like [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

## Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/stewartcelani/API.Scaffold.git
   ```

2. **Navigate to the Project Directory**:
   ```bash
   cd API.Scaffold
   ```

3. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

4. **Build the Project**:
   ```bash
   dotnet build
   ```

5. **Run the Application**:
   ```bash
   dotnet run --launch-profile dev
   ```

   The API will be available at `https://localhost:9000`.

## Configuration

### API Key

To configure API key authentication:

1. Open `appsettings.json`.
2. Find the `ApiKeys` array.
3. Add your desired API keys as strings.

   Example:
   ```json
   "ApiKeys": ["your_api_key_here"]
   ```

### Environment

The project supports multiple environments (Development, Testing, Production) with corresponding configuration files (`appsettings.{Environment}.json`). Use the appropriate launch profile (e.g., `dev`, `test`, `prod`) or set the `ASPNETCORE_ENVIRONMENT` variable manually.

## Usage

### Test Endpoints

Test endpoints are included to verify the setup:

- **Unauthenticated**: `GET /test`
   - No API key required.
- **Authenticated**: `GET /test/auth`
   - Requires the `X-API-Key` header with a valid key from `appsettings.json`.

Example using curl for the authenticated endpoint:
```bash
curl -H "X-API-Key: your_api_key_here" https://localhost:9000/test/auth
```

## Testing with Bruno

To simplify testing the API, a [Bruno](https://www.usebruno.com/) collection is provided in the **`Bruno_Collection` folder** at the root of this repository. Bruno is an open-source API client, similar to Postman, that helps you organize and run API requests effortlessly.

### What's Included

- **Unauthenticated Test Endpoint**: A ready-to-use request to test the API without authentication.
- **Authenticated Test Endpoint**: A pre-configured request to test the API using an API key.

### How to Use

1. **Install Bruno**: Download and install Bruno from [their official website](https://www.usebruno.com/).
2. **Import the Collection**: Launch Bruno and import the collection file from the **`Bruno_Collection` folder** in the repository root.
3. **Select the 'API' Environment**: The collection includes an 'API' environment pre-set with the base URL and API key. Switch to this environment in Bruno to use these settings.
4. **Run the Requests**: Execute the unauthenticated and authenticated requests to test the API endpoints.

With the Bruno collection located in the `Bruno_Collection` folder and the pre-configured 'API' environment, you can quickly test the API without manually configuring the base URL, API key, or request details.

## Note on Purpose

This project is all about **learning and testing**. I built it as a starting point for my own personal projects, giving me a head start with basic features already in place. While it's mainly for my own use, I'm sharing it publicly so others can use it as a base for their own learning, testing, or side projects if they find it helpful. Feel free to adapt it to your needs!