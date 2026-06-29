-- 1. Ensure the database container exists safely
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'prog_tasks')
BEGIN
    CREATE DATABASE prog_tasks;
END
GO

-- 2. Switch context to use your database
USE [prog_tasks];
GO

-- 3. Drop the table if it exists so we can rebuild it cleanly with the new column
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[task]') AND type in (N'U'))
BEGIN
    DROP TABLE task;
END
GO

-- 4. Create the final complete table structure
CREATE TABLE task (
    task_id INT PRIMARY KEY IDENTITY(1,1),
    task_name VARCHAR(100),
    task_desription VARCHAR(100),
    task_dueDate VARCHAR(20),
    task_status VARCHAR(20),
    task_reminderDate VARCHAR(20) -- The new column to store your reminder profile dates
);
GO