using UnityEngine;

public class fan : MonoBehaviour
{
    private Animator anim;
    public bool on;
    
    // For the fan on/off
    private const float FAN_SWITCH_TIME = 3f;
    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        if(!on){
            anim.enabled = false;
        }

        timer = FAN_SWITCH_TIME;
        
    }

    // Update is called once per frame
    void Update(){
        timer -= Time.deltaTime;
        if(timer <= 0) {
            flip();
            timer = FAN_SWITCH_TIME;
        }
    }

    void flip(){
        // This method turns it on if it is off, and off if it is on
        anim.enabled = !anim.enabled;
        on = !on;
    }


    void OnTriggerEnter(Collider other){
        print("object entered");
        if (other.gameObject.tag == "Player"){
            print("player entered area");
        }
    }
}
