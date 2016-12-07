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

namespace taxiSystem
{
    /// <summary>
    /// Interaction logic for DriversMode.xaml
    /// </summary>
    public partial class DriversMode : Window
    {
        public DriversMode()
        {
            InitializeComponent();
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver driver = context.Drivers.FirstOrDefault(c => c.Username == loginUser.Text && c.Password == loginPass.Password);
                if (driver != null)
                {
                    DriversMainWindow win = new DriversMainWindow(loginUser.Text);
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
                Driver driver = context.Drivers.FirstOrDefault(c => c.Username == regUser.Text);
                if (driver != null)
                {
                    labelErrReg.Content = "The username is already taken";
                    done = true;
                }
            }
            if (string.IsNullOrWhiteSpace(regUser.Text) || string.IsNullOrWhiteSpace(regPass.Text) || 
                string.IsNullOrWhiteSpace(regName.Text) || string.IsNullOrWhiteSpace(regPrice.Text) ||
                string.IsNullOrWhiteSpace(regCarYear.Text) || string.IsNullOrWhiteSpace(regCarModel.Text) ||
                string.IsNullOrWhiteSpace(regCarSize.Text) || string.IsNullOrWhiteSpace(regLastName.Text))
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
                DriversMainWindow win = new DriversMainWindow(registerAcc());
                win.Show();
                this.Close();
            }
        }
        private string registerAcc()
        {
            try
            {
                using (TaxiDBEntities2 context = new TaxiDBEntities2())
                {
                    context.Drivers.Add(new Driver
                    {
                        Username = regUser.Text,
                        Password = regPass.Text,
                        Price = double.Parse(regPrice.Text),
                        Availability = true,
                        Name = regName.Text,
                        Lastname = regLastName.Text
                    });
                    context.SaveChanges();
                    Driver driver = context.Drivers.FirstOrDefault(c => c.Username == regUser.Text);
                    context.Cars.Add(new Car
                    {
                        Year = int.Parse(regCarYear.Text),
                        Size = int.Parse(regCarSize.Text),
                        Model = regCarModel.Text,
                        DriverNr = driver.Id
                    });
                    context.SaveChanges();
                    Car car = context.Cars.FirstOrDefault(c => c.DriverNr == driver.Id);
                    driver.ActiveCar = car.Id;
                    context.SaveChanges();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input");
            }
            return regUser.Text;
        }

        private void passengersMode_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}

    //history of trips  +
    //rusiuoti pagal data   +
    //bendra kelioniu statistika    +
    //change locaation of driver

