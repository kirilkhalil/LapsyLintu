using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

    public Text huippuPisteet;

    void OnEnable()
    {
        huippuPisteet = GetComponent<Text>();
        huippuPisteet.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
    }
}
