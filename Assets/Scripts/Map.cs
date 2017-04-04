using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Map : MonoBehaviour {

    public int map_size = 10;
    public int tile_size = 32;
    public int a, b;
    public GameObject tile_prefab;
    private GameObject[] tiles;
    private List<int> obstacles;

	// Use this for initialization
	void Start () {
        obstacles = new List<int>();
    }
    public void GenerateNewMap(int new_map_size, int obstacle_number)
    {

        if (tiles != null)
            for (int i = 0; i < tiles.Length; i++)
            {
                GameObject.Destroy(tiles[i]);   
            }
        tiles = new GameObject[new_map_size * new_map_size];
        map_size = new_map_size;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = Instantiate(tile_prefab, new Vector3(i % new_map_size * tile_size, i / new_map_size * tile_size, 0), Quaternion.identity);
        }

        int tmp_target = Random.Range(0, map_size * map_size - 1);

        obstacles.Clear();
        for(int i = 0; i<obstacle_number; i++)
        {
            
            tmp_target = Random.Range(0, map_size * map_size - 1);
            tiles[tmp_target].GetComponent<Tile>().is_obstacle = true;
            if (!obstacles.Contains(tmp_target))
                obstacles.Add(tmp_target);
        }
        Debug.Log(obstacles.Count);

        while (tiles[tmp_target].GetComponent<Tile>().IsObstacle())
            tmp_target = Random.Range(0, map_size * map_size - 1);
        a = tmp_target;

        tmp_target = Random.Range(0, map_size * map_size - 1);
        while (tiles[tmp_target].GetComponent<Tile>().IsObstacle() || tmp_target == a)
            tmp_target = Random.Range(0, map_size * map_size - 1);
        b = tmp_target;

        ColorTiles();
    }

    private void ColorTiles()
    {
        tiles[a].GetComponent<SpriteRenderer>().color = Color.green;
        tiles[b].GetComponent<SpriteRenderer>().color = Color.blue;

        foreach (GameObject tile in tiles)
        {
            if (tile.GetComponent<Tile>().IsObstacle())
                tile.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    public void ColorPathTiles(List<int> path)
    {
        foreach (int current in path)
        {
            if ( current != a && current != b)
                tiles[current].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public List<int> Astar(int start, int finish)
    {
        List<int> closed_list = new List<int>();
        List<int> open_list = new List<int>();
        open_list.Add(start);

        Hashtable parents = new Hashtable();

        Hashtable g_costs = new Hashtable();
        g_costs.Add(start, 0.0f);
        Hashtable f_costs = new Hashtable();

        f_costs.Add(start, heuristic(start, finish));

        while (open_list.Count > 0)
        {
            int current = open_list[open_list.Count - 1];
            foreach (int tile in open_list)
            {
                if ((float)f_costs[current] > (float)f_costs[tile])
                    current = tile;
            }

            if (current == finish)
                return GetPath(parents, current);
            
            open_list.Remove(current);
            closed_list.Add(current);


            List<int> neighbors = new List<int>();

            if(current - map_size > 0)
                if (!tiles[current - map_size].GetComponent<Tile>().IsObstacle())
                    neighbors.Add(current - map_size);
            if (current + map_size < map_size * map_size)
                if (!tiles[current + map_size].GetComponent<Tile>().IsObstacle())
                    neighbors.Add(current + map_size);
            if (current % map_size != 0)
                if (!tiles[current - 1].GetComponent<Tile>().IsObstacle())
                    neighbors.Add(current - 1);
            if (current % map_size != map_size -1)
                if (!tiles[current + 1].GetComponent<Tile>().IsObstacle())
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
                f_costs[neighbor] = (float)g_costs[neighbor] + heuristic(neighbor, finish);
            }
        }


        return new List<int>();
    }

    private float heuristic(int start, int finish)
    {
        return (Mathf.Abs(start/map_size - finish/map_size) + Mathf.Abs(start % map_size - finish % map_size));
    }

    private List<int> GetPath(Hashtable parents, int from)
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

    public void SaveMapToFile(string filename)
    {
        StreamWriter file = File.CreateText(filename);

        file.WriteLine(map_size + " " + tile_size + " " + a + " " + b);
        int saved_obstacles = 0;
        foreach (int obstacle in obstacles)
        {
            file.Write(obstacle + " ");
            saved_obstacles++;
        }
        file.Close();
        Debug.Log("Saved obstacles:" + saved_obstacles);
        Debug.Log("Saving map to :" + filename);
    }

    public void LoadMapFromFile(string filename)
    {

        Debug.Log("Loading map from :" + filename);
        StreamReader file = File.OpenText(filename);

        string line = "";
        line = file.ReadLine();
        Debug.Log(line);

        string[] values = line.Split();

        map_size = int.Parse(values[0]);
        tile_size = int.Parse(values[1]);
        a = int.Parse(values[2]);
        b = int.Parse(values[3]);


        if (tiles != null)
            for (int i = 0; i < tiles.Length; i++)
            {
                GameObject.Destroy(tiles[i]);
            }
        tiles = new GameObject[map_size * map_size];

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = Instantiate(tile_prefab, new Vector3(i % map_size * tile_size, i / map_size * tile_size, 0), Quaternion.identity);
        }

        line = file.ReadLine();
        values = line.Split(' ');


        Debug.Log(values.Length + ": " + line);

        obstacles.Clear();
        foreach (string string_number in values)
        {
            if (string_number != "")
            {
                int number = int.Parse(string_number);
                obstacles.Add(number);
                tiles[number].GetComponent<Tile>().SetObstacle(true);
            }
            
        }
        ColorTiles();

        file.Close();
    }
}
