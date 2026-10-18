using System;
using UnityEngine;

public class sparks : MonoBehaviour
{
    public Boolean on;

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player") && on)
        {
            other.gameObject.GetComponent<Player>().take_damage("sparks");
        }
    }

    public void Link() {
        // called when linked with a button
        // allows the sparks to do stuff in response 
        // (i use it to turn off timers in my fans&pistons)
    }

    public void toggle() {
        on = !on;
    }
}