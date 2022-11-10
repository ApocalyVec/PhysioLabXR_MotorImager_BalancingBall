using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] CanvasGroup menuCG;
    private bool menuIsOpened;

    private void Start()
    {
        if (menuCG != null)
            menuCG.alpha = 0;
    }
    private void Update()
    {
        if (menuCG != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuIsOpened)
            {
                menuCG.alpha = 1;
                menuIsOpened = true;
            }
            else
            {
                menuCG.alpha = 0;
                menuIsOpened = false;
            }
        }
    }
    public void goToTesting()
    {
        SceneManager.LoadScene("Training", LoadSceneMode.Single);
    }

    public void goToGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
