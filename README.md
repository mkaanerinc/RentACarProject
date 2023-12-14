#### 📬 Find me at
[![Github Badge](http://img.shields.io/badge/-Github-black?style=flat&logo=github&link=https://github.com/mkaanerinc/)](https://github.com/mkaanerinc/) 
[![Linkedin Badge](https://img.shields.io/badge/-LinkedIn-blue?style=flat&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/mkaanerinc/)](https://www.linkedin.com/in/mkaanerinc)
[![Gmail Badge](https://img.shields.io/badge/-Gmail-d14836?style=flat&logo=Gmail&logoColor=white&link=mailto:mkaanerinc@gmail.com)](mailto:mkaanerinc@gmail.com)

<br />
<p align="center">
  <h2 align="center">Rent a Car Project</h2>
</p>
<br />

# About The Project

## Built With

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=flat&logo=c-sharp&logoColor=white)&nbsp;
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=flat&logo=.net&logoColor=white)&nbsp;
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=flat&logo=microsoft%20sql%20server&logoColor=white)&nbsp;
![ASP.NET CORE Web API](https://img.shields.io/badge/ASP.NET%20CORE%20Web%20API-02569B.svg?&style=flat&logo=rest&logoColor=white)&nbsp;
![Entity-Framework](https://img.shields.io/badge/Entity%20Framework%20Core-004880?style=flat&logo=nuget&logoColor=white)&nbsp;
[![Autofac](https://img.shields.io/badge/Autofac-004880?style=flat&logo&logo=nuget&logoColor=white)](https://autofac.org/)&nbsp;
[![Fluent-Validation](https://img.shields.io/badge/Fluent%20Validation-004880?style=flat&logo&logo=nuget&logoColor=white)](https://fluentvalidation.net/)&nbsp;
![JWT Authentication](https://img.shields.io/badge/JWT%20Authentication-004880?style=flat&logo&logo=nuget&logoColor=white)&nbsp;
![Serilog](https://img.shields.io/badge/Serilog-004880?style=flat&logo&logo=nuget&logoColor=white)&nbsp;
![xUnit](https://img.shields.io/badge/xUnit-004880?style=flat&logo&logo=nuget&logoColor=white)&nbsp;
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=flat&logo=visual-studio&logoColor=white)&nbsp;

## Layers

<details>
  <summary>Click to see layers</summary>

### Business

Business Layer created to process or control the incoming information according to the required conditions.

### Core

Core layer containing various particles independent of the project.

### DataAccess

Data Access Layer created to perform database CRUD operations.

### Entities

Entities Layer created for database tables.

### Console-UI

It is the layer that appears to the user, that the user interacts with and sends commands to the program.

### WebAPI

Web API Layer that opens the business layer to the internet.

</details><p></p>

## Some of the topics that this app covers

<details>
  <summary>Click to see topics</summary>
  
  - MSSQL
- Entity Framework Core
- LINQ
- Restful API
  - Postman(tested in this environment)
- IoC
  - Autofac
- Interceptor
- AOP (Aspect Oriented Programming)
- Generic Repository Design Pattern
- Cross Cutting Concerns
  - Validation(Fluent Validation)
  - Security (JWT)
  - Caching (In-Memory Caching)
  - Transaction
  - Performance
  - Logging (Serilog)
- JWT Authentication
- Claim
- Extensions
- Service Collection
- Result Types
- Hashing
- Salting

</details><p></p>

## Models

<details>
  <summary>Click to see models</summary>

### Brands

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| BrandId             | int           | False       |
| Name                | nvarchar(50)  | False       |

### Colors

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| ColorId             | int           | False       |
| Name                | nvarchar(50)  | False       |

### Customers

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| CustomerId          | int           | False       |
| UserId              | int           | False       |
| CompanyName         | nvarchar(255) | False       |

### CarImages

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| CarImageId          | int           | False       |
| CarId               | int           | False       |
| ImagePath           | nvarchar(MAX) | False       |
| Date                | datetime      | False       |

### Cars

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| CarId               | int           | False       |
| BrandId             | int           | False       |
| ColorId             | int           | False       |
| Name                | nvarchar(50)  | False       |
| ModelYear           | int           | False       |
| DailyPrice          | money         | False       |
| Description         | varchar(MAX)  | False       |

### Users

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| UserId              | int           | False       |
| FirstName           | nvarchar(50)  | False       |
| LastName            | nvarchar(50)  | False       |
| Email               | nvarchar(50)  | False       |
| PasswordHash        | varbinary(500)| False       |
| PasswordSalt        | varbinary(500) | False       |
| Status              | bit           | False       |

### OperationClaims

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| OperationClaimId    | int           | False       |
| Name                | varchar(250)  | False       |

### UserOperationClaims

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| UserId              | int           | False       |
| UserOperationClaimId| int           | False       |
| OperationClaimId    | int           | False       |

### Rentals

| Name                | Data Type     | Allow Nulls |
| :-----------------  | :-----------  | :---------- |
| RentalId            | int           | False       |
| CarId               | int           | False       |
| CustomerId          | int           | False       |
| RentDate            | datetime      | False       |
| ReturnDate          | datetime      | True        |

</details><p></p>

## Required Packages for Back-End

<details>
<summary>Click to see required packages</summary>

| Package Name  | Version |
| ------------- | ------------- |
| Autofac | 7.1.0  |
| Autofac.Extensions.DependencyInjection  | 8.0.0  |
| Autofac.Extras.DynamicProxy  | 7.1.0  |
| FluentValidation  | 11.8.0  |
| Microsoft.AspNetCore.Authentication.JwtBearer  | 6.0.24  |
| Serilog  | 3.1.0 |
| Serilog.Sinks.File  | 4.1.0 |
| Serilog.Sinks.MSSqlServer  | 5.8.0 |
| Newtonsoft.Json  | 13.0.3 |
| xunit    | 2.4.2 |
| Moq      | 4.20.69 |
| Microsoft.AspNetCore.Http  | 2.2.2 |
| Microsoft.AspNetCore.Http.Features  | 5.0.17 |
| Microsoft.EntityFrameworkCore | 7.0.13  |
| Microsoft.EntityFrameworkCore.SqlServer  | 7.0.12  |
| System.IdentityModel.Tokens.Jwt | 6.17.0 |

</details><p></p>

# License

Distributed under the MIT License. See `LICENSE` for more information.

# Contact

Mustafa Kaan Erinç - [Linkedin](https://www.linkedin.com/in/mkaanerinc/) [Gmail](mailto:mkaanerinc@gmail.com)


