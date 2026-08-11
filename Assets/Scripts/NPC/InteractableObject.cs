using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactIcone;
    bool alreadySpoke = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadySpoke)
        {
            interactIcone.SetActive(true);
            GameController.Instance.readyToInteract = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !alreadySpoke)
        {
            interactIcone.SetActive(false);
            GameController.Instance.readyToInteract = null;
        }
    }

    public void Interact()
    {
        alreadySpoke = true;
        interactIcone.SetActive(false);
        GetComponent<SpeakingPerson>().Interact();
    }
}
