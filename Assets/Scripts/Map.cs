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

	void Start ()
    {
        obstacles = new List<int>();
    }

    public void GenerateNewMap(int new_map_size, int obstacle_number)
    {

        //Destroy old Tiles
        if (tiles != null)
            for (int i = 0; i < tiles.Length; i++)
            {
                GameObject.Destroy(tiles[i]);   
            }
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
        for(int i = 0; i<obstacle_number; i++)
        {   
            tmp_target = Random.Range(0, map_size * map_size - 1);
            tiles[tmp_target].GetComponent<Tile>().SetObstacle(true);
            if (!obstacles.Contains(tmp_target))
                obstacles.Add(tmp_target);
        }


        // Randomly place start and finish point
        while (tiles[tmp_target].GetComponent<Tile>().IsObstacle())
            tmp_target = Random.Range(0, map_size * map_size - 1);
        a = tmp_target;

        tmp_target = Random.Range(0, map_size * map_size - 1);
        while (tiles[tmp_target].GetComponent<Tile>().IsObstacle() || tmp_target == a)
            tmp_target = Random.Range(0, map_size * map_size - 1);
        b = tmp_target;


        // Mark obstacles, a & b point with colors
        ColorTiles();
    }

    private void ColorTiles()
    {
        tiles[a].GetComponent<SpriteRenderer>().color = Color.green;
        tiles[b].GetComponent<SpriteRenderer>().color = Color.blue;

        foreach (GameObject tile in tiles)
        {
            if (tile.GetComponent<Tile>().IsObstacle())
                tile.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void Findpath(IPathfinding pathfinder)
    {
        ColorPathTiles(pathfinder.GetPath(a, b, tiles, map_size, obstacles));
    }

    private void ColorPathTiles(List<int> path)
    {
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
                tiles[number].GetComponent<Tile>().SetObstacle(true);
            }
            
        }
        ColorTiles();

        file.Close();
    }
}
