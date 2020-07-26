using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Graveyard;

[TrackColor(0f, 0.5f, 1f)]
[TrackClipType(typeof(TextPrinterClip))]
[TrackBindingType(typeof(TextPrinter))]
public class TextPrinterTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TextPrinterMixerBehaviour>.Create (graph, inputCount);
    }
}
