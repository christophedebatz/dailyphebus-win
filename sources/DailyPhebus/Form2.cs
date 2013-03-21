using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyPhebus
{
    public partial class LoadForm : Form
    {

        public enum LoadType { Lines, Stops, DailySchedules, HourlySchedules };
        private LoadType type;
        private MainForm parent;
        private BusLine selectedLine;
        private BusStop selectedStop;
        private short direction;

        public LoadForm(MainForm parent, LoadType loadType, BusLine selectedLine = null, BusStop selectedStop = null, short direction = 0)
        {
            this.parent = parent;
            this.type = loadType;
            this.selectedLine = selectedLine;
            this.selectedStop = selectedStop;
            this.direction = direction;

            parent.Enabled = false;
            InitializeComponent();
            bgwDesign.RunWorkerAsync();
        }

        private void bgwDesign_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            GetAndTreatSchedules gats = new GetAndTreatSchedules();
            switch (type)
            {
                case LoadType.Lines:
                    List<BusLine> linesList = gats.getBusLines();
                    e.Result = linesList;
                    break;

                case LoadType.Stops:
                    List<BusStop> stopsList = gats.getBusStops(this.selectedLine);
                    e.Result = stopsList;
                    break;

                case LoadType.DailySchedules:
                case LoadType.HourlySchedules:
                    e.Result = gats.getBusSchedules(
                        type,
                        this.selectedLine,
                        new BusStop(
                            this.selectedStop.name,
                            this.selectedStop.code
                        ), 
                        parent.getDateTimePicker2().Value.ToShortDateString().ToString(),
                        parent.getDateTimePicker1().Value.Hour.ToString(),
                        (short)this.direction
                     );
                    break;

                default:
                    this.Close();
                    parent.Enabled = true;
                    break;
            }
        }

        private void bgwDesign_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
 
            if (e.Error != null)
            {
                MessageBox.Show("Une erreur est survenue ! Détail : " + e.Error.Message);
                return;
            }

            switch (type)
            {
                case LoadType.Lines:
                    parent.busLines.Clear();
                    parent.getComboBusLines().Items.Clear();
                    parent.getComboBusStops().Items.Clear();
                    foreach (BusLine line in (List<BusLine>)e.Result)
                    {
                        parent.busLines.Add(new BusLine(line.letter, line.firstExtremity, line.secondExtremity));
                        parent.getComboBusLines().Items.Add(line.ToString());
                    }
                    parent.getComboBusLines().SelectedIndex = 0;
                    break;
                
                case LoadType.Stops:
                    parent.busStops.Clear();
                    parent.getComboBusStops().Items.Clear();
                    foreach (BusStop stop in (List<BusStop>)e.Result)
                    {
                        parent.busStops.Add(new BusStop(stop.name, stop.code));
                        parent.getComboBusStops().Items.Add(stop.ToString());
                    }
                    parent.getComboBusStops().SelectedIndex = 0;
                    parent.getComboBusStops().Enabled = true;
                    break;

                case LoadType.DailySchedules:
                case LoadType.HourlySchedules:
                    parent.showSchedules(type, (string)e.Result);
                    break;

                default:
                    break;
            }

            this.Close();
            parent.Enabled = true;
        }

    }
}