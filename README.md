
I have developed an online shopping application employing a microservices architecture.

The implementation consists of precisely seven Web APIs, facilitating the creation of essential services.

Additionally, an MVC Web Application serves as the frontend interface, complemented by a Gateway project utilizing Ocelot for seamless integration.

Furthermore, a class library has been configured for Azure Service Bus, ensuring efficient communication and orchestration within the system.

All of the projects that I created use .NET 8.

The project has 2 branches:

Master branch: This branch utilizes Azure Service Bus, where I have meticulously configured a range of queues, topics, and descriptions to facilitate seamless communication and messaging within the application.

RabbitMQ branch: In this branch, RabbitMQ takes precedence as the messaging middleware. Here, I have integrated RabbitMQ to replace Azure Service Bus, ensuring a robust and flexible messaging infrastructure tailored to the specific requirements of the project.

This is what the solution explorer looks like.

![image](https://github.com/ImesashviliIrakli/Shop-Microservices/assets/77686006/42a5f557-b623-4026-95eb-d9866b17cea7)
