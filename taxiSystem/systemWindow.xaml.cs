using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class TripDriver
    {
        public int id { get; set; }
        public string time { get; set; }
        public string car { get; set; }
        public string price { get; set; }
    }

    public class DriverDetails
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public bool availability { get; set; }
        public string carModel { get; set; }
        public int carSize { get; set; }
        public int carYear { get; set; }
        public double rating { get; set; }
        public string price { get; set; }
    }

    public class TripHistory
    {
        public DateTime duration { get; set; }
        public string status { get; set; }
        public double range { get; set; }
        public double price { get; set; }
        public string car { get; set; }
        public DateTime date { get; set; }

    }

    public partial class systemWindow : Window
    {
        public double durationKm = 0.3;
        public double pricePKM = 0.5;
        public string user;
        public systemWindow(string LoggedUser)
        {
            InitializeComponent();
            user = LoggedUser;
            LabelHelloUser.Content = "Hello "+user;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(xCordDest.Text) && !string.IsNullOrEmpty(yCordDest.Text))
            {
                try
                {
                    List<TripDriver> tripDr = new List<TripDriver>();
                    using (TaxiDBEntities2 context = new TaxiDBEntities2())
                    {
                        var drivers = context.Drivers.ToList();
                        foreach (Driver driver in drivers)
                        {
                            if (driver.Availability)
                            {
                                Car car = context.Cars.First(c => c.DriverNr == driver.Id);
                                TripDriver td = new TripDriver();
                                td.id = driver.Id;
                                td.car = (car.Model + " " + car.Year + " seats: " + car.Size);
                                td.time = time(driver);
                                td.price = price(driver);
                                tripDr.Add(td);
                            }
                        }
                        List<TripDriver> SortedList = tripDr.OrderBy(o => o.time).ToList();
                        DriversListView.ItemsSource = SortedList;
                    }
                }
                catch (FormatException)
                {
                    LabelErrLoc.Content = "Location input incorrect";
                }
            }
            else
            {
                MessageBox.Show("Enter your destination!");
            }
        }

        private string price(Driver driver)
        {
            double dist = double.Parse(distance(int.Parse(xCordDest.Text), int.Parse(yCordDest.Text), int.Parse(xCordLoc.Text), int.Parse(yCordLoc.Text)));
            string test = driver.Price.ToString();
            string price = !string.IsNullOrEmpty(test) ? test : pricePKM.ToString();
            double tripPrice = double.Parse(price) * dist;
            return tripPrice.ToString("N2");
        }

        public string time(Driver driver)
        {
            double dist = double.Parse(distance(driver.CoordX, driver.CordY, int.Parse(xCordLoc.Text), int.Parse(yCordLoc.Text)));
            double temp = durationKm * dist;
            int minutes = Convert.ToInt32(Math.Floor(temp));
            int seconds = Convert.ToInt32((temp - Math.Truncate(temp)) * 0.6 * 100);
            temp = seconds + minutes * 60;
            TimeSpan time = TimeSpan.FromSeconds(temp);
            string str = time.ToString(@"hh\:mm\:ss");
            return str;
        }

        public string distance(int x, int y, int destX, int destY)
        {
            var a = Math.Abs(x - destX);
            var b = Math.Abs(y - destY);
            var ret = string.Format("{0:0.00}", Math.Sqrt(b * b + a * a));
            return ret;
        }
        private void SelectDriver(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TripDriver tripdr = b.CommandParameter as TripDriver;
            int id = tripdr.id;
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver dr = context.Drivers.First(c => c.Id == id);
                Car car = context.Cars.First(c => c.Id == dr.ActiveCar);
                Passenger ps = context.Passengers.First(c => c.Username == user);
                dr.Availability = false;
                context.Trips.Add(new Trip
                {
                    StartX = int.Parse(xCordLoc.Text),
                    StartY = int.Parse(yCordLoc.Text),
                    EndX = int.Parse(xCordDest.Text),
                    EndY = int.Parse(yCordDest.Text),
                    StartTime = DateTime.Now,
                    CarNr = car.Id,
                    PassengerNr = ps.Id,
                    Price = double.Parse(distance(int.Parse(xCordLoc.Text), int.Parse(yCordLoc.Text), int.Parse(xCordDest.Text), int.Parse(yCordDest.Text))) * dr.Price,
                    Range = double.Parse(distance(int.Parse(xCordLoc.Text), int.Parse(yCordLoc.Text), int.Parse(xCordDest.Text), int.Parse(yCordDest.Text))),
                    Status = false
                });
                context.SaveChanges();
            }
            MessageBox.Show("Your driver will arrive in: " + tripdr.time + "\n" + "Car: " + tripdr.car);
            LabelTripData.Content = "your driver will arrive in: " + tripdr.time;
            btnEndTrip.Visibility = Visibility.Visible;
        }

        private void DisplayInfo(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            TripDriver tripdr = b.CommandParameter as TripDriver;
            Panel.SetZIndex(DriversListView, -1);
            Panel.SetZIndex(DriverDetails, 1);
            Panel.SetZIndex(btnBack, 1);
            int id = tripdr.id;
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Driver dr = context.Drivers.First(c => c.Id == id);
                Car car = context.Cars.First(c => c.DriverNr == id);
                DriverDetails.Items.Add("Name: "+dr.Name);
                DriverDetails.Items.Add("LastName: "+dr.Lastname);
                DriverDetails.Items.Add("Car model: "+car.Model);
                DriverDetails.Items.Add("Car year: "+car.Year);
                DriverDetails.Items.Add("Seats: " + car.Size);
            }
        }
        

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DriverDetails, -1);
            Panel.SetZIndex(DriversListView, 1);
            Panel.SetZIndex(btnBack, -1);
            DriverDetails.Items.Clear();
            btnSearch_Click(sender, e);
        }

        private void btnSearchByName_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DriversSearch, 1);
            Panel.SetZIndex(TripHistoryDatagrid, -1);
            Panel.SetZIndex(StatisticsBox, -1);
            List<DriverDetails> DrDet = new List<DriverDetails>();
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                var drivers = context.Drivers.ToList();
                foreach (Driver driver in drivers)
                {
                    if (Regex.IsMatch(driver.Lastname, SearchByName.Text, RegexOptions.IgnoreCase))
                    {
                        Car car = context.Cars.First(c => c.Id == driver.ActiveCar);
                        DriverDetails dd = new DriverDetails();
                        dd.carModel = car.Model;
                        dd.carYear = Convert.ToInt32(car.Year);
                        dd.carSize = Convert.ToInt32(car.Size);
                        dd.availability = driver.Availability;
                        dd.name = driver.Name;
                        dd.lastName = driver.Lastname;
                        dd.price = driver.Price.ToString();
                        DrDet.Add(dd);
                    }
                }
                DriversSearch.ItemsSource = DrDet;
            }
        }

        private void btnEndTrip_Click(object sender, RoutedEventArgs e)
        {
            LabelTripData.Content = "";
            btnEndTrip.Visibility = Visibility.Hidden;
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Passenger ps = context.Passengers.First(c => c.Username == user);
                Trip tr = context.Trips.First(c => c.PassengerNr == ps.Id && c.Status == false);
                Driver dr = context.Drivers.First(c => c.ActiveCar == tr.CarNr);
                tr.EndTime = DateTime.Now;
                tr.Status = true;
                tr.EndX = int.Parse(xCordDest.Text);
                tr.EndY = int.Parse(yCordDest.Text);
                dr.CoordX = int.Parse(xCordDest.Text);
                dr.CordY = int.Parse(yCordDest.Text);
                dr.Availability = true;
                context.SaveChanges();
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

        private void btnCheckHistory_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DriversSearch, -1);
            Panel.SetZIndex(TripHistoryDatagrid, 1);
            Panel.SetZIndex(StatisticsBox, -1);
            Passenger pas;
            List<TripHistory> TrDet = new List<TripHistory>();
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                pas = context.Passengers.First(c => c.Username == user);
                var trs = context.Trips.Where(c => c.PassengerNr == pas.Id).ToList();
            }
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TaxiDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandText = "SELECT Range, Price, StartTime, EndTime, Status, StartX, StartY, EndX, EndY FROM Trip WHERE PassengerNr = @pasNr";
            command.Parameters.AddWithValue("@pasNr", pas.Id);
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(data);
            TripHistoryDatagrid.DataContext = data.DefaultView;
        }

        private void btnCheckStatistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsBox.Items.Clear();
            Panel.SetZIndex(DriversSearch, -1);
            Panel.SetZIndex(TripHistoryDatagrid, -1);
            Panel.SetZIndex(StatisticsBox, 1);
            using (TaxiDBEntities2 context = new TaxiDBEntities2())
            {
                Passenger ps = context.Passengers.First(c => c.Username == user);
                StatisticsBox.Items.Add("Total range traveled: " + context.Trips.Where(o => o.PassengerNr == ps.Id).Sum(p => p.Range));
                StatisticsBox.Items.Add("Total Price paid: " + context.Trips.Where(o => o.PassengerNr == ps.Id).Sum(p => p.Price));
                StatisticsBox.Items.Add("Average range per trip: " + context.Trips.Where(o => o.PassengerNr == ps.Id).Average(p => p.Range));
                StatisticsBox.Items.Add("Average price per trip: " + context.Trips.Where(o => o.PassengerNr == ps.Id).Average(p => p.Price));
            }
        }
    }
}










/*            //Image i = new Image();
            //BitmapImage src = new BitmapImage();
            //src.BeginInit();
            //src.UriSource = new Uri("C:\\Computer\\studijos\\2k\\TOP\\trecioji\\taxiSystem\\taxi-driver.jpg", UriKind.Relative);
            //src.CacheOption = BitmapCacheOption.OnLoad;
            //src.EndInit();
            //i.Source = src;
            //i.Stretch = Stretch.Uniform;
            //DriverPhoto.Children.Add(i);
*/