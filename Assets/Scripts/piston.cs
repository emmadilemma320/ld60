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
    private float up; // = 1 if the piston is currently extended (default), -1 if it is down
    
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
        moving = false; up = 1;

        middle = this.transform.GetChild(0);
        platform = middle.GetChild(0);

    }

   private float new_y;
    void FixedUpdate(){
        if(immeuble) return;
        if (!(moving || button_controlled)) timer -= Time.fixedDeltaTime;
        
        if (timer <= 0 && !button_controlled) {
            timer = piston_rest_time;
            moving = true;
        }

        if (moving) {
            if(up == 1){    
                if(middle.position.y > middle_min) {
                    // if the middle part is not in position yet, we keep moving it
                    // if the new value is now *less* than the min, we snap it to the min
                    middle.localPosition = new Vector3(0,0,0);
                    new_y = Math.Max(middle.position.y - speed*this.transform.localScale.y, (this.transform.position.y+middle_min)*this.transform.localScale.y);
                    middle.position = new Vector3(middle.position.x, new_y, middle.position.z);
                } else {
                    // if the middle and platform are both in place, then the piston is done moving up/down
                    if (platform.position.y <= platform_min * this.transform.localScale.y){
                        // we turn 'moving' to off and switch the 'up' variable
                        up = -1;
                        moving = false;
                    } else { // otherwise we move the platform
                        new_y = Math.Max(platform.position.y - speed*this.transform.localScale.y, platform_min*this.transform.localScale.y);
                        platform.position = new Vector3(platform.position.x, new_y, platform.position.z);
                    }
                } 
            } else {
                if (middle.position.y < middle_max) {
                    // if it is now *over* the max, we snap it to the max
                    new_y = Math.Min(middle.position.y + speed*this.transform.localScale.y, middle_max*this.transform.localScale.y);
                    middle.position = new Vector3(middle.position.x, new_y, middle.position.z);
                } else {
                    if (platform.position.y >= platform_max*this.transform.localScale.y) {
                        up = -1;
                        moving = false;
                    } else {
                        new_y = Math.Min(platform.position.y - speed*this.transform.localScale.y, platform_max*this.transform.localScale.y);
                        platform.position = new Vector3(platform.position.x, new_y, platform.position.z);
                    }
                }
            }
            print(name+" new positions:\n\tmiddle: "+middle.position);
            print("\tplatform: "+platform.position);
        }
    }

    void Link() {
        button_controlled = true;
        immeuble = false;
    }
}
