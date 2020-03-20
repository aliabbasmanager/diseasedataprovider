namespace DiseaseDataProvider.Interfaces
{
    public interface IStateDataProvider
    {
        int get_total_confirmed_cases_ind_by_state(string state_name);

        int get_total_confirmed_cases_int_by_state(string state_name);
    }
}