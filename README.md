# .net Core Web API with raw SQL

## Motivation

[SQL for Web APIs: Examples, Benefits, and Implementation
](https://nordicapis.com/sql-for-web-apis-examples-benefits-and-implementation/)

## Reasons

Because I have systems that store databases in remote locations and I do not intend to expose these databases on the internet, the best way is to build a WebAPI that allows raw SQL and at the same time allows other specific endpoints for different occasions.

## Configuration

   - Change Connection String in appsettings.json
   - Change API limitations in Program.cs
     - Change Bearer to dynamic or make your own
     - Change IP of origin server or remove it if you don't need (Only accept requests from CLOUD api IP address in PROMISES web api)

## Diagram of use

![Diagram of use](https://raw.githubusercontent.com/pmcfernandes/web-api-for-raw-aql/main/example.png)

## Usage

    @WebApplication_HostAddress = http://localhost:5187

    POST {{WebApplication_HostAddress}}/api/sql
    Accept: application/json

    {
        "sql": "select * from users where iduser = @iduser",
        "args": {
            "iduser": 2
        }
    }

    ###
