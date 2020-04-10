using System;
using static System.IO.Directory;
using System.IO;
using System.Collections.Generic;

namespace DS_IP92_LR6._1_ZalizchukD
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "\\input.txt";
            
            Graph graph = new Graph(path);
            graph.MSmezhOutput();
        }
    }

    class Graph
    {
        private int n, m;
        private int[,] mSmezh;
        private int[] vertexPowers;
        
        public Graph(string path)
        {
            StreamReader sr = new StreamReader(path);
            string rd = sr.ReadLine();
            string[] temp = rd.Split(' ');
            n = Convert.ToInt32(temp[0]);
            m = Convert.ToInt32(temp[1]);
            mSmezh = new int[n, n];
            vertexPowers = new int[n];

            for (int i = 0; i < m; i++)
            {
                rd = sr.ReadLine();
                temp = rd.Split(' ');
                int a = Convert.ToInt32(temp[0]) - 1, b = Convert.ToInt32(temp[1]) - 1;
                mSmezh[a, b] = 1;
                mSmezh[b, a] = 1;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (mSmezh[i, j] == 1)
                        vertexPowers[i]++;
            
        }

        public void MSmezhOutput()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write("{0,4}", mSmezh[i,j]);
                Console.WriteLine();
            }
        }
        
    }
    
}