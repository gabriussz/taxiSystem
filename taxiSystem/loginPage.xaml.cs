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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace taxiSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Passenger passenger = context.Passengers.FirstOrDefault(c => c.Username == loginUser.Text && c.Password == loginPass.Password);
                if (passenger != null)
                { 
                    systemWindow win = new systemWindow(passenger.Username);
                    win.Show();
                    this.Close();
                }
                else
                {
                    labelErrLogin.Content = "Incorrect usename or password";
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

//history of trips  +
//rusiuoti pagal data   +
//bendra kelioniu statistika    +
//change locaation of driver
