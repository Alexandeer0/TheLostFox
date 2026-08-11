using UnityEngine;

public enum CameraTransitionType
{
    Cut,
    Smooth
}



public class CutscenePoint : MonoBehaviour
{
    [Min(0.1f)]
    public float duration = 2f;

    public CameraTransitionType transitionType =
        CameraTransitionType.Smooth;
}