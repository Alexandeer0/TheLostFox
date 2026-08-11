using UnityEngine;

public class SpawnLabirinth : MonoBehaviour
{
    [SerializeField] string direction;
    [SerializeField] GameObject labirinth, labirinthEnd;
    Vector3 newPos;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance.progression == 4)
            {
                labirinth = labirinthEnd; //В нужный момент спавним не очередной лабиринт, а его конечную часть
            }

            switch (direction)
            {
                case "Left":
                    newPos = new Vector3(transform.position.x + 172, 5, transform.position.z + 160);
                    Instantiate(labirinth, newPos, transform.rotation);
                    break;
                case "Right":
                    newPos = new Vector3(transform.position.x + 173, 5, transform.position.z + 110);
                    Instantiate(labirinth, newPos, transform.rotation);
                    break;
                case "Forward":
                    newPos = new Vector3(transform.position.x + 100, 5, transform.position.z + 100);
                    Instantiate(labirinth, newPos, transform.rotation);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
