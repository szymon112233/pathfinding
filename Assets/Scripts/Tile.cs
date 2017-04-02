using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private bool is_obstacle;
	// Use this for initialization
	void Start () {
        is_obstacle = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool IsObstacle()
    {
        return is_obstacle;
    }
    void SetObstacle(bool value)
    {
        is_obstacle = value;
    }
    public void SetPosition(Vector3 pos)
    {
        gameObject.GetComponent<Transform>().position = pos;
    }
   
}
