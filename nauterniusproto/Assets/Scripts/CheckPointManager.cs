using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {

    public List<CheckPoint> checkPoints;
    public CheckPoint CurCheckPoint { get { return checkPoints[curIndex]; } }
    public int curIndex;

    // Use this for initialization
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
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnCheckPointTrigger(CheckPoint newCp)
    {
        curIndex = checkPoints.IndexOf(newCp);
    }
}
