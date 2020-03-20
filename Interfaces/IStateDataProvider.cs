using System;
using System.Collections.Generic;

namespace DiseaseDataProvider.Interfaces
{
    public interface IStateDataProvider
    {
        int get_total_confirmed_cases_ind_by_state(string state_name);
        int get_total_confirmed_cases_int_by_state(string state_name);
        List<string> get_all_state_names();
        int get_total_case_count_by_date(DateTime date = default(DateTime), string state_name = "");
        Dictionary<string, string> get_current_case_count_all_states();
        Dictionary<string, string> get_historical_data_per_state(string state_name);
        Dictionary<string, string> get_cumulative_historical_data_per_state(string state_name);
    }
}