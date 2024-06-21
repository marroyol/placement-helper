using System;
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
    }
}
