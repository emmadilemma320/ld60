using System;
using System.Collections.Generic;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;

public class button : MonoBehaviour
{
    public List<Transform> linked_items;
    private List<fan> linked_fans;
    private List<piston> linked_pistons;
    private List<sparks> linked_sparks;
    private Player player;
    UnityEvent button_push;

    [SerializeField] Sprite ButtonUp;
    [SerializeField] Sprite ButtonDown;
    SpriteRenderer spriteRenderer;
    bool is_big_red_button = false;


    [Header("Audio Stuff")]
    EventReference switchOn;
    EventReference switchOff;
    bool pressed = false;

    string switchOnPath = "event:/Button Down";
    string switchOffPath = "event:/Button Up";

    public bool withinRange;
    float audioRange = 16f;
    // audio instances outside of range will be disabled, and enabled when within range.
    // Must be larger than the attenuation distance on FMOD Studio
    Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        linked_fans = new List<fan>();
        linked_pistons = new List<piston>();
        linked_sparks = new List<sparks>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach(Transform item in this.linked_items){
            if(item.gameObject.TryGetComponent<fan>(out fan f)) {
                //print("successfully connected fan "+f);
                linked_fans.Add(f);
                f.Link();
            } else if (item.gameObject.TryGetComponent<piston>(out piston p)) {
                //print("successfully connected "+p+" to button "+name);
                linked_pistons.Add(p);
                p.Link();
            } else if(item.gameObject.TryGetComponent<sparks>(out sparks s)) {
                //print("successfully connected "+p+" to button "+name);
                linked_sparks.Add(s);
                s.Link();
            } else if(item.gameObject.TryGetComponent<Player>(out Player pl)) {
                player = pl;
                is_big_red_button = true;
            }
        }

        camera = FindAnyObjectByType<Camera>();

        StartCoroutine(DelayedAudioLoad());

    }

    IEnumerator DelayedAudioLoad()
    {
        bool loaded;
        yield return new WaitUntil(() => loaded = RuntimeManager.HaveAllBanksLoaded);

        switchOn = RuntimeManager.PathToEventReference(switchOnPath);
        switchOff = RuntimeManager.PathToEventReference(switchOffPath);
    }
    void CheckAudioDistance()
    {
        // duplicate code from sparks. Should probably make it one method eventually? Idk!

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


    void FixedUpdate()
    {
        CheckAudioDistance();
    }

    void OnCollisionEnter2D(Collision2D other){
        // when the button is pushed, it will send out a signal to all linked fans that they 
        if (other.gameObject.CompareTag("Player")){
            //print("player entered area");

            pressed = !pressed;

            if (pressed)
            {
                RuntimeManager.PlayOneShot(switchOn);
                spriteRenderer.sprite = ButtonDown;
            }
            else
            {
                RuntimeManager.PlayOneShot(switchOff);
                spriteRenderer.sprite = ButtonUp;

            }

            foreach (fan f in this.linked_fans)
                {
                    //print("flipped fan"+f);
                    f.Flip();
                }
            foreach(piston p in this.linked_pistons) {
                //print("turned on piston "+p);
                p.move();
            }
            foreach(sparks s in this.linked_sparks) {
                s.toggle();
            }
            if(is_big_red_button) player.Win();
        }
        
    }
}
