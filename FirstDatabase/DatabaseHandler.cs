using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace FirstDatabase
{
    internal class DatabaseHandler
    {
        // String with attributes for connection to database
        public string ConnectionString { get; set; }

        // Constructor
        public DatabaseHandler(string connectionString)
        {
            ConnectionString = connectionString;
        }
       public ObservableCollection<Person> ExecuteSQLQueryRead(string query)
        {
            // Making the connection to the database
            SqlConnection conn = new SqlConnection(ConnectionString);

            // opening the connection
            conn.Open();


            // Making a new command
            SqlCommand cmd = conn.CreateCommand();
            // Giving the query (SQL - command)
            cmd.CommandText = query;

            // carrying out the command with a read command

            SqlDataReader reader = cmd.ExecuteReader();


            ObservableCollection<Person> list = new ObservableCollection<Person>(); 

            // Reading each line in the database
            // as long as there are lines to read, reader(read) returns true

            while (reader.Read())
            {
                Person tempPerson = new Person();
                // Reading the ID
                tempPerson.ID = reader.GetInt32(0);
                // Reading First name
                tempPerson.FirstName = reader.GetString(1);
                // Reading last name
                tempPerson.LastName = reader.GetString(2);
                // Reading Email
                tempPerson.Email = reader.GetString(3);
                // Reading Birthday
                tempPerson.Birthday = DateOnly.FromDateTime(reader.GetDateTime(4));

                // Adding to the list
                list.Add(tempPerson);
            }

            // closing the connection
            conn.Close();

            return list;
        }

           public void ExecuteSQLQueryWrite(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();  
                cmd.CommandText = query;

                // Executes the specified query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
