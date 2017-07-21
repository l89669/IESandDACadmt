using System;
using System.Windows;

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfAlternateCredentials.xaml
    /// </summary>
    public partial class WpfAlternateCredentials : Window
    {
        public WpfAlternateCredentials(Model.DbSqlSpController theSqlData)
        {
            InitializeComponent();
            _theSqlData = theSqlData;
        }

        private Model.DbSqlSpController _theSqlData = new Model.DbSqlSpController();

        

        private void FormAlternateCredentials_Load(object sender, EventArgs e)
        {
            if (_theSqlData.DbSqlSpControllerData.SqlConnUserName != "")
            {
                this.username.Text = _theSqlData.DbSqlSpControllerData.SqlConnUserName;
                this.password.Password = _theSqlData.DbSqlSpControllerData.SqlConnPassword;
            }
        }


        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if ((String.IsNullOrEmpty(this.username.Text)) || (String.IsNullOrWhiteSpace(this.username.Text)))
            {
                MessageBox.Show("Invalid username detected.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            if ((String.IsNullOrEmpty(this.password.Password)) || (String.IsNullOrWhiteSpace(this.password.Password)))
            {
                MessageBox.Show("Invalid password detected.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            _theSqlData.DbSqlSpControllerData.SqlConnUserName = this.username.Text.ToString();
            _theSqlData.DbSqlSpControllerData.SqlConnPassword = this.password.Password;
            this.DialogResult = true;
            return;
        }

        private void CANCEL_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
