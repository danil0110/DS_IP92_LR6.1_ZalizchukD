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
            graph.EilerPath();
            //graph.MSmezhOutput();
        }
    }

    class Graph
    {
        private int n, m;
        private int[,] mSmezh;
        private int[] vertexPowers;
        private bool eilerCycle = true, eilerPath;
        
        public Graph(string path)
        {
            StreamReader sr = new StreamReader(path);
            string read = sr.ReadLine();
            string[] temp = read.Split(' ');
            n = Convert.ToInt32(temp[0]);
            m = Convert.ToInt32(temp[1]);
            mSmezh = new int[n, n];
            vertexPowers = new int[n];

            for (int i = 0; i < m; i++)
            {
                read = sr.ReadLine();
                temp = read.Split(' ');
                int a = Convert.ToInt32(temp[0]) - 1, b = Convert.ToInt32(temp[1]) - 1;
                mSmezh[a, b] = 1;
                mSmezh[b, a] = 1;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (mSmezh[i, j] == 1)
                        vertexPowers[i]++;

            int count = 0;
            foreach (var power in vertexPowers)
                if (power % 2 != 0)
                {
                    eilerCycle = false;
                    count++;
                }

            if (eilerCycle == true)
                eilerPath = true;
            else if (count == 2)
                eilerPath = true;
            else
                eilerPath = false;

        }

        public void EilerPath()
        {
            int v = 0;
            if (!eilerCycle)
            {
                if (eilerPath)
                {
                    for (int i = 0; i < n; i++)
                        if (vertexPowers[i] % 2 != 0)
                        {
                            Console.Write("Эйлеров цикл отсутствует, найден Эйлеров путь: ");
                            v = i;
                            break;
                        }
                }
                else
                {
                    Console.WriteLine("Эйлеров цикл и путь отсутствуют.");
                    return;
                }
            }
            else
                Console.Write("Найден Эйлеров цикл и путь: ");

            int[,] rebra = mSmezh;
            Stack<int> stack = new Stack<int>();
            stack.Push(v);
            while (stack.Count != 0)
            {
                v = stack.Peek();
                for (int i = 0; i < n; i++)
                    if (rebra[v, i] == 1)
                    {
                        stack.Push(i);
                        rebra[v, i] = 0;
                        rebra[i, v] = 0;
                        break;
                    }

                if (v == stack.Peek())
                {
                    stack.Pop();
                    Console.Write($"{v + 1} ");
                }
            }
            
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