using System.IO;
using UnityEngine;

public class SpeakingPerson : MonoBehaviour, IInteractable
{
    public string dialogFileName;
    public DialogLineList dialog;

    public Transform[] cameraPoints;

    public void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, dialogFileName + ".json");
        dialog = JsonUtility.FromJson<DialogLineList>(File.ReadAllText(path));
    }

    public virtual void Interact()
    {
        DialogController.Instance.SetLines(dialog.list);
        DialogController.Instance.SetCameraPoints(cameraPoints);
        DialogController.Instance.StartDialog();
    }
}