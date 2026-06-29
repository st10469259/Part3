using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PART2POE
{
    public class user_name
    {
        public string submit_name(TextBox user_name, ListView chats)
        {
            string filename = "user_names.txt";
            string name = user_name.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                error_method("System", "Please enter a username.", chats);
                return string.Empty;
            }

            if (name.ToLower() == "exit" || name.ToLower() == "quit")
            {
                error_method("Portal", "Goodbye! Closing application...", chats);
                System.Environment.Exit(0);
                return string.Empty;
            }

            if (!File.Exists(filename))
            {
                File.AppendAllText(filename, "auto_create\n");
            }

            bool found = check_name(name);

            if (!found)
            {
                File.AppendAllText(filename, name + "\n");
                error_method("Portal", "Hey " + name + " welcome to AI cybersecurity", chats);
            }
            else
            {
                error_method("Portal", "Hey " + name + " welcome back, how can i help you today", chats);
            }

            return name;
        }

        private Boolean check_name(string name)
        {
            string filename = "user_names.txt";
            bool found_name = false;
            string[] names = File.ReadAllLines(filename);

            foreach (string name_found in names)
            {
                if (name_found.Trim().ToLower() == name.ToLower())
                {
                    found_name = true;
                    break;
                }
            }
            return found_name;
        }

        private void error_method(string name, string message, ListView chats)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5),
                BorderThickness = new Thickness(1)
            };

            // Enhanced theme colors to match your dark interface smoothly
            if (name.ToLower().Contains("portal") || name.ToLower().Contains("chat") || name.ToLower().Contains("system"))
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
            Brush nameColor = (name.ToLower().Contains("portal") || name.ToLower().Contains("chat") || name.ToLower().Contains("system")) ?
                Brushes.Cyan : Brushes.DarkGreen;
            Brush messageColor = (name.ToLower().Contains("portal") || name.ToLower().Contains("chat") || name.ToLower().Contains("system")) ?
                Brushes.White : Brushes.Black;

            messageText.Inlines.Add(new Run { Text = name + ": ", Foreground = nameColor, FontWeight = FontWeights.Bold });
            messageText.Inlines.Add(new Run { Text = message, Foreground = messageColor });

            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);
            chats.ScrollIntoView(messageBorder);
        }
    }
}