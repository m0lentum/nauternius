using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorController : MonoBehaviour {

    [SerializeField] private ChoiceGateController[] gates;
    private Animator anim;
    private bool isSolved
    {
        get { return anim.GetBool("isSolved"); }
        set { anim.SetBool("isSolved", value); }
    }

    private AudioSource audioSrc;
    [SerializeField] private AudioClip soundOnCorrect;
    [SerializeField] private AudioClip soundOnWrong;

    void Start()
    {
        ResetGates();
        anim = GetComponentInParent<Animator>();
        isSolved = false;
        audioSrc = GetComponentInParent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") CheckSolution();
    }


    public void CheckSolution()
    {
        if (!isSolved)
        {
            foreach (ChoiceGateController gate in gates)
            {
                // jos ei ole menty kaikista porteista läpi, ei tehdä mitään
                if (!gate.isActivated)
                {
                    return;
                }

                if (!gate.isCorrect)
                {
                    ResetGates();
                    anim.SetTrigger("checkAnswer");
                    audioSrc.PlayOneShot(soundOnWrong);
                    return;
                }
            }
        }

        SetSolved();
    }

    public void ResetGates()
    {
        foreach (ChoiceGateController gate in gates)
        {
            gate.Reset();
        }

        gates[0].Open();
    }

    public void SetSolved()
    {
        Debug.Log("Puzzle solved!");

        isSolved = true;
        foreach (ChoiceGateController gate in gates)
        {
            gate.Reset();
        }

        anim.SetTrigger("checkAnswer");

        audioSrc.PlayOneShot(soundOnCorrect);
    }
}
