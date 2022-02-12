using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void MakeChart(Clustering cluster, int xAxis, int yAxis, string xName, string yName)
        {
            label1.Text = xName;

            label2.Text = yName;

            chart1.Titles.Add("k-Means Clustering");

            for (int i = 1; i < cluster.k + 1; i++)
            {
                chart1.Series.Add("Cluster: " + i.ToString());

                chart1.Series["Cluster: " + i.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }

            for (int i = 0; i < cluster.normalizedData.Count(); i++)
            {
                chart1.Series["Cluster: " + (cluster.clusters[i]).ToString()].Points.AddXY(cluster.rawData[i][xAxis], cluster.rawData[i][yAxis]);
            }
        }
    }
}
