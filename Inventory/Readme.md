# Inventory Log Web Service
## Overview
This project is responsible for:

* An in-memory database,
* Managing and connecting clients,
* A web view for logging the clients' messages,
* A real-time web view of the database in a grid.



### Connecting to the Web Service Inventory Controller Endpoint

The interface between the [Bot Dialog] engine and the inventory service is managed via a triggered call to an HTTP endpoint.
This takes the form of a simple HTTP Get REST method which passes the action, the product and the quantity to update.


### Managing Client Connections

Web client connections are managed using [.NET Core SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-2.2) Hubs. Clients that send messages via the Inventory Controller HTTP endpoint are expected to manage their own life cycles.

### Managing the In-Memory Database

The in-memory database is injected as a Singleton to the Inventory Log SignalR Hubs and the Inventory Controller. The Inventory Log Hub context is injected to the Inventory Controller such that the Inventory Controller may broadcast any changes it makes from HTTP requests to the in-memory database.

### Displaying The Inventory and Log

The contents of the database and log messages are displayed to all connected web clients. Each time the in-memory inventory is updated, the SignalR Hub sends a message to all its web clients with a log message detailing the action taken. The web clients then request the in-memory database from the Hub server, and uses the updated information to refresh the grid view.