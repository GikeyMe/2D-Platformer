﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public Texture background;
    public GUIStyle Button1;
    public GUIStyle Button2;
    public GUIStyle Button3;
    public float guiPlacementY1;
    public float guiPlacementY2;
    public float guiPlacementY3;

    void OnGUI()
    {
        //Display our background texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);


        //Displays our Buttons
        if (GUI.Button(new Rect(Screen.width * .375f, Screen.height * guiPlacementY1, Screen.width * .25f, Screen.height * .1f), "", Button1))
        {
            PlayerPrefsX.SetBool("PlayerLoading", false);
            PlayerPrefsX.SetBool("BossLoading", false);
            PlayerPrefsX.SetBool("LeverLoading", false);
            Application.LoadLevel("Level1");
        }

        if (GUI.Button(new Rect(Screen.width * .375f, Screen.height * guiPlacementY2, Screen.width * .25f, Screen.height * .1f), "", Button2))
        {
            //load the saved level
            PlayerPrefsX.SetBool("PlayerLoading", true);
            if (PlayerPrefs.GetString("CurrentLevel") == "Level2" || PlayerPrefs.GetString("CurrentLevel") == "Level4" || PlayerPrefs.GetString("CurrentLevel") == "Level6")
                PlayerPrefsX.SetBool("BossLoading", true);
            else
                PlayerPrefsX.SetBool("BossLoading", false);
            PlayerPrefsX.SetBool("LeverLoading", true);
            Application.LoadLevel(PlayerPrefs.GetString("CurrentLevel"));
        }

        if (GUI.Button(new Rect(Screen.width * .375f, Screen.height * guiPlacementY3, Screen.width * .25f, Screen.height * .1f), "", Button3))
        {
            Application.Quit();
        } 

    }
}
