using System;
using System.Collections;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class sparks : MonoBehaviour
{
    public Boolean on = true;
    private Animator anim;
    private SpriteRenderer sprite;

    public float spark_rest_time = 2f;
    public Boolean on_timer;
    private float timer;

    [Header("SOUND")]
    string buzzpath = "event:/BuzzLoop";
    EventInstance buzz;
    public bool withinRange;
    [SerializeField] float audioRange = 20f;
    // audio instances outside of range will be disabled, and enabled when within range.
    // Must be larger than the attenuation distance on FMOD Studio

    Camera camera;

    void Start() {
        //print(name+" starting");
        timer = spark_rest_time;
        anim = gameObject.transform.GetComponent<Animator>();
        sprite = gameObject.transform.GetComponent<SpriteRenderer>();

        if (!on){
            anim.enabled = false;
            sprite.enabled = false;
        }

        camera = FindAnyObjectByType<Camera>();

        StartCoroutine(DelayedAudioLoadIn());

    }

    IEnumerator DelayedAudioLoadIn()
        // it was playing audio before it kinda realized where things were.
        // so Just a little delay
    {        
        yield return new WaitForSeconds(1);

        buzz = RuntimeManager.CreateInstance(buzzpath);

        RuntimeManager.AttachInstanceToGameObject(buzz, gameObject);

        buzz.start();
    }

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
            withinRange= false;
        }


    }

    void FixedUpdate() {

        CheckAudioDistance();

        ToggleBuzzSfx();

        if (!on_timer) return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            toggle();
            timer = spark_rest_time;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player") && on)
        {
            other.gameObject.GetComponent<Player>().take_damage("sparks");
        }
    }

    public void Link() {
        // called when linked with a button
        // allows the sparks to do stuff in response 
        // (i use it to turn off timers in my fans&pistons)
        on_timer = false;
    }

    [ContextMenu("Toggle")]
    public void toggle() {
        on = !on;
        anim.enabled = !anim.enabled;
        sprite.enabled = !sprite.enabled;

        //ToggleBuzzSfx();
        // NOTE TO TEAM:
        // For safety, ToggleBuzzSfx is in FixedUpdate(), but
        // if toggle becomes the only way they could possibly
        // switch states you can call it here instead
    }

    void ToggleBuzzSfx()
    {
        //Turning buzz on and off
        if (on && withinRange)
        {
            //buzz.setVolume(1f);
            Debug.Log("off");

            PLAYBACK_STATE state;
            buzz.getPlaybackState(out state);
            if (state == PLAYBACK_STATE.STOPPED)
            // if buzz is not playing (stopped)
            {
                buzz.start();
                // turn on
                Debug.Log("PLAING");
            }
        }
        else
        {
            //buzz.setVolume(0f);
            Debug.Log("on");
            PLAYBACK_STATE state;
            buzz.getPlaybackState(out state);
            if (state == PLAYBACK_STATE.PLAYING)
            // if buzz is playing
            {
                buzz.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                // turn off
                Debug.Log("tuRNING OFF");

            }
        }
    }

}