using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class button : MonoBehaviour
{
    public List<Transform> linked_items;
    private List<fan> linked_fans;
    private List<piston> linked_pistons;
    UnityEvent button_push;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        linked_fans = new List<fan>();
        linked_pistons = new List<piston>();
        
        foreach(Transform item in this.linked_items){
                print("\t"+item);
            
            // if it is a fan, use the link function
            if(item.gameObject.TryGetComponent<fan>(out fan f)) {
                print("successfully connected fan "+f);
                linked_fans.Add(f);
                f.Link();
            } else if (item.gameObject.TryGetComponent<piston>(out piston p)) {
                print("successfully connected "+p+" to button "+name);
                linked_pistons.Add(p);
                p.Link();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        
    }

    void OnCollisionEnter2D(Collision2D other){
        // when the button is pushed, it will send out a signal to all linked fans that they 
        if (other.gameObject.CompareTag("Player")){
            print("player entered area");
            foreach(fan f in this.linked_fans){
                print("flipped fan"+f);
                f.Flip();
                //fan.gameObject.GetComponent<fan>().Flip();

            }
        }
        
    }
}
