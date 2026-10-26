using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LostFoxScript : SpeakingPerson
{
    [Header("Movement Points")]
    [SerializeField] Transform walk1;
    [SerializeField] Transform jump1;
    [SerializeField] Transform swim1;
    [SerializeField] Transform swim2;
    [SerializeField] Transform swim3;
    [SerializeField] Transform stay1;
    [SerializeField] Transform stay2;
    [SerializeField] Transform jump2;
    [SerializeField] Transform swim4;
    [SerializeField] Transform swim5;
    [SerializeField] Transform swim6;
    [SerializeField] Transform swim7;
    [SerializeField] Transform swim8;
    [SerializeField] Transform stay3;
    [SerializeField] Transform walk2;

    [Header("Wave")]
    [SerializeField] WaveSpawner waveSpawner;

    [Header("Wave Reaction")]
    [SerializeField] float wavePushDistance = 1.2f;
    [SerializeField] float wavePushDuration = 0.8f;
    [SerializeField] float waveRotation = 35f;
    [SerializeField] float waveRotationDuration = 0.6f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpDuration = 1f;

    [Header("End")]
    [SerializeField] CanvasGroup playerCanvas;

    Animator animator;

    bool sequenceStarted;

    [SerializeField] AudioClip finalSong;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        base.Interact();

        DialogController.Instance.OnDialogFinished += OnDialogEnd;
    }

    void OnDialogEnd()
    {
        if (sequenceStarted)
            return;

        sequenceStarted = true;

        DialogController.Instance.OnDialogFinished -= OnDialogEnd;

        StartCoroutine(FinalSequence());
    }

    IEnumerator FinalSequence()
    {
        waveSpawner.automaticSpawn = false;

        // --------------------------------------------------
        // 1. Подходим к реке, сразу спавним первую волну
        // за 8 секунд до столкновения
        // --------------------------------------------------

        yield return new WaitForSeconds(2f);

        GameController.Instance.audioSource.clip = finalSong;
        GameController.Instance.audioSource.Play();
        
        DialogController.Instance.mainCam.GetComponent<FoxCameraFollow>().StartFollowing();

        waveSpawner.SpawnWave();

        yield return MoveTo(walk1, 2f);

        yield return new WaitForSeconds(2f);


        // --------------------------------------------------
        // 2. Прыгаем в воду
        // --------------------------------------------------

        yield return JumpTo(jump1);


        // --------------------------------------------------
        // 3. Немного плывём, спавним вторую волну
        // --------------------------------------------------

        waveSpawner.SpawnWave();

        StartCoroutine(RotateTo(swim1.rotation, 0.2f));

        yield return MoveTo(swim1, 3f);


        // --------------------------------------------------
        // 4. Первая волна
        // --------------------------------------------------

        yield return MoveTo(swim2, 0.5f);


        // --------------------------------------------------
        // 5. Подплываем к камню
        // --------------------------------------------------

        yield return MoveTo(swim3, 3.5f);


        // --------------------------------------------------
        // 6. Хватаемся за камень, спавним третью волну
        // --------------------------------------------------

        StartCoroutine(RotateTo(stay1.rotation, 0.2f));

        yield return MoveTo(stay1, 0.5f);

        waveSpawner.SpawnWave();
        
        yield return new WaitForSeconds(1f);


        // --------------------------------------------------
        // 7. Вскарабкиваемся на камень
        // --------------------------------------------------

        StartCoroutine(RotateTo(stay2.rotation, 0.5f));

        yield return MoveTo(stay2, 1f);

        yield return new WaitForSeconds(2f);


        // --------------------------------------------------
        // 8. Прыжок с камня, спавним последнюю волну
        // --------------------------------------------------

        waveSpawner.SpawnWave();

        yield return JumpTo(jump2);


        // --------------------------------------------------
        // 9. Плывём дальше
        // --------------------------------------------------

        StartCoroutine(RotateTo(swim1.rotation, 0.2f));

        yield return MoveTo(swim4, 3f);


        // --------------------------------------------------
        // 10. Вторая волна
        // --------------------------------------------------

        yield return MoveTo(swim5, 0.5f);


        // --------------------------------------------------
        // 11. Продолжаем плыть
        // --------------------------------------------------

        yield return MoveTo(swim6, 3.5f);


        // --------------------------------------------------
        // 12. Последняя, драматическая волна
        // --------------------------------------------------

        DialogController.Instance.mainCam.GetComponent<FoxCameraFollow>().StopFollowing();

        yield return MoveTo(swim7, 0.5f);
        
        yield return new WaitForSeconds(7f);


        // --------------------------------------------------
        // 13. Всплываем и выходим на берег
        // --------------------------------------------------

        DialogController.Instance.mainCam.GetComponent<FoxCameraFollow>().StartFollowing();

        yield return MoveTo(swim8, 2f);

        yield return new WaitForSeconds(2f);

        waveSpawner.automaticSpawn = true;


        // --------------------------------------------------
        // 14. Оборачиваемся
        // --------------------------------------------------

        yield return RotateTo(stay3.rotation, 1f);

        yield return new WaitForSeconds(2f);

        yield return RotateTo(walk2.rotation, 1f);


        // --------------------------------------------------
        // 15. Уходим
        // --------------------------------------------------

        yield return MoveTo(walk2, 3f);

        yield return EndScreen();
    }


    // ======================================================
    // MOVEMENT
    // ======================================================

    IEnumerator MoveTo(Transform point, float duration)
    {
        Vector3 start = transform.position;
        Vector3 end = point.position;

        float distance = Vector3.Distance(start, end);

        if (distance < 0.01f)
        {
            transform.position = end;
            yield break;
        }

        float time = 0f;

        SetWalking();

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);

            transform.position = Vector3.Lerp(start, end, t);

            yield return null;
        }

        transform.position = end;

        SetIdle();
    }


    IEnumerator RotateTo(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);

            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                t
            );

            yield return null;
        }

        transform.rotation = targetRotation;
    }


    // ======================================================
    // JUMP
    // ======================================================

    IEnumerator JumpTo(Transform point)
    {
        SetJump();

        Vector3 start = transform.position;
        Vector3 end = point.position;

        float time = 0f;

        while (time < jumpDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / jumpDuration);

            Vector3 position = Vector3.Lerp(start, end, t);

            // Парабола для прыжка
            position.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

            transform.position = position;

            yield return null;
        }

        transform.position = end;
    }

    // ======================================================
    // ANIMATION
    // ======================================================

    void SetWalking()
    {
        if (animator == null)
            return;

        animator.SetBool("Walking", true);
    }


    void SetIdle()
    {
        if (animator == null)
            return;

        animator.SetBool("Walking", false);
    }


    void SetJump()
    {
        if (animator == null)
            return;

        animator.SetBool("Walking", false);
        animator.Play("Jump");
    }


    // ======================================================
    // END
    // ======================================================

    IEnumerator EndScreen()
    {
        playerCanvas.GetComponent<Image>().color = Color.black;
        playerCanvas.alpha = 1f;

        yield return new WaitForSeconds(3f);

        playerCanvas.transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene("MainMenu");
    }
}