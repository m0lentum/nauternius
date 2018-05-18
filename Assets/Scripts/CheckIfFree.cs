using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CheckIfFree : MonoBehaviour {

    public bool inUse;
    public VoiceLineTrigger lastCallingScript;

    public void StartText(VoiceLineTrigger vlt)
    {
        if (inUse) lastCallingScript.StopAllCoroutines();
        lastCallingScript = vlt;
        inUse = true;
    }

    public void StopText()
    {
        inUse = false;
    }
}
