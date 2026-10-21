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
        bool loaded;
        yield return new WaitUntil(() => loaded = RuntimeManager.HaveAllBanksLoaded);

        music = RuntimeManager.CreateInstance(musicPath);

        music.start();

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
