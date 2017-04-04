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
    private GameObject pf_panel;

    private IPathfinding pathfinder;

    void Start () {
        main_panel = GameObject.Find("MainPanel");
        gen_panel = GameObject.Find("GenPanel");
        pf_panel = GameObject.Find("PathfindingPanel");

        pathfinder = new BFSPathfinding();
        MainMode();

    }
	
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
            MainMode();
        }
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

            if ((0.5f * (size * size) >= obstacles))
            {
                if (size >= 10)
                    map.GetComponent<Map>().GenerateNewMap(size, obstacles);
                else
                    EditorUtility.DisplayDialog("Error", "Minimalny rozmiar mapy to 10x10!", "Kurde!");
            }
            else
                EditorUtility.DisplayDialog("Error", "Za duzo obstacli!", "Kurde!");
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

    public void MainMode()
    {
        main_panel.SetActive(true);
        pf_panel.SetActive(false);
        gen_panel.SetActive(false);
    }

    public void GeneratingMode()
    {
        main_panel.SetActive(false);
        pf_panel.SetActive(false);
        gen_panel.SetActive(true);
    }

    public void PathfindingMode()
    {
        main_panel.SetActive(false);
        gen_panel.SetActive(false);
        pf_panel.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    public void Findpath()
    {
        GameObject map = GameObject.Find("Map");

        if (map != null)
        {
            if (GameObject.Find("Dropdown").GetComponentInChildren<Text>().text == "A*")
                pathfinder = new AstarPathfinding();
            else if (GameObject.Find("Dropdown").GetComponentInChildren<Text>().text == "BFS")
                pathfinder = new BFSPathfinding();



                map.GetComponent<Map>().Findpath(pathfinder);
        }
    }

}
