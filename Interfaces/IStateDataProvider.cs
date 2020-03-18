namespace DiseaseDataProvider.Interfaces
{
    public interface IStateDataProvider
    {
        int get_total_confirmed_cases_by_state(string state_name);
    }
}