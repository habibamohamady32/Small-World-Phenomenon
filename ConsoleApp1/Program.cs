using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
namespace graph
{
    class Program
    {
        public static List<movies_data> movies = new List<movies_data>();
        public static HashSet<string> actors = new HashSet<string>();
        public static Dictionary<string, int> actors_id = new Dictionary<string, int>();
        public static Dictionary<int, string> actors_name = new Dictionary<int, string>();
        public static Dictionary<int, HashSet<int>> adj = new Dictionary<int, HashSet<int>>();
        public static Dictionary<Tuple<int, int>, int> weight = new Dictionary<Tuple<int, int>, int>();
       // public static HashSet<int>[] adj;

        static void Main(string[] args)
        {
            read();
            read_queries();
        }
        public class movies_data
        {
            public string movie_name;
            public List<string> movie_actors;
            public movies_data()
            {
                movie_actors = new List<string>();
            }
        }

        public static void read()
        {
            FileStream fileStream = new FileStream("movies14129.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fileStream);
            string line = null;
            while (sr.Peek() > -1)
            {

                movies_data movie = new movies_data();
                line = sr.ReadLine();
                string[] parts = line.Trim().Split('/');
                movie.movie_name = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    movie.movie_actors.Add(parts[i]);
                    actors.Add(parts[i]);
                }

                movies.Add(movie);
            }

            sr.Close();
            //Uncomment To test whether it is reading the file correctly

            /*  foreach (string actor in actors) { Console.WriteLine(actor); }
              foreach(movies_data movie in movies) { Console.WriteLine(movie.movie_name); }*/

            // convert string vetices to integers *hashing*
            for (int i = 0; i < actors.Count; i++)
            {
                actors_id.Add(actors.ElementAt(i), i);
                actors_name.Add(i, actors.ElementAt(i));


            }
            foreach (movies_data m in movies)
            {
                for (int i = 0; i < m.movie_actors.Count; i++)
                {
                    int temp = actors_id[m.movie_actors[i]];
                    if (!adj.ContainsKey(temp)) { adj.Add(temp, new HashSet<int>()); }
                    
                    for (int j = 0; j < m.movie_actors.Count; j++)
                    {
                        
                        int temp2 = actors_id[m.movie_actors[j]];
                        if (m.movie_actors[j] != m.movie_actors[i])
                        {
                            adj[temp].Add(temp2);
                        }



                    }

                }
                for (int i = 0; i < m.movie_actors.Count; i++)
                {
                    for (int j = i + 1; j < m.movie_actors.Count; j++)
                    {
                        int temp = actors_id[m.movie_actors[i]];
                        int temp2 = actors_id[m.movie_actors[j]];
                        Tuple<int, int> tuple = new Tuple<int, int>(temp, temp2);
                        Tuple<int, int> inverse = new Tuple<int, int>(temp2, temp);

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
            // Uncomment to test whether it's calculate the weights correctly
            /* foreach (Tuple<int, int> e2 in weight.Keys)
             {
                 Tuple<string, string> e1 = new Tuple<string, string>(actors_name[e2.Item1],actors_name[e2.Item2]);

                 Console.WriteLine(e1 + "/" + weight[e2]);
             }*/


        }
        public static void read_queries()
        {
            FileStream fs = new FileStream("queries1.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = null;
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();
                string[] parts = line.Trim().Split('/');
                int source = actors_id[parts[0]];
                int destination = actors_id[parts[1]];
                Dictionary<int, int> levels = dos(source, destination);
                Console.WriteLine(actors_name[source] + "/" + actors_name[destination]);
                Console.WriteLine("dos = " + levels[destination]);
                rs(source, destination, levels);
                Tuple<int, int> strength = new Tuple<int, int>(destination, source);
                Console.WriteLine("rs = " + weight[strength]);

            }

        }
        public static Dictionary<int, int> dos(int src, int dist)
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
        static void rs(int source, int destination, Dictionary<int, int> level)
        {
            // Dictionary<int, List<string>> layers = new Dictionary<int, List<string>>();

            HashSet<int> visited = new HashSet<int>();
           // HashSet<int> layers = new HashSet<int>();
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
                    if (level.ContainsKey(adj[v].ElementAt(i)) &&level[adj[v].ElementAt(i)] < level[v])
                    {

                        if (!visited.Contains(adj[v].ElementAt(i)))
                        {
                         //if (!layers.Contains(level[destination]))
                         //{

                       

                            q.Enqueue(adj[v].ElementAt(i));
                            visited.Add(adj[v].ElementAt(i));
                            // layers.Add(level[adj[v].ElementAt(i)]);

                        }






                        if (v != destination )
                        {
                            
                            
                               
                                    e1 = new Tuple<int, int>(destination, adj[v].ElementAt(i));
                                    e2 = new Tuple<int, int>(destination, v);
                                    e3 = new Tuple<int, int>(v, adj[v].ElementAt(i));
                                    calc_path(e1, e2, e3);
                                
                        } //}

                    }
                    // for update weight of[src,dst]
                   




                }
            }






        }
        public static void calc_path(Tuple<int, int> t1, Tuple<int, int> t2, Tuple<int, int> t3)
        {
            int w;
            if (!weight.ContainsKey(t1)) { weight.Add(t1, 0); }
            w = weight[t2] + weight[t3];
            if (w > weight[t1])
            {

                weight[t1] = w;
            }




        }



    }

}