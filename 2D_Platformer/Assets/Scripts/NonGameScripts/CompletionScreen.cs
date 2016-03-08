using UnityEngine;
using System.Collections;

public class CompletionScreen : MonoBehaviour
{

    public Texture background;
    public Texture completionText;
    public GUIStyle Button1;
    public float guiPlacementY1;
    public float guiPlacementY2;

    void OnGUI()
    {
        //Display our background texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);


        //Displays our Buttons
        GUI.DrawTexture(new Rect(Screen.width * .125f, Screen.height * guiPlacementY1, Screen.width * .75f, Screen.height * .2f), completionText);

        if (GUI.Button(new Rect(Screen.width * .375f, Screen.height * guiPlacementY2, Screen.width * .25f, Screen.height * .1f), "", Button1))
        {
            Application.LoadLevel("MainMenu");
        }

    }
}
