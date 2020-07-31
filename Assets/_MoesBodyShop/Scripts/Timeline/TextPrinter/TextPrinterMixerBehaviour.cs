using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Graveyard;

public class TextPrinterMixerBehaviour : PlayableBehaviour
{
    private TextPrinter _trackBinding;
    private int _lastPlayed = -1;

    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        _trackBinding = playerData as TextPrinter;

        if (!_trackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TextPrinterBehaviour> inputPlayable = (ScriptPlayable<TextPrinterBehaviour>)playable.GetInput(i);
            TextPrinterBehaviour input = inputPlayable.GetBehaviour ();

            // Use the above variables to process each frame of this playable.
            if(inputWeight > 0 && i != _lastPlayed)
            {
                _trackBinding.SetText(input.TextToPrint, input.NameOfSpeaker);
                _lastPlayed = i;
            }
        }
    }
}
