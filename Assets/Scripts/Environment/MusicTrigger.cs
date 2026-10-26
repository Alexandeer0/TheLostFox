using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip song;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        GameController.Instance.audioSource.clip = song;
        GameController.Instance.audioSource.Play();

        Destroy(gameObject);
    }
}
