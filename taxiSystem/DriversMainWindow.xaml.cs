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
    /// Interaction logic for DriversMainWindow.xaml
    /// </summary>
    /// 
    public class DriversCar
    {
        public int id { get; set; }
        public string car { get; set; }
    }

    public partial class DriversMainWindow : Window
    {
        public string user;
        public DriversMainWindow(string LoggedUser)
        {
            InitializeComponent();
            user = LoggedUser;
            LabelHelloUser.Content = user;
            listDriversCars();
        }

        private void listDriversCars()
        {
            List<DriversCar> drCar = new List<DriversCar>();
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver dr = context.Drivers.First(c => c.Username == user);
                var cars = context.Cars.ToList();
                foreach (Car car in cars)
                {
                    if (car.DriverNr == dr.Id)
                    {
                        DriversCar dc = new DriversCar();
                        dc.id = car.Id;
                        dc.car = car.Model + " " + car.Year + " Seats: " + car.Size;
                        drCar.Add(dc);
                    }
                }
                List<DriversCar> SortedList = drCar.OrderBy(o => o.id).ToList();
                CarListView.ItemsSource = SortedList;
                Car activeCar = context.Cars.First(c => c.Id == dr.ActiveCar);
                LabelActiveCar.Content = "Active car: " + activeCar.Model + " " + activeCar.Year;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Disclaimer", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MainWindow log = new MainWindow();
                    log.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void addLoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TaxiDBEntities2 context = new TaxiDBEntities2())
                {
                    Driver dr = context.Drivers.First(c => c.Username == user);
                    dr.CoordX = int.Parse(xCordLoc.Text);
                    dr.CordY = int.Parse(yCordLoc.Text);
                    context.SaveChanges();
                    LabelSuccesfulLoc.Content = "Location updated";
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect input");
            }
        }

        private void SelectCar(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            DriversCar drCar = b.CommandParameter as DriversCar;
            int idToSelect = drCar.id;
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver dr = context.Drivers.First(c => c.Username == user);
                if (dr.ActiveCar == idToSelect)
                {
                    MessageBox.Show("The car is already selected");
                }
                else
                {
                    dr.ActiveCar = idToSelect;
                    context.SaveChanges();
                    Car activeCar = context.Cars.First(c => c.Id == dr.ActiveCar);
                    LabelActiveCar.Content = "Active car: " + activeCar.Model + " " + activeCar.Year;
                }
            }
        }

        private void RemoveCar(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            DriversCar drCar = b.CommandParameter as DriversCar;
            int idToRemove = drCar.id;
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver dr = context.Drivers.First(c => c.Username == user);
                if(dr.ActiveCar == idToRemove)
                {
                    MessageBox.Show("Cannot remove active car");
                }
                else
                {
                    var carToRemove = context.Cars.First(x => x.Id == idToRemove);
                    context.Cars.Remove(carToRemove);
                    context.SaveChanges();
                }
            }
            listDriversCars();
        }
        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver dr = context.Drivers.First(c => c.Username == user);
                context.Cars.Add(new Car
                {
                    Model = AddModel.Text,
                    Year = int.Parse(AddYear.Text),
                    Size = int.Parse(AddSize.Text),
                    DriverNr = dr.Id
                });
                context.SaveChanges();
            }
            LabelSuccesfulCar.Content = "Car added";
            listDriversCars();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            listDriversCars();
        }

    }
}


// eiles tvarka registracijoje


























/*        private void scan_Click(object sender, RoutedEventArgs e)
        {
            using (TaxiDBEntities1 context = new TaxiDBEntities1())
            {
                Driver dr = context.Drivers.First(c => c.Username == user);
                Trip tr = context.Trips.FirstOrDefault(c => c.Status == false && c.CarNr == dr.ActiveCar);
                if (tr != null)
                {
                    labelNewTrip.Content = String.Format("New trip! From X cord {0}, Y cord {0}, To X cord {2}, Y cord {3}",tr.StartX,tr.StartY,tr.EndX,tr.EndY);
                }
            }
            labelNewTrip.Content = "No active trips yet";
        }
*/