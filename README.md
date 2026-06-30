CyberGuard AI Interface - Part 3
Project Description
The CyberGuard AI Interface is a task management application designed for security professionals to audit and track security-related tasks. It features an intelligent chat-based input system that parses user intent and schedules tasks with specific security-focused descriptions and automated reminders.

Features
AI-Integrated Task Management: Allows users to add tasks using natural language inputs.

SQL Database Integration: Uses a robust tasks.cs layer to perform CRUD operations (Create, Read, Update, Delete) on an SQL Server database.

Smart Reminders: Automatically calculates and displays specific reminder dates (e.g., YYYY-MM-DD format) directly in the user roadmap.

Security Categorization: Automatically assigns relevant task titles and descriptions based on keywords like "password" or "2fa".

Code Structure
MainWindow.xaml.cs: Contains the UI logic and the AI-driven input parsing system that processes user commands.

tasks.cs: Acts as the data repository, handling all interactions with the prog_tasks database, including insertion, deletion, and retrieval.

CyberTask.cs: Defines the data model used to structure and display task information in the application roadmap.

Setup Instructions
Clone the Repository: Download or clone this repository to your local machine.

Database Setup: . The connection string in tasks.cs is configured to use (LocalDB)\dev_instance. Ensure your database prog_tasks is initialized.

Build the Project: Open the solution in Visual Studio and perform a Rebuild Solution to ensure all references are correctly resolved.

Run: Launch the application via the Start button in Visual Studio.
working SQL:SQLQuery1.1.1
Development Milestones
This project has been developed in iterations, following the submission requirements for commits and versioning:

Commits: The project history includes a minimum of six meaningful commits to track progress.

Releases: Official versions have been tagged and released within the GitHub repository to mark key development milestones.
