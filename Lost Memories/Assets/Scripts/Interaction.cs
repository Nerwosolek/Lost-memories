using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, Interactable
{
    [SerializeField]
    private string[] _scanningTexts;
    [SerializeField]
    private string[] _rememberedTexts;
    [SerializeField]
    private string[] _nearbyTexts;
    [SerializeField]
    private string _correctText;
    [SerializeField]
    private string _wrongAnswerText;
    public bool AlreadyGuessed { get; set; }
    public bool AlreadySeen { get; set; }
    public string CorrectText { get => _correctText;  }
    public string[] Nearby()
    {
        return _nearbyTexts;
    }

    public string[] Remember()
    {
        return _rememberedTexts;
    }

    public string[] Scan()
    {
        return _scanningTexts;
    }
}
