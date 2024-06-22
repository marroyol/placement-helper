using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;

namespace PlacementHelper
{
    public partial class MainWindow : Window
    {
        private List<WeeklyLog> logs;
        private const string LogFilePath = "weekly_logs.json";

        public MainWindow()
        {
            InitializeComponent();
            logs = LoadLogs();
            UpdateWeekNumber();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                SaveLog();
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseLogsWindow browseLogsWindow = new BrowseLogsWindow(ref logs, this);
            browseLogsWindow.ShowDialog();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(weekNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(activityDescriptionTextBox.Text) ||
                string.IsNullOrWhiteSpace(durationTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(weekNumberTextBox.Text, out _) || !int.TryParse(durationTextBox.Text, out _))
            {
                MessageBox.Show("Week number and duration must be valid numbers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SaveLog()
        {
            WeeklyLog log = new WeeklyLog
            {
                WeekNumber = int.Parse(weekNumberTextBox.Text),
                StartDate = startDatePicker.SelectedDate ?? DateTime.Now,
                ActivityDescription = activityDescriptionTextBox.Text,
                Duration = int.Parse(durationTextBox.Text),
                Describe = describeTextBox.Text,
                Interpret = interpretTextBox.Text,
                Evaluate = evaluateTextBox.Text,
                Plan = planTextBox.Text,
                AdditionalNotes = additionalNotesTextBox.Text,
                FeedbackReceived = feedbackReceivedTextBox.Text,
                GoalsForNextWeek = goalsForNextWeekTextBox.Text
            };

            logs.Add(log);
            SaveLogs();
            MessageBox.Show("Log saved successfully!");
            ClearForm();
        }
        public void SaveLogs()
        {
            string json = JsonConvert.SerializeObject(logs, Formatting.Indented);
            File.WriteAllText(LogFilePath, json);
        }

        private List<WeeklyLog> LoadLogs()
        {
            if (File.Exists(LogFilePath))
            {
                string json = File.ReadAllText(LogFilePath);
                return JsonConvert.DeserializeObject<List<WeeklyLog>>(json);
            }
            return new List<WeeklyLog>();
        }

        private void UpdateWeekNumber()
        {
            DateTime placementStartDate = new DateTime(2024, 7, 1);
            int weekNumber = (int)((DateTime.Now - placementStartDate).TotalDays / 7) + 1;
            weekNumberTextBox.Text = weekNumber.ToString();
        }

        private void ClearForm()
        {
            weekNumberTextBox.Clear();
            startDatePicker.SelectedDate = DateTime.Now;
            activityDescriptionTextBox.Clear();
            durationTextBox.Clear();
            describeTextBox.Clear();
            interpretTextBox.Clear();
            evaluateTextBox.Clear();
            planTextBox.Clear();
            additionalNotesTextBox.Clear();
            feedbackReceivedTextBox.Clear();
            goalsForNextWeekTextBox.Clear();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MaximizeButton_Click(sender, e);
            }
            else
            {
                DragMove();
            }
        }
    }

    public class WeeklyLog
    {
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string ActivityDescription { get; set; }
        public int Duration { get; set; }
        public string Describe { get; set; }
        public string Interpret { get; set; }
        public string Evaluate { get; set; }
        public string Plan { get; set; }
        public string AdditionalNotes { get; set; }
        public string FeedbackReceived { get; set; }
        public string GoalsForNextWeek { get; set; }
    }
}
