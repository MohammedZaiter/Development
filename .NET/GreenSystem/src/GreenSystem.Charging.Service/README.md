# Introduction

This is the **GreenSystem Charging Service** (.NET CORE), with the following structure:

- **Common Folder:** Conatins a list of projects that are used by different solution projects (Common classes, interfaces, and caching service (Redis)).
- **Store:** Contains the common resources and operations that we apply to the database.
- **Charging Service:** A web api service that hosts the REST API's.
- **Groups:**  Contains class libraries (.NET STANDARD) that are responsible for controllers, business rules managers, and database stores (SQL database), as well as the SQL project that defines all database resources (tables, procedures), and the Test folder for unit tests.

## Prerequisites

We need docker desktop configured and running on the machine (Windows).

## Structure And Services:

**Docker Compose Services:**

- **greenSystem**: Web api service that hosts controllers.
- **sqldb**: SQL database container instance to store the data (Alternatively, you can use your own server by modifying the **GREENSYSTEM_CONNECTIONSTRING** environment variable in docker-compose).
- **redis**: Redis container instance is used for caching and to minimize database interactions.

## Compile And Run:
**Steps to run project:**
- 1- Publish the database using the Sql project added to the Groups directory, or use the DACPAC file added as a static item in the solution (before publishing, you must run the docker compose file to initiate and run the sql container instance). 
- 2- Run Docker compose again after publishing the database.
- 3- Navigate to http://localhost:3190/swagger/index.html

## Unit Tests:

You can find the unit tests written to test the business technical requirements under the Groups folder in the Test subdirectory.
