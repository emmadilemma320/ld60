using UnityEngine;

public class fan : MonoBehaviour
{
    public bool on;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        print("fan ready");
        if(!on) anim.StopPlayback();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void flip(){
        // This method turns it on if it is off, and off if it is on
        if (on){
            print("fan turned off");
            anim.Play("fan-test");
        } else {
            print("fan turned off");
            anim.StopPlayback();
        }
        on = !on;
    }


    void OnTriggerEnter(Collider other){
        print("object entered");
        if (other.gameObject.tag == "Player"){
            print("player entered area");
        }
    }
}
