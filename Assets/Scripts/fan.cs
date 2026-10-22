using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
    using System.Collections;
using Unity.Hierarchy;

public class fan : MonoBehaviour
{
    private Animator anim;
    public bool on;
    private bool button_controlled;
    
    // For the fan on/off
    public float fan_time = 3f;
    public float timer;

    // Particles
    [SerializeField] ParticleSystem particles;    

   [Header("FMOD stuff ugh:")]
    private EventInstance fanSound;
    private string fanEventPath = "event:/Fan";
    [SerializeField] float fanMaxVolume = 1f;
    public bool withinRange;
    float audioRange = 3f;
    // audio instances outside of range will be disabled, and enabled when within range.
    // Must be larger than the attenuation distance on FMOD Studio
    bool audioLoaded = false;
    Camera camera;


    


        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){

        camera = FindAnyObjectByType<Camera>();

        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        if (!on){
            anim.enabled = false;
        }
        //timer = fan_time;

        //StartCoroutine(InitializeFanSound());

        particles.Play();

        UpdateParticles();

    }

    // Update is called once per frame
    void FixedUpdate(){

        CheckAudioDistance();

        if (!audioLoaded)
        {
            LoadAudio();
        }

        ManageFanSfx();

        if (button_controlled) return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            Debug.Log("Timer Triggered");
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

            PLAYBACK_STATE state;
            fanSound.getPlaybackState(out state);

            if (state != PLAYBACK_STATE.PLAYING)
            {
                fanSound.start();
            }

            //fanSound.setVolume(1f);
            //Debug.Log("Unmuting Fan!");

        }
        else
        {
            PLAYBACK_STATE state;
            fanSound.getPlaybackState(out state);

            if (state != PLAYBACK_STATE.STOPPED)
            {
                fanSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            
            //fanSound.setVolume(0f); // MUTE
            //Debug.Log("Muting Fan!");

        }

        UpdateParticles();
    }

    void UpdateParticles()
    {
        if (on)
        {
            particles.Play();
        }
        else if (!on)
        {
            particles.Stop();
        }
    }


    //IEnumerator InitializeFanSound()
    //{

    //    yield return new WaitUntil(() => RuntimeManager.HaveAllBanksLoaded);

    //    fanSound = RuntimeManager.CreateInstance(fanEventPath);

    //    fanSound.setVolume(1f);

    //    RuntimeManager.AttachInstanceToGameObject(fanSound, gameObject);

    //    Debug.Log(transform.position);

    //    fanSound.start();

    //}

    void CheckAudioDistance()
    {

        float distance = Vector2.Distance(transform.position, camera.transform.position);
        if (distance < audioRange)
        // if inside range
        {
            withinRange = true;
        }
        else
        {
            withinRange = false;
        }

    }

    void LoadAudio()
    // only loads when in range (to put less stress on command buffer)
    {
        if (withinRange && RuntimeManager.HaveAllBanksLoaded)
        {
            fanSound = RuntimeManager.CreateInstance(fanEventPath);

            RuntimeManager.AttachInstanceToGameObject(fanSound, gameObject);

            fanSound.start();

            audioLoaded = true;

            Debug.Log("Fan Loaded. Audio range is :" + Vector2.Distance(transform.position, camera.transform.position));
        }
    }

    void ManageFanSfx()
    {
        //Turning fan on and off
        if (on && withinRange)
        {
            //fanSound.setVolume(1f);
            //Debug.Log("off");

            PLAYBACK_STATE state;
            fanSound.getPlaybackState(out state);
            if (state == PLAYBACK_STATE.STOPPED)
            // if fanSound is not playing (stopped)
            {
                fanSound.start();
                // turn on
                //Debug.Log("PLAING");
            }
        }
        else
        {
            //fanSound.setVolume(0f);
            //Debug.Log("on");
            PLAYBACK_STATE state;
            fanSound.getPlaybackState(out state);
            if (state == PLAYBACK_STATE.PLAYING)
            // if fanSound is playing
            {
                fanSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                // turn off
                //Debug.Log("tuRNING OFF");

            }
        }
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