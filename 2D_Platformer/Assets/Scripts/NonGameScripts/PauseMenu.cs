using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseUI;
    private bool paused = false;
    private GameObject player;
    private GameObject LeftLever;
    private GameObject RightLever;
    private GameObject boss;

    void Start()
    {
        PauseUI.SetActive(false);
        player = GameObject.Find("Player");
        if (Application.loadedLevelName == "Level2" || Application.loadedLevelName == "Level4" || Application.loadedLevelName == "Level6")
        {
            boss = GameObject.Find("Boss");
        }
        else
        {
            LeftLever = GameObject.Find("LeftLever");
            RightLever = GameObject.Find("RightLever");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        paused = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public void SaveGame()
    {
        PlayerPrefsX.SetVector3("PlayerPosition", player.transform.position);
        PlayerPrefsX.SetVector3("PlayerVelocity", player.GetComponent<Rigidbody2D>().velocity);
        PlayerPrefsX.SetVector3("LastCheckpoint", player.GetComponent<Player>().getLastCheckpoint());
        PlayerPrefs.SetInt("PlayerHealth", player.GetComponent<Player>().getHitPoints());
        PlayerPrefs.SetInt("PlayerLives", player.GetComponent<Player>().getRemainingLives());
        player.GetComponent<Player>().capturePowerUpInfo();
        PlayerPrefs.SetString("CurrentLevel", Application.loadedLevelName);
        if (Application.loadedLevelName == "Level2" || Application.loadedLevelName == "Level4" || Application.loadedLevelName == "Level6")
        {
            PlayerPrefsX.SetVector3("BossPosition", boss.transform.position);
            PlayerPrefsX.SetVector3("BossVelocity", boss.GetComponent<Rigidbody2D>().velocity);
            PlayerPrefs.SetInt("BossHealth", boss.GetComponent<Agent>().getHitPoints());
            if (Application.loadedLevelName == "Level2")
            {
                boss.GetComponent<Boss>().capturePhaseInformation();                
            }
            if (Application.loadedLevelName == "Level4")
            {
                boss.GetComponent<BossTwo>().capturePowerUpInformation();                
            }
            if(Application.loadedLevelName == "Level6")
            {
                boss.GetComponent<BossThree>().capturePowerUpInformation();
            }
        }
        else
        {
            PlayerPrefsX.SetBool("LeftLeverPulled", LeftLever.GetComponent<Animator>().GetBool("Pulled"));
            PlayerPrefsX.SetBool("RightLeverPulled", RightLever.GetComponent<Animator>().GetBool("Pulled"));
        }
    }
}
