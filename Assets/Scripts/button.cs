using UnityEngine;

public class button : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider other){
        print("object entered");
        if (other.gameObject.tag == "Player"){
            print("player entered area");
        }
    }
}
