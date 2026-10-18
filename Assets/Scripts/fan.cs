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
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        if (!on){
            anim.enabled = false;
        }
        timer = FAN_SWITCH_TIME;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (button_controlled) return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            Flip();
            timer = FAN_SWITCH_TIME;
        }
    }

    public void Flip(){
        // This method turns it on if it is off, and off if it is on
        anim.enabled = !anim.enabled;
        on = !on;
    }

    public void Link(){
        button_controlled = true;
    }


    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Player"))
        {
            if (on){
                other.gameObject.GetComponent<Player>().fan = true;
            }
        }
    }
    void OnCollisionStay2D(Collision2D other){
        if (other.gameObject.CompareTag("Player"))
        {
            if (on){
                other.gameObject.GetComponent<Player>().fan = true;
            }
        }
    }
    void OnCollisionExit2D(Collision2D other){
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().fan = false;
        }
    }
}