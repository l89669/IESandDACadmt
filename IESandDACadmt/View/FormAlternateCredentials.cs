using System;
using System.Windows.Forms;

namespace Lumension_Advanced_DB_Maintenance.Forms
{
    public partial class FormAlternateCredentials : Form
    {
        private Data.DbSqlSpController _theSqlData = new Data.DbSqlSpController();

        public FormAlternateCredentials(Data.DbSqlSpController theSqlData)
        {
            InitializeComponent();
            _theSqlData = theSqlData;
        }

        private void FormAlternateCredentials_Load(object sender, EventArgs e)
        {
            if (_theSqlData.SqlConnUserName != "")
            {
                textBoxUsername.Text = _theSqlData.SqlConnUserName;
                textBoxPassword.Text = _theSqlData.SqlConnPassword;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(textBoxUsername.Text)) || (String.IsNullOrWhiteSpace(textBoxUsername.Text)))
            {
                MessageBox.Show("Invalid username detected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if ((String.IsNullOrEmpty(textBoxPassword.Text)) || (String.IsNullOrWhiteSpace(textBoxPassword.Text)))
            {
                MessageBox.Show("Invalid password detected.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            _theSqlData.SqlConnUserName = textBoxUsername.Text;
            _theSqlData.SqlConnPassword = textBoxPassword.Text;
            this.DialogResult = DialogResult.OK;
            return;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
