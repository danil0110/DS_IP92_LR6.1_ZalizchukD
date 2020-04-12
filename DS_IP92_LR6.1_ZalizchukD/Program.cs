using System;
using static System.IO.Directory;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DS_IP92_LR6._1_ZalizchukD
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "\\input.txt";
            string choice;
            Graph graph = new Graph(path);
            
            Console.WriteLine("1. Проверить наличие Эйлерового пути\n2. Проверить наличие Гамильтонового пути");
            choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Clear();
                graph.Eiler();
            } else if (choice == "2")
            {
                Console.Clear();
                graph.Hamiltonian();
            }
            else
            {
                Console.WriteLine("Ошибка: неверный ввод.");
                Environment.Exit(0);
            }
        }

    }

    // ================= КЛАСС "ГРАФ" =================
    
    class Graph
    {
        private int n, m, start;
        private int[,] mSmezh;
        private int[] vertexPowers;
        private bool eilerCycle = true, eilerPath, hamiltonianCycle;
        private bool[] visited;
        private List<int> hamiltonianPath;

        private Stack<int> stack;

        // ================= ЧТЕНИЕ ПРОСТЫХ ДАННЫХ О ГРАФЕ =================
        
        public Graph(string path)
        {
            StreamReader sr = new StreamReader(path);
            string read = sr.ReadLine();
            string[] temp = read.Split(' ');
            n = Convert.ToInt32(temp[0]);
            m = Convert.ToInt32(temp[1]);
            mSmezh = new int[n, n];
            vertexPowers = new int[n];
            visited = new bool[n];
            hamiltonianPath = new List<int>();

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
            
            CheckEiler();
            CheckHamiltonian();
        }

        // ================= ПРОВЕРКА НА ЭЙЛЕРОВОСТЬ =================
        
        private void CheckEiler()
        {
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

        // ================= ПРОВЕРКА НА ГАМИЛЬТОНОВЫЙ ЦИКЛ =================
        
        private void CheckHamiltonian()
        {
            hamiltonianCycle = true;
            if (n >= 3)
            {
                for (int i = 0; i < n; i++)
                    if (vertexPowers[i] < n / 2)
                    {
                        hamiltonianCycle = false;
                        break;
                    }

            }
            else
                hamiltonianCycle = false;
        }

        // ================= ЭЙЛЕРОВ ПУТЬ =================
        
        public void Eiler()
        {
            int v = 0;
            if (!eilerCycle)
            {
                if (eilerPath)
                {
                    for (int i = 0; i < n; i++)
                        if (vertexPowers[i] % 2 != 0)
                        {
                            Console.Write("Найден Эйлеров путь: ");
                            v = i;
                            break;
                        }
                }
                else
                {
                    Console.WriteLine("Эйлеров путь отсутствует.");
                    return;
                }
            }
            else
                Console.Write("Найден Эйлеров цикл: ");

            int[,] rebra = mSmezh;
            stack = new Stack<int>();
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
            Console.WriteLine();
            
        }

        // ================= ГАМИЛЬТОНОВ ПУТЬ =================
        
        public void Hamiltonian()
        {
            if (hamiltonianCycle)
            {
                for (int i = 0; i < n; i++)
                {
                    start = i;
                    HamiltonianCycle(start);
                }
                
                Console.WriteLine("Гамильтонов путь отсутствует.");
            }
            else
            {
                for (int i = 0; i < n; i++)
                    HamiltonianPath(i);
                
                Console.WriteLine("Гамильтонов путь отсутствует.");
            }
        }

        // ================= ГАМИЛЬТОНОВ ЦИКЛ, БЕКТРЕКИНГ =================
        
        private void HamiltonianCycle(int v)
        {
            if (hamiltonianPath.Count == n && mSmezh[hamiltonianPath[0], hamiltonianPath[n - 1]] == 1)
            {
                hamiltonianPath.Add(hamiltonianPath[0]);
                Console.Write("Найден Гамильтонов цикл: ");
                foreach (var el in hamiltonianPath)
                    Console.Write($"{el + 1} ");
                
                Console.WriteLine();
                Environment.Exit(0);
            }

            for (int i = 0; i < n; i++)
            {
                if (mSmezh[v, i] == 1 && !visited[i])
                {
                    visited[i] = true;
                    hamiltonianPath.Add(i);
                    HamiltonianCycle(i);

                    visited[i] = false;
                    hamiltonianPath.RemoveAt(hamiltonianPath.Count - 1);
                }
            }

        }

        // ================= ГАМИЛЬТОНОВ ПУТЬ, БЕКТРЕКИНГ =================
        
        private void HamiltonianPath(int v)
        {
            if (hamiltonianPath.Count == n)
            {
                Console.Write("Найден Гамильтонов путь: ");
                foreach (var el in hamiltonianPath)
                    Console.Write($"{el + 1} ");
                
                Console.WriteLine();
                Environment.Exit(0);
            }

            for (int i = 0; i < n; i++)
            {
                if (mSmezh[v, i] == 1 && !visited[i])
                {
                    visited[i] = true;
                    hamiltonianPath.Add(i);
                    HamiltonianPath(i);

                    visited[i] = false;
                    hamiltonianPath.RemoveAt(hamiltonianPath.Count - 1);
                }
            }

        }

    }
}