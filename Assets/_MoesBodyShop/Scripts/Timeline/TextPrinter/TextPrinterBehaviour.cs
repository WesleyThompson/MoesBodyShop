using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Graveyard;

[Serializable]
public class TextPrinterBehaviour : PlayableBehaviour
{
    public string TextToPrint;
    public string NameOfSpeaker;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
