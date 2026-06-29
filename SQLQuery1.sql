--creating a database called prog_tasks--
create database prog_tasks;

--use the prog_tasks database--
use [prog_tasks];

--creating a table [ entity ]
--columns are task_id, task_name,task_desription,task_dueDate,task_status
--List of the colums
--task_id datatype int, and auto-increment
--tasks_name datatype varchar()
--task_desription datatype varchar()
--task_dueDate datatype varchar()
--task_status datatype

create table task(
task_id int primary key identity(1,1),
task_name varchar(100),
task_desription varchar(100),
task_dueDate varchar (20),
task_status varchar (20) 
);

--select all from table task
select * from  task