using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public int map_size = 10;
    public int tile_size = 32;
    public GameObject tile_prefab;
    private GameObject[] tiles;

	// Use this for initialization
	void Start () {
        GenerateNewMap(map_size);
    }
    public void GenerateNewMap(int new_map_size)
    {
        if (tiles != null)
            for (int i = 0; i < tiles.Length; i++)
            {
                GameObject.Destroy(tiles[i]);   
            }
        tiles = new GameObject[new_map_size * new_map_size];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = Instantiate(tile_prefab, new Vector3(i % new_map_size * tile_size, i / new_map_size * tile_size, 0), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
