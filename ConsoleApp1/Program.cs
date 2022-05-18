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
    public static List<HashSet<int>> adj = new List<HashSet<int>>();
    public static Dictionary<Tuple<int, int>, Stack<string>> actor_to_movie = new Dictionary<Tuple<int, int>, Stack<string>>();
    public static Dictionary<string, int> map = new Dictionary<string, int>();
    public static Dictionary<int, string> revMap = new Dictionary<int, string>();
    // public static Dictionary<edge, string> relation_strength = new Dictionary<edge, string>();
    //  public static List<int> strength;
    // public static Tuple<string, string> edges;
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
        string text = File.ReadAllText(@"Movies122806.txt");
        string[] movies = text.Split('\n');
        string[][] actors = new string[movies.Length][];

        //  edge e;
        Tuple<int, int> tuple;
        //Tuple<string, String> inverse;
        for (int i = 0; i < movies.Length; i++)
        {
            actors[i] = movies[i].Split('/');

        }
        for (int i = 0; i < movies.Length; i++)
        {
            int length = actors[i].Length;
            for (int j = 1; j < length; j++)
            {
                /*  if (!actor_to_movie.ContainsKey(actors[i][j]))
                  {
                      actor_to_movie.Add(actors[i][j], new List<string>());
                      actor_to_movie[actors[i][j]].Add(actors[i][0]);
                  }
                  else { actor_to_movie[actors[i][j]].Add(actors[i][0]); }*/

                if (!map.ContainsKey(actors[i][j]))
                {
                    map.Add(actors[i][j], map.Count);
                    adj.Add(new HashSet<int>());
                    revMap.Add(revMap.Count, actors[i][j]);
                }
                for (int k = 1; k < length; k++)
                {

                    if (!map.ContainsKey(actors[i][k]))
                    {
                        adj.Add(new HashSet<int>());
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
                    //   inverse = new Tuple<string, string>(actors[i][k], actors[i][j]);
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
                        actor_to_movie.Add(tuple, new Stack<string>());
                        string s = actors[i][0];
                        actor_to_movie[tuple].Push(s);

                    }


                }
            }
        }
        /*   foreach (Tuple<string,string> e2 in weight.Keys)
            {
                 Console.WriteLine(e2+"/"+weight[e2]);
            }*/
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
            Console.WriteLine("dos = " + (level[dst]-1));

            rs(src, dst, level);
        }
    }
    public static int[] dos(int src, int dist)
    {
       // HashSet<int> visited = new HashSet<int>();
        Queue<int> q = new Queue<int>();
        int[] level = new int[map.Count];
        q.Enqueue(src);
       // visited.Add(src);
        level[src] = 1;

        while (q.Count != 0)
        {
            if (level[dist]==0)
            {
                int v = q.Dequeue();
                for (int i = 0; i < adj[v].Count; i++)
                {
                    if (level[adj[v].ElementAt(i)]==0)
                    {
                        q.Enqueue(adj[v].ElementAt(i));
                        level[adj[v].ElementAt(i)] = level[v] + 1;
                      //  visited.Add(adj[v].ElementAt(i));
                    }
                }
            }
            else return level;
        }
       // visited.Clear();
        return level;
    }
    static void rs(int source, int destination, int[] level)
    {
        if (revMap[source] == "C" && revMap[destination] == "E")
            _ = -1;
        HashSet<int> visited = new HashSet<int>();
        
        //HashSet<int> layers = new HashSet<int>();
        // weight from source to dist "weight[e1]" = weight from source to parent "weight [e2]" + weight from parent to dist "weight [e3]"
        Tuple<int, int> e1;
        Tuple<int, int> e2;
        Tuple<int, int> e3;

        // layers[0].Add(source);
        Queue<int> q = new Queue<int>();
        q.Enqueue(destination);
        // layers.Add(level[source]);
        visited.Add(destination);
        while (q.Count != 0)
        {
            int v = q.Dequeue();


            for (int i = 0; i < adj[v].Count; i++)
            {


                //if (!layers.Contains(level[destination]))
                //{

                if (level[adj[v].ElementAt(i)]!=0 && level[adj[v].ElementAt(i)] < level[v])
                {
                    if (level[adj[v].ElementAt(i)] != level[source])
                    {
                        if (!visited.Contains(adj[v].ElementAt(i)))

                        {
                            q.Enqueue(adj[v].ElementAt(i));
                            visited.Add(adj[v].ElementAt(i));
                            // layers.Add(level[adj[v].ElementAt(i)]);

                        }
                    }

                    if (v != destination)
                    {


                        int min = Math.Min(destination, adj[v].ElementAt(i));
                        int max = Math.Max(destination, adj[v].ElementAt(i));
                        e1 = new Tuple<int, int>(min, max);
                        int min1 = Math.Min(destination, v);
                        int max1 = Math.Max(destination, v);
                        e2 = new Tuple<int, int>(min1, max1);
                        int min2 = Math.Min(adj[v].ElementAt(i), v);
                        int max2 = Math.Max(adj[v].ElementAt(i), v);
                        e3 = new Tuple<int, int>(min2, max2);
                        calc_path(e1, e2, e3);

                    }



                    //}

                }
                // for update weight of[src,dst]





            }
        }

        int minimum = Math.Min(source, destination);
        int maximum = Math.Max(source, destination);
        Tuple<int, int> strength = new Tuple<int, int>(minimum, maximum);
        Console.WriteLine("rs = " + weight[strength]);
        Console.Write("chain = ");
        Stack<string> stack = new Stack<string>(actor_to_movie[strength].Reverse());



        while (stack.Count != 0)
        {
            Console.Write(stack.Pop());
            if (stack.Count != 0) { Console.Write("=>"); }
        }
        Console.WriteLine("\r");




    }
    public static void calc_path(Tuple<int, int> t1, Tuple<int, int> t2, Tuple<int, int> t3)
    {
        int w;
        if (!weight.ContainsKey(t1))
        {
            weight.Add(t1, 0);
            actor_to_movie.Add(t1, new Stack<string>());
        }
        w = weight[t2] + weight[t3];
        Stack<string> s1 = new Stack<string>();
        Stack<string> s2 = new Stack<string>(actor_to_movie[t2]);

        Stack<string> s3 = new Stack<string>(actor_to_movie[t3]);


        for (int i = 0; i < actor_to_movie[t2].Count; i++)
        {
            string first = s2.Pop();
            s1.Push(first);
        }
        for (int i = 0; i < actor_to_movie[t3].Count; i++)
        {
            string first = s3.Pop();
            s1.Push(first);
        }


        if (w > weight[t1])
        {
            actor_to_movie[t1] = s1;

            weight[t1] = w;
        }

    }
}
struct edge
{
    public string v1;
    public string v2;
}