using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------
// Copyright © Janne Isoaho, Aarne Manneri, Mikael Myyrä, Lauri Niskanen, Saska Sinkkonen
//---------------------------------------------------------------------------------------

public class FlashCanvas : MonoBehaviour {

    private CanvasGroup canvasGroup;
    private Image panelImg;
    private float fadeTime;
    [SerializeField] private float fadeSteps;
    
    void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        panelImg = transform.GetChild(0).GetComponent<Image>();
	}

    public void Flash(Color color, float fTime)
    {
        fadeTime = fTime;
        panelImg.color = color;
        StartCoroutine(Flash());
    }

    public IEnumerator Flash()
    {
        //panelImg.color = color;
        for (int i = 0; i < fadeSteps; i++)
        {
            canvasGroup.alpha += 1/fadeSteps;
            //if (i == 9) yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(fadeTime/fadeSteps);
        }
        for (int i = 0; i < fadeSteps; i++)
        {
            canvasGroup.alpha -= 1/fadeSteps;
            yield return new WaitForSeconds(fadeTime/fadeSteps);
        }
    }
}
