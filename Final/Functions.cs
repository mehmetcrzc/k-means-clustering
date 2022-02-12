using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final
{
    public class Functions
    {
        public static List<List<Double>> Transpose(List<List<Double>> data)//To transpose the data list for calculations.
        {
            List<List<Double>> temporaryData = new List<List<Double>>();

            for (int i = 0; i < data[0].Count(); i++)
            {
                temporaryData.Add(new List<Double>());

                for (int j = 0; j < data.Count(); j++)
                {
                    temporaryData[i].Add(data[j][i]);
                }
            }
            return temporaryData;
        }

        public static List<List<Double>> Normalize(List<List<Double>> rawData)//To normalize (min-max) the rawdata.
        {
            List<List<Double>> temporaryData = new List<List<Double>>();

            temporaryData = Transpose(rawData);

            foreach (var a in temporaryData)
            {
                Double max = a.Max();

                Double min = a.Min();

                for (int i = 0; i < a.Count(); i++)
                {
                    a[i] = Math.Round((a[i] - min) / (max - min), 2);
                }
            }
            return Transpose(temporaryData);
        }


        public static List<List<Double>> FindDistance(List<List<Double>> normalizedData, List<List<Double>> centers)//To find the distances between data and center of clusters.
        {
            Double temporaryDistance;

            List<List<Double>> distance = new List<List<Double>>();

            for (int i = 0; i < normalizedData.Count(); i++)
            {
                distance.Add(new List<Double>());

                for (int l = 0; l < centers.Count(); l++)
                {
                    temporaryDistance = 0;

                    for (int j = 0; j < normalizedData[0].Count(); j++)
                    {
                        temporaryDistance += Math.Pow((normalizedData[i][j] - centers[l][j]), 2);//To find the Euclidean distances.
                    }

                    distance[i].Add(Math.Sqrt(temporaryDistance));
                }
            }
            return distance;
        }


        public static List<int> FindMinDistance(List<List<Double>> distance)//To assign data to clusters.
        {
            List<int> clusters = new List<int>();

            Double value;

            for (int i = 0; i < distance.Count(); i++)
            {
                clusters.Add(0);

                value = distance[i].Min<Double>();

                for (int j = 0; j < distance[0].Count(); j++)
                {
                    if (value.Equals(distance[i][j]))
                        clusters[i] = j + 1;
                }
                
            }
            return clusters;
        }


        public static List<List<Double>> FindCenters(List<List<Double>> normalizedData, List<int> clusters, int k)//To find the new centers of clusters.
        {
            List<List<Double>> centers = new List<List<Double>>();

            int[] centerNumber = new int[k];

            for (int i = 0; i < k; i++)
            {
                centers.Add(new List<Double>());

                for (int j = 0; j < normalizedData[0].Count(); j++)
                {
                    centers[i].Add(0);
                }
            }

            for (int i = 0; i < clusters.Count(); i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (clusters[i] == j + 1)
                    {
                        centerNumber[j] += 1;//To find the number of clusters.

                        for (int z = 0; z < normalizedData[0].Count(); z++)
                        {
                            centers[j][z] += normalizedData[i][z];
                        }
                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < normalizedData[0].Count(); j++)
                {
                    centers[i][j] /= centerNumber[i];//To find average values.
                }
            }
            return centers;
        }


        public static bool CheckClusters(List<int> cluster1, List<int> cluster2)//To check similarity of clusters.
        {
            for (int i = 0; i < cluster1.Count(); i++)
            {
                if (cluster1[i] != cluster2[i])
                    return false;
            }
            return true;
        }


        public static void CreateTxtFile(Clustering cluster, string fileDirectory)
        {
            StreamWriter file = new StreamWriter(fileDirectory);

            int[] centerNumber = new int[cluster.k];

            for (int i = 0; i < cluster.clusters.Count(); i++)
            {
                for (int j = 0; j < cluster.k; j++)
                {
                    if (cluster.clusters[i] == j + 1)
                    {
                        centerNumber[j] += 1;//To find the number of clusters.
                    }
                }
            }

            for (int i = 0; i < cluster.normalizedData.Count(); i++)
            {
                file.WriteLine("Record: " + (i+1).ToString() + "\t" + "Cluster: " + cluster.clusters[i].ToString());
            }

            for (int i = 0; i < centerNumber.Length; i++)
            {
                file.WriteLine("Cluster " + (i + 1).ToString() + ": " + centerNumber[i].ToString() + "\tRecords");
            }

            file.WriteLine("WCSS: " + cluster.WCSS);

            file.WriteLine("BCSS: " + cluster.BCSS);

            file.WriteLine("Dunn Index: " + cluster.DunnIndex);

            file.Close();
        }

        public static Double CalculateWCSS(List<List<Double>> centers, List<List<Double>> normalizedData, List<int> clusters)
        {
            Double WCSS = 0;

            for (int i = 0; i < normalizedData.Count(); i++)
            {
                for (int j = 0; j < normalizedData[0].Count(); j++)
                {
                    WCSS += Math.Pow((normalizedData[i][j] - centers[clusters[i] - 1][j]), 2);
                }
            }

            return Math.Round(WCSS, 2);
        }

        public static Double CalculateTSS(List<List<Double>> normalizedData)
        {
            List<Double> mean = new List<Double>();

            Double TSS = 0;

            for (int i = 0; i < normalizedData[0].Count(); i++)
            {
                mean.Add(0);
            }

            for (int i = 0; i < normalizedData.Count(); i++)
            {
                for (int j = 0; j < normalizedData[0].Count(); j++)
                {
                    mean[j] += normalizedData[i][j];
                }
            }

            for (int i = 0; i < normalizedData[0].Count(); i++)
            {
                mean[i] /= normalizedData.Count();
            }

            for (int i = 0; i < normalizedData.Count(); i++)
            {
                for (int j = 0; j < normalizedData[0].Count(); j++)
                {
                    TSS += Math.Pow((normalizedData[i][j] - mean[j]), 2);
                }
            }

            return Math.Round(TSS, 2);
        }

        public static Double CalculateDunnIndex(List<List<Double>> centers, List<List<Double>> normalizedData, List<int> clusters, int k)
        {
            List<Double> interDistance = new List<Double>();

            List<Double> intraDistance = new List<Double>();

            int[] centerNumber = new int[k];

            Double temporaryValue = 0;

            Double DunnIndex = 0;

            for (int i = 0; i < centers.Count(); i++)
            {
                for (int z = 0; z < centers.Count() - 1; z++)
                {
                    if (i != z)
                    {
                        for (int j = 0; j < centers[0].Count(); j++)
                        {
                            temporaryValue += Math.Pow((centers[i][j] - centers[z][j]), 2);
                        }

                        interDistance.Add(Math.Sqrt(temporaryValue));

                        temporaryValue = 0;
                    }                 
                }
            }

            for (int i = 0; i < k; i++)
            {
                intraDistance.Add(0);
            }

            for (int i = 0; i < clusters.Count(); i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (clusters[i] == j + 1)
                    {
                        centerNumber[j] += 1;//To find the number of clusters.
                    }
                }
            }

            for (int i = 0; i < normalizedData.Count(); i++)
            {
                for (int j = 0; j < normalizedData[0].Count(); j++)
                {
                    temporaryValue += Math.Pow((normalizedData[i][j] - centers[clusters[i]-1][j]), 2);
                }

                intraDistance[clusters[i] - 1] += Math.Sqrt(temporaryValue);

                temporaryValue = 0;                 
            }

            for (int i = 0; i < k; i++)
            {
                intraDistance[i] /= centerNumber[i];//To find average distance of intra-distances.
            }

            DunnIndex = Math.Round(interDistance.Min() / intraDistance.Max(), 2);

            return DunnIndex;
        }

    }
}
