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

        // DEBUG
        if(Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.LeftControl)) {
            TriggerParameter("Discovery");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKeyDown(KeyCode.LeftControl)) {
            TriggerParameter("Reminiscence");
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKeyDown(KeyCode.LeftControl)) {
            TriggerParameter("Movement");
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

    /// wait: -1 = default (next downbeat), 0 = instant, more = seconds)
    /// fadeInTime: -1 default = until next downbeat, 0 = none, more = seconds
    /// fadeOutTime: -1 default = 4 bars, 0 = none (keep at value), more = seconds
    void TriggerParameter(string parameter, float wait = -1, float fadeInTime = -1, float fadeOutTime = -1)
    {
        if(wait == 0) {
            DoTriggerParameter(parameter, wait, fadeInTime, fadeOutTime);
        } else {
            if(wait == -1) {
                wait = GetTimeToNextDownbeat();
            }

            StartCoroutine(DoTriggerParameter(parameter, wait, fadeInTime, fadeOutTime));
        }
    }

    private IEnumerator DoTriggerParameter(string parameter, float wait = -1, float fadeInTime = -1, float fadeOutTime = -1)
    {
        yield return new WaitForSeconds(wait);

        if(fadeInTime == -1) {
            fadeInTime = GetTimeToNextDownbeat();
        }

        SetValueOverTime(parameter, 1, fadeInTime);

        if(fadeOutTime == -1) {
            fadeOutTime = GetBarDuration() * 4;
        }

        yield return new WaitForSeconds(fadeOutTime);

        SetValueOverTime(parameter, 0, fadeOutTime);
    }

    void SetValueOverTime(string property, float value, float duration)
    {
        AudioSource source = null;
        switch(property) {
            case "Discovery":
                source = layer2Source;
                break;
            case "Reminescence":
                source = layer3Source;
                break;
            case "Movement":
                source = layer4Source;
                break;
        }

        if(source != null) {
            StartCoroutine(DoSetValueOverTime(source, value, duration));
        }
    }

    IEnumerator DoSetValueOverTime(AudioSource source, float value, float duration)
    {
        float leftDuration = duration;
        
        while(leftDuration > 0) {
            float volumeDelta = (value - source.volume) * (Time.fixedDeltaTime / duration);
            source.volume += volumeDelta;

            leftDuration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        source.volume = value; // hard set target at end
    }

    float GetTimeToNextDownbeat()
    {
        float barDuration = GetBarDuration();
        float currentTimeInBar = layer1Source.time % barDuration;

        return barDuration - currentTimeInBar;
    }

    private float GetBarDuration() {
        float beatDuration = 60 / TempoBPM;
        float barDuration = beatDuration * 4; // HACK 4/4 bars
        
        return barDuration;
    }
}