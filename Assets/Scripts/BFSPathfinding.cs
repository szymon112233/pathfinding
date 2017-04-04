using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSPathfinding : IPathfinding {

    public List<int> GetPath(int start, int finish, GameObject[] tiles, int map_size, List<int> obstacles)
    {
        List<int> closed_list = new List<int>();
        Queue<int> tiles_queue = new Queue<int>();
       
        Hashtable parents = new Hashtable();

        closed_list.Add(start);
        tiles_queue.Enqueue(start);

        while (tiles_queue.Count > 0)
        {
            
            int current = tiles_queue.Dequeue();
            Debug.Log("Checking tile: " + current);

            if (current == finish)
                return GetParentsPath(parents, current);

            List<int> neighbors = new List<int>();

            if (current - map_size > 0)
                if (!obstacles.Contains(current - map_size))
                    neighbors.Add(current - map_size);
            if (current + map_size < map_size * map_size)
                if (!obstacles.Contains(current + map_size))
                    neighbors.Add(current + map_size);
            if (current % map_size != 0)
                if (!obstacles.Contains(current - 1))
                    neighbors.Add(current - 1);
            if (current % map_size != map_size - 1)
                if (!obstacles.Contains(current + 1))
                    neighbors.Add(current + 1);

            foreach (int neighbor in neighbors)
            {
                if (closed_list.Contains(neighbor))
                    continue;

                closed_list.Add(neighbor);
                parents[neighbor] = current;
                tiles_queue.Enqueue(neighbor);
            }
        }


        return new List<int>();
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
