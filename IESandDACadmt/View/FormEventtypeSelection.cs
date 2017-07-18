using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IESandDACadmt.Data;
using IESandDACadmt.Logging;

namespace IESandDACadmt.Forms
{
    public partial class FormEventtypeSelection : Form
    {
        DbSqlSpController _currentDbSqlSpController = null;
        
        public FormEventtypeSelection(DbSqlSpController liveDbSqlSpController)
        {
            _currentDbSqlSpController = liveDbSqlSpController;
            InitializeComponent();
            AddEventTypesToCheckedListBox();
        }

        private void AddEventTypesToCheckedListBox()
        {
            foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.EventTypesToDelete)
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
                    DialogResult goNoGoResponse = MessageBox.Show(@"We do not recommend altering the default selection of Event Types To Delete unless instructed to do so by the Support Team. Do you still want to continue?",
                                                                          "EVENT SELECTION WARNING:",
                                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (goNoGoResponse == DialogResult.Yes)
                    {
                        _currentDbSqlSpController.EventTypesToDelete = tempoEventSelection;
                        LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.LogFileLocation, " Events to Delete selections altered:");
                        foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.EventTypesToDelete)
                        {
                            LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.LogFileLocation, kvp.Key.ToString() + " = " + kvp.Value.ToString());
                        }
                        this.Close();
                    }
                    else
                    {
                        LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.LogFileLocation, " Event Type Selection changes cancelled.");
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
                LoggingClass.SaveEventToLogFile(_currentDbSqlSpController.LogFileLocation, " User selected no Event Types. Raising warning message.");
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
            foreach (KeyValuePair<string, bool> kvp in _currentDbSqlSpController.EventTypesToDelete)
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
