using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final
{
    public class Clustering
    {
        public List<List<Double>> rawData = new List<List<Double>>();

        public List<List<Double>> normalizedData = new List<List<Double>>();

        public List<List<Double>> centers = new List<List<Double>>();

        public List<List<Double>> distance = new List<List<Double>>();

        public List<int> clusters = new List<int>();

        public Double WCSS;

        public Double BCSS;

        public Double DunnIndex;

        public int k;

        public Clustering(List<List<double>> rawData, int k)
        {
            this.rawData = rawData;

            this.k = k;

            DefineClusters();
        }

        public void SelectInitials()
        {
            for (int i = 0; i < k; i++)
            {
                centers.Add(normalizedData[i]); //At the beginning, select 'k' data from normalized data for center of clusters.
            }

            for (int j = 0; j < normalizedData.Count(); j++)
            {
                distance.Add(new List<Double>());

                clusters.Add(1); //At the beginning, all data are assigned to first cluster.
            }
        }

        public void DefineClusters()
        {
            normalizedData = Functions.Normalize(rawData);

            List<int> temporaryClusters = new List<int>();

            SelectInitials();

            for (int i = 0; i < clusters.Count(); i++)
            {
                temporaryClusters.Add(0);
            }

            while (!Functions.CheckClusters(temporaryClusters, clusters))
            {
                temporaryClusters = clusters;

                distance = Functions.FindDistance(normalizedData, centers);

                clusters = Functions.FindMinDistance(distance);

                centers = Functions.FindCenters(normalizedData, clusters, k);
            }

            WCSS = Functions.CalculateWCSS(centers, normalizedData, clusters);

            BCSS = Functions.CalculateTSS(normalizedData) - WCSS;

            DunnIndex = Functions.CalculateDunnIndex(centers, normalizedData, clusters, k);
        }


    }

}
