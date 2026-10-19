using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class fan : MonoBehaviour
{
    private Animator anim;
    public bool on;
    private bool button_controlled;
    
    // For the fan on/off
    public float fan_time = 3f;
    private float timer;

    [Header("FMOD stuff ugh:")]
    private EventInstance fanSound;
    private string fanEventPath = "event:/Fan";
    [SerializeField] float fanMaxVolume = 1f;

    //Camera camera;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        if (!on){
            anim.enabled = false;
        }
        timer = fan_time;

        InitializeFanSound();

        //camera = FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (button_controlled) return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            Flip();
            timer = fan_time;
        }
    }

    public void Flip(){

        //Debug.Log("flipping");

        // This method turns it on if it is off, and off if it is on
        anim.enabled = !anim.enabled;
        on = !on;

        if (on)
        {
            fanSound.setVolume(1f);
            //Debug.Log("Unmuting Fan!");
        }
        else if (!on)
        {
            fanSound.setVolume(0f); // MUTE
            //Debug.Log("Muting Fan!");

        }

    }

    void InitializeFanSound()
    {


        fanSound = RuntimeManager.CreateInstance(fanEventPath);

        fanSound.setVolume(0f); 

        RuntimeManager.AttachInstanceToGameObject(fanSound, gameObject);

        Debug.Log(transform.position);

        Debug.Log(fanSound);

        fanSound.start(); 

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