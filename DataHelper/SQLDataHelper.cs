using System;
using System.Data;
using System.Data.SqlClient;
using diseasedataprovider.Interfaces;
using Microsoft.Extensions.Configuration;

public class SQLDataHelper : IDataHelper
{
    private readonly IConfiguration _configuration;
    public SQLDataHelper(IConfiguration Configuration)
    {
        _configuration = Configuration;
    }
    public DataSet executeDataQuery(string query)
    {
        try {
            var _dataset = new DataSet();
            using (SqlConnection connection = new SqlConnection(get_connection_string(_configuration["sqlServerDefaultDatabase"])))
            {
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(_dataset, "Data");
                }
            }
            return _dataset;
        } catch (SqlException ex) {
            // log exception
            throw ex;
        }
    }

    private string get_connection_string(string database_name)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = _configuration["sqlServerUrl"];
        builder.UserID = _configuration["sqlServerUsername"];
        builder.Password = _configuration["sqlServerPassword"];
        builder.InitialCatalog = database_name;
        return builder.ConnectionString;
    }
}