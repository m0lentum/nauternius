using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour {

    Canvas exitMenu;


    private void Start()
    {
        exitMenu = gameObject.GetComponent<Canvas>();
    }
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) exitMenu.enabled = !exitMenu.enabled;
        else if (Input.GetButtonDown("Menu")) exitMenu.enabled = !exitMenu.enabled;
    }

    public void DisableCanvas()
    {
        exitMenu.enabled = false;
    }
}
