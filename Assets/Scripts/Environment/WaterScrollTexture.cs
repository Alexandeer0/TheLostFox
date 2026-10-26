using UnityEngine;

public class WaterScrollTexture : MonoBehaviour
{
    public float speed = 0.5f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * speed;

        // Применяем смещение к текстуре материала
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
