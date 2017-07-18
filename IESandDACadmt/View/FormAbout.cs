using System;
using System.Drawing;
using System.Windows.Forms;

namespace IESandDACadmt.Forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            LoadTextIntoRichtextbox();
            labelProductVersion.Text = Application.ProductVersion;
        }

        private void LoadTextIntoRichtextbox()
        {
            richTextBox1.DeselectAll();
            richTextBox1.AppendText("This tool-set is meant to assist in the maintenance of the ");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
            richTextBox1.AppendText("HEMSS & HES databases");
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Regular);
            richTextBox1.AppendText(". It is best used for performing periodic analysis/clean-up/maintenance, or targeted maintenance in situations where a specific user/computer/event type is using up too much Database space.");
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
            richTextBox1.AppendText("This is a Heat UEMS Technical Support Team provided tool. It is provided with no direct guarantees.");
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
            richTextBox1.AppendText("To ensure correct operation, please contact the HEAT UEMS Technical Support Team for the latest version.");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
