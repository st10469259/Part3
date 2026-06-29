-- Create the database with an exact physical file link
CREATE DATABASE prog_tasks 
ON PRIMARY (NAME = prog_tasks_data, FILENAME = 'C:\Users\ntsik\prog_tasks.mdf')
LOG ON (NAME = prog_tasks_log, FILENAME = 'C:\Users\ntsik\prog_tasks.ldf');
GO

USE [prog_tasks];
GO

-- Recreate the table structure cleanly
CREATE TABLE task (
    task_id INT PRIMARY KEY IDENTITY(1,1),
    task_name VARCHAR(100),
    task_desription VARCHAR(100),
    task_dueDate VARCHAR(20),
    task_status VARCHAR(20) 
);
GO