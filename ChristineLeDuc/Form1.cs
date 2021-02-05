using System;
using System.Windows.Forms;
using Logiclayer;
using Globals;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ChristineLeDuc {
    public partial class Form1 : Form {
        private ILogic logic;
        private string medewerker;
        private string database;
        private bool login;
        private Order searchOrder;
        private List<Log> editLogs;
        private Log selected;
        private SynchronizationContext label;
        public Form1() {
            InitializeComponent();
            logic = new Logic();
            logic.Connected += ConnectionEstablished;
            this.errorLabel.Text = "";
            this.addErrorLabel.Text = "";
            this.editErrorLabel.Text = "";
            this.panel1.BringToFront();
            ClearLabels();
            this.selected = new Log();
            this.searchOrder = new Order();
            this.editLogs = new List<Log>();
            label = SynchronizationContext.Current;
        }

        private void InitializeComboboxes() {
            try {
                var thread1 = Task.Run(() => {
                    lock (logic) {
                        var main = logic.GetMainSubject();
                        foreach (string item in main) {
                            label.Post((c) => this.comboBox1.Items.Add(item), null);
                        }
                    }                    
                });
                var thread2 = Task.Run(() => {
                    lock (logic) {
                        var sub = logic.GetSubSubject();
                        foreach (string item in sub) {
                            label.Post((c) => this.comboBox2.Items.Add(item), null);
                        }
                    }                    
                });
                var thread3 = Task.Run(() => {
                    lock (logic) {
                        var worker = logic.GetWorkers();
                        foreach (string item in worker) {
                            label.Post((c) => this.comboBox3.Items.Add(item), null);
                        }
                    }                    
                    label.Post((c) => this.comboBox3.SelectedIndex = comboBox3.FindStringExact(medewerker), null);
                });
                var thread4 = Task.Run(() => {
                    label.Post((c) => this.comboBox4.Items.AddRange(new object[] { "Open", "Gesloten" }), null);
                    label.Post((c) => this.comboBox5.Items.AddRange(new object[] { "Open", "Gesloten" }), null);
                }); 
            }
            catch (Exception ex) {
                this.errorLabel.Text = "Oeps er ging iets mis...";
            }
        }

        private void button1_Click(Object sender, EventArgs e) {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK) database = file.FileName;
            this.textBox2.Text = database;
        }

        private void button2_Click(Object sender, EventArgs e) {
            errorLabel.Text = "";
            if (string.IsNullOrEmpty(this.textBox1.Text)) this.errorLabel.Text = "Gelieve een medewerker id in te voeren.";
            if (string.IsNullOrEmpty(this.textBox2.Text)) this.errorLabel.Text = "Gelieve een access database toe te voegen (*.accdb).";
            else {
                try {
                    long id = Int64.Parse(this.textBox1.Text);
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    var loginThread = Task.Run(() => {
                        logic.MakeConnection(database, id);
                    });
                } catch(Exception ex) {
                    this.errorLabel.Text = "Oeps er ging iets mis...";
                }
            }
        }

        private void ConnectionEstablished(string medewerker) {
            if (!string.IsNullOrEmpty(medewerker)) {
                label.Post((c) => this.medewerker = medewerker, null);
                this.BeginInvoke(new MethodInvoker(InitializeComboboxes));
                label.Post((c) => this.toolStripMenuItem1.Text = medewerker, null);
                label.Post((c) => this.login = true, null);
                label.Post((c) => this.panel2.BringToFront(), null);
                label.Post((c) => this.label7.Text = medewerker, null);
                label.Post((c) => this.label8.Text = database, null);
            }
            else {
                label.Post((c) => this.logic.DestroyConnection(), null);
                label.Post((c) => this.database = "", null);
                label.Post((c) => this.login = false, null);
                label.Post((c) => errorLabel.Text = "Medewerker niet gevonden.", null);
            }
        }

        private void button3_Click(Object sender, EventArgs e) {
            var logicThread = Task.Run(() => {
                label.Post((c) => this.logic.DestroyConnection(), null);
                label.Post((c) => this.errorLabel.Text = "", null);
                label.Post((c) => this.label7.Text = "", null);
                label.Post((c) => this.label8.Text = "", null);
                label.Post((c) => this.database = "", null);
                label.Post((c) => this.medewerker = "", null);
                label.Post((c) => this.comboBox1.Items.Clear(), null);
                label.Post((c) => this.comboBox2.Items.Clear(), null);
                label.Post((c) => this.comboBox3.Items.Clear(), null);
                label.Post((c) => this.comboBox4.Items.Clear(), null);
            });
            var logicThread2 = Task.Run(() => {
                label.Post((c) => this.panel1.BringToFront(), null);
            });            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            switch (e.ClickedItem.Text) {
                case "Login" : LoginOverview(); break;
                case "Nieuwe klacht" : NewComplaintOverview(); break;
                case "Bestaande klacht" : ExistingComplaintOverview(); break;
                default : LoginOverview(); break;
            }
        }

        private void LoginOverview() {
            if (login) {
                var thread1 = Task.Run(() => {
                    label.Post((c) => this.panel2.BringToFront(), null);
                });
            }
            else {
                var thread2 = Task.Run(() => {
                    label.Post((c) => this.panel1.BringToFront(), null);
                });
            }
        }

        private void NewComplaintOverview() {
            if (login) {
                var thread1 = Task.Run(() => {
                    label.Post((c) => this.panel3.BringToFront(), null);
                });
            }
            else {
                var thread2 = Task.Run(() => {
                    label.Post((c) => this.panel1.BringToFront(), null);
                });
            }
        }

        private void ExistingComplaintOverview() {
            if (login) {
                var thread1 = Task.Run(() => {
                    label.Post((c) => this.panel4.BringToFront(), null);
                });
            }
            else {
                var thread2 = Task.Run(() => {
                    label.Post((c) => this.panel1.BringToFront(), null);
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.comboBox4.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void ClearLabels() {
            var thread1 = Task.Run(() => {
                label.Post((c) => this.label28.Text = "" , null);
                label.Post((c) => this.label29.Text = "" , null);
                label.Post((c) => this.label30.Text = "" , null);
                label.Post((c) => this.label31.Text = "" , null);
                label.Post((c) => this.label32.Text = "" , null);
                label.Post((c) => this.label33.Text = "" , null);
                label.Post((c) => this.label34.Text = "" , null);
                label.Post((c) => this.label35.Text = "" , null);
                label.Post((c) => this.label36.Text = "" , null);
                label.Post((c) => this.label37.Text = "" , null);
                label.Post((c) => this.label38.Text = "" , null);
                label.Post((c) => this.label39.Text = "" , null);
                label.Post((c) => this.label40.Text = "" , null);
                label.Post((c) => this.errorLabel.Text = "" , null);
                label.Post((c) => this.addErrorLabel.Text = "" , null);
            });
        }

        private void button7_Clicked(Object sender, EventArgs e) {
            ClearLabels();
        }

        private void textBox3_TextChanged(Object sender, EventArgs e) {
            if (this.textBox3.TextLength == 10) FindOrder();
            else this.addErrorLabel.Text = "";
        }

        private void button6_Clicked(Object sender, EventArgs e) {
            this.addErrorLabel.Text = "";
            if (this.textBox3.Text.Length == 10)  FindOrder();
            else this.addErrorLabel.Text = "Gelieve een correct Referentienummer-formaat in te geven.";
        }

        private void FindOrder() {
            try {
                var thread = Task.Run(() => {
                    lock (logic) {
                        try {
                            var order = logic.GetOrderData(this.textBox3.Text);
                            label.Post((c) => searchOrder = order, null);
                            label.Post((c) => this.label28.Text = order.OrderId, null);
                            label.Post((c) => this.label29.Text = order.Factuuradres, null);
                            label.Post((c) => this.label30.Text = order.FactuurPostcode, null);
                            label.Post((c) => this.label31.Text = order.FactuurWoonplaats, null);
                            label.Post((c) => this.label32.Text = order.FactuurLand, null);
                            label.Post((c) => this.label33.Text = order.Verzendnaam, null);
                            label.Post((c) => this.label34.Text = order.Verzendadres, null);
                            label.Post((c) => this.label35.Text = order.VerzendPostcode, null);
                            label.Post((c) => this.label36.Text = order.VerzendLand, null);
                            label.Post((c) => this.label37.Text = order.Telefoonnummer, null);
                            label.Post((c) => this.label38.Text = order.Emailadres, null);
                            label.Post((c) => this.label39.Text = order.Orderdatum, null);
                            label.Post((c) => this.label40.Text = order.Ordertijd, null);
                        } catch(ArgumentException argex) {
                            label.Post((c) => this.addErrorLabel.Text = argex.Message, null);
                        }
                    }                    
                });
            }
            catch (ArgumentException argex) {
                ClearLabels();
                this.addErrorLabel.Text = argex.Message;
            }
            catch (Exception ex) {
                ClearLabels();
                this.addErrorLabel.Text = "Oeps er ging iets mis...";
            }
        }

        private void button5_Clicked(Object sender, EventArgs e) {
            ClearLabels();
            LoginOverview();
        }

        private void button4_Clicked(Object sender, EventArgs e) {
            this.addErrorLabel.Text = "";
            if (CheckIfFIlledInCorrectly()) {                
                int id = logic.GetLastId();
                searchOrder.OrderId = searchOrder.OrderId == null ? "unknown" : searchOrder.OrderId;                
                string dataString = $"{this.textBox3.Text};{searchOrder.OrderId};{this.textBox3.Text}_{searchOrder.OrderId}_{id};" +
                                    $"{this.comboBox1.Text};{this.comboBox2.Text};{this.textBox4.Text};{DateTime.Now.ToString()};" +
                                    $"{this.comboBox4.Text}";
                MessageBox.Show(logic.MedewerkerId.ToString());
                try {
                    logic.InsertData(dataString);
                    MessageBox.Show("Logboek succesvol toegevoegd");
                    this.BeginInvoke(new MethodInvoker(ClearLabels));
                    label.Post((c) => this.textBox3.Text = "", null);
                    label.Post((c) => this.textBox4.Text = "", null);
                    label.Post((c) => this.comboBox1.Text = "", null);
                    label.Post((c) => this.comboBox2.Text = "", null);
                    label.Post((c) => this.comboBox4.Text = "", null);
                } catch(Exception ex) {
                    this.addErrorLabel.Text = ex.Message;
                }                
            }
            else {
                this.addErrorLabel.Text = "Gelieve alle velden correct in te vullen.";
            }
        }

        private bool CheckIfFIlledInCorrectly() {
            if (string.IsNullOrEmpty(this.comboBox1.Text)) return false;
            else if (!string.IsNullOrEmpty(this.comboBox1.Text)) {
                if (string.IsNullOrEmpty(logic.GetMainSubject(this.comboBox1.Text))) return false;
            }
            if (string.IsNullOrEmpty(this.comboBox2.Text)) return false;
            else if (!string.IsNullOrEmpty(this.comboBox2.Text)) {                
                if (string.IsNullOrEmpty(logic.GetSubSubject(this.comboBox2.Text))) return false;
            }
            if (string.IsNullOrEmpty(this.comboBox3.Text)) return false;
            else if (!string.IsNullOrEmpty(this.comboBox3.Text)) {                
                if (string.IsNullOrEmpty(logic.GetWorker(this.comboBox3.Text))) return false;
            }
            if (string.IsNullOrEmpty(this.comboBox4.Text)) return false;
            else if (!string.IsNullOrEmpty(this.comboBox4.Text)) {
                if (this.comboBox4.Text != "Open" && this.comboBox4.Text != "Gesloten") return false;
            }
            
            if (string.IsNullOrEmpty(this.textBox4.Text)) return false;
            if (string.IsNullOrEmpty(this.textBox3.Text) || this.textBox3.Text.Length != 10) return false;
            else if (!string.IsNullOrEmpty(this.textBox3.Text)) {
                try {
                    var order = logic.GetOrderData(this.textBox3.Text);
                    if (order.ReferentieNummer == "Unknown" || string.IsNullOrEmpty(order.ReferentieNummer)) this.errorLabel.Text = "Dit order is niet meer actief.";
                } catch(ArgumentException argex) {
                    this.addErrorLabel.Text = argex.Message;
                }
                catch (Exception ex) {
                    this.errorLabel.Text = "Oeps er ging iets mis...";
                }
            }
            return true;
        }

        private void button9_Click(object sender, EventArgs e) {
            this.editErrorLabel.Text = "";
            if (this.textBox5.Text.Length == 10) {
                var thread1 = Task.Run(() => {
                    lock (logic) {
                        var edit = logic.GetMultipleLogs(this.textBox5.Text);
                        label.Post((c) => this.editLogs = edit, null);
                        this.BeginInvoke(new MethodInvoker(PopulateDataGrid));
                    }
                });
            }
        }

        private void RefreshDataGrid() {
            button9_Click(null, null);
        }

        private void PopulateDataGrid() {
            var thread1 = Task.Run(() => {
                label.Post((c) => this.dataGridView1.Rows.Clear(), null);
                foreach (Log log in editLogs) {
                    label.Post((c) => this.dataGridView1.Rows.Add(log.Volgnummer, log.ReferentieNummer, log.OrderId, log.Index, log.Hoofdonderwerp, log.Subonderwerp, log.Datum, log.Medewerker), null);
                }
            });
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
            this.editErrorLabel.Text = "";
            selected = editLogs[e.RowIndex];
            this.comboBox5.SelectedIndex = this.comboBox5.FindStringExact(selected.Status);
            this.textBox6.Text = selected.Memo;
        }

        private void button10_Click(object sender, EventArgs e) {
            try {
                this.editErrorLabel.Text = "";
                if (this.comboBox5.Text == selected.Status && this.textBox6.Text == selected.Memo || selected == null) this.editErrorLabel.Text = "Er is niets veranderd.";
                else {
                    logic.UpdateLog(this.textBox6.Text, this.comboBox5.Text, selected.Volgnummer);
                    this.dataGridView1.Rows.Clear();
                    RefreshDataGrid();
                }
            } catch(Exception ex) {
                this.editErrorLabel.Text = "Oeps er ging iets mis...";
            }
            
        }

        private void button8_Click(object sender, EventArgs e) {
            this.dataGridView1.Rows.Clear();
        }
    }
}
