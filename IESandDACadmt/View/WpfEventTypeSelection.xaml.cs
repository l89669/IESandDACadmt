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
        Model.DbSqlSpController _currentDbSqlSpController = null;
        Dictionary<string, bool> tempoEventSelection = new Dictionary<string, bool>();
        private Model.Logging.ILogging _theLogger;

        public WpfEventTypeSelection(Model.DbSqlSpController liveDbSqlSpController, Model.Logging.ILogging theLogger)
        {
            _currentDbSqlSpController = liveDbSqlSpController;
            InitializeComponent();
            _theLogger = theLogger;
            tempoEventSelection = AddEventTypesToCheckedListView(_currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete, tempoEventSelection);
            //tempoEventSelection = _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete;
        }

        private Dictionary<string, bool> AddEventTypesToCheckedListView(Dictionary<string, bool> theSelectedItems, Dictionary<string, bool> tempoEventSelection)
        {
            foreach (KeyValuePair<string, bool> kvp in theSelectedItems)
            {
                tempoEventSelection.Add(kvp.Key, kvp.Value);
            }
            return tempoEventSelection;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {            
            //tempoEventSelection = LoadNewEventSelections(tempoEventSelection);
            bool atLeastOneEventTypesSelected = CheckForAtLEastOneSelection(tempoEventSelection);
            if (atLeastOneEventTypesSelected)
            {
                bool selectedEventTypeDiffer = false;
                selectedEventTypeDiffer = CheckForSelectionDifferences(tempoEventSelection, _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete);
                if (selectedEventTypeDiffer)
                {
                    MessageBoxResult goNoGoResponse = MessageBox.Show(@"We do not recommend altering the default selection of Event Types To Delete unless instructed to do so by the Support Team. Do you still want to continue?",
                                                                          "EVENT SELECTION WARNING:",
                                                                          MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (goNoGoResponse == MessageBoxResult.Yes)
                    {
                        _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete = tempoEventSelection;
                        _theLogger.SaveEventToLogFile(  " Events to Delete selections altered:");
                        foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.DbSqlSpControllerData.EventTypesToDelete)
                        {
                            _theLogger.SaveEventToLogFile(  kvp.Key.ToString() + " = " + kvp.Value.ToString());
                        }
                        this.Close();
                    }
                    else
                    {
                        _theLogger.SaveEventToLogFile(  " Event Type Selection changes cancelled.");
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
                _theLogger.SaveEventToLogFile(  " User selected no Event Types. Raising warning message.");
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

        private bool CheckForSelectionDifferences(Dictionary<string, bool> tempoEventSelection, Dictionary<string, bool> originalEventSelection)
        {
            bool result = false;
            foreach (KeyValuePair<string, bool> kvp in originalEventSelection)
            {
                if (kvp.Value != tempoEventSelection[kvp.Key])
                {
                    result = true;
                }
            }
            return result;
        }

        //private Dictionary<string, bool> LoadNewEventSelections(Dictionary<string, bool> tempoEventSelection)
        //{
        //    for (int i = 0; i < checkedListBoxEventTypes.Items.Count; i++)
        //    {
        //        if (checkedListBoxEventTypes.GetItemCheckState(i) == CheckState.Checked)
        //        {
        //            tempoEventSelection.Add(checkedListBoxEventTypes.Items[i].ToString(), true);
        //        }
        //        else
        //        {
        //            tempoEventSelection.Add(checkedListBoxEventTypes.Items[i].ToString(), false);
        //        }

        //    }
        //    return tempoEventSelection;
        //}
    }
}


