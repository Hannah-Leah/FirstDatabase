using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Saving the person the user clicks on for edit and delete
        private Person selectedItem;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        private DateTime _birthday = DateTime.Now;

        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public ObservableCollection<Person> PersonsObsv { get; set; }
        private DatabaseHandler db;
        public MainWindow()
        {
            InitializeComponent();
            string connection = "Data Source=HannahLeah;Initial Catalog=MyFirstDB;Integrated Security=True;Trust Server Certificate=True";
            PersonsObsv = new ObservableCollection<Person>();
            DataContext = this;
           
            db = new DatabaseHandler(connection);
        }

        // Event handler for user's list selection

        private void ListData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = (Person)ListData.SelectedItem;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete: {selectedItem.FirstName}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                int userID = selectedItem.ID;

                string query = $"DELETE FROM Persons WHERE ID = {userID}";
                db.DeleteItem(query);

            }
        }



        private void BtnLoadDB_Click(object sender, RoutedEventArgs e)
        {
            PersonsObsv.Clear();
          var temp =  db.ExecuteSQLQueryRead("SELECT * FROM Persons;");

            foreach (var item in temp)
            {
                PersonsObsv.Add(item);
            }
           
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Person tempPerson = new Person();
            tempPerson.FirstName = FirstName;
            tempPerson.LastName = LastName;
            tempPerson.Email = Email;
            tempPerson.Birthday = DateOnly.FromDateTime(Birthday);

            string query = $"INSERT INTO Persons(FirstName,LastName,Email,Birthday)" + $" VALUES ('{FirstName}', '{LastName}', '{Email}', "
                + $"'{DateOnly.FromDateTime(Birthday).ToString("yyyy-MM-dd")}'); ";
            db.ExecuteSQLQueryWrite(query);
        }

       
    }
}