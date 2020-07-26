using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Graveyard;

[Serializable]
public class TextPrinterClip : PlayableAsset, ITimelineClipAsset
{
    public TextPrinterBehaviour template = new TextPrinterBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextPrinterBehaviour>.Create (graph, template);
        TextPrinterBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
