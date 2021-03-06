﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(EchoCaster))]
[RequireComponent (typeof(PixelCollider))]
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance;
    private PixelCollider pixCollider;
    private EchoCaster echo;
    private BoxCollider2D hitBox;
    
    private Text debugText;

    public static PlayerControl Instance { get { return instance; } }
    public EchoCaster Echolot { get { return echo; } }

    public Animator anim;
    public float speed = 0.3f;
    
	// Use this for initialization
	void Awake ()
    {
        instance = this;

        debugText = GameObject.FindGameObjectWithTag("DEBUG_UI").GetComponent<Text>();
        
        transform.position = PixelPerfect.Align(transform.position);

        pixCollider = GetComponent<PixelCollider>();
        echo = GetComponent<EchoCaster>();
        hitBox = GetComponent<BoxCollider2D>();
        hitBox.isTrigger = true;

        if(anim == null) {
            Debug.Log("Bat Animator not assigned on Player");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // ---- PIXEL PERFECT MOVEMENT ----
        // get axes
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");

        //  XXX REMOVE IN RELEASE XXX
        debugText.text = "PRESS 'R' TO USE ECHOLOT \nHeadingVec: " + xSpeed + ", " + ySpeed;

        pixCollider.UpdateMovement(xSpeed, ySpeed, speed);

        if(anim != null)
        {
            if (xSpeed > 0)
                anim.SetTrigger("RIGHT");
            else if (xSpeed < 0)
                anim.SetTrigger("LEFT");
            else if (ySpeed < 0)
                anim.SetTrigger("UP");
            else if(ySpeed > 0)
                anim.SetTrigger("DOWN");
        }

        // CAST ECHO
        if(Input.GetButtonUp("Echolot"))
        {
            echo.CastEcho();
        }
    }

    
}
