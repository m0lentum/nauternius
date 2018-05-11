using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour {

    [SerializeField] private Vector2 animationRate = new Vector2(0, 0.07f);
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
