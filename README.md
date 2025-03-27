# CodeOverflow

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)]()

CodeOverflow is a console-based Q&A application inspired by Stack Overflow. It demonstrates Object-Oriented Programming (OOP), Object-Oriented Design (OOD), and database management using MS SQL with ADO.NET. The project includes features such as user registration, login, posting questions/answers, voting with restrictions, and managing preferred tags.

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Contributing](#contributing)
- [Contact](#contact)

---

## Features

- **User Management:**  
  - Create new accounts  
  - Login using username or email  
  - Manage preferred tags

- **Question & Answer:**  
  - Post questions with associated tags  
  - Post answers to questions (enforcing one answer per user per question)  
  - Edit answers (with an "edited" flag)

- **Voting System:**  
  - Upvote or downvote questions and answers  
  - Prevent self-voting and conflicting votes  
  - Dynamic vote counting via a separate Vote table

- **Database Integration:**  
  - MS SQL database integration using ADO.NET  
  - Separated layers for data access, business logic, and presentation  
  - Full documentation including ERD, database schema, and class diagrams

---

## Architecture

CodeOverflow follows a layered architecture. The layers are:

1. **Entities (Models):**  
   - Domain classes (User, Question, Answer, Tag, Vote) with minimal logic.

2. **Data Access (Repositories):**  
   - Uses ADO.NET to interact with the MS SQL database and perform CRUD operations.

3. **Service (Business Logic):**  
   - It implements application rules such as registration, login, posting, voting, and tag management.

4. **Presentation (Console Application):**  
   - A command-line interface that allows users to interact with the application.

For a visual representation, refer to the [ERD](docs/ERD.png) and [Class Diagram](docs/ClassDiagram.png).

---

## Project Structure

```plaintext
/CodeOverflow
├── docs
│   ├── ERD.png               # Entity-Relationship Diagram
│   ├── DatabaseSchema.sql    # MS SQL Database Schema script
│   ├── ClassDiagram.png      # UML Class Diagram
├── src
│   └── CodeOverflow          # Main C# project folder
│       ├── Program.cs        # Application entry point
│       ├── appsettings.json  # Application configuration file (e.g., connection string)
│       ├── Entities          # Domain model classes
│       │   ├── User.cs
│       │   ├── Question.cs
│       │   ├── Answer.cs
│       │   ├── Tag.cs
│       │   └── Vote.cs
│       ├── Data              # Data access layer (repositories, DB configuration)
│       │   ├── DbConfig.cs
│       │   ├── UserRepository.cs
│       │   ├── QuestionRepository.cs
│       │   ├── AnswerRepository.cs
│       │   └── VoteRepository.cs
│       └── Services          # Business logic layer (application services)
│           ├── UserService.cs
│           ├── QuestionService.cs
│           ├── AnswerService.cs
│           └── VoteService.cs
├── README.md                 # Project overview and instructions
├── LICENSE                   # Project license file (e.g., MIT License)
└── .gitignore                # Git ignore rules for build artifacts and sensitive files
```

## Installation

### 1. Clone the Repository
To get started, clone the repository to your local machine using the following command:

```bash
git clone https://github.com/Kareem20/CodeOverflow.git
cd CodeOverflow
```

### 2. Configure the Database
- Ensure the MS SQL Server is installed and running.
- Create a new database for the project.
- Execute the SQL script located at docs/DatabaseSchema.sql to set up the required tables.
- Update the database connection string in src/CodeOverflow/Data/DbConfig.cs to match your database credentials.


### 3. Build the Project
- Open the solution in Visual Studio or your preferred IDE.
- Restore NuGet packages and build the solution.

## **Contributing**

If you'd like to contribute to this project, please follow these steps:

- Fork the repository.
- Create a new feature branch (``git checkout -b feature/your-feature-name``).
- Commit your changes (``git commit -am 'Add a new feature'``).
- Push to the branch (``git push origin feature/your-feature-name``).
- Open a pull request.

## Contact
For questions or feedback, please contact:
kareemabdelrhmane@gmail.com






