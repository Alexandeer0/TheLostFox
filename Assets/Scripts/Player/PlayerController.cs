using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public InputSystem_Actions actions;

    public CameraController mainCam;
    [SerializeField] PlayerGroundCheck groundCheck;

    Animator animator;
    AudioSource stepsSound;
    
    public float speed, rotationSpeed, jumpForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        actions = new InputSystem_Actions();
        stepsSound = GetComponent<AudioSource>();

        actions.Player.Jump.performed += Jump;
        actions.Player.Interact.performed += Interact;
        actions.Player.Escape.performed += Escape;
    }

    void OnEnable()
    {
        actions.Enable();
    }
    void OnDisable()
    {
        actions.Disable();
    }

    void FixedUpdate()
    {
        Vector2 movement = actions.Player.Move.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {
            animator.SetBool("Walking", true);
            Move(movement);

            if (!stepsSound.isPlaying)
            {
                stepsSound.loop = true;
                stepsSound.Play();
            }
        }
        else
        {
            animator.SetBool("Walking", false);
            stepsSound.loop = false;
        }

        if (transform.position.y < -1)
        {
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        }
    }

    void Move(Vector2 movement)
    {
        Vector3 direction = mainCam.transform.right * movement.x + mainCam.transform.forward * movement.y ;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (groundCheck.numberCollisions > 0)
            {
                animator.Play("Jump");
                
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (GameController.Instance.currentInteract != null)
        {
            GameController.Instance.currentInteract.Interact();
        }
        else if (GameController.Instance.readyToInteract != null)
        {
            GameController.Instance.currentInteract = GameController.Instance.readyToInteract;
            GameController.Instance.readyToInteract = null;
            GameController.Instance.currentInteract.Interact();
        }
    }

    void Escape(InputAction.CallbackContext context)
    {
        GameController.Instance.ShowMenu();
    }
}
