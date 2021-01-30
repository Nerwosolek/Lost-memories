using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, Interactable
{
    [SerializeField]
    private string[] _scanningTexts;
    [SerializeField]
    private string[] _remeberedTexts;
    [SerializeField]
    private string[] _nearbyTexts;
    public bool ReadyToInteract()
    {
        return true;
    }

    public string[] Remember()
    {
        return _remeberedTexts;
    }

    public string[] Scan()
    {
        return _scanningTexts;
    }
}
