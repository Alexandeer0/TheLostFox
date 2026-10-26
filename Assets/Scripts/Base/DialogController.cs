using UnityEngine;
using TMPro;

public class DialogController : MonoBehaviour, IInteractable
{
    public CameraController mainCam;

    [Header("UI")]
    [SerializeField] GameObject dialogPanel;
    [SerializeField] TMP_Text nameText, bodyText;

    [Header("Dialog Data")]
    [SerializeField] DialogLine[] lines;

    int currentIndex = 0;
    bool isActive = false;

    Transform[] cameraPoints;

    public static DialogController Instance;
    public System.Action OnDialogFinished;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void Interact()
    {
        if (!isActive)
        {
            StartDialog();
            return;
        }

        currentIndex++;

        if (currentIndex >= lines.Length)
        {
            EndDialog();
            return;
        }

        CameraSetUp();
        ShowCurrentLine();
    }

    public void StartDialog()
    {
        if (lines == null || lines.Length == 0)
            return;

        currentIndex = 0;
        GameController.Instance.currentInteract = this;

        isActive = true;
        dialogPanel.SetActive(true);

        mainCam.SetCameraMode(2);

        CameraSetUp();
        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        nameText.text = lines[currentIndex].characterName;
        bodyText.text = lines[currentIndex].text;
        
        bodyText.color = GetColorByName(lines[currentIndex].characterName);
    }

    void EndDialog()
    {
        GameController.Instance.currentInteract = null;

        mainCam.transform.SetParent(null);
        mainCam.SetCameraMode(1);
        
        isActive = false;
        dialogPanel.SetActive(false);

        OnDialogFinished?.Invoke();
    }

    public void SetLines(DialogLine[] newLines)
    {
        lines = newLines;
    }
    public void SetLines(string name, string text)
    {
        DialogLine tempLine = new DialogLine();
        tempLine.characterName = name;
        tempLine.text = text;

        lines = new DialogLine[] {tempLine};
    }

    public void SetCameraPoints(Transform[] points)
    {
        cameraPoints = points;
    }

    void CameraSetUp()
    {
        if (cameraPoints == null) return;
        
        if (currentIndex >= cameraPoints.Length) return;
        
        /*  Неактуально
        if (currentIndex == 0 || currentIndex == lines.Length - 1)
        {
            currentPoint = 0;
        }
        else if (cameraChangeCount == 0)
        {
            return;
        }
        else if (currentIndex % cameraChangeCount == 0)
        {
            currentPoint = currentIndex / cameraChangeCount;

            // Проверка на переполнение списка cameraPoints
            currentPoint -= currentPoint > cameraPoints.Length - 1 ? cameraPoints.Length : 0;
        }
        else return;
        */

        mainCam.transform.SetPositionAndRotation(cameraPoints[currentIndex].position, cameraPoints[currentIndex].rotation);
        mainCam.transform.SetParent(cameraPoints[currentIndex]);
    }

    public Color GetColorByName(string name)
    {
        switch (name)
        {
            case "Лис":
                return Color.white;
            case "Медведь":
                return Color.orange;
            case "Заяц":
                return Color.greenYellow;
            case "Олень":
                return Color.rosyBrown;
            case "Волк":
                return Color.blueViolet;
            case "Воробей":
                return Color.wheat;
            case "Лисёнок":
                return Color.gray7;
            default:
                return Color.red;
        }
    }
}


[System.Serializable]
public class DialogLine
{
    public string characterName;
    public string text;
}

[System.Serializable]
public class DialogLineList
{
    public DialogLine[] list;
}
