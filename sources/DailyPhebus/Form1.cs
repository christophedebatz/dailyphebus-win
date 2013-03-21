using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DailyPhebus
{
    public partial class MainForm : Form
    {
        private TimeListBox tlb;
        private BusLine selectedLine;
        public List<BusLine> busLines { get; set; }
        public List<BusStop> busStops { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.Visible = true;
            this.button1.Enabled = false;
            this.button2.Enabled = false;

            busLines = new List<BusLine>();
            busStops = new List<BusStop>();

            LoadForm lf = new LoadForm(this, LoadForm.LoadType.Lines);
            lf.ShowDialog();
        }

        private void comboBusLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBusLines.Enabled = true;
            this.comboDirection.Items.Clear();
            selectedLine = BusLine.getLineByComboSelection(comboBusLines.GetItemText(comboBusLines.SelectedItem));
          
            if (selectedLine != BusLine.Zero)
            {
                this.comboDirection.Items.Add((string)"Vers " + selectedLine.firstExtremity);
                this.comboDirection.Items.Add((string)"Vers " + selectedLine.secondExtremity);
                this.comboDirection.Enabled = true;
                this.comboDirection.SelectedIndex = 0;
                
                LoadForm lf = new LoadForm(this, LoadForm.LoadType.Stops, selectedLine);
                lf.ShowDialog();

                this.button1.Enabled = true;
                this.button2.Enabled = true;

                string letter = selectedLine.letter;
                if (selectedLine.letter.Equals("00N1"))
                    letter = "Phebus de Nuit";
             
                this.labelTime.Text = "Horaires ligne " + letter + ", direction " + selectedLine.getExtremityByDirection((short) (this.comboDirection.SelectedIndex == 0 ? 1 : 2));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadForm lf = new LoadForm(this, LoadForm.LoadType.DailySchedules, selectedLine, this.busStops[comboBusStops.SelectedIndex], (short)(this.comboDirection.SelectedIndex == 0 ? 2 : 1));
            lf.ShowDialog(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadForm lf = new LoadForm(this, LoadForm.LoadType.HourlySchedules, selectedLine, this.busStops[comboBusStops.SelectedIndex], (short)(this.comboDirection.SelectedIndex == 0 ? 2 : 1));
            lf.ShowDialog();
        }




        private void button3_Click(object sender, EventArgs e)
        {
            Controls.Remove(tlb);
            this.button3.Visible = false;
            this.button2.Visible = true;
            this.button1.Visible = true;
            this.groupBox1.Visible = true;
            this.groupBox2.Visible = true;
            this.labelTime.Visible = false;
        }

        public void showSchedules(LoadForm.LoadType type, string data)
        {
            if (data == null || data == "")
            {
                MessageBox.Show("Il n'y a pas d'horaire disponible pour cette recherche...", 
                                "Aucun horaire", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information
                                );
                return;
            }
            this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = true;
            this.labelTime.Visible = true;

            SuspendLayout();
            tlb = new TimeListBox();
            tlb.setType(type);
            tlb.Visible = true;

            // daily schedules
            if (type == LoadForm.LoadType.DailySchedules)
            {
                string[] eachHourArray = data.Split(';');
                for (int i = 0; i < eachHourArray.Length; i++)
                {
                    if (this.selectedLine.letter.Equals("00N1") && !eachHourArray[i].Contains('|'))
                        continue;

                    string[] hourSchedulesArray = eachHourArray[i].Split('|');
                    if (!this.selectedLine.letter.Equals("00N1") && int.Parse(hourSchedulesArray[0]) < 5)
                        continue;

                    string tempRow = int.Parse(hourSchedulesArray[0]) < 10 ? "0" + hourSchedulesArray[0] + "h  " : hourSchedulesArray[0] + "h  ";
                    for (int j = 1; j < hourSchedulesArray.Length; j++)
                        tempRow += (j > 1) ? " | " + hourSchedulesArray[j] : hourSchedulesArray[j];

                    tlb.Items.Add(tempRow);
                }

            }

            // hourly schedules
            else
            {
                int selectedHour = (int) this.getDateTimePicker1().Value.Hour;

                string[] eachTimeArray = data.Split('|');
                int currentTime, dateMinute = int.Parse(DateTime.Now.ToString("mm"));

                for (int i = 0; i < eachTimeArray.Length; i++)
                {
                    currentTime = int.Parse(eachTimeArray[i].Trim().ToString());
                    if (currentTime < dateMinute)
                        tlb.Items.Add(" - ");
                    else
                        tlb.Items.Add(" " + selectedHour + ":" + eachTimeArray[i] + "  (dans " + (currentTime - dateMinute).ToString() + " minutes)");
                }


            }
            Controls.Add(tlb);
            ResumeLayout(false);
        }



        public ComboBox getComboBusLines()
        {
            return this.comboBusLines;
        }

        public ComboBox getComboBusStops()
        {
            return this.comboBusStops;
        }

        public DateTimePicker getDateTimePicker2()
        {
            return this.dateTimePicker2;
        }

        public DateTimePicker getDateTimePicker1()
        {
            return this.dateTimePicker1;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "http://camasoft.debatz.fr/applications.html";
            p.Start();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "http://christophe.debatz.fr";
            p.Start();
        } 

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = string.Format("{HH:mm:ss}", DateTime.Now);
        }

    }
}
