using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public int numberCollisions = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
            numberCollisions++;
    }

    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terrain"))
            numberCollisions--;
    }

    public void SetNullCollisions()
    {
        numberCollisions = 0;
    }
}
