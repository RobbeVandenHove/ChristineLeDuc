using Globals;
using System;
using System.Collections.Generic;

namespace Logiclayer {
    public interface ILogic {
        long MedewerkerId { get; set; }

        event Action<string> Connected;
        void MakeConnection(string path, long medewerkerId);
        void DestroyConnection();
        List<string> GetMainSubject();
        string GetMainSubject(string query);
        List<string> GetSubSubject();
        string GetSubSubject(string query);
        List<string> GetWorkers();
        string GetWorker(long query);
        string GetWorker(string query);
        Order GetOrderData(string query);
        List<Log> GetMultipleLogs(string query);
        void InsertData(string data);
        int GetLastId();
        void UpdateLog(string memo, string status, string id);
    }
}
