using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    [SerializeField] private Transform platformsParent;
    GameObject[] platforms;

    void Start()
    {
        platforms = GameObject.FindGameObjectsWithTag("CollapsingPlatform");
        Debug.Log("kuinka monta plattia: " + platforms.Length);
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        ResetPlatforms();
    }

    void ResetPlatforms()
    {
        foreach (GameObject platform in platforms)
        {
            CollapsingPlatform script = platform.transform.GetChild(0).GetComponent<CollapsingPlatform>();
            script.ResetPlatform();
        }
    }
}
