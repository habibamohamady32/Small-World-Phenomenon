using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;


class graph
{
    public static Dictionary<Tuple<int,int>, int> weight = new Dictionary<Tuple<int, int>, int>(); //to minize memory by half, we order pairs value before add or get
    public static Dictionary<int, HashSet<int>> adj = new Dictionary<int, HashSet<int>>();         //vertex will be converted to int
    public static Dictionary<string,int> map = new Dictionary<string,int>();
    public static Dictionary<int, string> revMap = new Dictionary<int, string>();

    // public static Dictionary<edge, string> relation_strength = new Dictionary<edge, string>();
    //  public static List<int> strength;
    // public static Tuple<string, string> edges;
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
        
      //  edge e;
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
                //Console.WriteLine("loop: " + j);
                if (!map.ContainsKey(actors[i][j])){
                    //Console.WriteLine(actors[i][j] + " " + (map.Count+1));
                    map.Add(actors[i][j], map.Count+1);
                    //Console.WriteLine(map[actors[i][j]] + " As " + actors[i][j]);
                    revMap.Add(revMap.Count, actors[i][j]);
                }
                for (int k = 1; k < length; k++)
                {
                    //Console.Write("  " + j);
                    if (!adj.ContainsKey(map[actors[i][j]]))
                    {
                        adj.Add(map[actors[i][j]], new HashSet<int>());
                    }
                    if (!map.ContainsKey(actors[i][k])) {
                        //Console.WriteLine(actors[i][k] + " " + (map.Count + 1));
                        map.Add(actors[i][k], map.Count+1);
                        //Console.WriteLine(map[actors[i][k]] + " As " + actors[i][k]);
                        revMap.Add(revMap.Count, actors[i][k]);
                    }
                    if (k != j)
                    {
                     //   Console.WriteLine(map[actors[i][j]] + " add " + map[actors[i][k]]);

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
                    tuple =new Tuple<int, int>(min, max);
                    if (weight.ContainsKey(tuple))
                    {
                        weight[tuple]++;    
                    }
                    else
                    {
                        weight.Add(tuple, 1);
                    }
                    
                }
            }
        }
     /*   foreach (Tuple<string,string> e2 in weight.Keys)
         {
              Console.WriteLine(e2+"/"+weight[e2]);
         }*/
     string text2 = File.ReadAllText(@"queries4000.txt");
        string[] queries = text2.Split('\n');
        text2 = File.ReadAllText(@"queries4000 - Solution.txt");
        string[] sol = text2.Split('\n');
        for (int i = 0; i < queries.Length - 1; i++)
        {
            string[] values = queries[i].Trim().Split('/');
            int src = map[values[0]];
            int dst = map[values[1]];
            Dictionary<int, int> level = dos(src, dst);
            //Console.WriteLine(src + "/" + dst);
            //Console.WriteLine("dos = " + level[dst]);
            rs(src, dst, level);
            Tuple<int, int> strength = new Tuple<int, int>(src, dst);
            //Console.WriteLine("rs = " + weight[strength]);
            string[] sol2 = sol[1+5*i].Trim().Split();

            // Console.WriteLine("solution is");
            int x = Int32.Parse(sol2[2].Substring(0, sol2[2].Length - 1));
            int y = Int32.Parse(sol2[5].Substring(0, sol2[5].Length));
           //Console.WriteLine("Answer is: " + x + " " + y);
            if (x == level[dst] && y == weight[strength]){
                Console.WriteLine("Case: " + i + " Accepted");
            }
            else {
                Console.WriteLine("Wrong Answer Case: " + i);
            }
            
        }
    }
     public static Dictionary<int,int> dos(int src, int dist)
    {
        HashSet<int> visited = new HashSet<int>();
        Queue<int> q = new Queue<int>();
        Dictionary<int, int> level = new Dictionary<int, int>();
        q.Enqueue(src);
        visited.Add(src);
        level[src] = 0;
        
       


        while (q.Count != 0)
        {
            if (!visited.Contains(dist))
            {
                int v = q.Dequeue();
                for (int i = 0; i < adj[v].Count; i++)
                {
                    if (!visited.Contains(adj[v].ElementAt(i)))
                    {

                        q.Enqueue(adj[v].ElementAt(i));
                        level.Add(adj[v].ElementAt(i), level[v] + 1);
                        visited.Add(adj[v].ElementAt(i));
                       
                       
                    }
                }
            }
            else return level;
        }
        visited.Clear();
        return level;
    }
    static void rs(int source, int destination,Dictionary<int,int> level)
    {
        HashSet<int> visited = new HashSet<int>();
        HashSet<int> layers = new HashSet<int>();
        Tuple<int, int> e1;
        Tuple<int, int> e2;
        Tuple<int, int> e3;

        Queue<int> q = new Queue<int>();
        q.Enqueue(source);
        layers.Add(level[source]);
        visited.Add(source);
        while (q.Count != 0)
        {
            int v = q.Dequeue();


            for(int i = 0; i < adj[v].Count; i++)
            {
                
                if (!visited.Contains(adj[v].ElementAt(i)))
                {
                    //if (!layers.Contains(level[destination]))
                    //{

                        if (level.ContainsKey(adj[v].ElementAt(i)) &&level[adj[v].ElementAt(i)] != level[destination])
                        {
                            q.Enqueue(adj[v].ElementAt(i));
                            visited.Add(adj[v].ElementAt(i));
                           // layers.Add(level[adj[v].ElementAt(i)]);

                        }
                        


                       
                       
                    //}
                   
                }
                // for update weight of[src,dst]
                if (v != source && adj[v].ElementAt(i)!= source)
                {
                    if ( level.ContainsKey(adj[v].ElementAt(i))) {
                        if ( level[adj[v].ElementAt(i)]> level[v])
                        {
                            e1 = new Tuple<int, int>(source, adj[v].ElementAt(i));
                            e2 = new Tuple<int, int>(source, v);
                            e3 = new Tuple<int, int>(v, adj[v].ElementAt(i));
                            calc_path(e1, e2, e3);
                        }
                    }
                       
                   

                    
                }




            }
        }

        

      //  Tuple<string, string> e=new Tuple<string, string>(source, destination);
        //Console.WriteLine("rs =" + weight[e]);

       
    }
    public static void calc_path(Tuple<int,int> t1, Tuple<int, int> t2, Tuple<int, int> t3)
    {
        int w;
      if (!weight.ContainsKey(t1)) { weight.Add(t1, 0); }
        w=  weight[t2] + weight[t3];
        if (w > weight[t1])
        {
            weight[t1] = w;
        }



       
    }
}
struct edge
{
    public string v1;
    public string v2;
}