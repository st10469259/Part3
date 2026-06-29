using PART2POE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PART2POE
{
    public partial class MainWindow : Window
    {
        private readonly ArrayList reply = new ArrayList();
        private readonly ArrayList ignore = new ArrayList();
        private readonly user_name check_name = new user_name();
        private string username = string.Empty;
        private readonly string pre_question = string.Empty;
        private int counting = 0;

        private readonly tasks tasks = new tasks();
        private readonly List<CyberTask> functionalTasks = new List<CyberTask>();
        private readonly List<QuizQuestion> securityQuizDeck = new List<QuizQuestion>();
        private int dynamicQuizIndex = 0;
        private int absoluteScore = 0;
        private readonly List<string> runningActionsHistory = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            _ = new respond(reply, ignore);

            try
            {
                voice_greeting greet = new voice_greeting();
                greet.greet();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Vocal greeting notice: " + ex.Message);
            }
        }

        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            string resultName = check_name.submit_name(usernames_input, chats);

            if (!string.IsNullOrEmpty(resultName))
            {
                username = resultName;
                username_grid.Visibility = Visibility.Hidden;
                chat_grid.Visibility = Visibility.Visible;
                input_action_panel.Visibility = Visibility.Visible;

                InitializePart3Subsystems();
            }
        }

        private void send(object sender, RoutedEventArgs e)
        {
            string rawPrompt = question.Text.Trim();
            if (string.IsNullOrEmpty(rawPrompt)) return;

            error_method(username, rawPrompt);
            ai_check(rawPrompt);
        }

        private void InitializePart3Subsystems()
        {
            LogExecutionAction("Session initiated for profile identity: " + username);
            SeedQuizDeck();
            tasks.testing_connection();
            RefreshTaskDatabaseView();
        }

        private void LogExecutionAction(string descriptiveAction)
        {
            string logEntry = $"[{DateTime.Now:HH:mm:ss}] {descriptiveAction}";
            runningActionsHistory.Add(logEntry);
            activity_log_view.Items.Add(logEntry);
            activity_log_view.ScrollIntoView(logEntry);
        }

        private void RefreshTaskDatabaseView()
        {
            functionalTasks.Clear();
            taskList.Items.Clear();

            try
            {
                List<CyberTask> currentRecords = tasks.GetAllTasks();
                foreach (var task in currentRecords)
                {
                    functionalTasks.Add(task);
                    taskList.Items.Add(task.DisplaySummary);
                }
            }
            catch (Exception ex)
            {
                LogExecutionAction("UI refresh notice: " + ex.Message);
            }
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskList.SelectedIndex != -1)
            {
                var selected = functionalTasks[taskList.SelectedIndex];
                bool isUpdated = tasks.UpdateTaskStatus(selected.Id, "Completed");
                if (isUpdated)
                {
                    LogExecutionAction($"Marked Security Roadmap Item #{selected.Id} as Resolved.");
                    RefreshTaskDatabaseView();
                }
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskList.SelectedIndex != -1)
            {
                var selected = functionalTasks[taskList.SelectedIndex];
                bool isDeleted = tasks.DeleteTask(selected.Id);
                if (isDeleted)
                {
                    LogExecutionAction($"Purged data object roadmap reference #{selected.Id}");
                    RefreshTaskDatabaseView();
                }
            }
        }

        private void ai_check(string questions)
        {
            string lowerQuery = questions.ToLower().Trim();

            // Check if the user is trying to add a task or set a reminder
            if (lowerQuery.Contains("task") || lowerQuery.Contains("remind") || lowerQuery.Contains("schedule") || lowerQuery.Contains("add"))
            {
                string taskTitle = "Generic Task";
                string taskDescription = "Audit security parameters.";
                string reminderDate = "";
                string dueDate = DateTime.Now.ToString("yyyy-MM-dd");

                // 1. Logic for Titles/Descriptions
                if (lowerQuery.Contains("password"))
                {
                    taskTitle = "Password Security";
                    taskDescription = "Generate complex string matrices metrics.";
                }
                else if (lowerQuery.Contains("2fa") || lowerQuery.Contains("auth"))
                {
                    taskTitle = "Deploy 2FA";
                    taskDescription = "Implement hardware tokens or biometric verification.";
                }
                else
                {
                    // Extract title from input
                    taskTitle = Regex.Replace(questions, "(?i)add task|task|remind me|in \\d+ days|schedule", "").Trim();
                    if (string.IsNullOrEmpty(taskTitle)) taskTitle = "Custom Task";
                    taskDescription = "Standard audit task.";
                }

                // 2. Calculate Date (The "07-07-2026" format)
                var match = Regex.Match(lowerQuery, @"(\d+)\s*days");
                if (match.Success)
                {
                    int days = int.Parse(match.Groups[1].Value);
                    reminderDate = DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");
                }
                else if (lowerQuery.Contains("tomorrow"))
                {
                    reminderDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }

                // 3. Save to database using your 5-parameter method
                tasks.insert_task(taskTitle, taskDescription, dueDate, "Active", reminderDate);

                error_method("ChatBot", $"Task '{taskTitle}' added. Reminder set for: {reminderDate}");
                RefreshTaskDatabaseView();
                question.Clear();
                return;
            }
        }
        private void error_method(string name, string message)
        {
            Border messageBorder = new Border
            {
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(5),
                Padding = new Thickness(8),
                BorderThickness = new Thickness(1),
                MaxWidth = 450,
                HorizontalAlignment = name == username ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            bool isBot = name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat");
            if (isBot)
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(49, 50, 68));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(69, 71, 90));
            }
            else
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(148, 226, 213));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(166, 227, 161));
            }

            TextBlock messageText = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(2) };
            Brush nameColor = isBot ? Brushes.Cyan : Brushes.DarkGreen;
            Brush messageColor = isBot ? Brushes.White : Brushes.Black;

            messageText.Inlines.Add(new Run { Text = name + ": ", Foreground = nameColor, FontWeight = FontWeights.Bold });
            messageText.Inlines.Add(new Run { Text = message, Foreground = messageColor });

            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);
            chats.ScrollIntoView(messageBorder);
        }

        private void SeedQuizDeck()
        {
            securityQuizDeck.Clear();
            securityQuizDeck.Add(new QuizQuestion
            {
                QuestionText = "You receive an urgent message from 'IT Support' requesting validation credentials immediately. What represents the safest tactical reaction profile?",
                Options = new List<string> { "A) Compliance through secure response", "B) Disregard and clear message instantly", "C) Forward layout details directly to security auditing channels", "D) Perform verification by modifying your master credentials" },
                CorrectAnswer = "C) Forward layout details directly to security auditing channels",
                Explanation = "Isolating anomalous emails into verified validation channels allows corporate firewalls to build blacklists against credential mining campaigns."
            });

            securityQuizDeck.Add(new QuizQuestion
            {
                QuestionText = "True or False: Public Wi-Fi access configurations are generally resilient against local packet inspection operations.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Open air networks often experience active intercept matrices where threat actors can skim plaintext data streams seamlessly."
            });
        }

        private void QuizAction_Click(object sender, RoutedEventArgs e)
        {
            string currentStatus = quiz_action_btn.Content.ToString();

            if (currentStatus == "Start Quiz" || currentStatus == "Restart Game")
            {
                absoluteScore = 0;
                dynamicQuizIndex = 0;
                quiz_score_txt.Text = "Score: 0";
                quiz_options_list.Visibility = Visibility.Visible;
                quiz_action_btn.Content = "Submit Answer";
                PresentCurrentQuizQuestion();
            }
            else if (currentStatus == "Submit Answer")
            {
                if (quiz_options_list.SelectedItem == null) return;

                var immediateQuestion = securityQuizDeck[dynamicQuizIndex];
                string selection = quiz_options_list.SelectedItem.ToString();

                if (selection == immediateQuestion.CorrectAnswer)
                {
                    absoluteScore++;
                    quiz_score_txt.Text = "Score: " + absoluteScore;
                    MessageBox.Show($"Excellent Analysis!\n\n💡 {immediateQuestion.Explanation}");
                }
                else
                {
                    MessageBox.Show($"Threat Containment Compromised.\n\n⚠️ Correct Answer: {immediateQuestion.CorrectAnswer}");
                }

                dynamicQuizIndex++;
                PresentCurrentQuizQuestion();
            }
        }

        private void PresentCurrentQuizQuestion()
        {
            if (dynamicQuizIndex < securityQuizDeck.Count)
            {
                var questionItem = securityQuizDeck[dynamicQuizIndex];
                quiz_question_txt.Text = $"Scenario Track {dynamicQuizIndex + 1} of {securityQuizDeck.Count}:\n\n{questionItem.QuestionText}";
                quiz_options_list.ItemsSource = questionItem.Options;
            }
            else
            {
                quiz_options_list.Visibility = Visibility.Collapsed;
                quiz_question_txt.Text = $"Quiz Phase Concluded!\n\nEvaluated Metric Score: {absoluteScore} / {securityQuizDeck.Count}";
                LogExecutionAction($"Completed security quiz engagement suite. Score calculated: {absoluteScore}/{securityQuizDeck.Count}");
                quiz_action_btn.Content = "Restart Game";
            }
        }
    }

    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Reminder { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string DisplaySummary => $"[{(Status == "Completed" ? "✔" : " ")}] {Title} - {Description} ({Reminder})";
    }

    public class QuizQuestion
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }
}