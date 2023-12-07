using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimator : MonoBehaviour
{
    public enum WingSide{
        Left,
        Right,
        Tail
    }


    public WingAnimator leftWing;
    public WingAnimator rightWing;
    public WingAnimator tail;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void SetWingAlpha(float alpha, WingSide wingSide){
        // Sets the target alpha for the given wing
        WingAnimator wing = GetWing(wingSide);
        wing.SetTargetAlpha(alpha);
    }


    private WingAnimator GetWing(WingSide wingSide){
        // Returns a reference to the specified wing or both wings
        switch (wingSide){
            case WingSide.Left:
                return leftWing;
            case WingSide.Right:
                return rightWing;
            case WingSide.Tail:
                return tail;
            default:
                return null; // ruh roh
        }
    }
}
