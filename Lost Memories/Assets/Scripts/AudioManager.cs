using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource layer1Source, layer2Source, layer3Source, layer4Source;

    public AudioClip ExplorationIntro;
    public AudioClip ExplorationLoop;
    public AudioClip ExplorationLoopReminiscence;
    public AudioClip ExplorationLoopDiscovery;
    public AudioClip ExplorationLoopDrums;

    private float TempoBPM = 72;

    // State

    [Range(0, 1)]
    public float Discovery = 0;

    [Range(0, 1)]
    public float Reminiscence = 0;

    [Range(0, 1)]
    public float Movement = 0;

    string currentCue = "";

    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCue("Exploration");
    }

    void Init()
    {
        layer1Source = gameObject.AddComponent<AudioSource>();
        layer2Source = gameObject.AddComponent<AudioSource>();
        layer3Source = gameObject.AddComponent<AudioSource>();
        layer4Source = gameObject.AddComponent<AudioSource>();

        layer1Source.playOnAwake = false;
        layer1Source.loop = true;

        layer2Source.playOnAwake = false;
        layer2Source.loop = true;
        
        layer3Source.playOnAwake = false;
        layer3Source.loop = true;
        
        layer4Source.playOnAwake = false;
        layer4Source.loop = true;
    }


    // Update is called once per frame
    void Update()
    {
        switch(currentCue)
        {
            case "Exploration":
                layer2Source.volume = Discovery;
                layer3Source.volume = Reminiscence;
                layer4Source.volume = Movement;
                break;
        }
    }

    void StartCue(string name)
    {
        // TODO wait for next bar
        // TODO start with intro, keep track of time
        switch(name) {
            case "Exploration":
                Debug.Log("StartCue: Exploration");

                layer1Source.clip = ExplorationLoop;
                layer1Source.volume = 1;
                layer1Source.Play();//Scheduled(AudioSettings.dspTime + 1);

                layer2Source.clip = ExplorationLoopDiscovery;
                layer2Source.volume = Discovery;
                layer2Source.Play();//Scheduled(AudioSettings.dspTime + 1);

                layer3Source.clip = ExplorationLoopReminiscence;
                layer3Source.volume = Reminiscence;
                layer3Source.Play();//Scheduled(AudioSettings.dspTime + 1);

                layer4Source.clip = ExplorationLoopDrums;
                layer4Source.volume = Movement;
                layer4Source.Play();//Scheduled(AudioSettings.dspTime + 1);

                currentCue = "Exploration";
                break;
        }
    }
}