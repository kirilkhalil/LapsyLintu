using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PisteHallinta : MonoBehaviour {

    public static PisteHallinta pistehallinta; //Voidaan kutsua tämän luokan metodeja muualta.

    public Text NykyPisteetTeksti;
    int pisteet;
    int huippuPisteet;

    public void pisteenAlustus() //Kun peli käynnistetään asetetaan pistesarake 0:ksi.
    {
        pisteet = 0;
        NykyPisteetTeksti = GetComponent<Text>();
        NykyPisteetTeksti.text = pisteet.ToString();
    }

    public void pisteenLisays() //Tätä kutsutaan, kun osutaan pisteTriggeriin.
    {
        pisteet++;
        NykyPisteetTeksti = GetComponent<Text>();
        NykyPisteetTeksti.text = pisteet.ToString();
    }

    public int getPisteet()
    {
        return pisteet;
    }

    public void highScoreVertaus()
    {
        LoppuPisteet.lopetusPisteet.asetaPisteet(pisteet);
        huippuPisteet = PlayerPrefs.GetInt("HighScore"); //Haetaan tämän hetkinen paras suoritus.

        if (pisteet > huippuPisteet) //Vertaillaan onko nykyinen suoritus parempi kuin paras ja jos on niin sijoitetaan se uudeksi parhaimmaksi.
        {
            PlayerPrefs.SetInt("HighScore", pisteet);
        }
    }

    private void Start()
    {
        pisteenAlustus();
        pistehallinta = this;
    }
}
