# Online Shopping Microservices Application

Welcome to the Online Shopping Microservices Application, a .NET-based project employing a robust microservices architecture. This application serves as an excellent sandbox for testing and learning containerized application orchestration and monitoring techniques.

![ECOM](https://github.com/ImesashviliIrakli/Shop-Microservices/assets/77686006/a491e9af-0e81-46b6-bfd7-3431fb6562c2)

# Project Overview

The solution is crafted using .NET 8 and consists of the following components:

Seven Web APIs: These APIs are strategically designed to facilitate the creation of essential services for a comprehensive online shopping experience.

MVC Web Application: Serving as the frontend interface, this application provides a user-friendly interaction point for customers. The integration of Ocelot in the Gateway project ensures seamless communication and integration across the system.

Azure Service Bus Integration: A class library has been configured to use Azure Service Bus for efficient communication and orchestration within the microservices ecosystem.

## Branches
### Master Branch
The Master branch utilizes Azure Service Bus, with meticulously configured queues, topics, and descriptions to ensure smooth communication and messaging within the application.

### RabbitMQ Branch
The RabbitMQ branch replaces Azure Service Bus with RabbitMQ as the messaging middleware. This integration provides a robust and flexible messaging infrastructure tailored to the specific project requirements.

## How the Solution Explorer looks:
![image](https://github.com/ImesashviliIrakli/Shop-Microservices/assets/77686006/42a5f557-b623-4026-95eb-d9866b17cea7)

## Getting Started

### To download and run the project:

Clone the repository.
Navigate to each project in the solution and run the `update-database` command to ensure database setup for each component.
