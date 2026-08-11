using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target; // Ссылка на игрока
    [SerializeField] Vector3 offset; // Смещение камеры

    [SerializeField] float sensitivity = 0.5f; // Чувствительность мыши
    Vector2 mouseMove, savedMouseMove, smoothMove, mouseVelocity;

    public int mode = 1;

    public float velocity;
    PlayerController Player;

    void Start()
    {
        Player = GameController.Instance.Player;
        
        // Скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;

        mouseMove.x = transform.rotation.eulerAngles.y;
        mouseMove.y = transform.rotation.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (!target) return;
        
        // Получаем ввод мыши
        mouseMove.x += Player.actions.Player.Look.ReadValue<Vector2>().x * sensitivity;
        mouseMove.y -= Player.actions.Player.Look.ReadValue<Vector2>().y * sensitivity;
        
        // Ограничение взгляда вверх/вниз
        mouseMove.y = Mathf.Clamp(mouseMove.y, -20f, 70f);

        switch (mode)
        {
            case 1:
                // Вращение и смещение
                transform.localRotation = Quaternion.Euler(mouseMove.y, mouseMove.x, 0);
                transform.position = target.position + transform.rotation * offset;
                return;
            
            case 2:
                /*
                // Ограничение взгляда вправо/влево
                mouseMove.x = Mathf.Clamp(mouseMove.x, -10f, 10f);
                // Ограничение взгляда вверх/вниз
                mouseMove.y = Mathf.Clamp(mouseMove.y, -10f, 10f);
                // Вращение
                transform.localRotation = Quaternion.Euler(mouseMove.y, mouseMove.x, 0);
                */
                return;

            case 3:
                smoothMove.x = Mathf.SmoothDamp(smoothMove.x, mouseMove.x, ref mouseVelocity.x, velocity);
                smoothMove.y = Mathf.SmoothDamp(smoothMove.y, mouseMove.y, ref mouseVelocity.y, velocity);

                transform.localRotation = Quaternion.Euler(smoothMove.y, smoothMove.x, 0);
                transform.position = target.position + transform.rotation * offset;
                return;
        }
    }

    public void SetCameraMode(int value)
    {
        if (value == 1)
        {
            Player.actions.Player.Move.Enable();
            Player.actions.Player.Jump.Enable();
            Player.actions.Player.Look.Enable();

            if (mode == 2) mouseMove = savedMouseMove;
        }
        else if (value == 2)
        {
            Player.actions.Player.Move.Disable();
            Player.actions.Player.Jump.Disable();
            Player.actions.Player.Look.Disable();

            savedMouseMove = mouseMove;
        }
        mode = value;
    }
}
