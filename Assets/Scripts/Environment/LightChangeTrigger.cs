using System.Collections;
using UnityEngine;

public class LightChangeTrigger : MonoBehaviour
{
    [SerializeField] float additionalProgress, duration;
    bool active = true;

    static int countTriggers = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            active = false;
            StartCoroutine(AddNightProgress());
        }
    }

    IEnumerator AddNightProgress()
    {
        if (++countTriggers > 4)
        {
            GameController.Instance.AddProgress(1);
        }
        
        float iterCount = 10 * duration;
        float iteration = additionalProgress / iterCount;

        for (int i = 0; i < iterCount; i++)
        {
            SetDayNight.Instance.nightProgress += iteration;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
