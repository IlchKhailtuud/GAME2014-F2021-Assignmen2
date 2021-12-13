using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D col;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
    }

    public void OpenDoor()
    {
        anim.Play("door_open");
        col.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.gameWinPanel.SetActive(true);
        }
    }
}
