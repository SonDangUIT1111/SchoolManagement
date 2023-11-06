using System;
using System.Data;
using System.Data.SqlClient;
using StudentManagement.Model;

public class DatabaseService : IDatabaseService
{
    private readonly string connectionString;
    private SqlConnection connection;

    public DatabaseService()
    {
        this.connection = new SqlConnection(ConnectionString.connectionString);
    }

    public void Open()
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    public void Close()
    {
        if (connection.State != ConnectionState.Closed)
        {
            connection.Close();
        }
    }

    public SqlConnection GetSqlConnection()
    {
        return connection;
    }

    public SqlDataReader ExecuteReader(string query)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        SqlCommand command = new SqlCommand(query, connection);
        return command.ExecuteReader();
    }

    public int ExecuteNonQuery(string query)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        SqlCommand command = new SqlCommand(query, connection);
        return command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        if (connection != null)
        {
            connection.Dispose();
        }
    }
}