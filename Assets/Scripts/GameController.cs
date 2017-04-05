using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject main_camera;
    public float camera_move_speed = 50.0f;
    public float camera_zoom_speed = 50.0f;

    private GameObject main_panel;
    private GameObject gen_panel;
    private GameObject pf_panel;

    private IPathfinding pathfinder;

    private Rect windowRect = new Rect((Screen.width - 300) / 2, (Screen.height - 200) / 2, 300, 200);
    private bool show_error = false;
    private bool show_dialog = false;
    private string error_message;
    string filename = "map.txt";

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
                {
                    OpenError("The map must be at least 10x10");
                }
                    
            }
            else
            {
                OpenError("Too many obstacles");
            }
        }
        catch (System.FormatException)
        {
            OpenError("Wrong value of Map size or Obstacles count");
        }
        CenterCamera();
    }

    private void LoadMap(string filename)
    {
        GameObject map = GameObject.Find("Map");
        map.GetComponent<Map>().LoadMapFromFile(filename);
        CenterCamera();
    }

    private void SaveMap(string filename)
    {
        GameObject map = GameObject.Find("Map");
        map.GetComponent<Map>().SaveMapToFile(filename);
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

    void OnGUI()
    {
        if (show_error)
            windowRect = GUI.Window(0, windowRect, ErrorWindow, "Error");
        if (show_dialog)
            windowRect = GUI.Window(0, windowRect, DialogWindow, "Save/Open Dialog");
    }

    void ErrorWindow(int windowID)
    {
        float y = 20;
        GUI.Label(new Rect(5, y, windowRect.width, 20), error_message);

        if (GUI.Button(new Rect(5, y + 20, windowRect.width - 10, 20), "Shame :("))
        {
            show_error = false;
        }
    }

    void DialogWindow(int windowID)
    {
        GUI.Label(new Rect(5, 20, windowRect.width, 20), "Type your file name:");
        
        filename = GUI.TextField(new Rect(5, 50, windowRect.width, 20), filename);

        if (GUI.Button(new Rect(5, 80, windowRect.width - 10, 20), "Save"))
        {
            SaveMap(filename);
            show_dialog = false;
        }

        if (GUI.Button(new Rect(5, 100, windowRect.width - 10, 20), "Load"))
        {
            LoadMap(filename);
            show_dialog = false;
        }

        if (GUI.Button(new Rect(5, 120, windowRect.width - 10, 20), "Cancel"))
        {
            show_dialog = false;
        }
    }

    public void OpenError(string message)
    {
        error_message = message;
        show_error = true;
    }

    public void OpenDialog()
    {
        show_dialog = true;
    }

    private void CenterCamera()
    {
        GameObject map = GameObject.Find("Map");
       Vector3 center = map.GetComponent<Map>().GetCenter();
        center.z = -10;
        main_camera.transform.position = center;

        Camera.main.orthographicSize = 1.15f * center.y;

    }

}
