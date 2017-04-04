using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool is_obstacle;

	void Awake () {
        is_obstacle = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsObstacle()
    {
        return is_obstacle;
    }
    public void SetObstacle(bool value)
    {
        is_obstacle = value;
    }
    public void SetPosition(Vector3 pos)
    {
        gameObject.GetComponent<Transform>().position = pos;
    }
   
}
