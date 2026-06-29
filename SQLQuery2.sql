-- 1. Create a fresh database container called prog_tasks
CREATE DATABASE prog_tasks;
GO

-- 2. Switch context to use the newly created prog_tasks database
USE [prog_tasks];
GO

-- 3. Create the tasks entity table structure
-- Columns: task_id (Primary Key, Auto-increment), task_name, task_desription, task_dueDate, task_status
CREATE TABLE task (
    task_id INT PRIMARY KEY IDENTITY(1,1),
    task_name VARCHAR(100),
    task_desription VARCHAR(100),
    task_dueDate VARCHAR(20),
    task_status VARCHAR(20) 
);
GO

-- 4. Verify the layout by selecting all rows from the empty table
SELECT * FROM task;
GO