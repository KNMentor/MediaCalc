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
using System.Data.Entity.Migrations;

using MediaCalc.L.Model;
using MediaCalc.L.Data;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace MediaCalc.V
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Flat> Flats { get; set; }
        public ObservableCollection<Lease> Leases { get; set; }
        public ObservableCollection<ConstFees> ConstFees { get; set; }
        public MediaCalcDbContext dbContext { get; set; } = new MediaCalcDbContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region OwnMethods
       
        private void onAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = getPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                if (!displayName.StartsWith("hidden-"))
                    e.Column.Header = displayName;
                else
                    e.Column.Visibility = Visibility.Hidden;
            }

            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";

        }
        public static string getPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }
        #endregion

        #region Flats
        private void dataGrid_flats_Initialized(object sender, EventArgs e)
        {
            Flats = new ObservableCollection<Flat>(dbContext.Flats.ToList());
            dataGrid_flats.ItemsSource = Flats;
        }

        private void dataGrid_flats_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            onAutoGeneratingColumn(sender,e);
            if (e.Column.Header as String == "Leases")
                e.Column.Visibility = Visibility.Hidden;
            e.Column.Width = new DataGridLength(150);
        }

        private void dataGrid_flats_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dataGrid_constFees.SelectedIndex != -1)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć ten obiekt?", "Uwaga", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    dbContext.Flats.Remove(dataGrid_flats.CurrentItem as Flat);
                else
                    e.Handled = true;
            }
        }

        private void button_flats_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbContext.Flats.AddOrUpdate(Flats.ToArray());
                dbContext.SaveChanges();

                dataGrid_flats.ItemsSource = null;
                dataGrid_flats.ItemsSource = Flats;
            }
            catch (Exception ex)
            {
                //TODO: obsługa błedu
                MessageBox.Show(ex.Message);
            }
        }

        private void button_flats_load_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Users
        private void dataGrid_users_Initialized(object sender, EventArgs e)
        {

            Users = new ObservableCollection<User>(dbContext.Users.ToList());
            dataGrid_users.ItemsSource = Users;
        }

        private void button_users_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dbContext.Users.AddOrUpdate(Users.ToArray());
                dbContext.SaveChanges();

                dataGrid_users.ItemsSource = null;
                dataGrid_users.ItemsSource = Users;
            }
            catch(Exception ex)
            {
                //TODO: obsługa błedu
                MessageBox.Show(ex.Message);
            }
        }

        private void button_users_load_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_users_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dataGrid_constFees.SelectedIndex != -1)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć ten obiekt?", "Uwaga", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    dbContext.Users.Remove(dataGrid_users.CurrentItem as User);
                else
                    e.Handled = true;
            }
        }

        private void dataGrid_users_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            onAutoGeneratingColumn(sender, e);
            if (e.Column.Header as String == "RentalPeriods")
                e.Column.Visibility = Visibility.Hidden;
            e.Column.Width = new DataGridLength(150);
        }

        private void preventDelete(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć ten obiekt?", "Uwaga", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    dbContext.Users.Remove(dataGrid_users.CurrentItem as User);
                else
                    e.Handled = true;
            }
        }
        #endregion

        #region ConstFees
        private void dataGrid_constFees_Initialized(object sender, EventArgs e)
        {
            ConstFees = new ObservableCollection<ConstFees>(dbContext.ConstFees.ToList());
            dataGrid_constFees.ItemsSource = ConstFees;
        }

        private void dataGrid_constFees_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            onAutoGeneratingColumn(sender, e);
        }

        private void dataGrid_constFees_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dataGrid_constFees.SelectedIndex != -1)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć ten obiekt?", "Uwaga", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    dbContext.ConstFees.Remove(dataGrid_constFees.CurrentItem as ConstFees);
                else
                    e.Handled = true;
            }
        }

        private void button_constFees_add_Click(object sender, RoutedEventArgs e)
        {
            BoolHelper bh = new BoolHelper();
            ConstFeesAdd cfa = new ConstFeesAdd(Flats.ToList(), bh);
            ConstFees cf = new ConstFees();
            cfa.DataContext = cf;
            cfa.ShowDialog();

            if (bh.BoolHelp)
            { 
                try
                {
                    dbContext.ConstFees.Add(cf);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            ConstFees = new ObservableCollection<ConstFees>(dbContext.ConstFees.ToList());
            dataGrid_constFees.ItemsSource = null;
            dataGrid_constFees.ItemsSource = ConstFees;
        }

        private void button_constFees_modify_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_constFees.SelectedIndex == -1)
                return;

            BoolHelper bh = new BoolHelper();
            ConstFeesAdd cfa = new ConstFeesAdd(Flats.ToList(), bh);
            ConstFees cf = ConstFees[dataGrid_constFees.SelectedIndex];
            ConstFees cfc = cf.ShallowCopy();
            cfa.DataContext = cf;

            cfa.ShowDialog();

            if (bh.BoolHelp)
            {
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                cf = cfc.ShallowCopy();
                dbContext.ConstFees.AddOrUpdate(cf);
                dbContext.SaveChanges();
            }

            dataGrid_constFees.ItemsSource = null;
            dataGrid_constFees.ItemsSource = ConstFees;
        }
        #endregion

        #region Leases
        private void dataGrid_leases_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            onAutoGeneratingColumn(sender, e);
        }

        private void dataGrid_leases_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && dataGrid_constFees.SelectedIndex != -1)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć ten obiekt?", "Uwaga", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    dbContext.ConstFees.Remove(dataGrid_constFees.CurrentItem as ConstFees);
                else
                    e.Handled = true;
            }
        }

        private void dataGrid_leases_Initialized(object sender, EventArgs e)
        {
            Leases = new ObservableCollection<Lease>(dbContext.Leases.ToList());
            dataGrid_leases.ItemsSource = Leases;
        }

        private void button_leases_add_Click(object sender, RoutedEventArgs e)
        {
            BoolHelper bh = new BoolHelper();
            LeaseAdd la = new LeaseAdd(Flats.ToList(), Users.ToList(), bh);
            Lease l = new Lease();
            la.DataContext = l;
            la.ShowDialog();

            if (bh.BoolHelp)
            {
                try
                {
                    dbContext.Leases.Add(l);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            Leases = new ObservableCollection<Lease>(dbContext.Leases.ToList());
            dataGrid_leases.ItemsSource = null;
            dataGrid_leases.ItemsSource = Leases;
        }

        private void button_leases_modify_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_leases.SelectedIndex == -1)
                return;

            BoolHelper bh = new BoolHelper();
            LeaseAdd la = new LeaseAdd(Flats.ToList(), Users.ToList(), bh);
            Lease l = Leases[dataGrid_leases.SelectedIndex];
            Lease lcf = l.ShallowCopy();
            la.DataContext = l;

            la.ShowDialog();

            if (bh.BoolHelp)
            {
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                l = lcf.ShallowCopy();
                dbContext.Leases.AddOrUpdate(l);
                dbContext.SaveChanges();
            }

            dataGrid_leases.ItemsSource = null;
            dataGrid_leases.ItemsSource = Leases;
        }
        #endregion
    }
}
