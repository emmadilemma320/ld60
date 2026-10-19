using System;
using UnityEngine;

public class sparks : MonoBehaviour
{
    public Boolean on = true;

    public float spark_rest_time = 2f;
    public Boolean on_timer;
    private float timer;

    void Start() {
        timer = spark_rest_time;
    }

    void FixedUpdate() {
        if (!on_timer) return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            toggle();
            timer = spark_rest_time;
        }
    }

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
        on_timer = true;
    }

    public void toggle() {
        on = !on;
    }
}