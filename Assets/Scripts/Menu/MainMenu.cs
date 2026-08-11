using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainPanel, optionsPanel;
    [SerializeField] CanvasGroup canvasGroup, blackScreen;
    [SerializeField] AudioSource music;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            StartCoroutine(FadeMenu(1, 0));
        }
    }

    public void StartGame()
    {
        StartCoroutine(FadeMenu(0, 1));
    }

    public void OpenOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public IEnumerator FadeMenu(float start, float end, float step = 0.002f)
    {
        blackScreen.alpha = start;
        canvasGroup.alpha = end;

        if (start > end)
        {
            while (blackScreen.alpha > end)
            {
                blackScreen.alpha -= step;
                yield return new WaitForEndOfFrame();
            }
            while (canvasGroup.alpha < start)
            {
                canvasGroup.alpha += step;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (blackScreen.alpha < end)
            {
                blackScreen.alpha += step;
                canvasGroup.alpha -= step;
                music.volume -= 0.002f;

                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Forest");
        }
    }
}