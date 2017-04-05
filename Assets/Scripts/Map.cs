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
    private List<int> last_path;

	void Start ()
    {
        obstacles = new List<int>();
        last_path = new List<int>();
    }

    public void GenerateNewMap(int new_map_size, int obstacle_number)
    {

        //Destroy old Tiles
        if (tiles != null)
            for (int i = 0; i < tiles.Length; i++)
            {
                GameObject.Destroy(tiles[i]);   
            }
        last_path.Clear();
        // Create new tiles
        tiles = new GameObject[new_map_size * new_map_size];
        map_size = new_map_size;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = Instantiate(tile_prefab, new Vector3(i % new_map_size * tile_size, i / new_map_size * tile_size, 0), Quaternion.identity);
        }

        // Init random place generator
        int tmp_target = Random.Range(0, map_size * map_size - 1);

        // Randomly place obstacles
        obstacles.Clear();
        do
        {   
            tmp_target = Random.Range(0, map_size * map_size - 1);
            if (!obstacles.Contains(tmp_target))
            {
                // Wylosuj rozmiar
                int x = Random.Range(0, 2);
                int y = Random.Range(0, 2);

                obstacles.Add(tmp_target);
                if (tmp_target % map_size != map_size - 1)
                    if (!obstacles.Contains(tmp_target + x))
                    obstacles.Add(tmp_target + x);
                if (tmp_target + map_size < map_size * map_size)
                    if (!obstacles.Contains(tmp_target + y * map_size))
                        obstacles.Add(tmp_target + y * map_size);

                if (x + y == 2)
                {
                    if (tmp_target % map_size != map_size - 1 && tmp_target + map_size < map_size * map_size)
                        if (!obstacles.Contains(tmp_target + x + y * map_size))
                            obstacles.Add(tmp_target + x + y * map_size);
                }

                obstacle_number--;
            }
                
        } while (obstacle_number > 0);


        // Randomly place start and finish point
        while (obstacles.Contains(tmp_target))
            tmp_target = Random.Range(0, map_size * map_size - 1);
        a = tmp_target;

        tmp_target = Random.Range(0, map_size * map_size - 1);
        while (obstacles.Contains(tmp_target) || tmp_target == a)
            tmp_target = Random.Range(0, map_size * map_size - 1);
        b = tmp_target;


        // Mark obstacles, a & b point with colors
        
        ColorTiles();
    }

    private void ColorTiles()
    {
        tiles[a].GetComponent<SpriteRenderer>().color = Color.green;
        tiles[b].GetComponent<SpriteRenderer>().color = Color.blue;

        foreach (int tile in obstacles)
        {
                tiles[tile].GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void Findpath(IPathfinding pathfinder)
    {
        List<int> path = pathfinder.GetPath(a, b, tiles, map_size, obstacles);
        ColorPathTiles(path);
        last_path = path;
    }

    private void ColorPathTiles(List<int> path)
    {
        foreach (int current in last_path)
        {
            if (current != a && current != b)
                tiles[current].GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (int current in path)
        {
            if ( current != a && current != b)
                tiles[current].GetComponent<SpriteRenderer>().color = Color.red;
        }
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
        Debug.Log("Saving map to :" + filename);
    }

    public void LoadMapFromFile(string filename)
    {

        Debug.Log("Loading map from :" + filename);
        StreamReader file = File.OpenText(filename);

        string line = "";
        line = file.ReadLine();

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

        obstacles.Clear();
        foreach (string string_number in values)
        {
            if (string_number != "")
            {
                int number = int.Parse(string_number);
                obstacles.Add(number);
            }
            
        }

        last_path.Clear();
        ColorTiles();

        file.Close();
    }

    public Vector3 GetCenter()
    {
        return gameObject.transform.position + new Vector3(map_size * tile_size /2, map_size * tile_size / 2, 0);
    }
}
