using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;


class graph
{
    public static Dictionary<Tuple<string,string>, int> weight = new Dictionary<Tuple<string, string>, int>();
    public static Dictionary<string, HashSet<string>> adj = new Dictionary<string, HashSet<string>>();

   // public static Dictionary<edge, string> relation_strength = new Dictionary<edge, string>();
  //  public static List<int> strength;
   // public static Tuple<string, string> edges;
    static public void Main(String[] args)
    {
        read();
    }
    static void read()
    {
        string text = File.ReadAllText(@"Movies193.txt");
        string[] movies = text.Split('\n');
        string[][] actors = new string[movies.Length][];
        
      //  edge e;
        Tuple<string, string> tuple;
        Tuple<string, String> inverse;
        for (int i = 0; i < movies.Length; i++)
        {
            actors[i] = movies[i].Split('/');
            
        }
        for (int i = 0; i < movies.Length; i++)
        {
            int length = actors[i].Length;
            for (int j = 1; j < length; j++)
            {
                if (!adj.ContainsKey(actors[i][j]))
                {
                    adj.Add(actors[i][j], new HashSet<string>());
                }
                for (int k = 1; k < length; k++)
                {
                    if (k != j)
                    {
                        adj[actors[i][j]].Add(actors[i][k]);
                    }
                }
            }
            for (int j = 1; j < length; j++)
            {
                for (int k = j + 1; k < length; k++)
                {
                   tuple=new Tuple<string, string>(actors[i][j], actors[i][k]);
                   inverse=new Tuple<string, string>(actors[i][k], actors[i][j]);                 
                    if (weight.ContainsKey(tuple))
                    {
                        weight[tuple]++;
                    }
                    else
                    {
                        weight.Add(tuple, 1);
                    }
                    if (weight.ContainsKey(inverse))
                    {
                        weight[inverse]++;
                    }
                    else
                    {
                        weight.Add(inverse, 1);
                    }
                }
            }
        }
     /*   foreach (Tuple<string,string> e2 in weight.Keys)
         {
              Console.WriteLine(e2+"/"+weight[e2]);
         }*/
     string text2 = File.ReadAllText(@"queries110.txt");
        string[] queries = text2.Split('\n');

        for (int i = 0; i < queries.Length; i++)
        {
            string[] values = queries[i].Trim().Split('/');
            string src = values[0];
            string dst = values[1];
            Dictionary<string,int> level = dos(src, dst);
            Console.WriteLine(src + "/" + dst);
            Console.WriteLine("dos = " + level[dst]);
            if (src == "Hall, Douglas Kent")
                _ = 1;
            rs(src, dst, level);
            Tuple<string,string> strength = new Tuple<string,string>(src, dst);
            Console.WriteLine("rs = " + weight[strength]);


        }
    }
     public static Dictionary<string,int> dos(string src, string dist)
    {
        HashSet<string> visited = new HashSet<string>();
        Queue<string> q = new Queue<string>();
        Dictionary<string, int> level = new Dictionary<string, int>();
        q.Enqueue(src);
        visited.Add(src);
        level[src] = 0;
        
       


        while (q.Count != 0)
        {
            if (!visited.Contains(dist))
            {
                string v = q.Dequeue();
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
    static void rs(string source, string destination,Dictionary<string,int> level)
    {
        // Dictionary<int, List<string>> layers = new Dictionary<int, List<string>>();
        if (source == "Steele, Tom" && destination == "Clemenson, Christian")
            _ = 1;
        if (source == "Hall, Douglas Kent" && destination == "Baldwin, Daniel")
            _ = 1;
        HashSet<string> visited = new HashSet<string>();
        HashSet<int> layers = new HashSet<int>();
        // weight from source to dist "weight[e1]" = weight from source to parent "weight [e2]" + weight from parent to dist "weight [e3]"
        Tuple<string, string> e1;
        Tuple<string, string> e2;
        Tuple<string, string> e3;

       // layers[0].Add(source);
        Queue<string> q = new Queue<string>();
        q.Enqueue(source);
        layers.Add(level[source]);
        visited.Add(source);
        while (q.Count != 0)
        {
            string v = q.Dequeue();


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
                            e1 = new Tuple<string, string>(source, adj[v].ElementAt(i));
                            e2 = new Tuple<string, string>(source, v);
                            e3 = new Tuple<string, string>(v, adj[v].ElementAt(i));
                            calc_path(e1, e2, e3);
                        }
                    }
                       
                   

                    
                }




            }
        }

        

      //  Tuple<string, string> e=new Tuple<string, string>(source, destination);
        //Console.WriteLine("rs =" + weight[e]);

       
    }
    public static void calc_path(Tuple<string,string> t1, Tuple<string, string> t2, Tuple<string, string> t3)
    {
        int w;
      if (!weight.ContainsKey(t1)) { weight.Add(t1, 0); }
        w=  weight[t2] + weight[t3];
        if (w > weight[t1])
        {
            if (t1.Item1 == "Hall, Douglas Kent")
                _ = 1;
            weight[t1] = w;
        }



       
    }
}
struct edge
{
    public string v1;
    public string v2;
}