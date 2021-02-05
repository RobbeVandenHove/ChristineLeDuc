using Globals;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Datalayer {
    public interface IDatabase {
        void SetConnectionString(string path);
        List<string> MainSubject();
        string MainSubject(string query);
        List<string> SubSubject();
        string SubSubject(string query);
        List<string> Workers();
        string Worker(long query);
        string Worker(string query);
        Order GetOrderById(string query);
        List<Log> GetLogsByRef(string query);
        void Crud(OleDbCommand cmd);
        int GetLastId();
    }
}
