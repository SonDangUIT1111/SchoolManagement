// ViewModel/Services/IDatabaseService.cs
using System;
using System.Data.SqlClient;

public interface IDatabaseService : IDisposable
{
    void Open();
    void Close();
    SqlConnection GetSqlConnection() ;
    SqlDataReader ExecuteReader(string query);
    int ExecuteNonQuery(string query);
}