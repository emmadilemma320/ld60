using UnityEngine;

public class fan : MonoBehaviour
{
    private Animator anim;
    public bool on;
    private bool button_controlled;
    
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
        if (button_controlled) return;
        timer -= Time.deltaTime;
        if(timer <= 0) {
            flip();
            timer = FAN_SWITCH_TIME;
        }
    }

    public void flip(){
        // This method turns it on if it is off, and off if it is on
        anim.enabled = !anim.enabled;
        on = !on;
    }

    public void link(){
        button_controlled = true;
    }


    void OnTriggerEnter(Collider other){
        // this will be the code that pushes the player up if the fan is on
        print("object entered");
        if (other.gameObject.tag == "Player"){
            if (on){
               print("player entered area"); 
            }
            
        }
    }
}
