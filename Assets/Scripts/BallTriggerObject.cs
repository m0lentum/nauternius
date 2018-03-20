using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class BallTriggerObject : MonoBehaviour {
    
    public void BallTrigger()
    {
        transform.position = transform.position - new Vector3(0,200, 0);
    }
}
