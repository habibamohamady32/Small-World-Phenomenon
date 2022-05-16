using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Numerics;

class graph
{
    public static Dictionary<Tuple<int, int>, int> we = new Dictionary<Tuple<int, int>, int>(); //to minize memory by half, we order pairs value before add or get
    public static List<HashSet<int>> adj = new List<HashSet<int>>();         //vertex will be converted to int
    public static Dictionary<string, int> map = new Dictionary<string, int>();
    public static Dictionary<int, string> revMap = new Dictionary<int, string>();// we can use it as an array not dictionary
    public static int[] level;
    public static List<List<int>> lvl;
    public static int[] rsw;
    static public void Main(String[] args)
    {
        read();
        Console.WriteLine("finished");
    }
    static void read()
    {
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
                    adj.Add(new HashSet<int>());
                    map.Add(actors[i][j], map.Count);
                    revMap.Add(revMap.Count, actors[i][j]);


                }
                for (int k = 1; k < length; k++)
                {
                    //Console.Write("  " + j);

                    if (!map.ContainsKey(actors[i][k]))
                    {
                        adj.Add(new HashSet<int>());
                        map.Add(actors[i][k], map.Count);
                        revMap.Add(revMap.Count, actors[i][k]);

                    }
                    if (k != j)
                    {
                        //Console.WriteLine(map[actors[i][j]]);
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




        string text2 = File.ReadAllText(@"queries22.txt");
        string[] queries = text2.Split('\n');
        text2 = File.ReadAllText(@"queries22 - Solution.txt");
        string[] sol = text2.Split('\n');
        level = new int[(map.Count + 1)];
        rsw = new int[map.Count + 1];
        for (int i = 0; i < queries.Length - 1; i++)
        {

            string[] values = queries[i].Trim().Split('/');
            int src = map[values[0]];
            int dst = map[values[1]];
            rsw = new int[level.Length];
            bfs(src, dst);
            
            rs(src, dst);
            string[] sol2 = sol[1 + 5 * i].Trim().Split();
            int x = Int32.Parse(sol2[2].Substring(0, sol2[2].Length - 1));
            int y = Int32.Parse(sol2[5].Substring(0, sol2[5].Length));

            if (x == (level[dst] - 1) && y == rsw[src])
            {
                Console.WriteLine("Case: " + i + " Accepted");
            }
            else
            {
                Console.WriteLine("soruce: "+src + " dest " + dst);
                Console.WriteLine("output: " + (level[dst] - 1) + " " + rsw[dst]);
                Console.WriteLine("output: source " + (level[src] - 1) + " " + rsw[src]);

                Console.WriteLine("outpu: " + x + " " + y);
                Console.WriteLine("Wrong Answer Case: " + i);
                break;
            }

        }
        Console.WriteLine(" \n \n Finished");
    }
    static void bfs(int source, int dest)
    {
        bool[] visited = new bool[level.Length];
        Queue<int> q = new Queue<int>();
        level = new int[level.Length];
        lvl = new List<List<int>>();
        lvl.Add(new List<int>());
        lvl[0].Add(source);
        //created after doing the bfs or while doing it
        
        level[source] = 1;
        visited[source] = true;
        q.Enqueue(source);
        while (q.Count != 0)
        {
            int v = q.Dequeue();
            if (level[v] >= level[dest] && level[dest] != 0)
            {
                break;
            }
            for (int i = 0; i < adj[v].Count; i++)
            {

                int index = adj[v].ElementAt(i);

                /*int x;
                if (v > index) { x = rsw[v] + we[new Tuple<int, int>(index, v)]; }
                else { x = rsw[v] + we[new Tuple<int, int>(v, index)]; }*/
                if (!visited[index] && level[dest] == 0)
                {
                    if (lvl.Count < level[v] + 1) { lvl.Add(new List<int>()); }
                    lvl[level[v]].Add(index);
                    level[index] = level[v] + 1;
                    visited[index] = true;
                    q.Enqueue(index);
                }

                /*if (rsw[index] < x && level[v] == level[index] - 1)
                {
                    rsw[index] = x;
                }*/
                    
            }

        }

    }
    static void rs(int source, int dest) {
        Queue<int> q =  new Queue<int>();
        List<int> visited = new List<int> ();
        q.Enqueue(dest);
        while (q.Count != 0) {
            int v = q.Dequeue();
            if (v == source) { break; }
            for (int i = 0; i < adj[v].Count; i++)
            {
                int index = adj[v].ElementAt(i);

                if (lvl[level[v]-2].Contains(index)){

                    if (!visited.Contains(index) &&!q.Contains(index)) {
                        q.Enqueue(index);
                        visited.Add(index);
                    }

                    int x;
                    if (v > index) { x = rsw[v] + we[new Tuple<int, int>(index, v)]; }
                    else { x = rsw[v] + we[new Tuple<int, int>(v, index)]; }
                    if (rsw[index] < x )
                    {
                        rsw[index] = x;
                       // Console.WriteLine(index + " " + x);
                    }
                }
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