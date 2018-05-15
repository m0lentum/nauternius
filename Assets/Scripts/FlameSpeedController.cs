using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpeedController : MonoBehaviour {

    [SerializeField] private float minSpeed = 0.01f;
    [SerializeField] private float maxSpeed = 0.04f;
    [SerializeField] private bool useHorizontalAxis;

    private UVScroller scroller;

    void Start()
    {
        scroller = GetComponent<UVScroller>();
    }

    void Update()
    {
        float input = useHorizontalAxis ? Input.GetAxis("Steer") : Input.GetAxis("Forward") - Input.GetAxis("Reverse");
        scroller.animationRate.y = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Abs(input));
    }
}
