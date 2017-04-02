using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject main_camera;

    public float camera_move_speed = 50.0f;
    public float camera_zoom_speed = 50.0f;
    private
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.orthographicSize += Time.deltaTime * -Input.mouseScrollDelta.y * camera_zoom_speed;

        if (Input.GetKey(KeyCode.RightArrow))
            main_camera.transform.Translate(new Vector3(Time.deltaTime * camera_move_speed, 0, 0));
        if (Input.GetKey(KeyCode.LeftArrow))
            main_camera.transform.Translate(new Vector3(Time.deltaTime * -camera_move_speed, 0, 0));
        if (Input.GetKey(KeyCode.UpArrow))
            main_camera.transform.Translate(new Vector3(0, Time.deltaTime * camera_move_speed, 0));
        if (Input.GetKey(KeyCode.DownArrow))
            main_camera.transform.Translate(new Vector3(0, Time.deltaTime * -camera_move_speed, 0));
    }
    public void GenerateMap()
    {
        GameObject main_panel = GameObject.Find("MainPanel");
        GameObject gen_panel = GameObject.Find("GenPanel");

        main_panel.SetActive(false);
        gen_panel.SetActive(true);

    }

    public void GenerateNewMap()
    {
        GameObject gen_panel = GameObject.Find("GenPanel");
        GameObject map = GameObject.Find("Map");

        map.GetComponent<Map>().GenerateNewMap(int.Parse(gen_panel.GetComponentInChildren<InputField>().text));
       // gen_panel.SetActive(false);

    }

    public void LoadMap()
    {
    }

    public void Exit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
