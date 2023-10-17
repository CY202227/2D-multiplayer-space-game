using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()

    {
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void WindowsMode()
    {
        Screen.fullScreen = false;
    }
    public void FullScreen()
    {
        Screen.fullScreen = true;
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void Option()
    {
        SceneManager.LoadScene(1);
    }
}