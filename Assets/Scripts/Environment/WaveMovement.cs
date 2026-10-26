using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private float endX = 300f;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= endX)
        {
            Destroy(gameObject);
        }
    }
}