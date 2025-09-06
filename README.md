# 🏗️ SchoolRowingApp - School Rowing Club Management System

[![.NET](https://img.shields.io/badge/.NET-7.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/7.0)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15.0-green.svg)](https://www.postgresql.org/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-blueviolet)](https://github.com/jasontaylordev/CleanArchitecture)

## 🌐 English Version

### 📌 Project Overview

**SchoolRowingApp** is a professional-grade management system for rowing schools and clubs, designed with **Domain-Driven Design (DDD)** and **Clean Architecture** principles. This application manages athletes, payers, and their relationships in a rowing school environment.

The project demonstrates **enterprise-level development practices** commonly required by European companies, making it an excellent portfolio piece for developers targeting international markets.

### 🔧 Key Technologies

- **.NET 7.0** - Modern cross-platform framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core 7.0** - ORM with PostgreSQL
- **MediatR** - CQRS pattern implementation
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **PostgreSQL** - Production-grade database
- **Swagger/OpenAPI** - API documentation
- **xUnit & Moq** - Unit testing

### 🌟 Key Features

- **Domain-Driven Design Implementation**:
  - Properly structured domain layer with rich domain model
  - Domain events for business process orchestration
  - Value objects and entities with proper encapsulation

- **Clean Architecture**:
  - Strict separation of concerns (Domain, Application, Infrastructure, Presentation)
  - Dependency inversion principle
  - Testable architecture

- **Advanced Data Management**:
  - Many-to-many relationships between athletes and payers
  - Composite keys implementation
  - Optimistic concurrency control
  - Proper handling of database migrations

- **Professional REST API Design**:
  - Correct HTTP status codes
  - Proper resource representation
  - CamelCase JSON naming convention
  - Comprehensive API documentation with Swagger

### 🏆 Why This Project Stands Out

This project demonstrates **European enterprise development standards** that are highly valued by employers:

1. **Domain Modeling Excellence**  
   Proper implementation of DDD concepts like aggregates, domain events, and value objects

2. **Database Expertise**  
   Correct handling of complex relationships, migrations, and schema management

3. **REST API Best Practices**  
   Adherence to RESTful principles with proper error handling and resource design

4. **Modern Architecture Patterns**  
   Implementation of CQRS, MediatR, and clean architecture

5. **Production-Ready Code**  
   Comprehensive error handling, logging, and performance considerations

### 📂 Project Structure

```
SchoolRowingApp/
├── SchoolRowingApp.Domain/       # Domain layer (entities, value objects, domain services)
├── SchoolRowingApp.Application/  # Application layer (commands, queries, DTOs, mappers)
├── SchoolRowingApp.Infrastructure/ # Infrastructure layer (EF Core, repositories)
├── SchoolRowingApp.WebApi/       # Presentation layer (controllers, startup)
├── SchoolRowingApp.Tests/        # Unit tests
└── README.md                     # This file
```

### 🚀 Getting Started

1. **Prerequisites**:
   - .NET 7.0 SDK
   - PostgreSQL 15.0+
   - Visual Studio 2022 or VS Code

2. **Configuration**:
   - Update `appsettings.json` with your PostgreSQL connection string
   - Set schema name in `appsettings.json` (default: "bob")

3. **Database Setup**:
   ```bash
   dotnet ef database update --project SchoolRowingApp.Infrastructure --startup-project SchoolRowingApp.WebApi
   ```

4. **Run the Application**:
   ```bash
   dotnet run --project SchoolRowingApp.WebApi
   ```

5. **Access API Documentation**:
   - Open `https://localhost:5001/swagger` in your browser

### 💼 Why This Is Perfect For Your Portfolio

This project demonstrates **exactly what European employers look for** in .NET developers:

> "Implemented clean domain-driven architecture with proper separation of concerns, ensuring testable business logic and data integrity in compliance with European enterprise development standards"

> "Resolved complex EF Core migration and database relationship issues, ensuring smooth database evolution and production-ready deployment"

> "Designed professional REST API following industry best practices with proper resource modeling and error handling"

When applying for jobs in Europe, you can confidently highlight how this project demonstrates your ability to deliver enterprise-grade applications that meet international standards.

---

## 🇷🇺 Русская Версия

### 📌 Обзор Проекта

**SchoolRowingApp** — это профессиональная система управления школой гребного спорта, разработанная с использованием принципов **Domain-Driven Design (DDD)** и **Чистой Архитектуры**. Приложение управляет атлетами, плательщиками и их взаимосвязями в среде школы гребного спорта.

Проект демонстрирует **enterprise-уровня разработку**, часто требуемую европейскими компаниями, что делает его отличным примером для портфолио разработчиков, ориентированных на международный рынок.

### 🔧 Основные Технологии

- **.NET 7.0** - Современный кросс-платформенный фреймворк
- **ASP.NET Core Web API** - Разработка RESTful API
- **Entity Framework Core 7.0** - ORM с PostgreSQL
- **MediatR** - Реализация паттерна CQRS
- **AutoMapper** - Маппинг объектов
- **FluentValidation** - Валидация входных данных
- **PostgreSQL** - Промышленная база данных
- **Swagger/OpenAPI** - Документирование API
- **xUnit & Moq** - Модульное тестирование

### 🌟 Ключевые Возможности

- **Реализация Domain-Driven Design**:
  - Корректно структурированный доменный слой с богатой моделью
  - Доменные события для оркестрации бизнес-процессов
  - Значимые объекты и сущности с правильной инкапсуляцией

- **Чистая Архитектура**:
  - Четкое разделение ответственности (Domain, Application, Infrastructure, Presentation)
  - Принцип инверсии зависимостей
  - Тестируемая архитектура

- **Продвинутое управление данными**:
  - Связи многие-ко-многим между атлетами и плательщиками
  - Реализация составных ключей
  - Управление оптимистичным параллелизмом
  - Корректная обработка миграций базы данных

- **Профессиональный REST API**:
  - Корректные HTTP статус-коды
  - Правильное представление ресурсов
  - Использование camelCase в JSON
  - Комплексная документация API с Swagger

### 🏆 Почему Этот Проект Выделяется

Этот проект демонстрирует **стандарты enterprise-разработки**, высоко ценимые европейскими работодателями:

1. **Отличное Доменное Моделирование**  
   Правильная реализация DDD-концепций: агрегаты, доменные события, значимые объекты

2. **Экспертиза по работе с БД**  
   Корректная обработка сложных связей, миграций и управления схемой

3. **Профессиональный REST API**  
   Соблюдение RESTful принципов с правильной обработкой ошибок и проектированием ресурсов

4. **Современные Архитектурные Паттерны**  
   Реализация CQRS, MediatR и чистой архитектуры

5. **Готовый к Продакшену Код**  
   Комплексная обработка ошибок, логирование и учет производительности

### 📂 Структура Проекта

```
SchoolRowingApp/
├── SchoolRowingApp.Domain/       # Доменный слой (сущности, значимые объекты, доменные сервисы)
├── SchoolRowingApp.Application/  # Прикладной слой (команды, запросы, DTO, мапперы)
├── SchoolRowingApp.Infrastructure/ # Инфраструктурный слой (EF Core, репозитории)
├── SchoolRowingApp.WebApi/       # Слой представления (контроллеры, запуск)
├── SchoolRowingApp.Tests/        # Модульные тесты
└── README.md                     # Этот файл
```

### 🚀 Начало Работы

1. **Предварительные требования**:
   - .NET 7.0 SDK
   - PostgreSQL 15.0+
   - Visual Studio 2022 или VS Code

2. **Конфигурация**:
   - Обновите `appsettings.json` с вашей строкой подключения к PostgreSQL
   - Установите имя схемы в `appsettings.json` (по умолчанию: "bob")

3. **Настройка Базы Данных**:
   ```bash
   dotnet ef database update --project SchoolRowingApp.Infrastructure --startup-project SchoolRowingApp.WebApi
   ```

4. **Запуск Приложения**:
   ```bash
   dotnet run --project SchoolRowingApp.WebApi
   ```

5. **Доступ к Документации API**:
   - Откройте `https://localhost:5001/swagger` в вашем браузере

### 💼 Почему Это Идеально для Вашего Портфолио

Этот проект демонстрирует **именно то, что ищут европейские работодатели** у .NET разработчиков:

> "Реализована чистая доменно-ориентированная архитектура с четким разделением ответственности, обеспечивающая тестируемую бизнес-логику и целостность данных в соответствии со стандартами enterprise-разработки в Европе"

> "Решены сложные проблемы с миграциями EF Core и отношениями в базе данных, обеспечена плавная эволюция схемы и готовность к production-развертыванию"

> "Спроектирован профессиональный REST API в соответствии с отраслевыми best practices с правильным моделированием ресурсов и обработкой ошибок"

При подаче заявок на работу в Европе вы можете уверенно указывать, как этот проект демонстрирует вашу способность создавать enterprise-приложения, соответствующие международным стандартам.

---

## 📌 How to Highlight This Project in Interviews

When discussing this project in interviews, focus on these key points:

1. **Domain Modeling Decisions**  
   "I carefully designed the domain model to reflect business rules, such as preventing the same payer from being assigned to an athlete with multiple roles, which required proper composite key configuration in EF Core."

2. **Database Relationship Handling**  
   "I resolved complex EF Core relationship issues by properly configuring composite keys and navigation properties, ensuring data integrity while maintaining clean domain model."

3. **REST API Design Choices**  
   "I implemented RESTful principles with appropriate HTTP status codes, using 204 NoContent for successful updates instead of returning data, which aligns with industry best practices."

4. **Architecture Justification**  
   "I chose Clean Architecture with DDD because it provides clear separation of concerns, making the codebase more maintainable and testable, which is critical for enterprise applications."

5. **Problem-Solving Approach**  
   "When facing the concurrency exception issue, I analyzed EF Core's change tracking behavior and implemented the correct solution by removing AsNoTracking from update operations."

This project demonstrates not just technical skills, but also **professional maturity** and understanding of enterprise development standards.

## 🌐 Project Links

- [GitHub Repository](https://github.com/Shm-Alex/SchoolRowingApp)
- [Live Demo](https://schoolrowingapp.azurewebsites.net/swagger) (coming soon)

---

*This project was developed with ❤️ following European enterprise development standards*  
*Разработано с ❤️ в соответствии со стандартами enterprise-разработки в Европе*
