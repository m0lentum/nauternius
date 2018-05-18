using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CheckIfFree : MonoBehaviour {

    public bool InUse { get; set; }
    private VoiceLineTrigger lastCallingScript;

    public void StartText(VoiceLineTrigger vlt)
    {
        if (InUse) lastCallingScript.StopAllCoroutines();
        lastCallingScript = vlt;
        InUse = true;
    }

    public void StopText()
    {
        InUse = false;
    }
}
