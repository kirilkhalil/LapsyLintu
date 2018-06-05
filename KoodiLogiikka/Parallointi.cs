using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallointi : MonoBehaviour {

    public static Parallointi parallointi; //Yritys saada tämä luokka kutsuttavaksi Pelihahmon_ohjaus luokasta

    class KasatutObjektit
    {

        public Transform transform;
        public bool kaytossa;
        public KasatutObjektit(Transform t) { transform = t; }
        public void Kaytossa() { kaytossa = true; }
        public void EiKaytossa() { kaytossa = false; }

    }

    [System.Serializable]
    public struct YLuontiVali
    {
        public float min;
        public float max;
    }

    public GameObject Prefab;
    public int kasanKoko;
    public float siirtymisNopeus;
    public float luontiTahti;
    bool onkoPeliLoppunut; //Muuttuja, millä estetään skriptin juoksu, jos peli on loppunut.

    public YLuontiVali yLuontiVali;
    public Vector3 vakioLuontiKohta;

    float luontiAjastin;

    KasatutObjektit[] kasaObjektit;

    private void Awake()
    {
        Maarittele();
        parallointi = this;
        onkoPeliLoppunut = true;
    }

    void Update()
    {
        if (onkoPeliLoppunut) 
        {
            return;
        }

        Siirra();
        luontiAjastin += Time.deltaTime;
        if (luontiAjastin > luontiTahti)
        {
            Luo();
            luontiAjastin = 0;
        }
    }

    void Maarittele()
    {
        kasaObjektit = new KasatutObjektit[kasanKoko];
        for (int i = 0; i < kasaObjektit.Length; i++)
        {
            GameObject peliObjekti = Instantiate(Prefab) as GameObject;
            Transform t = peliObjekti.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            kasaObjektit[i] = new KasatutObjektit(t);
        }
    }

    void Luo()
    {
        Transform t = HaeKasatutObjektit();
        if (t == null) return;
        Vector3 paikka = Vector3.zero;
        paikka.x = vakioLuontiKohta.x;
        paikka.y = Random.Range(yLuontiVali.min, yLuontiVali.max);
        t.position = paikka;
    }

    void Siirra()
    {
        for (int i = 0; i < kasaObjektit.Length; i++)
        {
            kasaObjektit[i].transform.position += -Vector3.right * siirtymisNopeus * Time.deltaTime;
            TarkistaObjektienPoisto(kasaObjektit[i]);
        }
    }

    void TarkistaObjektienPoisto(KasatutObjektit kasatutObjektit)
    {
        if (kasatutObjektit.transform.position.x < -vakioLuontiKohta.x)
        {
            kasatutObjektit.EiKaytossa();
            kasatutObjektit.transform.position = Vector3.one * 1000;
        }
    }

    Transform HaeKasatutObjektit()
    {
        for (int i = 0; i < kasaObjektit.Length; i++)
        {
            if (!kasaObjektit[i].kaytossa)
            {
                kasaObjektit[i].Kaytossa();
                return kasaObjektit[i].transform;
            }
        }

        return null;
    }

    public void pelinLoppu() //Tällä pysäytetään peli, kun osutaan johonkin mikä tagätty "kuolema".
    {
        onkoPeliLoppunut = true;
    }
    
    public void pelinAlustus() //Tämä triggeröityy kun pelaaja painaa "Uusinta"-nappia gameover näkymässä. Resetoidaan putket pois näkyvistä uutta pelisessiota varten.
    {
        for (int i = 0; i < kasaObjektit.Length; i++)
        {
            kasaObjektit[i].EiKaytossa();
            kasaObjektit[i].transform.position = Vector3.one * 1000;
        }

        Maarittele();
    }

    public void pelinAloitus() //Tämä triggeröityy, kun pelaaja painaa "Aloita"-nappia. Eli parallointi-skripti käynnistyy uudestaan ja putket rullaavat ruudulle.
    {
        onkoPeliLoppunut = false;
    }
}
