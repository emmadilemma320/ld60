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
            fan.gameObject.GetComponent<fan>().link();
        }
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnTriggerEnter(Collider other){
        // when the button is pushed, it will send out a signal to all linked fans that they 
        print("object entered");
        if (other.gameObject.tag == "Player"){
            print("player entered area");
            foreach(Transform fan in this.linked_fans){
                fan.gameObject.GetComponent<fan>().flip();
            }
        }
        
    }
}
