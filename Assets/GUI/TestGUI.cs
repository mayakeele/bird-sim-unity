using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestGUI : MonoBehaviour
{
    public TMP_Text airspeedText;
    public TMP_Text LDText;

    public BirdController birdController;

    float airspeed;
    float LD;

    public int updateFrame = 10;
    int counter = 0;


    void Update()
    {
        counter++;

        if (counter >= updateFrame) {
            RefreshGUI();
            counter = 0;
        }
    }


    private void RefreshGUI() {
        airspeedText.text = "Airspeed: " + airspeed.ToString("F1") + " m/s";
        LDText.text = "L/D: " + LD.ToString("F1");
    }


    public void SetAirspeed(float airspeed) {
        this.airspeed = airspeed;
    }
    public void SetLD(float LD) {
        this.LD = LD;
    }
}
