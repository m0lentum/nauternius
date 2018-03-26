using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class BallSocketManager : MonoBehaviour {

    [SerializeField] private Transform ballSocketParent;
    [SerializeField] private Transform ballsParent;
    [SerializeField] private GameObject targetObj;

    // todo private
    public List<BallSocket> ballSockets;
    public List<Ball> balls;

	void Start ()
    {
        foreach (Transform tr in ballSocketParent)
        {
            BallSocket ballSocket = tr.GetChild(0).GetComponent<BallSocket>();
            ballSocket.OnSocketActivated += SocketActivated;
            ballSockets.Add(ballSocket);
        }

        foreach (Transform tr in ballsParent)
        {
            Ball ball = tr.GetComponent<Ball>();
            balls.Add(ball);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Reset();
        }
    }

    //Aina kun socket aktivoidaan, tarkistaa onko kaikki socketit aktivoitu 
    void SocketActivated()
    {
        Debug.Log("managerista socket activated kuultu");
        foreach (BallSocket script in ballSockets)
        {
            if (!script.isActive) return;
        }

        TriggerTargetObject();
    }

    void TriggerTargetObject()
    {
        targetObj.GetComponent<BallTriggerObject>().BallTrigger();
    }

    //Resettaa kaikki pallot ja socketit
    void Reset()
    {
        foreach (BallSocket script in ballSockets)
        {
            script.ResetSocket();
        }

        foreach (Ball ball in balls)
        {
            ball.ResetBall();
        }
    }
}
