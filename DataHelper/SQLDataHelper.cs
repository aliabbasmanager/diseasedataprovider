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
            using (SqlConnection connection = new SqlConnection(get_connection_string(_configuration["sql_server_default_database"])))
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
        builder.DataSource = _configuration["sql_server_url"];
        builder.UserID = _configuration["sql_server_username"];
        builder.Password = _configuration["sql_server_password"];
        builder.InitialCatalog = database_name;
        return builder.ConnectionString;
    }
}