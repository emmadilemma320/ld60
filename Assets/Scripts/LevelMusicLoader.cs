using UnityEngine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;

public class LevelMusicLoader : MonoBehaviour
{

    string musicPath = "event:/Background Music";

    EventInstance music;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DelayedAudioLoad());
    }

    IEnumerator DelayedAudioLoad()
    {
        yield return new WaitForSeconds(1);

        music = RuntimeManager.CreateInstance(musicPath);

        music.start();

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
