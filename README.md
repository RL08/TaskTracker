# TaskTracker
Started on 1. July 2023 <br>

## Description 
TaskTracker is a web-based task management application designed to assist individuals in efficiently organizing their tasks and to-do lists. 
With its user-friendly interface, TaskTracker simplifies the process of managing tasks, helping users stay organized and productive.

## Features
1. Task creation and management: Allow users to create, edit, delete and mark tasks. Each task should include information such as the task name, status, priority level, and due date.
2. Task lists: Allow users to create multiple lists to better organize their tasks, such as work, personal, shopping, and more. Each list should include information such as the list name, the lowest task status, the lowest task priority level, and the progress in terms of completed tasks.
3. Priority Levels: Implement a priority system (low, medium, high) for tasks so users can easily identify and focus on important tasks.
4. Status Levels: Implement a status system (not finished, in progress, completed) for tasks so that users can easily identify and focus on important tasks.
5. Search and Filtering: Provide search functionality to help users quickly find specific tasks and lists.
6. Persist data: Tasks and lists are persisted in the database.
7. Register: TaskTracker has a user registration system. New users can provide necessary details such as username, password, and email, which are securely stored in the database. 
8. Login with JWT: Implement a secure user authentication system that requires users to enter their unique credentials (username, password) to access the application. Upon successful login, a JSON Web Token (JWT) is generated and sent to the client. This token contains user-specific information and cannot be manipulated. It is included in future requests to verify the user's identity and grant access to protected resources.
9. Login using Student Account: Implement a secure user authentication system that requires users to enter their student account information (username, password, email) to access the application.

If you'd like to see examples of the features mentioned above, you can find them in [Example](https://github.com/RL08/TaskTracker/tree/main/Example/README.md).
## Getting Started
Follow the instructions below to get started with TaskTracker on your local machine.

### Requirement
The project uses .NET version >= 6 and Docker. Verify whether the .NET SDK version 6 or 7 and Docker are installed on your computer with the following command:
```
dotnet --version
docker --version
```

Before you begin, ensure that you have the following tools installed:
 - [Git](https://git-scm.com/downloads)
 - [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1. Clone the repository:
```
git clone https://github.com/RL08/TaskTracker.git
```

### 2. Start server
Before you start the server, ensure that Docker Desktop is running.

#### Window
```
startServer.cmd
```

#### Linux
```
chmod a+x startServer.sh
./startServer.sh
```

## Contact 
| Name                   | Email                    | GitHub |
| ---------------------- | ------------------------ | ------ |
| Richard *Liu*          | LIU22291@spengergasse.at | RL08   |
