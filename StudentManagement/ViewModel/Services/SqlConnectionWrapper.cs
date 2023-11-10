using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Model;

namespace StudentManagement.ViewModel.Services
{
    public class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        private SqlConnection connection;

        public SqlConnectionWrapper(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            //if (connection.State != System.Data.ConnectionState.Open)
            //{
                connection.Open();
            //}
        }

        public void Close()
        {
            //if (connection.State != System.Data.ConnectionState.Closed)
            //{
                connection.Close();
            //}
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public void OpenAsync()
        {
            connection.OpenAsync();
        }

        public SqlConnection GetSqlConnection()
        {
            return connection;
        }
    }
}
