using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class CheckPointManager : MonoBehaviour {

    public List<CheckPoint> checkPoints; //
    private int curIndex;

    public CheckPoint CurCheckPoint { get { return checkPoints[curIndex]; } }

    void Start ()
    {
        curIndex = 0;

        foreach (Transform child in transform)
        {
            CheckPoint checkPoint = child.gameObject.GetComponent<CheckPoint>();

            checkPoint.onTrigger += OnCheckPointTrigger;
            checkPoints.Add(checkPoint);
        }
    }
	
	void Update ()
    {
		
	}

    void OnCheckPointTrigger(CheckPoint newCp)
    {
        curIndex = checkPoints.IndexOf(newCp);
    }
}
