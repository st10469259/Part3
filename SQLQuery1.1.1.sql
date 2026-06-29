-- Switch focus to your plural database container explicitly
USE [prog_tasks];
GO

-- If an incomplete version of the table exists, drop it to start fresh
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[task]') AND type in (N'U'))
BEGIN
    DROP TABLE task;
END
GO

-- Create the perfect structure that matches your tasks.cs file properties
CREATE TABLE task (
    task_id INT PRIMARY KEY IDENTITY(1,1),
    task_name VARCHAR(100) NOT NULL,
    task_desription VARCHAR(100) NULL, -- Preserves the exact spelling used in your code's INSERT query
    task_dueDate VARCHAR(20) NULL,
    task_status VARCHAR(20) DEFAULT 'Pending',
    task_reminderDate VARCHAR(20) NULL
);
GO