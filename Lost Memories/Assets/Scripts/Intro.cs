using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void InitText()
    {
        Interaction interaction = gameObject.GetComponent<Interaction>();
        if(interaction) {
            uIManager.CacheText(interaction.Scan());
        }
    }

    public void ShowText(string text = null)
    {
        if(uIManager) {
            // uIManager.StartInteraction();
            uIManager.ShowDialogueBox();
            uIManager.SetText(null, false);
        }
    }

    public void ExitIntro()
    {
        uIManager.ShowDialogueBox(false);
        AudioManager.instance.StartCue("Exploration");
        SceneManager.LoadScene("Scene01");
    }
}
