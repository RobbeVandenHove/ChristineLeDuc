using Globals;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Datalayer {
    public class Database : IDatabase {
        private OleDbConnection conn;
        private string ConnectionString { get; set; }
        public Database() {

        }
        public Database(string path) {
            ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path + "; Persist Security Info = False;";
        }
        public void SetConnectionString(string path) {
            ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + path + "; Persist Security Info = False;";
        }
        public List<string> MainSubject() {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand("SELECT Naam FROM Hoofdonderwerp", conn);
                List<string> result = new List<string>();
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result.Add((string)rdr["Naam"]);
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }
        public string MainSubject(string query) {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand($"SELECT Naam FROM Hoofdonderwerp WHERE Naam = '{query}'", conn);
                string result = null;
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result = (string)rdr["Naam"];
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }
        public List<string> SubSubject() {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand("SELECT Naam FROM Subonderwerp", conn);
                List<string> result = new List<string>();
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result.Add((string)rdr["Naam"]);
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }
        public string SubSubject(string query) {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand($"SELECT Naam FROM Subonderwerp WHERE Naam = '{query}'", conn);
                string result = null;
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result = (string)rdr["Naam"];
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }
        public List<string> Workers() {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand("SELECT Voornaam FROM Medewerker", conn);
                List<string> result = new List<string>();
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result.Add((string)rdr["Voornaam"]);
                    }
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
                return result;
            }
        }
        public string Worker(long query) {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand($"SELECT Voornaam FROM Medewerker WHERE MedewerkerId = {query}", conn);
                string result = null;
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result = (string)rdr["Voornaam"];
                    }
                    conn.Close();
                    conn.Dispose();
                    return result;
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
        }
        public string Worker(string query) {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand($"SELECT Voornaam FROM Medewerker WHERE Voornaam = '{query}'", conn);
                string result = null;
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        result = (string)rdr["Voornaam"];
                    }
                    conn.Close();
                    conn.Dispose();
                    return result;
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
        }
        public Order GetOrderById(string query) {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand($"SELECT * FROM OrderData WHERE ReferentieNummer = '{query}'", conn);
                Order result = new Order();
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    result = new Order {
                        ReferentieNummer = rdr["ReferentieNummer"] == DBNull.Value ? "unknown" : (string)rdr["ReferentieNummer"],
                        OrderId = rdr["OrderId"] == DBNull.Value ? "unknown" : (string)rdr["OrderId"],
                        Factuuradres = rdr["Factuuradres"] == DBNull.Value ? "unknown" : (string)rdr["Factuuradres"],
                        FactuurPostcode = rdr["FactuurPostcode"] == DBNull.Value ? "unknown" : (string)rdr["FactuurPostcode"],
                        FactuurWoonplaats = rdr["FactuurWoonplaats"] == DBNull.Value ? "unknown" : (string)rdr["FactuurWoonplaats"],
                        FactuurLand = rdr["FactuurLand"] == DBNull.Value ? "unknown" : (string)rdr["FactuurLand"],
                        Verzendnaam = rdr["Verzendnaam"] == DBNull.Value ? "unknown" : (string)rdr["Verzendnaam"],
                        Verzendadres = rdr["Verzendadres"] == DBNull.Value ? "unknown" : (string)rdr["Verzendadres"],
                        VerzendPostcode = rdr["VerzendPostcode"] == DBNull.Value ? "unknown" : (string)rdr["VerzendPostcode"],
                        VerzendWoonplaats = rdr["VerzendWoonplaats"] == DBNull.Value ? "unknown" : (string)rdr["VerzendWoonplaats"],
                        VerzendLand = rdr["VerzendLand"] == DBNull.Value ? "unknown" : (string)rdr["VerzendLand"],
                        Telefoonnummer = rdr["Telefoonnummer"] == DBNull.Value ? "unknown" : (string)rdr["Telefoonnummer"],
                        Emailadres = rdr["Emailadres"] == DBNull.Value ? "unknown" : (string)rdr["Emailadres"],
                        Orderdatum = rdr["Orderdatum"] == DBNull.Value ? "unknown" : (string)rdr["Orderdatum"],
                        Ordertijd = rdr["Ordertijd"] == DBNull.Value ? "unknown" : (string)rdr["Ordertijd"]
                    };
                    conn.Close();
                    conn.Dispose();
                    return result;
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
        }
        public void Crud(OleDbCommand cmd) {
            using (conn = new OleDbConnection(ConnectionString)) {
                try {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }            
        }
        public int GetLastId() {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand("SELECT Volgnummer FROM LogBoek ORDER BY Volgnummer DESC", conn);
                try {
                    conn.Open();
                    var id = cmd.ExecuteScalar() == null ? 0 : (int)cmd.ExecuteScalar();
                    conn.Close();
                    conn.Dispose();
                    return id;
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
            }            
        }
        public List<Log> GetLogsByRef(string query) {
            using (conn = new OleDbConnection(ConnectionString)) {
                OleDbCommand cmd = new OleDbCommand($"SELECT * FROM LogBoek WHERE ReferentieNummer = '{query}' ORDER BY Volgnummer DESC", conn);
                List<Log> result = new List<Log>();
                try {
                    conn.Open();
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        Log order = new Log {
                            Volgnummer = rdr["Volgnummer"] == DBNull.Value ? "unknown" : rdr["Volgnummer"].ToString(),
                            ReferentieNummer = rdr["ReferentieNummer"] == DBNull.Value ? "unknown" : (string)rdr["ReferentieNummer"],
                            OrderId = rdr["OrderId"] == DBNull.Value ? "unknown" : (string)rdr["OrderId"],
                            Index = rdr["Index"] == DBNull.Value ? "unknown" : (string)rdr["Index"],
                            Hoofdonderwerp = rdr["Hoofdonderwerp"] == DBNull.Value ? "unknown" : (string)rdr["Hoofdonderwerp"],
                            Subonderwerp = rdr["Subonderwerp"] == DBNull.Value ? "unknown" : (string)rdr["Subonderwerp"],
                            Memo = rdr["Memo"] == DBNull.Value ? "unknown" : (string)rdr["Memo"],
                            MedewerkerId = rdr["MedewerkerId"] == DBNull.Value ? "Unknown" : rdr["MedewerkerId"].ToString(),
                            Medewerker = rdr["MedewerkerId"] == DBNull.Value ? "Unknown" : Worker(Int32.Parse(rdr["MedewerkerId"].ToString())),
                            Datum = rdr["Datum"] == DBNull.Value ? "unknown" : (string)rdr["Datum"],
                            Status = rdr["Status"] == DBNull.Value ? "unknown" : (string)rdr["Status"],
                        };
                        result.Add(order);
                    }
                    conn.Close();
                    conn.Dispose();
                    return result;
                } catch(Exception ex) {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
