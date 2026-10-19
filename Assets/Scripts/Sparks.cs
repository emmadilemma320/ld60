using System;
using System.Collections;
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


    void Start() {
        //print(name+" starting");
        timer = spark_rest_time;
        anim = gameObject.transform.GetComponent<Animator>();
        sprite = gameObject.transform.GetComponent<SpriteRenderer>();

        if (!on){
            anim.enabled = false;
            sprite.enabled = false;
        }

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

    void FixedUpdate() {

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
        if (on)
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
        else if (!on)
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