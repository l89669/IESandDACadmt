using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfEventTypeSelection.xaml
    /// </summary>
    public partial class WpfEventTypeSelection : Window
    {
        public WpfEventTypeSelection(Model.DbSqlSpController liveDbSqlSpController)
        {
            _currentDbSqlSpController = liveDbSqlSpController;
            InitializeComponent();
            AddEventTypesToCheckedListBox();
        }

        Model.DbSqlSpController _currentDbSqlSpController = null;

 
        private void AddEventTypesToCheckedListBox()
        {
            foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete)
            {
                checkedListBoxEventTypes.Items.Add(kvp.Key);
                int index = checkedListBoxEventTypes.Items.IndexOf(kvp.Key);
                checkedListBoxEventTypes.SetItemChecked(index, kvp.Value);
            }
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Dictionary<string, bool> tempoEventSelection = new Dictionary<string, bool>();
            tempoEventSelection = LoadNewEventSelections(tempoEventSelection);

            bool atLeastOneEventTypesSelected = CheckForAtLEastOneSelection(tempoEventSelection);
            if (atLeastOneEventTypesSelected)
            {
                bool selectedEventTypeDiffer = false;
                selectedEventTypeDiffer = CheckForSelectionDifferences(tempoEventSelection, selectedEventTypeDiffer);
                if (selectedEventTypeDiffer)
                {
                    MessageBoxResult goNoGoResponse = MessageBox.Show(@"We do not recommend altering the default selection of Event Types To Delete unless instructed to do so by the Support Team. Do you still want to continue?",
                                                                          "EVENT SELECTION WARNING:",
                                                                          MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (goNoGoResponse == MessageBoxResult.Yes)
                    {
                        _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete = tempoEventSelection;
                        Model.Logging.LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.DbSqlSpControllerData.LogFileLocation, " Events to Delete selections altered:");
                        foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete)
                        {
                            Model.Logging.LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.DbSqlSpControllerData.LogFileLocation, kvp.Key.ToString() + " = " + kvp.Value.ToString());
                        }
                        this.Close();
                    }
                    else
                    {
                        Model.Logging.LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.DbSqlSpControllerData.LogFileLocation, " Event Type Selection changes cancelled.");
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("You need to select at least one Event Type to delete.");
                Model.Logging.LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.DbSqlSpControllerData.LogFileLocation, " User selected no Event Types. Raising warning message.");
            }

        }

        private bool CheckForAtLEastOneSelection(Dictionary<string, bool> tempoEventSelection)
        {
            bool oneEventFound = false;
            foreach (KeyValuePair<string, bool> kvp in tempoEventSelection)
            {
                if (kvp.Value == true)
                {
                    oneEventFound = true;
                }
            }
            return oneEventFound;
        }

        private bool CheckForSelectionDifferences(Dictionary<string, bool> tempoEventSelection, bool selectedEventTypeDiffer)
        {
            foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete)
            {
                if (kvp.Value != tempoEventSelection[kvp.Key])
                {
                    selectedEventTypeDiffer = true;
                }
            }
            return selectedEventTypeDiffer;
        }

        private Dictionary<string, bool> LoadNewEventSelections(Dictionary<string, bool> tempoEventSelection)
        {
            for (int i = 0; i < checkedListBoxEventTypes.Items.Count; i++)
            {
                if (checkedListBoxEventTypes.GetItemCheckState(i) == CheckState.Checked)
                {
                    tempoEventSelection.Add(checkedListBoxEventTypes.Items[i].ToString(), true);
                }
                else
                {
                    tempoEventSelection.Add(checkedListBoxEventTypes.Items[i].ToString(), false);
                }

            }
            return tempoEventSelection;
        }
    }
}


}
