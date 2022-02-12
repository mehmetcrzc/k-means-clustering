using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final
{
    public partial class Form1 : Form
    {
        public string[] identity;

        public List<List<Double>> data = new List<List<Double>>();

        public Clustering clustering = null;

        public string filePath = null;

        public int[] kValues = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public Form1()
        {
            InitializeComponent();

            comboBox3.DataSource = kValues;

            comboBox3.Enabled = false;

            button2.Enabled = false;

            label3.Enabled = false;

            label5.Enabled = false;

            comboBox1.Enabled = false;

            comboBox2.Enabled = false;

            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Text|*.txt|All|*.*";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    ReadTextFile(System.IO.Path.GetFullPath(ofd.FileName));

                    filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ofd.FileName), "Sonuc.txt");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please check the data file, error info: " + ex.Message);
                }
            }                   
        }

        private void ReadTextFile(string filePath)
        {
            data.Clear();

            string[] lines = File.ReadAllLines(filePath);

            identity = lines[0].Split(',');

            foreach (var line in lines)
            {
                data.Add(new List<Double>());

                if (lines.findIndex(line) != 0)
                {
                    string[] values = line.Split(',');

                    for (int i = 0; i < values.Length; i++)
                        data[lines.findIndex(line)].Add(Convert.ToDouble(values[i]));
                }   
            }
            data.RemoveAt(0);

            try//To check data file.
            {
                Functions.Transpose(data);        
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please check the data file, error info: " + ex.Message);   
            }

            foreach (string a in identity)
            {
                comboBox1.Items.Add(a);

                comboBox2.Items.Add(a);
            }

            comboBox3.Enabled = true;

            button2.Enabled = true;

            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(comboBox3.SelectedItem.ToString()) > data.Count() || Int32.Parse(comboBox3.SelectedItem.ToString()) < 2)
            {
                MessageBox.Show("Please select a valid k value");
            }
            else
            {
                comboBox1.Enabled = true;

                comboBox2.Enabled = true;

                label3.Enabled = true;

                label5.Enabled = true;

                button3.Enabled = true;

                clustering = new Clustering(data, Int32.Parse(comboBox3.SelectedItem.ToString()));

                MessageBox.Show("Calculations are done and 'Sonuc.txt' is created at input directory");

                Functions.CreateTxtFile(clustering, filePath);
            }          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select the axes of the graph");
            }
            else if (comboBox1.SelectedItem == comboBox2.SelectedItem)
            {
                MessageBox.Show("Please select different axes for the graph");
            }
            else
            {
                Form2 form2 = new Form2();

                try
                {
                    form2.MakeChart(clustering, identity.findIndex(comboBox1.SelectedItem), identity.findIndex(comboBox2.SelectedItem), comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());

                    form2.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something happened, please try again, error info: " + ex.Message);
                }
            }    
        }
    }

    public static class Extensions
    {
        public static int findIndex<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item);
        }
    }


    
}
