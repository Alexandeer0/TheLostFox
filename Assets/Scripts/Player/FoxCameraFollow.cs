using UnityEngine;

public class FoxCameraFollow : MonoBehaviour
{
    [SerializeField] float followSpeed = 3f;
    [SerializeField] float lookHeight = 1f;

    CameraController cameraController;
    Transform lostFox;

    bool isFollowing = false;

    void Start()
    {
        cameraController = GetComponent<CameraController>();
    }

    void Update()
    {
        if (!isFollowing || lostFox == null)
            return;

        FollowFox();
    }

    void FollowFox()
    {
        Vector3 targetPosition = lostFox.position;
        targetPosition.y += lookHeight;

        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            followSpeed * Time.deltaTime
        );
    }

    public void StartFollowing()
    {
        lostFox = GameController.Instance.lostFox.transform;

        isFollowing = true;
        
        GetComponent<CameraController>().SetCameraMode(4);
    }

    public void StopFollowing()
    {
        isFollowing = false;

        GetComponent<CameraController>().SetCameraMode(1);
    }
}