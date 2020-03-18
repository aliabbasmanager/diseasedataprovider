using System;
using System.Data.SqlClient;
using System.Text;
using DiseaseDataProvider.Interfaces;

namespace DiseaseDataProvider.DataLayer
{
    public class StateDataProvider : IStateDataProvider
    {
        public int get_total_confirmed_cases_by_state(string state_name)
        {
            var count = 0;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "diseasedata.database.windows.net";
                builder.UserID = "superuser";
                builder.Password = "P@ssw0rd";
                builder.InitialCatalog = "corona";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select state_name, sum(countPerDay) as total_cases from covid19_india group by state_name having state_name = '");
                    sb.Append(state_name);
                    sb.Append("'");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            count = reader.GetInt32(1);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            return count;
        }
    }
}