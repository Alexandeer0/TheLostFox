using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour, IInteractable
{
    Camera targetCamera;
    CutscenePoint[] points;

    int currentPointIndex = -1;
    Coroutine transitionCoroutine;

    [SerializeField] string ActionOnEnd;
    CanvasGroup canvas;

    bool isPlaying, isActive;

    void Start()
    {
        targetCamera = Camera.main;
        points = GetComponentsInChildren<CutscenePoint>();

        canvas = GameController.Instance.PlayerCanvas;
        isActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")  && isActive)
        {
            isActive = false;
            StartCutscene();
        }
    }

    public void StartCutscene()
    {
        if (points.Length == 0)
            return;

        isPlaying = true;
        targetCamera.GetComponent<CameraController>().SetCameraMode(2);

        GameController.Instance.currentInteract = this;

        GoToNextPoint();
    }

    public void Interact()
    {
        if (!isPlaying)
            return;

        GoToNextPoint();
    }

    private void GoToNextPoint()
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        if (++currentPointIndex >= points.Length)
        {
            EndCutscene();
            return;
        }

        CutscenePoint point = points[currentPointIndex];

        if (point.transitionType == CameraTransitionType.Cut)
        {
            targetCamera.transform.SetPositionAndRotation(
                point.transform.position,
                point.transform.rotation);

            transitionCoroutine = StartCoroutine(WaitForNextPoint(point.duration));
        }
        else
        {
            transitionCoroutine = StartCoroutine(SmoothMove(point));
        }
    }

    IEnumerator SmoothMove(CutscenePoint point)
    {
        Vector3 startPos = targetCamera.transform.position;
        Quaternion startRot = targetCamera.transform.rotation;

        float timer = 0f;

        while (timer < point.duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.SmoothStep(0f, 1f, timer / point.duration);

            targetCamera.transform.position =
                Vector3.Lerp(startPos, point.transform.position, t);

            targetCamera.transform.rotation =
                Quaternion.Slerp(startRot, point.transform.rotation, t);

            yield return null;
        }

        targetCamera.transform.SetPositionAndRotation(
            point.transform.position,
            point.transform.rotation);

        /*transitionCoroutine =
            StartCoroutine(WaitForNextPoint(point.duration));*/
        GoToNextPoint();
    }

    private IEnumerator WaitForNextPoint(float duration)
    {
        yield return new WaitForSeconds(duration);

        GoToNextPoint();
    }

    private void EndCutscene()
    {
        isPlaying = false;
        GameController.Instance.currentInteract = null;

        targetCamera.GetComponent<CameraController>().SetCameraMode(1);

        if (ActionOnEnd?.Split()[0] == "Scene")
        {
            StartCoroutine(FadeSceneLoad(ActionOnEnd.Split()[1]));
        }
        else if (ActionOnEnd?.Split()[0] == "Event")
        {
            GameController.Instance.sceneEvent.Invoke();
        }

        gameObject.SetActive(false);
    }

    IEnumerator FadeSceneLoad(string sceneName)
    {
        while (canvas.alpha < 1)
        {
            canvas.alpha += 0.01f;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene(sceneName);
    }
}
