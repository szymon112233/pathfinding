using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathfinding : IPathfinding {


    public List<int> GetPath(int start, int finish, GameObject[] tiles, int map_size, List<int> obstacles)
    {
        List<int> closed_list = new List<int>();
        List<int> open_list = new List<int>();
        open_list.Add(start);

        Hashtable parents = new Hashtable();

        Hashtable g_costs = new Hashtable();
        g_costs.Add(start, 0.0f);
        Hashtable f_costs = new Hashtable();

        f_costs.Add(start, heuristic(start, start, finish, map_size));

        while (open_list.Count > 0)
        {
            int current = open_list[open_list.Count - 1];
            foreach (int tile in open_list)
            {
                if ((float)f_costs[current] > (float)f_costs[tile])
                    current = tile;
            }

            if (current == finish)
                return GetParentsPath(parents, current);

            open_list.Remove(current);
            closed_list.Add(current);


            List<int> neighbors = new List<int>();

            if (current - map_size > 0 && !obstacles.Contains(current - map_size))
                neighbors.Add(current - map_size);
            if (current + map_size < map_size * map_size && !obstacles.Contains(current + map_size))
                neighbors.Add(current + map_size);
            if (current % map_size != 0 && !obstacles.Contains(current - 1))
                neighbors.Add(current - 1);
            if (current % map_size != map_size - 1 && !obstacles.Contains(current + 1))
                neighbors.Add(current + 1);

            foreach (int neighbor in neighbors)
            {
                if (closed_list.Contains(neighbor))
                    continue;

                float temp_g_cost = (float)g_costs[current] + 10.0f;

                if (!open_list.Contains(neighbor))
                    open_list.Add(neighbor);
                else if (temp_g_cost >= (float)g_costs[neighbor])
                    continue;

                parents[neighbor] = current;

                g_costs[neighbor] = temp_g_cost;

                

                f_costs[neighbor] = (float)g_costs[neighbor] + heuristic(neighbor, start, finish, map_size);
            }
        }


        return new List<int>();
    }

    private float heuristic(int current, int start, int finish, int map_size)
    {

        int dist_current_x = current % map_size - finish % map_size;
        int dist_current_y = current / map_size - finish / map_size;
        int dist_start_x = start % map_size - finish % map_size;
        int dist_start_y = start / map_size - finish / map_size;

        float result = (Mathf.Abs(dist_current_x) + Mathf.Abs(dist_current_y));
        float cross = Mathf.Abs(dist_current_x * dist_start_y - dist_start_x * dist_current_y);

        result += cross * 0.001f;
        return result;
    }

    private List<int> GetParentsPath(Hashtable parents, int from)
    {
        List<int> path = new List<int>();

        int current = from;
        while (parents.ContainsKey(current))
        {
            path.Add(current);
            current = (int)parents[current];
        }

        return path;
    }
}
