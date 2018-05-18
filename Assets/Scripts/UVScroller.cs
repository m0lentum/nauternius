using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour {

    public Vector2 animationRate = new Vector2(0, 0.015f);
    private Vector2 totalOffset;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

	void Update ()
    {
		if (rend.enabled)
        {
            totalOffset += animationRate;
            rend.materials[0].mainTextureOffset = totalOffset;
        }
	}
}
