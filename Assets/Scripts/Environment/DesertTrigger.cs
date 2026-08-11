using System.Collections;
using UnityEngine;

public class DesertTrigger : MonoBehaviour
{
    bool active = true;
    CameraEffects playerCamera;

    void Start()
    {
        playerCamera = DialogController.Instance.mainCam.GetComponent<CameraEffects>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            active = false;
            GameController.Instance.AddProgress(1);

            switch (GameController.Instance.progression)
            {
                case 1:
                    break;
                case 2:
                    StartCoroutine(DesertEffects(0.5f, 0));
                    break;
                case 4:
                    StartCoroutine(DesertEffects(0.75f, 0.1f));
                    break;
                case > 4 and < 7:
                    StartCoroutine(DesertEffects(1, 0.3f));
                    break;
                case 7:
                    StartCoroutine(DesertEffects(1, 0, 3));
                    break;
            }
        }
    }

    IEnumerator DesertEffects(float destination, float end, float time = 1f)
    {
        while (playerCamera.effectStrength < destination)
        {
            playerCamera.effectStrength += 0.005f;
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForSeconds(time);

        while (playerCamera.effectStrength > end)
        {
            playerCamera.effectStrength -= 0.002f;
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }
}
