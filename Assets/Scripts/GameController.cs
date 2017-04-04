using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameController : MonoBehaviour {

    public GameObject main_camera;

    public float camera_move_speed = 50.0f;
    public float camera_zoom_speed = 50.0f;

    private GameObject main_panel;
    private GameObject gen_panel;
    // Use this for initialization
    void Start () {
        main_panel = GameObject.Find("MainPanel");
        gen_panel = GameObject.Find("GenPanel");
        gen_panel.SetActive(false);
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

        if (Input.GetMouseButton(1))
        {
            main_camera.transform.Translate(new Vector3(Time.deltaTime * camera_move_speed * Input.GetAxis("Horizontal"), Time.deltaTime * camera_move_speed * Input.GetAxis("Vertical"), 0));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            main_panel.SetActive(true);
            gen_panel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject map = GameObject.Find("Map");

            if (map != null)
            {
                map.GetComponent<Map>().ColorPathTiles(map.GetComponent<Map>().Astar(map.GetComponent<Map>().a, map.GetComponent<Map>().b));
            }
        }
    }
    public void GenerateMap()
    {
      
        main_panel.SetActive(false);
        gen_panel.SetActive(true);

    }

    public void GenerateNewMap()
    {
        GameObject input_size = GameObject.Find("SizeInputField");
        GameObject input_obstacles = GameObject.Find("ObstaclesInputField");
        GameObject map = GameObject.Find("Map");

       

        try
        {
            int size = int.Parse(input_size.GetComponent<InputField>().text);
            int obstacles = int.Parse(input_obstacles.GetComponent<InputField>().text);

            map.GetComponent<Map>().GenerateNewMap(size, obstacles);

        }
        catch (System.FormatException exception)
        {
            EditorUtility.DisplayDialog("Error", "Zle wartości w polu Map size  i/lub Obstacles count", "Kurde!");
        }
    }

    public void LoadMap()
    {
        GameObject map = GameObject.Find("Map");
        map.GetComponent<Map>().LoadMapFromFile(EditorUtility.OpenFilePanel("Load generated map", "/", "txt"));
    }

    public void SaveMap()
    {
        GameObject map = GameObject.Find("Map");
        map.GetComponent<Map>().SaveMapToFile(EditorUtility.SaveFilePanel("Save generated map", "/", "map", "txt"));
    }

    public void Exit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}
