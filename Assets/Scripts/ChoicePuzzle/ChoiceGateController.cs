using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceGateController : MonoBehaviour {

    public bool isClosed
    {
        get { return anim.GetBool("isClosed"); }
        private set { anim.SetBool("isClosed", value); }
    }
    private bool _isActivated;
    public bool isActivated // onko portista ajettu läpi
    {
        get { return _isActivated; }
        private set { _isActivated = value; }
    }
    private bool _isCorrect; // onko vastaus oikein
    public bool isCorrect
    {
        get { return _isCorrect; }
        private set { _isCorrect = value; }
    }

    [SerializeField] private ChoiceGateController nextGate;
    private Animator anim;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    public void Answer(bool value)
    {
        if (!isActivated && !isClosed)
        {
            Debug.Log("Answer given: " + value);

            isActivated = true;
            isCorrect = value;
            Close();
        }
    }

    public void Close()
    {
        if (nextGate) nextGate.Open();
        isClosed = true;
    }

    public void Open()
    {
        isClosed = false;
    }

    public void Reset()
    {
        if (!anim) anim = GetComponentInChildren<Animator>();
        isClosed = true;
        isActivated = false;
        isCorrect = false;
    }

    public bool GetIsCorrect()
    {
        return isActivated && isCorrect;
    }
}
