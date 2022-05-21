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
    public static Dictionary<Tuple<int, int>, int> weight = new Dictionary<Tuple<int, int>, int>();//O(1)
    public static List<ArrayList> adj = new List<ArrayList>();//O(1)
    public static Dictionary<Tuple<int, int>, string> actor_to_movie = new Dictionary<Tuple<int, int>, string>();//O(1)
    public static Dictionary<string, int> map = new Dictionary<string, int>();//O(1)
    public static Dictionary<int, string> revMap = new Dictionary<int, string>();//O(1)
    static public void Main(String[] args)
    {
        Stopwatch sw = Stopwatch.StartNew();
        read();
        sw.Stop();
        Console.WriteLine("\n");//O(1)
        Console.WriteLine("finished in \r");//O(1)
        Console.WriteLine(sw.Elapsed.ToString());//O(1)
    }

    static void read()//O(movies*V^2)
    {
        //sample
        string text = File.ReadAllText(@"movies1.txt");//O()                                                                
        //small case1
        //string text = File.ReadAllText(@"Movies193.txt");//O(1)
        //small case2
        //string text = File.ReadAllText(@"Movies187.txt");//O(1)
        //medium case1
        //string text = File.ReadAllText(@"Movies967.txt");//O(1)
        //medium case2
        //string text = File.ReadAllText(@"Movies4736.txt");//O(1)
        //large
        //string text = File.ReadAllText(@"Movies14129.txt");//O(1)
        //Extreme
        // string text = File.ReadAllText(@"Movies122806.txt");//O(1)

        string[] movies = text.Split('\n');//O(n) where n num of characters in string 
        string[][] actors = new string[movies.Length][];//O(1)
        Tuple<int, int> tuple;//O(1)
        for (int i = 0; i < movies.Length; i++)//O(movies*n)
        {
            actors[i] = movies[i].Split('/');//O(n) where n num of characters in string 
        }

        for (int i = 0; i < movies.Length; i++)//O(movies*V^2)
        {
            int length = actors[i].Length;//O(1)
            for (int j = 1; j < length; j++)//O(V)
            {
                if (!map.ContainsKey(actors[i][j]))//O(1)
                {
                    map.Add(actors[i][j], map.Count);//O(1) Till finishing capacity becomes O(V)
                    adj.Add(new ArrayList());//O(1)
                    revMap.Add(revMap.Count, actors[i][j]);//O(1) Till finishing capacity becomes O(V)
                }
                for (int k = 1; k < length; k++)//O(V)
                {
                    if (!map.ContainsKey(actors[i][k]))//O(1)
                    {
                        adj.Add(new ArrayList());//O(1)
                        map.Add(actors[i][k], map.Count);//O(1) Till finishing capacity becomes O(V)
                        revMap.Add(revMap.Count, actors[i][k]);//O(1) Till finishing capacity becomes O(V)
                    }
                    if (k != j)//O(1)
                    {
                        adj[(map[actors[i][j]])].Add(map[actors[i][k]]);//O(1) Till finishing capacity becomes O(V)
                    }
                }
            }


            for (int j = 1; j < length; j++)//O(V^2)
            {
                for (int k = j + 1; k < length; k++)//O(V)
                {
                    int min = Math.Min(map[actors[i][j]], map[actors[i][k]]);//O(1)
                    int max = Math.Max(map[actors[i][j]], map[actors[i][k]]);//O(1)
                    tuple = new Tuple<int, int>(min, max);//O(1)

                    if (weight.ContainsKey(tuple))//O(1)
                    {
                        weight[tuple]++;//O(1)
                    }
                    else
                    {
                        weight.Add(tuple, 1);//O(1)
                    }
                    if (!actor_to_movie.ContainsKey(tuple))//O(1)
                    {
                        actor_to_movie.Add(tuple, actors[i][0]);//O(1) Till finishing capacity becomes O(V)
                    }
                }
            }
        }
        //sample
        string text2 = File.ReadAllText(@"queries1.txt");
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
        // string text2 = File.ReadAllText(@"queries22.txt");

        string[] queries = text2.Trim().Split('\n');//O(n) Where n numbers of letters per line
        for (int i = 0; i < queries.Length; i++)//O(M) where M number of queries
        {
            string[] values = queries[i].Trim().Split('/');//O(n) complexity where n is the number of characters in the string.
            int src = map[values[0]];//O(1)
            int dst = map[values[1]];//O(1)
            int[] level = new int[map.Count];//O(1)
            level = dos(src, dst);//O(V+E)
            Console.WriteLine(revMap[src] + "/" + revMap[dst]);//O(1)
            Console.WriteLine("dos = " + (level[dst] - 1));//O(1)
            rs(src, dst, level);//O(V+E)
        }
    }

    static int[] dos(int src, int dist)// O(V+E)
    {
        Queue<int> q = new Queue<int>();// O(1)
        int[] level = new int[map.Count];// θ(1)
        q.Enqueue(src);// O(1)

        level[src] = 1;// O(1)

        while (q.Count != 0)// O(V)
        {
            if (level[dist] == 0)//O(1)
            {
                int v = q.Dequeue();//O(1)
                for (int i = 0; i < adj[v].Count; i++)//O(E)
                {
                    int index = (int)adj[v][i];//O(1)
                    if (level[index] == 0)//O(1)
                    {
                        q.Enqueue(index);//O(1)
                        level[index] = level[v] + 1;//O(1)
                    }
                }
            }
            else return level;//O(1)
        }

        return level;//O(1)
    }


    static void rs(int source, int destination, int[] level)//O(V+E)
    {
        HashSet<int> visited = new HashSet<int>();//O(1)
        Tuple<int, int> e1;//O(1)
        Tuple<int, int> e3;//O(1)
        Queue<int> q = new Queue<int>();//O(1)
        int[] parent = new int[level.Length];// O(1)
        int[] distance = new int[level.Length];//O(1)
        q.Enqueue(destination);//O(1)
        visited.Add(destination);//O(1) Till finishing capacity becomes O(V)
        while (q.Count != 0)//O(V)
        {
            int v = q.Dequeue();//O(1)
            for (int i = 0; i < adj[v].Count; i++)//O(V+E)
            {
                int index = (int)adj[v][i];//O(1)
                if (level[index] != 0 && level[index] < level[v])//O(1)
                {
                    if (level[index] != level[source])//O(1)
                    {
                        if (!visited.Contains(index))//O(1)
                        {
                            q.Enqueue(index);//O(1)
                            visited.Add(index);//O(1) 
                        }
                    }
                    int min = Math.Min(destination, index);//O(1)
                    int max = Math.Max(destination, index);//O(1)
                    e1 = new Tuple<int, int>(min, max);
                    if (v != destination)//O(1)
                    {
                        int min2 = Math.Min(index, v);//O(1)
                        int max2 = Math.Max(index, v);//O(1)
                        e3 = new Tuple<int, int>(min2, max2);//O(1)
                        int w = distance[v] + weight[e3];//O(1)
                        if (w > distance[index])
                        {
                            distance[index] = w;//O(1)
                            parent[index] = v;
                        }//O(1)
                    }
                    else
                    {
                        distance[index] = weight[e1];//O(1)
                        parent[index] = v;//O(1)
                    }
                }
            }
        }
        Console.WriteLine("rs = " + distance[source]);//O(1)

        chains(source, destination, parent);//O(V)
    }


    static void chains(int source, int destination, int[] parent)//O(V)
    {
        Console.Write("chain of movies = ");//O(1)
        Queue<int> m = new Queue<int>();//O(1)
        m.Enqueue(source);//O(1)
        while (m.Count > 0)//O(1)
        {
            int d = m.Dequeue();//O(1)
            int min = Math.Min(d, parent[d]);//O(1)
            int max = Math.Max(d, parent[d]);//O(1)
            Tuple<int, int> e = new Tuple<int, int>(min, max);//O(1)
            Console.Write(actor_to_movie[e]);//O(1)
            if (parent[d] != destination)//O(1)
            {
                Console.Write("=>");//O(1)
                m.Enqueue(parent[d]);//O(1)
            }
        }
        Console.WriteLine();//O(1)
        Console.Write("chain of actors = ");//O(1)
        Queue<int> a = new Queue<int>();//O(1)
        a.Enqueue(source);
        while (a.Count > 0) //O(V)
        {
            int d = a.Dequeue();//O(1)
            Console.Write(revMap[d] + " -> ");
            if (parent[d] != destination)//O(1)
            {
                a.Enqueue(parent[d]);//O(1)
            }
        }
        Console.WriteLine(revMap[destination]);//O(1)
    }
}