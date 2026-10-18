using System;
using UnityEngine;

public class piston : MonoBehaviour {

    private const float PISTON_REST_TIME = 3f;
    private const float PISTON_SPEED = 5f;
    private Boolean button_controlled;
    private float timer;
    private Boolean moving;
    public Boolean immeuble;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        timer = PISTON_REST_TIME;
        moving = false;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(immeuble) return;
        if (!(moving || button_controlled)) timer -= Time.fixedDeltaTime;
        
        if (timer <= 0 && !button_controlled) {
            timer = PISTON_REST_TIME;
            moving = true;
        }

        if (moving) {
            print(name+" moving");
            // do stuff
        }
    }

    void Link() {
        button_controlled = true;
        immeuble = false;
    }
}
