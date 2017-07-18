using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lumension_Advanced_DB_Maintenance.Forms
{
    public partial class FormHelpRequirements : Form
    {
        public FormHelpRequirements()
        {
            InitializeComponent();
            LoadTextIntoRichtextbox();
        }

        private void LoadTextIntoRichtextbox()
        {
            richTextBox1.DeselectAll();
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
            richTextBox1.AppendText("1.   ");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            richTextBox1.AppendText("The User Credentials of this tool-set need access to the UPCCommon and PLUS SQL databases for EMSS with rights to Read, Write and to Create/Drop Stored Procedures.");
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
            richTextBox1.AppendText("2.   ");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            richTextBox1.AppendText("For SQL servers with a Named-Instance, use the format of 'ServerAddress\\InstanceName' in the SQL Server Address field.");
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
            richTextBox1.AppendText("3.   ");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            richTextBox1.AppendText("Full write access is required to the folder this EMSS DB Maintenance Tool-set is launched from so that all activities can be recorded in its Log File.");
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
            richTextBox1.AppendText("4.   ");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            richTextBox1.AppendText("The default Settings for the DELETION tool excludes deleting the following event types:");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Italic);
            richTextBox1.AppendText(Environment.NewLine + "        ->DEVICE-ATTACHED");
            richTextBox1.AppendText(Environment.NewLine + "        ->GRANTED");
            richTextBox1.AppendText(Environment.NewLine + "        ->MEDIUM-ENCRYPTED");
            richTextBox1.AppendText(Environment.NewLine + "        ->WRITE-GRANTED");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            richTextBox1.AppendText(Environment.NewLine + "        This can be modified if required in the \"OPTIONS=>Event Types\" menu.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
