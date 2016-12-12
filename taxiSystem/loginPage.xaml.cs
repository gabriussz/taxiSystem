using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace taxiSystem
{
    public class userLogin: INotifyPropertyChanged
    {
        private string log = null;
        private string pas = null;
        public string regpas = null;
        private string pasrep = null;
        public event PropertyChangedEventHandler PropertyChanged;
        public string loginBox
        {
            get{return log;}
            set
            {
                log = value;
                OnPropertyChanged("loginBox");
            }
        }
        public string passBox
        {
            get{return pas;}
            set
            {
                pas = value;
                OnPropertyChanged("passBox");
            }
        }
        public string regBox
        {
            get { return pas; }
            set
            {
                pas = value;
                OnPropertyChanged("regBox");
            }
        }
        public string regPassBox
        {
            get { return regpas; }
            set
            {
                regpas = value;
                OnPropertyChanged("regPassBox");
            }
        }
        public string regPassBoxRep
        {
            get { return pasrep; }
            set
            {
                pasrep = value;
                OnPropertyChanged("regPassBoxRep");
            }
        }
        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    
    public class loginValidator : ValidationRule
    {
        public override ValidationResult Validate (object value, CultureInfo cultureInfo)
        {
            if (value.ToString() == "")
                return new ValidationResult(false, null);
            else
            {
                if (value.ToString().Length < 3)
                    return new ValidationResult(false, null);
            }
            return ValidationResult.ValidResult;
        }
    }
    public class registerValidator : ValidationRule
    {
        public userLogin usLog = new userLogin();
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString() != usLog.regpas)
                return new ValidationResult(false, null);
            return ValidationResult.ValidResult;
        }
    }

    public partial class MainWindow : Window
    {
        public int errorCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            userLogin usLog = new userLogin();
            this.DataContext = usLog;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Control)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errorCount++;
            else
                errorCount--;
            login.IsEnabled = errorCount > 0 ? false : true;
            register.IsEnabled = errorCount > 0 ? false : true;
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Passenger passenger = context.Passengers.FirstOrDefault(c => c.Username == loginUser.Text && c.Password == loginPass.Text);
                if (passenger != null)
                {
                    systemWindow win = new systemWindow(passenger.Username);
                    win.Show();
                    this.Close();
                }
            }
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            bool done = false;
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Passenger passenger = context.Passengers.FirstOrDefault(c => c.Username == regUser.Text);
                if (passenger != null)
                {
                    labelErrReg.Content = "The username is already taken";
                    done = true;
                }
            }
            if (string.IsNullOrWhiteSpace(regUser.Text) || string.IsNullOrWhiteSpace(regPass.Text))
            {
                labelErrReg.Content = "You must enter all required fields";
                done = true;
            }
            else if (regPass.Text != regPassRep.Text)
            {
                labelErrReg.Content = "Passwords entered do not match";
                done = true;
            }
            if (!done)
            {
                systemWindow win = new systemWindow(registerAcc());
                win.Show();
                this.Close();
            }
        }
        private string registerAcc()
        {
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                context.Passengers.Add(new Passenger
                {
                    Username = regUser.Text,
                    Password = regPass.Text
                });
                context.SaveChanges();
            }
            return regUser.Text;
        }

        private void driversMode_Click(object sender, RoutedEventArgs e)
        {
            DriversMode win = new DriversMode();
            win.Show();
            this.Close();
        }
    }
} 

