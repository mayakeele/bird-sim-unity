using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestGUI : MonoBehaviour
{
    public TMP_Text airspeedText;
    //public Text airspeedText;

    public BirdController birdController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshAirspeed(birdController.rb.velocity.magnitude);
    }


    private void RefreshAirspeed(float airspeed) {
        airspeedText.text = "Airspeed: " + airspeed.ToString("F1") + " m/s";
    }
}
