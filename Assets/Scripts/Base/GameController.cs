using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public IInteractable currentInteract, readyToInteract;

    public PlayerController Player;
    public CanvasGroup PlayerCanvas;

    public int progression = 0;

    public delegate void SceneEvent();
    public SceneEvent sceneEvent;

    public GameObject mainMenu;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Forest":
                sceneEvent = new SceneEvent(StartEventForest);
                StartCoroutine(FadeScreen(1, 0));
                break;
            case "Desert":
                sceneEvent = new SceneEvent(StartEventDesert);
                StartCoroutine(FadeScreen(1, 0));
                break;
            case "River":
                sceneEvent = new SceneEvent(StartEventRiver);
                StartCoroutine(FadeScreenDesert(1, 0));
                break;
        }
    }

    public void AddProgress(int num)
    {
        progression += num;
        sceneEvent.Invoke();
    }

    public void StartEventForest()
    {
        switch (progression)
        {
            case 0:
                DialogController.Instance.SetLines(Player.GetComponent<SpeakingPerson>().dialog.list[0..4]);
                DialogController.Instance.SetCameraPoints(Player.GetComponent<SpeakingPerson>().cameraPoints, 3);
                DialogController.Instance.StartDialog();
                return;
            case 1:
                DialogController.Instance.SetLines(Player.GetComponent<SpeakingPerson>().dialog.list[4..5]);
                DialogController.Instance.SetCameraPoints(null, 0);
                DialogController.Instance.StartDialog();
                return;
            case 2:
                DialogController.Instance.SetLines(Player.GetComponent<SpeakingPerson>().dialog.list[5..6]);
                DialogController.Instance.SetCameraPoints(null, 0);
                DialogController.Instance.StartDialog();
                return;
            case 5:
                DialogController.Instance.SetLines(Player.GetComponent<SpeakingPerson>().dialog.list[6..7]);
                DialogController.Instance.SetCameraPoints(null, 0);
                DialogController.Instance.StartDialog();
                return;
        }
    }

    public void StartEventDesert()
    {
        switch (progression)
        {
            case 1:
                DialogController.Instance.SetLines(Player.GetComponent<SpeakingPerson>().dialog.list[6..7]);
                DialogController.Instance.SetCameraPoints(null, 0);
                DialogController.Instance.StartDialog();
                return;
            case 3:
                DialogController.Instance.SetLines(Player.GetComponent<SpeakingPerson>().dialog.list[7..8]);
                DialogController.Instance.SetCameraPoints(null, 0);
                DialogController.Instance.StartDialog();
                return;
            case 7:
                StartCoroutine(FadeScreenDesert(0, 1));
                return;
        }
    }

    public void StartEventRiver()
    {
        return;
    }

    public IEnumerator FadeScreen(float start, float end, float step = 0.002f)
    {
        PlayerCanvas.alpha = start;
        if (start > end)
        {
            while (PlayerCanvas.alpha > end)
            {
                PlayerCanvas.alpha -= step;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (PlayerCanvas.alpha < end)
            {
                PlayerCanvas.alpha += step;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator FadeScreenDesert(float start, float end)
    {
        PlayerCanvas.GetComponent<Image>().color = Color.white;

        if (SceneManager.GetActiveScene().name == "Desert")
        {
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(FadeScreen(start, end, 0.002f));
            yield return new WaitForSeconds(3);

            SceneManager.LoadScene("River");
        }
        else
        {
            yield return StartCoroutine(FadeScreen(start, end, 0.002f));
        }
    }

    public void ShowMenu()
    {
        if (!mainMenu.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            mainMenu.SetActive(true);
                
            Player.actions.Player.Move.Disable();
            Player.actions.Player.Jump.Disable();
            Player.actions.Player.Interact.Disable();
            
            Player.mainCam.mode = 2;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            mainMenu.SetActive(false);

            if (currentInteract == null)
            {
                Player.actions.Player.Move.Enable();
                Player.actions.Player.Jump.Enable();
                Player.mainCam.mode = 1;
            }
            Player.actions.Player.Interact.Enable();
            
            Time.timeScale = 1;
        }
    }
}
