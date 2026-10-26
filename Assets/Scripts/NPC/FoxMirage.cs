using UnityEngine;

public class FoxMirage : MonoBehaviour
{
    [SerializeField] float disappearDistance = 50f;

    [SerializeField] Material[] materials;

    Transform player;

    void Start()
    {
        player = GameController.Instance.Player.transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        float opacity = Mathf.InverseLerp(
            0f,
            disappearDistance,
            distance
        );

        foreach (Material material in materials)
        {
            Color newColor = material.color;
            newColor.a = opacity;
            material.SetColor("_BaseColor", newColor);
        }
    }
}