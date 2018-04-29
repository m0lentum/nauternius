using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class Ball : MonoBehaviour {
    
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }
}
