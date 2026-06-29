using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace PART2POE
{
    public class tasks
    {
        // Update this string to match your exact server name connection configuration if needed
        private readonly string connectionString = @"Data Source=(LocalDB)\dev_instance;Initial Catalog=prog_tasks;Integrated Security=True;Connect Timeout=30;";

        public void testing_connection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Successfully connected to dev_instance database!", "Connection Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error: " + ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // =========================================================================
        // UPDATED: Accepting 5 string parameter slots to record reminders safely
        // =========================================================================
        public void insert_task(string name, string desc, string dueDate, string status, string reminderDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO task (task_name, task_desription, task_dueDate, task_status, task_reminderDate) VALUES (@name, @desc, @due, @status, @reminder)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@desc", desc);
                    command.Parameters.AddWithValue("@due", dueDate);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@reminder", reminderDate);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to record task transaction: " + ex.Message, "Write Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        // Backwards compatibility method matching your secondary prompt workflow logic loops
        public bool add_task(string title, string description, string timelineDate)
        {
            try
            {
                insert_task(title, description, timelineDate, "Active", "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<CyberTask> GetAllTasks()
        {
            List<CyberTask> trackingList = new List<CyberTask>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT task_id, task_name, task_desription, task_dueDate, task_status, task_reminderDate FROM task";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string reminderRaw = reader["task_reminderDate"].ToString();
                                string noticeText = "No Reminder";

                                if (!string.IsNullOrEmpty(reminderRaw))
                                {
                                    // Simply display the date as stored in the DB (e.g., 2026-07-07)
                                    // Or if you want to be fancy, keep your days-left logic
                                    noticeText = $"Reminder: {reminderRaw}";
                                }

                                trackingList.Add(new CyberTask
                                {
                                    Id = Convert.ToInt32(reader["task_id"]),
                                    Title = reader["task_name"].ToString(),
                                    Description = reader["task_desription"].ToString(),
                                    Status = reader["task_status"].ToString(),
                                    Reminder = noticeText
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load tasks from repository mapping: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            return trackingList;
        }

        public void load_tasks(System.Windows.Controls.ListBox taskListControl)
        {
            taskListControl.Items.Clear();
            List<CyberTask> items = GetAllTasks();
            foreach (var item in items)
            {
                taskListControl.Items.Add(item.DisplaySummary);
            }
        }

        public bool UpdateTaskStatus(int taskId, string newStatus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE task SET task_status = @status WHERE task_id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", newStatus);
                    command.Parameters.AddWithValue("@id", taskId);
                    try
                    {
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public bool DeleteTask(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM task WHERE task_id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", taskId);
                    try
                    {
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public void delete_task(int taskId)
        {
            DeleteTask(taskId);
        }
    }
}