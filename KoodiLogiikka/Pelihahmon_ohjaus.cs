using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelihahmon_ohjaus : MonoBehaviour {

    public Vector3 aloitusKohta = new Vector3(-5,0,0); //Määritellään julkinen muuttuja pelihahmon aloituskoordinaateille.
    public Vector3 kattoRaja = new Vector3(-5,4.5f,0); //Määritellään raja ruudun yläreunaan, jota pelaaja ei voi ylittää.
    public float liikeVoima = 300; //Voima joka kohdistuu pelihahmoon kun painetaan "lento"-nappia.
    public float kaantoLiike = 5;
    public float painoVoima = 0.8f; // Ei hyväksy double ja floatilla pitänee olla f-desimaalien perässä ettei yritä kääntää sitä ajossa doubleksi.

    new Rigidbody2D rigidbody; //Alustetaan yleiseksi muuttujaksi rigidbody ettei tarvitse aina luoda uusi kun kutsutaan jotain.
    GameObject AloitusKohtaus; //Alustetaan objekti, jolla saadaan kiinni eri kohtauksista, jota voidaan UI:ssa pyöritellä tarpeen mukaan.
    GameObject LoppumisKohtaus; //Alustetaan objekti, jolla saadaan kiinni eri kohtauksista, jota voidaan UI:ssa pyöritellä tarpeen mukaan.

    Quaternion alasKaanto;
    Quaternion ylosKaanto;

    public AudioSource naputteluAani;
    public AudioSource pisteAani;
    public AudioSource kuolemaAani;

    // Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>(); //Haetaan itse objekti ja sen fysiikka muuttujaan.
        rigidbody.gravityScale = painoVoima;
        AloitusKohtaus = GameObject.FindGameObjectWithTag("AloitusKohtaus"); //Nämä alustukset eivät toimi mikäli editorin puolella on objektit disabloitu ensin. Lieneekö tähän jotain workaroundia?
        LoppumisKohtaus = GameObject.FindGameObjectWithTag("LoppumisKohtaus"); //Nämä alustukset eivät toimi mikäli editorin puolella on objektit disabloitu ensin. Lieneekö tähän jotain workaroundia?
        rigidbody.simulated = false; //Poistetaan fysiikkamoottori käytöstä, jolloin saadaan pelihahmo "jäädytettyä".
        AloitusKohtaus.SetActive(true); //Alustuksen yhteydessä asetetaan oikeat UI-elementit aktiiviseksi / in-aktiiviseksi.
        LoppumisKohtaus.SetActive(false); //Alustuksen yhteydessä asetetaan oikeat UI-elementit aktiiviseksi / in-aktiiviseksi.
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            naputteluAani.Play();
            rigidbody.AddForce(Vector2.up * liikeVoima); //Defaulttina Unityssä "Jump" = spacebar. Jos pelaaja painaa spacebaria, niin kohdistetaan hahmoon ylöspäin suuntaava voima.
        }

        if (rigidbody.transform.position.y >= 4.5)
        {
            rigidbody.transform.position = kattoRaja;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) //Vaihtoehtoisesti "OnCollisionEnter2D", mikäli "Trigger" täppä ei käytössä.
    {
        if (collision.gameObject.tag == "Kuolema")
        {
            kuolemaAani.Play();
            Parallointi.parallointi.pelinLoppu();
            PisteHallinta.pistehallinta.highScoreVertaus(); //Verrataan nykyistä tulosta parhaimpaan.
            rigidbody.simulated = false; //Poistetaan fysiikkamoottori käytöstä, jolloin saadaan pelihahmo "jäädytettyä".
            LoppumisKohtaus.SetActive(true);
            //Tässä välissä pitäisi tuoda esille skeneistä LoppumisKohtaus ja kun pelaaja painaa "AloitaAlustaNappia", niin siirrytään alla olevaan paikan siirtämiseen ja tuodaan esille "AloitusKohtaus".
            //Debug.Log("Toimii!");
        }

        if (collision.gameObject.tag == "Piste")
        {
            pisteAani.Play();
            PisteHallinta.pistehallinta.pisteenLisays(); //Kutsutaan metodia joka lisää pistemuuttujan arvoa ja päivittää sen ruudulle
            //Debug.Log("Toimii!");
        }
    }

    public void AloitusSkripti() //Kun UI:sta painetaan Aloita-nappia
    {
        rigidbody.simulated = true;
        AloitusKohtaus.SetActive(false);
        Parallointi.parallointi.pelinAloitus();
    }

    public void LopetusSkripti()//Kun UI:sta painetaan Uusinta-nappia
    {
        rigidbody.simulated = false;
        rigidbody.transform.localPosition = aloitusKohta; //Tällä saadaan pelihahmo takaisin aloituskohtaan.
        LoppumisKohtaus.SetActive(false);
        AloitusKohtaus.SetActive(true);
        Parallointi.parallointi.pelinAlustus();
        PisteHallinta.pistehallinta.pisteenAlustus();
    }

    public void ExitSkripti() //Exit-nappia painamalla sammutetaan applikaatio.
    {
        Application.Quit();
    }
}
