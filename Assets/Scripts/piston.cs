using System;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;

public class piston : MonoBehaviour {

    public float piston_rest_time = 3f;
    //public float speed = 0.1f;
    public Boolean immeuble;

    private Boolean button_controlled;
    private float timer;
    private Boolean moving;
    private Boolean up; // true if the piston is currently extended (default)
    
    public const float speed = 0.01f;
    private const float middle_min = 0f;
    private const float middle_max = 1f;
    private const float platform_min = 0.4f;
    private const float platform_max = 1f;
    

    private Transform middle;
    private Transform platform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // variable set-up
        timer = piston_rest_time;
        moving = false; up = true;

        middle = this.transform.GetChild(0);
        platform = middle.GetChild(0);

    }

   private float new_y;
    void FixedUpdate(){
        if(immeuble) return;
        if (!(moving || button_controlled)) timer -= Time.fixedDeltaTime;
        
        if (timer <= 0 && !button_controlled && !moving) {
            timer = piston_rest_time;
            moving = true;
        }

        if (moving) {
            if(up){    
                if(middle.localPosition.y > middle_min) {
                    // if the middle part is not in position yet, we keep moving it
                    // if the new value is now *less* than the min, we snap it to the min
                    new_y = Math.Max(middle.localPosition.y - speed, middle_min);
                    middle.localPosition = new Vector3(middle.localPosition.x, new_y, middle.localPosition.z);
                } else {
                    // if the middle and platform are both in place, then the piston is done moving up/down
                    if (platform.localPosition.y <= platform_min){
                        // we turn 'moving' to off and switch the 'up' variable
                        up = false;
                        moving = false;
                    } else { // otherwise we move the platform
                        new_y = Math.Max(platform.localPosition.y - speed, platform_min);
                        platform.localPosition = new Vector3(platform.localPosition.x, new_y, platform.localPosition.z);
                    }
                } 
            } else {
                if (middle.localPosition.y < middle_max) {
                    // if it is now *over* the max, we snap it to the max
                    new_y = Math.Min(middle.localPosition.y + speed, middle_max);
                    middle.localPosition = new Vector3(middle.localPosition.x, new_y, middle.localPosition.z);
                } else {
                    if (platform.localPosition.y >= platform_max) {
                        up = true;
                        moving = false;
                    } else {
                        new_y = Math.Min(platform.localPosition.y + speed, platform_max);
                        platform.localPosition = new Vector3(platform.localPosition.x, new_y, platform.localPosition.z);
                    }
                }
            }
        }
    }

    public void Link() {
        button_controlled = true;
        immeuble = false;
    }

    public void button_pushed() {
        if (!moving) {
            moving = true;
        }
    }
}
