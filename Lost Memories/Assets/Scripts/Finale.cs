using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finale : MonoBehaviour
{
    float startTime;
    UIManager uIManager;

    bool started = false;

    int step = 0;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = gameObject.GetComponent<UIManager>();
        AudioManager.instance.StartCue("Finale", 0);

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - startTime;

        if(!started) {
            Interaction interaction = gameObject.GetComponent<Interaction>();
            if(interaction) {
                uIManager.CacheText(interaction.Scan());
            //     uIManager.StartInteraction();
            }
            

            started = true;

        } else if(timeElapsed < 60) {
            if(Mathf.Floor(timeElapsed / 4) > step) {
                step++;
                ShowText();
            }
        } else {
            // Done
            uIManager.ShowDialogueBox(false);
            // TODO Choice
            if(Input.anyKeyDown) {
                GameObject.DestroyImmediate(AudioManager.instance.gameObject);
                SceneManager.LoadScene("Title");
            }
        }
    }

    private void ShowText(string text = null)
    {
        if(uIManager) {
            // uIManager.StartInteraction();
            uIManager.ShowDialogueBox();
            uIManager.SetText(null, false);
        }
    }
}
