using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class button : MonoBehaviour
{
    public List<Transform> linked_fans;
    UnityEvent button_push;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Transform fan in this.linked_fans){
                print("\t"+fan);
            fan.gameObject.GetComponent<fan>().Link();
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        
    }

    void OnCollisionEnter2D(Collision2D other){
        // when the button is pushed, it will send out a signal to all linked fans that they 
        if (other.gameObject.CompareTag("Player")){
            print("player entered area");
            foreach(Transform fan in this.linked_fans){
                print("flipped fan"+fan);
                fan.gameObject.GetComponent<fan>().Flip();
            }
        }
        
    }
}
