using System;
using System.Windows;
using System.Windows.Input;

namespace PlacementHelper
{
    public partial class EditLogWindow : Window
    {
        private WeeklyLog log;

        public EditLogWindow(WeeklyLog log)
        {
            InitializeComponent();
            this.log = log;
            PopulateFields();
        }

        private void PopulateFields()
        {
            weekNumberTextBox.Text = log.WeekNumber.ToString();
            startDatePicker.SelectedDate = log.StartDate;
            activityDescriptionTextBox.Text = log.ActivityDescription;
            durationTextBox.Text = log.Duration.ToString();
            describeTextBox.Text = log.Describe;
            interpretTextBox.Text = log.Interpret;
            evaluateTextBox.Text = log.Evaluate;
            planTextBox.Text = log.Plan;
            additionalNotesTextBox.Text = log.AdditionalNotes;
            feedbackReceivedTextBox.Text = log.FeedbackReceived;
            goalsForNextWeekTextBox.Text = log.GoalsForNextWeek;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                UpdateLog();
                DialogResult = true;
                Close();
            }
        }

        private bool ValidateInput()
        {
            if (!int.TryParse(weekNumberTextBox.Text, out _))
            {
                MessageBox.Show("Week number must be a valid integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(durationTextBox.Text, out _))
            {
                MessageBox.Show("Duration must be a valid integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (startDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select a valid start date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(activityDescriptionTextBox.Text))
            {
                MessageBox.Show("Activity description cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void UpdateLog()
        {
            log.WeekNumber = int.Parse(weekNumberTextBox.Text);
            log.StartDate = startDatePicker.SelectedDate ?? DateTime.Now;
            log.ActivityDescription = activityDescriptionTextBox.Text;
            log.Duration = int.Parse(durationTextBox.Text);
            log.Describe = describeTextBox.Text;
            log.Interpret = interpretTextBox.Text;
            log.Evaluate = evaluateTextBox.Text;
            log.Plan = planTextBox.Text;
            log.AdditionalNotes = additionalNotesTextBox.Text;
            log.FeedbackReceived = feedbackReceivedTextBox.Text;
            log.GoalsForNextWeek = goalsForNextWeekTextBox.Text;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}