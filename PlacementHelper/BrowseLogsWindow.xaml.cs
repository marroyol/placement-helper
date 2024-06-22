using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace PlacementHelper
{
    public partial class BrowseLogsWindow : Window
    {
        private List<WeeklyLog> logs;
        private MainWindow mainWindow;

        public BrowseLogsWindow(ref List<WeeklyLog> logs, MainWindow mainWindow)
        {
            InitializeComponent();
            this.logs = logs;
            this.mainWindow = mainWindow;
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
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Open Logs"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    List<WeeklyLog> loadedLogs = JsonConvert.DeserializeObject<List<WeeklyLog>>(json);

                    if (loadedLogs != null && loadedLogs.Count > 0)
                    {
                        logs.Clear();
                        logs.AddRange(loadedLogs);
                        PopulateLogList();
                        mainWindow.SaveLogs(); 
                        MessageBox.Show("Logs loaded successfully.", "Load Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("The selected file doesn't contain any valid logs.", "Load Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
                Title = "Save Logs"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(logs, Formatting.Indented);
                    File.WriteAllText(saveFileDialog.FileName, json);
                    MessageBox.Show("Logs saved successfully.", "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (logListBox.SelectedIndex >= 0)
            {
                WeeklyLog selectedLog = logs[logListBox.SelectedIndex];
                string logText = FormatLogForClipboard(selectedLog);
                Clipboard.SetText(logText);
                MessageBox.Show("Log details copied to clipboard.", "Copy Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a log to copy.", "No Log Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (logListBox.SelectedIndex >= 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cut this log? This action will remove the log from the list and copy it to the clipboard.", "Confirm Cut", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    WeeklyLog selectedLog = logs[logListBox.SelectedIndex];
                    string logText = FormatLogForClipboard(selectedLog);
                    Clipboard.SetText(logText);
                    logs.RemoveAt(logListBox.SelectedIndex);
                    mainWindow.SaveLogs();
                    PopulateLogList();
                    detailsTextBox.Clear();
                    MessageBox.Show("Log details cut to clipboard and removed from the list.", "Cut Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a log to cut.", "No Log Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();
                WeeklyLog pastedLog = ParseLogFromClipboard(clipboardText);
                if (pastedLog != null)
                {
                    logs.Add(pastedLog);
                    mainWindow.SaveLogs();
                    PopulateLogList();
                    MessageBox.Show("Log pasted successfully.", "Paste Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Invalid log format in clipboard.", "Paste Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No text in clipboard to paste.", "Paste Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (logListBox.SelectedIndex >= 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this log?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    logs.RemoveAt(logListBox.SelectedIndex);
                    mainWindow.SaveLogs();
                    PopulateLogList();
                    detailsTextBox.Clear();
                    MessageBox.Show("Log deleted successfully.", "Delete Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a log to delete.", "No Log Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private string FormatLogForClipboard(WeeklyLog log)
        {
            return $"Week Number: {log.WeekNumber}\n" +
                   $"Start Date: {log.StartDate.ToShortDateString()}\n" +
                   $"Activity Description: {log.ActivityDescription}\n" +
                   $"Duration: {log.Duration}\n" +
                   $"Describe: {log.Describe}\n" +
                   $"Interpret: {log.Interpret}\n" +
                   $"Evaluate: {log.Evaluate}\n" +
                   $"Plan: {log.Plan}\n" +
                   $"Additional Notes: {log.AdditionalNotes}\n" +
                   $"Feedback Received: {log.FeedbackReceived}\n" +
                   $"Goals for Next Week: {log.GoalsForNextWeek}";
        }

        private WeeklyLog ParseLogFromClipboard(string clipboardText)
        {
            try
            {
                var lines = clipboardText.Split('\n');
                var log = new WeeklyLog();
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        switch (key)
                        {
                            case "Week Number":
                                log.WeekNumber = int.Parse(value);
                                break;
                            case "Start Date":
                                log.StartDate = DateTime.Parse(value);
                                break;
                            case "Activity Description":
                                log.ActivityDescription = value;
                                break;
                            case "Duration":
                                log.Duration = int.Parse(value);
                                break;
                            case "Describe":
                                log.Describe = value;
                                break;
                            case "Interpret":
                                log.Interpret = value;
                                break;
                            case "Evaluate":
                                log.Evaluate = value;
                                break;
                            case "Plan":
                                log.Plan = value;
                                break;
                            case "Additional Notes":
                                log.AdditionalNotes = value;
                                break;
                            case "Feedback Received":
                                log.FeedbackReceived = value;
                                break;
                            case "Goals for Next Week":
                                log.GoalsForNextWeek = value;
                                break;
                        }
                    }
                }
                return log;
            }
            catch
            {
                return null;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (logListBox.SelectedIndex >= 0)
            {
                WeeklyLog selectedLog = logs[logListBox.SelectedIndex];
                EditLogWindow editLogWindow = new EditLogWindow(selectedLog);
                if (editLogWindow.ShowDialog() == true)
                {
                    mainWindow.SaveLogs();
                    PopulateLogList();
                    DisplayLogDetails(selectedLog);
                }
            }
            else
            {
                MessageBox.Show("Please select a log to edit.", "No Log Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            MessageBox.Show("Placement Helper\nVersion 1.0", "About");
        }
    }
}