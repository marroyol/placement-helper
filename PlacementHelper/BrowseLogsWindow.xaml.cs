using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Implement open functionality
            MessageBox.Show("Open functionality not implemented yet.");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Implement save functionality
            MessageBox.Show("Save functionality not implemented yet.");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            // Implement cut functionality
            MessageBox.Show("Cut functionality not implemented yet.");
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            // Implement copy functionality
            MessageBox.Show("Copy functionality not implemented yet.");
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            // Implement paste functionality
            MessageBox.Show("Paste functionality not implemented yet.");
        }
        private void UpdateMaximizeButtonIcon()
        {
            var maximizeButton = this.FindName("MaximizeButton") as Button;
            if (maximizeButton != null)
            {
                maximizeButton.Content = (this.WindowState == WindowState.Maximized) ? "\uE923" : "\uE739";
            }
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
            UpdateMaximizeButtonIcon();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Browse Logs Application\nVersion 1.0", "About");
        }
    }
}