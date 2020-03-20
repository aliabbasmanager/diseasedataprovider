using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using diseasedataprovider.Interfaces;
using DiseaseDataProvider.Interfaces;

namespace DiseaseDataProvider.DataLayer
{
    public class StateDataProvider : IStateDataProvider
    {
        private IDataHelper _sqlDataHelper;
        public StateDataProvider(IDataHelper sqlDataHelper)
        {
            _sqlDataHelper = sqlDataHelper;
        }
 
        public int get_total_confirmed_cases_ind_by_state(string state_name)
        {
            var count = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select state_name, sum(confirmed_cases_ind) as total_cases from covid19_india group by state_name having state_name = '");
                sb.Append(state_name);
                sb.Append("'");
                String sql = sb.ToString();
                var case_data = _sqlDataHelper.executeDataQuery(sql);
                if (case_data.Tables[0].Rows.Count < 1)
                {
                    throw new ArgumentException("State not found");
                }
                count = Convert.ToInt32(case_data.Tables[0].Rows[0][1]);
            }
            catch (SqlException e)
            {
                throw e;
            }
            return count;
        }

        public int get_total_confirmed_cases_int_by_state(string state_name)
        {
            var count = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select state_name, sum(confirmed_cases_int) as total_cases from covid19_india group by state_name having state_name = '");
                sb.Append(state_name);
                sb.Append("'");
                String sql = sb.ToString();
                var case_data = _sqlDataHelper.executeDataQuery(sql);
                if (case_data.Tables[0].Rows.Count < 1)
                {
                    throw new ArgumentException("State not found");
                }
                count = Convert.ToInt32(case_data.Tables[0].Rows[0][1]);
            }
            catch (SqlException e)
            {
                throw e;
            }
            return count;
        }
    }
}