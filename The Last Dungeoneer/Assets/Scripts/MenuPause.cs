using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour {

    public GameObject mainMenu;
    private float timeScaleRef;
    private float volumeRef;
    private bool paused;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            MenuStatusChange();
    }


    private void MenuOn()
    {
        timeScaleRef = Time.timeScale;
        Time.timeScale = 0f;

        volumeRef = AudioListener.volume;
        AudioListener.volume = 0f;

        mainMenu.SetActive(true);
        paused = true;
    }


    public void MenuOff()
    {
        Time.timeScale = timeScaleRef;
        AudioListener.volume = volumeRef;

        mainMenu.SetActive(false);
        paused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void MenuStatusChange()
    {
        if (paused)
            MenuOff();
        else
            MenuOn();
    }
}
