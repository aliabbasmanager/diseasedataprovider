using System.Data;

namespace diseasedataprovider.Interfaces
{
    public interface IDataHelper
    {
        DataSet executeDataQuery(string query);
    }
}