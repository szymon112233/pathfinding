using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {

    private Rect windowRect = new Rect((Screen.width - 300) / 2, (Screen.height - 200) / 2, 300, 200);
    private bool show_error = false;
    private bool show_dialog = false;
    private string error_message;
    string filename = "map.txt";


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
}
}
