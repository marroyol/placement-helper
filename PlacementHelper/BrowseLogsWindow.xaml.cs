using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlacementHelper
{
    public partial class BrowseLogsWindow : Window
    {
        private List<WeeklyLog> logs;

        public BrowseLogsWindow(List<WeeklyLog> logs)
        {
            InitializeComponent();
            this.logs = logs;
            PopulateLogList();
        }

        private void PopulateLogList()
        {
            logListBox.Items.Clear();
            foreach (var log in logs)
            {
                logListBox.Items.Add($"Week {log.WeekNumber} - {log.StartDate.ToShortDateString()}");
            }
        }

        private void LogListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (logListBox.SelectedIndex >= 0)
            {
                DisplayLogDetails(logs[logListBox.SelectedIndex]);
            }
        }

        private void DisplayLogDetails(WeeklyLog log)
        {
            detailsTextBox.Text = $"Week Number: {log.WeekNumber}\r\n" +
                                  $"Start Date: {log.StartDate.ToShortDateString()}\r\n" +
                                  $"Activity Description: {log.ActivityDescription}\r\n" +
                                  $"Duration: {log.Duration} hours\r\n" +
                                  $"Describe: {log.Describe}\r\n" +
                                  $"Interpret: {log.Interpret}\r\n" +
                                  $"Evaluate: {log.Evaluate}\r\n" +
                                  $"Plan: {log.Plan}\r\n" +
                                  $"Additional Notes: {log.AdditionalNotes}\r\n" +
                                  $"Feedback Received: {log.FeedbackReceived}\r\n" +
                                  $"Goals for Next Week: {log.GoalsForNextWeek}";
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Implement open functionality
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Implement save functionality
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            // Implement cut functionality
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            // Implement copy functionality
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            // Implement paste functionality
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Browse Logs Application\nVersion 1.0", "About");
        }
    }
}
