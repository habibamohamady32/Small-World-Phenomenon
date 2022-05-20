using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Diagnostics;


class graph
{
    public static Dictionary<Tuple<int, int>, int> weight = new Dictionary<Tuple<int, int>, int>();
    public static List<ArrayList> adj = new List<ArrayList>();
    public static Dictionary<Tuple<int, int>, string> actor_to_movie = new Dictionary<Tuple<int, int>, string>(); // to save one movie for each edge
    public static Dictionary<string, int> map = new Dictionary<string, int>();// for incoding the vertcies
    public static Dictionary<int, string> revMap = new Dictionary<int, string>();
    static public void Main(String[] args)
    {
        Stopwatch sw = Stopwatch.StartNew();
        read();
        sw.Stop();
        Console.WriteLine("\n");
        Console.WriteLine("finished in \r");
        Console.WriteLine(sw.Elapsed.ToString());
    }
    static void read()
    {
        //sample
        //string text = File.ReadAllText(@"movies1.txt");
        //small case1
        //string text = File.ReadAllText(@"Movies193.txt");
        //small case2
        //string text = File.ReadAllText(@"Movies187.txt");
        //medium case1
        //string text = File.ReadAllText(@"Movies967.txt");
        //medium case2
        //string text = File.ReadAllText(@"Movies4736.txt");
        //large
        //string text = File.ReadAllText(@"Movies14129.txt");
        //Extreme
        string text = File.ReadAllText(@"Movies122806.txt");
        string[] movies = text.Split('\n');
        string[][] actors = new string[movies.Length][];
        Tuple<int, int> tuple;
        for (int i = 0; i < movies.Length; i++)
        {
            actors[i] = movies[i].Split('/');

        }
        for (int i = 0; i < movies.Length; i++)
        {
            int length = actors[i].Length;
            for (int j = 1; j < length; j++)
            {
                if (!map.ContainsKey(actors[i][j]))
                {
                    map.Add(actors[i][j], map.Count);
                    adj.Add(new ArrayList());
                    revMap.Add(revMap.Count, actors[i][j]);
                }
                for (int k = 1; k < length; k++)
                {
                    if (!map.ContainsKey(actors[i][k]))
                    {
                        adj.Add(new ArrayList());
                        map.Add(actors[i][k], map.Count);
                        revMap.Add(revMap.Count, actors[i][k]);

                    }
                    if (k != j)
                    {
                        adj[(map[actors[i][j]])].Add(map[actors[i][k]]);
                    }
                }
            }
            for (int j = 1; j < length; j++)
            {
                for (int k = j + 1; k < length; k++)
                {
                    int min = Math.Min(map[actors[i][j]], map[actors[i][k]]);
                    int max = Math.Max(map[actors[i][j]], map[actors[i][k]]);
                    tuple = new Tuple<int, int>(min, max);

                    if (weight.ContainsKey(tuple))
                    {
                        weight[tuple]++;
                    }
                    else
                    {
                        weight.Add(tuple, 1);
                    }
                    if (!actor_to_movie.ContainsKey(tuple))
                    {
                        actor_to_movie.Add(tuple, actors[i][0]);
                    }
                }
            }
        }
        //sample
        //string text2 = File.ReadAllText(@"queries1.txt");
        //small case1
        //string text2 = File.ReadAllText(@"queries110.txt");
        //small case2
        //string text2 = File.ReadAllText(@"queries50.txt");
        //medium case1 85
        //string text2 = File.ResadAllText(@"queries85.txt");
        //medium case1 4000
        //string text2 = File.ReadAllText(@"queries4000.txt");
        //medium case2 110
        //string text2 = File.ReadAllText(@"queries110.txt");
        //medium case2 2000
        //string text2 = File.ReadAllText(@"queries2000.txt");
        //large 26
        //string text2 = File.ReadAllText(@"queries26.txt");
        //large 600
        //string text2 = File.ReadAllText(@"queries600.txt");
        //Extreme200
        //string text2 = File.ReadAllText(@"queries200.txt");
        //Extreme22
        string text2 = File.ReadAllText(@"queries22.txt");
        string[] queries = text2.Trim().Split('\n');


        for (int i = 0; i < queries.Length; i++)
        {
            string[] values = queries[i].Trim().Split('/');
            int src = map[values[0]];
            int dst = map[values[1]];
            int[] level = new int[map.Count];
            level = dos(src, dst);
            Console.WriteLine(revMap[src] + "/" + revMap[dst]);
            Console.WriteLine("dos = " + (level[dst] - 1));

            rs(src, dst, level);
        }
    }
    public static int[] dos(int src, int dist)
    {
        Queue<int> q = new Queue<int>();
        int[] level = new int[map.Count];
        q.Enqueue(src);

        level[src] = 1;

        while (q.Count != 0)
        {
            if (level[dist] == 0)
            {
                int v = q.Dequeue();
                for (int i = 0; i < adj[v].Count; i++)
                {
                    int index = (int)adj[v][i];
                    if (level[index] == 0)
                    {
                        q.Enqueue(index);
                        level[index] = level[v] + 1;
                    }
                }
            }
            else return level;
        }

        return level;
    }
    static void rs(int source, int destination, int[] level)
    {
        HashSet<int> visited = new HashSet<int>();

        Tuple<int, int> e1;
        Tuple<int, int> e2;
        Tuple<int, int> e3;
        Queue<int> q = new Queue<int>();
        int[] parent = new int[level.Length];

        int[] distance = new int[level.Length];
        q.Enqueue(destination);
        visited.Add(destination);

        while (q.Count != 0)
        {
            int v = q.Dequeue();
            for (int i = 0; i < adj[v].Count; i++)
            {
                int index = (int)adj[v][i];

                if (level[index] != 0 && level[index] < level[v])
                {
                    if (level[index] != level[source])
                    {
                        if (!visited.Contains(index))

                        {
                            q.Enqueue(index);
                            visited.Add(index);
                        }
                    }
                    int min = Math.Min(destination, index);
                    int max = Math.Max(destination, index);
                    e1 = new Tuple<int, int>(min, max);

                    if (v != destination)
                    {
                        int min1 = Math.Min(destination, v);
                        int max1 = Math.Max(destination, v);
                        e2 = new Tuple<int, int>(min1, max1);
                        int min2 = Math.Min(index, v);
                        int max2 = Math.Max(index, v);
                        e3 = new Tuple<int, int>(min2, max2);

                        int w = distance[v] + weight[e3];

                        if (w > distance[index])
                        {
                            distance[index] = w;
                            parent[index] = v;
                        }
                    }
                    else
                    {
                        distance[index] = weight[e1];
                        parent[index] = v;
                    }
                }
            }
        }
        Console.WriteLine("rs = " + distance[source]);
        Console.Write("chain of movies = ");
        Queue<int> m = new Queue<int>();
        Queue<int> a = new Queue<int>();
        m.Enqueue(source);
        a.Enqueue(source);
        while (m.Count > 0)
        {
            int d = m.Dequeue();
            int min = Math.Min(d, parent[d]);
            int max = Math.Max(d, parent[d]);
            Tuple<int, int> e = new Tuple<int, int>(min, max);
            Console.Write(actor_to_movie[e]);
            if (parent[d] != destination)
            {
                Console.Write("=>");
                m.Enqueue(parent[d]);
            }
        }
        Console.WriteLine();
        Console.Write("chain of actors = ");
        while (a.Count > 0)
        {
            int d = a.Dequeue();

            Console.Write(revMap[d] + " -> ");
            if (parent[d] != destination)
            {

                a.Enqueue(parent[d]);
            }
        }
        Console.WriteLine(revMap[destination]);
    }
}