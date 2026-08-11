using System.Collections;
using UnityEngine;

public class FirefliesTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] fireflies;
    bool active = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            active = false;
            StartCoroutine(StartFireflies());
        }
    }

    IEnumerator StartFireflies()
    {
        foreach(GameObject firefly in fireflies)
        {
            firefly.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }
}
