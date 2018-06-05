using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoppuPisteet : MonoBehaviour {

    public static LoppuPisteet lopetusPisteet;
    public Text nykyPisteet;

    public void asetaPisteet(int pisteet)
    {
        Debug.Log("Päästiin LoppuPiste-luokkaan!");
        nykyPisteet.text = "Score: " + pisteet.ToString();
    }

    private void Awake()
    {
        nykyPisteet = GetComponent<Text>();
        lopetusPisteet = this;      
    }
}
