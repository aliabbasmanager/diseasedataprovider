using System;
using System.Collections.Generic;
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
        public List<string> get_all_state_names()
        {
            List<string> states = new List<string>();
            try
            {
                var query = "select distinct state_name from covid19_india";
                var case_data = _sqlDataHelper.executeDataQuery(query);
                if (string.IsNullOrEmpty(case_data.Tables[0].Rows[0][0].ToString()))
                {
                    throw new DataException("No States Found");
                }
                foreach (DataRow row in case_data.Tables[0].Rows)
                {
                    states.Add(row[0].ToString());
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            return states;
        }

        public int get_total_case_count_by_date(DateTime date = default(DateTime), string state_name = "")
        {
            var count = 0;
            try
            {
                // Provide Current Date Value if Date is empty
                if (date == default(DateTime))
                {
                    date = DateTime.Now;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("select sum(confirmed_cases_ind + confirmed_cases_int) from covid19_india_stage where CONVERT(date, date) = '");
                sb.Append(date.Date.ToString("d"));
                sb.Append("'");

                if (!string.IsNullOrWhiteSpace(state_name))
                {
                    sb.Append(" and state_name='");
                    sb.Append(state_name);
                    sb.Append("'");
                }

                String sql = sb.ToString();
                var case_data = _sqlDataHelper.executeDataQuery(sql);
                if (string.IsNullOrEmpty(case_data.Tables[0].Rows[0][0].ToString()))
                {
                    throw new DataException("No Data Found");
                }
                count = Convert.ToInt32(case_data.Tables[0].Rows[0][0]);
            }
            catch (SqlException e)
            {
                throw e;
            }
            return count;
        }

        public Dictionary<string, string> get_current_case_count_all_states()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                var query = "select state_name, sum(confirmed_cases_ind + confirmed_cases_int) from covid19_india group by state_name";
                var case_data = _sqlDataHelper.executeDataQuery(query);
                if (string.IsNullOrEmpty(case_data.Tables[0].Rows[0][0].ToString()))
                {
                    throw new DataException("No States Found");
                }

                foreach (DataRow row in case_data.Tables[0].Rows)
                {
                    data.Add(row[0].ToString(), row[1].ToString());
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            return data;
        }

        public Dictionary<string, string> get_historical_data_per_state(string state_name)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select CONVERT(date, date), confirmed_cases_ind + confirmed_cases_int as number_of_cases from covid19_india where state_name = '");
                sb.Append(state_name);
                sb.Append("'");

                String query = sb.ToString();

                var historical_data = _sqlDataHelper.executeDataQuery(query);
                if (historical_data.Tables[0].Rows.Count < 1)
                {
                    throw new DataException("State Not Found");
                }

                foreach (DataRow row in historical_data.Tables[0].Rows)
                {
                    data.Add( Convert.ToDateTime(row[0]).Date.ToString("d"), row[1].ToString());
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            return data;
        }

         public Dictionary<string, string> get_cumulative_historical_data_per_state(string state_name)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select CONVERT(date, date), confirmed_cases_ind + confirmed_cases_int as number_of_cases from covid19_india_stage where state_name = '");
                sb.Append(state_name);
                sb.Append("'");

                String query = sb.ToString();

                var historical_data = _sqlDataHelper.executeDataQuery(query);
                if (historical_data.Tables[0].Rows.Count < 1)
                {
                    throw new DataException("State Not Found");
                }

                foreach (DataRow row in historical_data.Tables[0].Rows)
                {
                    data.Add( Convert.ToDateTime(row[0]).Date.ToString("d"), row[1].ToString());
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            return data;
        }

    }
}