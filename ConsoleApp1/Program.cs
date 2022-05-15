using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;


class graph
{
    public static Dictionary<Tuple<int, int>, int> we = new Dictionary<Tuple<int, int>, int>(); //to minize memory by half, we order pairs value before add or get
    public static Dictionary<int, HashSet<int>> adj = new Dictionary<int, HashSet<int>>();         //vertex will be converted to int
    public static Dictionary<string, int> map = new Dictionary<string, int>();
    public static Dictionary<int, string> revMap = new Dictionary<int, string>();
    public static int[] level;
    public static int[] rsw;
    static public void Main(String[] args)
    {
        read();
        Console.WriteLine("finished");
    }
    static void read()
    {
        string text = File.ReadAllText(@"Movies967.txt");
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
                    map.Add(actors[i][j], map.Count + 1);
                    revMap.Add(revMap.Count, actors[i][j]);
                }
                for (int k = 1; k < length; k++)
                {
                    //Console.Write("  " + j);
                    if (!adj.ContainsKey(map[actors[i][j]]))
                    {
                        adj.Add(map[actors[i][j]], new HashSet<int>());
                    }
                    if (!map.ContainsKey(actors[i][k]))
                    {
                        map.Add(actors[i][k], map.Count + 1);
                        revMap.Add(revMap.Count, actors[i][k]);
                    }
                    if (k != j)
                    {
                        adj[map[actors[i][j]]].Add(map[actors[i][k]]);
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
                    if (we.ContainsKey(tuple))
                    {
                        we[tuple]++;
                    }
                    else
                    {
                        we.Add(tuple, 1);
                    }

                }
            }
        }
 



        string text2 = File.ReadAllText(@"queries4000.txt");
        string[] queries = text2.Split('\n');
        text2 = File.ReadAllText(@"queries4000 - Solution.txt");
        string[] sol = text2.Split('\n');
        level = new int[(map.Count+1)];
        rsw = new int[map.Count + 1];
        for (int i = 0; i < queries.Length - 1; i++)
        {

            string[] values = queries[i].Trim().Split('/');
            int src = map[values[0]];
            int dst = map[values[1]];
            //src = 11862;
            //dst = 552;
            rs(dst, src);

            string[] sol2 = sol[1 + 5 * i].Trim().Split();
            int x = Int32.Parse(sol2[2].Substring(0, sol2[2].Length - 1));
            int y = Int32.Parse(sol2[5].Substring(0, sol2[5].Length));
            
            if (x == (level[src]-1) && y == rsw[src])
            {
                Console.WriteLine("Case: " + i + " Accepted");
            }
            else
            {
                Console.WriteLine(src + " " + dst);
                Console.WriteLine("output: " + (level[dst]-1) + " " + rsw[dst]);
                Console.WriteLine("outpu: " + x + " " + y);
                Console.WriteLine("Wrong Answer Case: " + i);
                break;
            }

        }
        Console.WriteLine(" \n \n Finished");
    }
    static void rs(int source, int dest)
    {
        Queue<int> q = new Queue<int>();
        rsw = new int[level.Length];
        bool[] visited = new bool[level.Length];
        level = new int[level.Length];
        level[source] = 1;
        
        visited[source] = true;
        q.Enqueue(source);

        while (q.Count != 0)
        {
            int v = q.Dequeue();
            for (int i = 0; i < adj[v].Count; i++)
            {
                int index = adj[v].ElementAt(i);
                //Console.WriteLine(v + " " + index);

                int x;
                if (v > index){ x = rsw[v] + we[new Tuple<int, int>(index,v)]; }
                else {x = rsw[v] + we[new Tuple<int, int>(v,index)];}
                if (!visited[index])
                {
                    level[index] = level[v] + 1;
                    visited[index] = true;
                    q.Enqueue(index);
                }
                if (rsw[index] < x && level[v] == level[index] - 1)
                {
                    rsw[index] = x;
                }
                
            }
            if (level[v] > level[dest] && level[dest] != 0) {
                break;
            }
        }
        
    }
}
struct edge
{
    public int v1;
    public int v2;
}
struct Pair
{
    public int v;
    public int i;
    private int dst;

    public Pair(int dst, int i) : this()
    {
        this.dst = dst;
        this.i = i;
    }
}