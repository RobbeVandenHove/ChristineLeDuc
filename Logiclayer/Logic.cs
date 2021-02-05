using Datalayer;
using Globals;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Logiclayer {
    public class Logic : ILogic {
        public IDatabase db;
        public long MedewerkerId { get; set; }

        public event Action<string> Connected;
        public Logic() {
            db = new Database();
        }
        public void MakeConnection(string path, long medewerkerId) {
            db.SetConnectionString(path);
            MedewerkerId = medewerkerId;
            try {
                Connected?.Invoke(GetWorker(medewerkerId));
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }            
        }
        public void DestroyConnection() {
            db = new Database();
        }
        public List<string> GetMainSubject() {
            try {
                return db.MainSubject();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public string GetMainSubject(string query) {
            try {
                return db.MainSubject(query);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public List<string> GetSubSubject() {
            try {
                return db.SubSubject();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public string GetSubSubject(string query) {
            try {
                return db.SubSubject(query);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public List<string> GetWorkers() {
            try {
                return db.Workers();
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public string GetWorker(long query) {
            try {
                return db.Worker(query);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public string GetWorker(string query) {
            try {
                return db.Worker(query);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public Order GetOrderData(string query) {
            try {
                var order = db.GetOrderById(query);
                if (string.IsNullOrEmpty(order.ReferentieNummer)) throw new ArgumentException("Order niet gevonden of is inactief.");
                else return order;
            } catch(ArgumentException argex) {
                throw new ArgumentException(argex.Message);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public void InsertData(string data) {
            try {
                var dataArr = data.Split(";");
                OleDbCommand cmd = new OleDbCommand($"INSERT INTO LogBoek ([ReferentieNummer], [OrderId], [Index], [Hoofdonderwerp], [Subonderwerp], [Memo], [Datum], [MedewerkerId], [Status]) " +
                                                    $"VALUES (@ref, @order, @ind, @hoofd, @sub, @mem, @dtum, @mede, @stat)");
                cmd.Parameters.AddWithValue("@ref", dataArr[0]);
                cmd.Parameters.AddWithValue("@order", dataArr[1]);
                cmd.Parameters.AddWithValue("@ind", dataArr[2]);
                cmd.Parameters.AddWithValue("@hoofd", dataArr[3]);
                cmd.Parameters.AddWithValue("@sub", dataArr[4]);
                cmd.Parameters.AddWithValue("@mem", dataArr[5]);
                cmd.Parameters.AddWithValue("@dtum", dataArr[6]);
                cmd.Parameters.AddWithValue("@mede", this.MedewerkerId.ToString());
                cmd.Parameters.AddWithValue("@stat", dataArr[7]);
                db.Crud(cmd);
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }            
        }
        public int GetLastId() {
            int id = db.GetLastId();
            return ++id;
        }
        public List<Log> GetMultipleLogs(string query) {
            return db.GetLogsByRef(query);
        }
        public void UpdateLog(string memo, string status, string id) {
            try {
                int volg = Int32.Parse(id);
                OleDbCommand cmd = new OleDbCommand(@"UPDATE LogBoek SET [Memo] = @mem, [Status] = @stat WHERE Volgnummer = @volgnr");
                cmd.Parameters.AddWithValue("@mem", memo);
                cmd.Parameters.AddWithValue("@stat", status);
                cmd.Parameters.AddWithValue("@volgnr", volg);
                db.Crud(cmd);
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
