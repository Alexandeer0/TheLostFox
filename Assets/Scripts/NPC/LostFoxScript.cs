using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LostFoxScript : SpeakingPerson
{
    [SerializeField] Transform targetPoint;
    [SerializeField] CanvasGroup playerCanvas;

    public override void Interact()
    {
        base.Interact();
        DialogController.Instance.OnDialogFinished += OnDialogEnd;
    }

    void OnDialogEnd()
    {
        StartCoroutine(SwimAway());
    }

    IEnumerator SwimAway()
    {
        yield return new WaitForSeconds(1);

        float duration = 10f;
        float t = 0;

        Vector3 start = transform.position;
        Vector3 end = targetPoint.position;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }

        yield return StartCoroutine(EndScreen());
    }

    IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(1);
        playerCanvas.GetComponent<Image>().color = Color.black;
        playerCanvas.alpha = 1;

        yield return new WaitForSeconds(1);
        playerCanvas.transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(3);
        
        SceneManager.LoadScene("MainMenu");
    }
}
