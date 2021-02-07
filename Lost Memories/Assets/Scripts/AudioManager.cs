using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource introSource, layer1Source, layer2Source, layer3Source, layer4Source, finaleSource;

    public AudioClip ExplorationIntro;
    public AudioClip ExplorationLoop;
    public AudioClip ExplorationLoopReminiscence;
    public AudioClip ExplorationLoopDiscovery;
    public AudioClip ExplorationLoopDrums;
    public AudioClip Finale;

    private float TempoBPM = 72;

    // State

    [Range(0, 1)]
    public float Discovery = 0;

    [Range(0, 1)]
    public float Reminiscence = 0;

    [Range(0, 1)]
    public float Movement = 0;

    string currentCue = "";

    public static AudioManager instance { get; private set; }

    Dictionary<AudioSource, IEnumerator> runningFades;

    // Start is called before the first frame update
    void Start()
    {
        if(!instance || !GameObject.FindWithTag("AudioManager"))
        {
            // init/replace
            instance = this;

            Init();
            StartCue("Intro", 0);

            GameObject.DontDestroyOnLoad(gameObject); // make persistent
        }
        else
        {
            Debug.LogError("Only one AudioManager allowed!");
            GameObject.Destroy(gameObject);
        }
    }

    void Init()
    {
        runningFades = new Dictionary<AudioSource, IEnumerator>();

        // Intro
        introSource = gameObject.AddComponent<AudioSource>();

        // Exploration
        layer1Source = gameObject.AddComponent<AudioSource>();
        layer2Source = gameObject.AddComponent<AudioSource>();
        layer3Source = gameObject.AddComponent<AudioSource>();
        layer4Source = gameObject.AddComponent<AudioSource>();
        
        // Finale
        finaleSource = gameObject.AddComponent<AudioSource>();

        // Settings
        introSource.playOnAwake = false;
        introSource.loop = true;

        layer1Source.playOnAwake = false;
        layer1Source.loop = true;

        layer2Source.playOnAwake = false;
        layer2Source.loop = true;
        
        layer3Source.playOnAwake = false;
        layer3Source.loop = true;
        
        layer4Source.playOnAwake = false;
        layer4Source.loop = true;

        finaleSource.playOnAwake = false;
        finaleSource.loop = false;
    }


    // Update is called once per frame
    void Update()
    {
        switch(currentCue)
        {
            case "Intro":
                // static - TODO trigger on next bar after intro scene exit?
                // if(layer1Source.time > GetBarDuration() * 5) { // 4 when we can crossfade
                //     StartCue("Exploration");
                // }
                break;

            case "Exploration":
                layer2Source.volume = Discovery;
                layer3Source.volume = Reminiscence;
                layer4Source.volume = Movement;
                
                 // DEBUG
                if(Input.GetKeyDown(KeyCode.Alpha1)) {
                    TriggerParameter("Discovery");
                }
                if(Input.GetKeyDown(KeyCode.Alpha2)) {
                    TriggerParameter("Reminiscence", GetTimeToNextDownbeat(2));
                }
                if(Input.GetKeyDown(KeyCode.Alpha3)) {
                    TriggerParameter("Movement");
                }

                break;

            case "Finale":
                // TODO?
                break;
        }

        // General Debug
        if(Input.GetKeyDown(KeyCode.F11)) {
            StartCue("Exploration");
        }
    }

    /// wait: -1 = default (next downbeat), 0 = instant, more = seconds)
    public void StartCue(string name, float wait = -1)
    {
        if(wait == -1) {
            wait = GetTimeToNextDownbeat();
        }

        StartCoroutine(DoStartCue(name, wait));
    }


    IEnumerator DoStartCue(string name, float wait)
    {
        yield return new WaitForSeconds(wait);

        Debug.Log("StartCue: " + name);

        // TODO wait for next bar
        // TODO start with intro, keep track of time
        switch(name) {
            case "Intro":
                introSource.clip = ExplorationIntro;
                introSource.volume = 1;
                introSource.Play();//Scheduled(AudioSettings.dspTime + 1);

                layer1Source.Stop();
                layer2Source.Stop();
                layer3Source.Stop();
                layer4Source.Stop();

                finaleSource.Stop();
                break;

            case "Exploration":
                SetValueOverTime("Intro", 0, GetBarDuration(), true);

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

                if(finaleSource.isPlaying) {
                    SetValueOverTime("Finale", 0, GetBarDuration(), true);
                }
                break;

            case "Finale":
                // HACK shouldn't happen during gameplay, but might in editor
                if(introSource.isPlaying) {
                    SetValueOverTime("Intro", 0, GetBarDuration(), true);
                }
                
                // fade out others
                SetValueOverTime("BaseLoop", 0, GetBarDuration(), true);
                SetValueOverTime("Discovery", 0, GetBarDuration(), true);
                SetValueOverTime("Reminiscence", 0, GetBarDuration(), true);
                SetValueOverTime("Movement", 0, GetBarDuration(), true);
                
                finaleSource.clip = Finale;
                finaleSource.Play();
                break;
        }

        currentCue = name;
    }

    /// wait: -1 = default (next downbeat), 0 = instant, more = seconds)
    /// fadeInTime: -1 default = until next downbeat, 0 = none, more = seconds
    /// fadeOutTime: -1 default = 4 bars, 0 = none (keep at value), more = seconds
    public void TriggerParameter(string parameter, float wait = -1, float fadeInTime = -1, float fadeOutTime = -1)
    {
        Debug.Log("TriggerParameter: "+parameter);

        // if(wait == 0) {
        //     DoTriggerParameter(parameter, wait, fadeInTime, fadeOutTime);
        // } else {
            if(wait == -1) {
                wait = GetTimeToNextDownbeat();
            }

            StartCoroutine(DoTriggerParameter(parameter, wait, fadeInTime, fadeOutTime));
        // }
    }

    private IEnumerator DoTriggerParameter(string parameter, float wait = -1, float fadeInTime = -1, float fadeOutTime = -1)
    {
        Debug.Log("DoTriggerParameter: wait for " + wait + " seconds...");

        yield return new WaitForSeconds(wait);

        Debug.Log("DoTriggerParameter: Done waiting.");

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

    public void SetValueOverTime(string property, float value, float duration, bool stopAtEnd = false)
    {
        AudioSource source = null;
        switch(property) {
            case "Intro":
                source = introSource;
                break;
            case "BaseLoop":
                source = layer1Source;
                break;
            case "Discovery":
                source = layer2Source;
                break;
            case "Reminiscence":
                source = layer3Source;
                break;
            case "Movement":
                source = layer4Source;
                break;
            case "Finale":
                source = finaleSource;
                break;
        }

        if(source != null) {
            // Kill previous coroutines for this source
            IEnumerator previousFade;
            if(runningFades.TryGetValue(source, out previousFade)) {
                StopCoroutine(previousFade);
                runningFades.Remove(source);
            }

            IEnumerator newFade = DoSetValueOverTime(source, value, duration, stopAtEnd);
            runningFades.Add(source, newFade);
            StartCoroutine(newFade);
        }
    }

    IEnumerator DoSetValueOverTime(AudioSource source, float value, float duration, bool stopAtEnd = false)
    {
        float startTime = Time.time;
        float startVolume = source.volume;
        float leftDuration = duration;

        Debug.Log("DoSetValueOverTime: source: " + source.name + ", value: " + value + ", duration: " + duration);
        
        while(leftDuration > 0) {
            source.volume = Mathf.Lerp(startVolume, value, (1 - leftDuration/duration));
            UpdateParametersFromSource();

            // Debug.Log("duration: " + duration);
            // Debug.Log("leftDuration: " + leftDuration);
            // Debug.Log("source.volume: " + source.volume);

            leftDuration -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        source.volume = value; // hard set target at end
        UpdateParametersFromSource();

        if(value == 0 && stopAtEnd) {
            source.Stop();
        }
    }

    void UpdateParametersFromSource()
    {
        if(currentCue == "Exploration") {
            Discovery = layer2Source.volume;
            Reminiscence = layer3Source.volume;
            Movement = layer4Source.volume;
        }
    }

    // Timing functions publicly accessible to trigger gameplay reactions

    public float GetTimeToNextDownbeat(float minBeats = 0, bool waitForNextBar = false)
    {
        float barDuration = GetBarDuration();
        float currentTimeInBar = (layer1Source ? layer1Source.time : 0) % barDuration;
        float timeToNextDownbeat = barDuration - currentTimeInBar;

        // for smooth slopes, enforce minimum duration
        while(timeToNextDownbeat < GetBeatDuration() * minBeats) {
            if(waitForNextBar) {
                timeToNextDownbeat += barDuration;
            } else {
                timeToNextDownbeat += GetBeatDuration();
            }
        }

        return timeToNextDownbeat;
    }

    public float GetBarDuration()
    {
        float beatDuration = GetBeatDuration();
        float barDuration = beatDuration * 4; // HACK 4/4 bars
        
        return barDuration;
    }

    public float GetBeatDuration()
    {
        return 60 / TempoBPM;
    }
}