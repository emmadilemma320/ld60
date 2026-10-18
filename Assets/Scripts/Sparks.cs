using UnityEngine;

public class sparks : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().take_damage("sparks");
        }
    }
}