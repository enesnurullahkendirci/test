using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


//Deste oluşturulcak

//Desteler dağıtılacak kalan kartlar hesaplanacak

public class DeskManager : MonoBehaviour
{

    public static DeskManager singleton;
    //DESTEYI TUTAN LISTE TANIMI
    public GameObject kartYuzeyiObjesi;
    public List<string> deste = new List<string>();
    public Sprite[] kartyuzeyi = new Sprite[52];
    public Sprite kart_arka;
    //DESTEYI OLUSTURMAK ICIN BILINMESI GEREKENLER
    public static string[] tip = new string[] { "S", "M", "A", "U" };
    public static string[] degerler = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    //KARTLARIN KOYULDUGU BLOKLARIN TAKIBI ICIN TOPLU KOD
    public GameObject[] AltKonum = new GameObject[4];
    public GameObject[] UstKonum = new GameObject[4];
    public GameObject OrtaKonum; //ortakonumdan kartlar alındıgı zaman burası bosalacak. Kart koyuldukca dolacak.
    //OYUN SIRASININ KIMDE OLDUGUNU TUTAN ELEMAN
    public int OyunSirasi = -1;//0 ise kullanıcı oynar 1 ise bot oynar.
    //PUAN HESAPLAMA KISMI
    public List<string> AltKonumHazinesi = new List<string>();
    public List<string> UstKonumHazinesi = new List<string>();
    public int AltKonumPisti;
    public int UstKonumPisti;
    //MEVCUT ELIN SAYISI
    public int mevcutturn = 0;
    //PUAN HESAPLAMA
    public int puan_insan;
    public int puan_bot;
    //TEXT
    public Text skorboard;
    //RESET KOMUT
    int oyun_sonu;
    public Button buton;
    // Start is called before the first frame update


    
    void Start()
    {
        kartYuzeyiObjesi = GameObject.FindGameObjectWithTag("Deste");
        for (int i = 0; i < 52; i++)
        {
            kartyuzeyi[i] = kartYuzeyiObjesi.GetComponent<KartYuzeyi>().kartyuzeyi[i];
            Debug.Log(kartyuzeyi[i]);
        }
        kart_arka = kartYuzeyiObjesi.GetComponent<KartYuzeyi>().kartArkasi;

        //Bug önlemek için // puanhesaplamada altkonumhazinesi && üstkonumhazinesi null ref verdiğinden hata veriyor.
        AltKonumHazinesi.Add("Z0");//oyunu etkilemeyecek kart isimi veriyorum
        UstKonumHazinesi.Add("Z0");//oyunu etkilemeyecek kart isimi veriyorum


        IlkAsama();
        skorboard = GameObject.FindGameObjectWithTag("skorboard").GetComponent<Text>();
        buton = GameObject.FindGameObjectWithTag("canvas").GetComponentInChildren<Button>();



    }
    // Update is called once per frame

    public static DeskManager GetSingleton()
    {
        if(singleton == null)
        {
            if(GameObject.Find("MasaYoneticisi") != null)
            {
                singleton = GameObject.Find("MasaYoneticisi").GetComponent<DeskManager>();
            }
            else
            {
                GameObject Masa_Yoneticisi= new GameObject("MasaYoneticisi");
                singleton = Masa_Yoneticisi.AddComponent<DeskManager>();
            }

        }
        return singleton;
    }


    void Update()
    {
        if(singleton != this)
        {
            Destroy(gameObject);
        }

        oyna();
        puan_Hesapla();

    }
    void IlkAsama()
    {
        DesteOlustur();
        DesteKaristir();
        DesteYazdir();
        KonumObjeCekici();
        DesteDagit();


    }
    void DesteOlustur()
    {
        deste.Clear();
        foreach (string t in tip)
        {
            foreach (string d in degerler)
            {
                deste.Add(t + d); // C2, C3, C4 gibi gibi stringleri listede topla
            }
        }

    }

    //deneme
    public static List<string> DesteOlusturma()
    {
        List<string> yeniDeste = new List<string>();
        foreach (string t in tip)
        {
            foreach (string d in degerler)
            {
                yeniDeste.Add(t + d); // C2, C3, C4 gibi gibi stringleri listede topla
            }
        }
        return yeniDeste;
    }
    

    void DesteKaristir()
    {
        System.Random random = new System.Random();
        int n = deste.Count;
        string temp;
        Sprite temp2;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            temp = deste[k];
            temp2 = kartyuzeyi[k];
            deste[k] = deste[n];
            kartyuzeyi[k] = kartyuzeyi[n];
            deste[n] = temp;
            kartyuzeyi[n] = temp2;
        }
    }
    void oyun_bitir()
    {
        if (OyunSirasi == 1 && MasaKontrolu())
        {


            foreach (string item in OrtaKonum.GetComponent<centerPosManager>().kartlar)
            {
                AltKonumHazinesi.Add(item);

            }
            OrtaKonum.GetComponent<centerPosManager>().kartlar.Clear();

        }
        else if (OyunSirasi == 0 && MasaKontrolu())
        {

            foreach (string item in OrtaKonum.GetComponent<centerPosManager>().kartlar)
            {
                UstKonumHazinesi.Add(item);

            }
            OrtaKonum.GetComponent<centerPosManager>().kartlar.Clear();

        }
        return;
    }
    void DesteDagit()
    {
        if (OyunSirasi == -1)
        {
            OyunSirasi = Random.Range(0, 2);
            Debug.Log(OyunSirasi);

        }
        if (deste.Count == 0)
        {

            Debug.Log("BURDA BUG VAR");
        }
        if (deste.Count == 4)
        {
            for (int i = 0; i < 2; i++)
            {
                AltKonum[i].GetComponent<CardScript>().kartismi = deste[0];
                if (AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.Count > 0)
                {
                    AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                    AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                }
                AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kartyuzeyi[mevcutturn * 8 + i]);
                AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kart_arka);
                deste.RemoveAt(0);

            }
            for (int i = 0; i < 2; i++)
            {
                UstKonum[i].GetComponent<CardScript>().kartismi = deste[0];
                if (UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.Count > 0)
                {
                    UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                    UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                }
                UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kartyuzeyi[mevcutturn * 8 + i + 2]);
                UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kart_arka);
                deste.RemoveAt(0);


            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                AltKonum[i].GetComponent<CardScript>().kartismi = deste[0];
                if (AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.Count > 0)
                {
                    AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                    AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                }

                AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kartyuzeyi[mevcutturn * 8 + i]);
                AltKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kart_arka);
                deste.RemoveAt(0);

            }
            for (int i = 0; i < 4; i++)
            {
                UstKonum[i].GetComponent<CardScript>().kartismi = deste[0];
                if (UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.Count > 0)
                {
                    UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                    UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.RemoveAt(0);
                }
                UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kartyuzeyi[mevcutturn * 8 + i + 4]);
                UstKonum[i].GetComponent<CardScript>().kartyuzeyleri.Add(kart_arka);
                deste.RemoveAt(0);


            }
        }
        mevcutturn++;

    }
    void DesteYazdir()
    {
        foreach (string t in deste)
        {
            Debug.Log(t);
        }
    }
    void KonumObjeCekici()
    {
        AltKonum[0] = GameObject.Find("bot0");
        AltKonum[1] = GameObject.Find("bot1");
        AltKonum[2] = GameObject.Find("bot2");
        AltKonum[3] = GameObject.Find("bot3");
        UstKonum[0] = GameObject.Find("top0");
        UstKonum[1] = GameObject.Find("top1");
        UstKonum[2] = GameObject.Find("top2");
        UstKonum[3] = GameObject.Find("top3");
        OrtaKonum = GameObject.Find("mid");

    }
    bool MasaKontrolu()
    {
        if (AltKonum[0].GetComponent<CardScript>().kartismi == "" && AltKonum[1].GetComponent<CardScript>().kartismi == ""
            && AltKonum[2].GetComponent<CardScript>().kartismi == "" && AltKonum[3].GetComponent<CardScript>().kartismi
            == "" && UstKonum[0].GetComponent<CardScript>().kartismi == "" && UstKonum[1].GetComponent<CardScript>().kartismi
            == "" && UstKonum[2].GetComponent<CardScript>().kartismi == "" && UstKonum[3].GetComponent<CardScript>().kartismi == "")
        {
            //Debug.Log("KART  CEKME ZAMANI BEBEIM");
            return true;
        }
        else
        {
            //Debug.Log("KART CEKEMZSIN BEBEGIM");
            return false;
        }
    }
    void puan_Hesapla()
    {
        int temp_Alt = 0;
        int temp_Ust = 0;
        foreach (string item in AltKonumHazinesi)
        {
            if (item[1] == 'A' || item[1] == 'J')
            {
                temp_Alt += 1;
            }
            else if (item == "S2")
            {
                temp_Alt += 2;
            }
            else if (item == "A10")
            {
                temp_Alt += 3;
            }
        }
        foreach (string item in UstKonumHazinesi)
        {
            if (item[1] == 'A' || item[1] == 'J')
            {
                temp_Ust += 1;
            }
            else if (item == "S2")
            {
                temp_Ust += 2;
            }
            else if (item == "A10")
            {
                temp_Ust += 3;
            }
        }
        temp_Alt += (AltKonumPisti * 10);
        temp_Ust += (UstKonumPisti * 10);
        skorboard.text = "PUAN BOT ==" + temp_Ust + "\nPUAN INSAN==" + temp_Alt;
        if (AltKonumHazinesi.Count > UstKonumHazinesi.Count)
        {
            temp_Alt += 3;
        }
        else if (AltKonumHazinesi.Count < UstKonumHazinesi.Count)
        {
            temp_Ust += 3;
        }

        puan_bot = temp_Ust;
        puan_insan = temp_Alt;


    }
    void oyun_bitir_hesapla()
    {
        Debug.Log("OYUN BITTI");



        buton.GetComponentInChildren<Text>().text = "BOT=" + puan_bot + "İNSAN=" + puan_insan + "YENİDEN BAŞLAT";



        buton.transform.position = new Vector3(360, 236, 0);
    }
    void oyna()
    {
        if (deste.Count == 0 && MasaKontrolu() && OrtaKonum.GetComponent<centerPosManager>().kartlar.Count == 0)
        {
            oyun_bitir_hesapla();
        }
        if (deste.Count == 0 && MasaKontrolu())
        {
            oyun_bitir();
        }
        else if (MasaKontrolu())
        {
            DesteDagit();
        }
        else
        {
            if (OyunSirasi == 0)
            {
                oyuncu_oyna();

            }
            else if (OyunSirasi == 1)
            {
                bot_oyna();
            }
        }
    }
    void oyuncu_oyna()
    {
        GetMouseClick();
    }
    void bot_oyna()
    {
        bool oynadi_mi = false;
        char ortadakikart;
        centerPosManager centerscript = OrtaKonum.GetComponent<centerPosManager>();
        if (centerscript.kartlar.Count == 0)
        {
            ortadakikart = 'x';
        }
        else
        {
            ortadakikart = centerscript.kartlar[centerscript.kartlar.Count - 1][1];

        }

        if (UstKonum[0].GetComponent<CardScript>().kartismi != "")
        {
            Debug.Log("Kart ismi0 " + UstKonum[0].GetComponent<CardScript>().kartismi[1]);
            if (UstKonum[0].GetComponent<CardScript>().kartismi[1] == ortadakikart)
            {
                Debug.Log("AYNI ELINDE VAR");
                kart_oyna(UstKonum[0]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[1].GetComponent<CardScript>().kartismi != "")
        {
            Debug.Log("Kart ismi1 " + UstKonum[1].GetComponent<CardScript>().kartismi[1]);
            if (UstKonum[1].GetComponent<CardScript>().kartismi[1] == ortadakikart)
            {
                Debug.Log("AYNI ELINDE VAR");
                kart_oyna(UstKonum[1]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[2].GetComponent<CardScript>().kartismi != "")
        {
            Debug.Log("Kart ismi2 " + UstKonum[2].GetComponent<CardScript>().kartismi[1]);
            if (UstKonum[2].GetComponent<CardScript>().kartismi[1] == ortadakikart)
            {
                Debug.Log("AYNI ELINDE VAR");
                kart_oyna(UstKonum[2]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[3].GetComponent<CardScript>().kartismi != "")
        {
            Debug.Log("Kart ismi3 " + UstKonum[3].GetComponent<CardScript>().kartismi[1]);
            if (UstKonum[3].GetComponent<CardScript>().kartismi[1] == ortadakikart)
            {
                Debug.Log("AYNI ELINDE VAR");
                kart_oyna(UstKonum[3]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[0].GetComponent<CardScript>().kartismi != "")
        {
            if (UstKonum[0].GetComponent<CardScript>().kartismi[1] == 'J')
            {
                Debug.Log("JOKER ELINDE VAR");
                kart_oyna(UstKonum[0]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[1].GetComponent<CardScript>().kartismi != "")
        {
            if (UstKonum[1].GetComponent<CardScript>().kartismi[1] == 'J')
            {
                Debug.Log("JOKER ELINDE VAR");
                kart_oyna(UstKonum[1]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[2].GetComponent<CardScript>().kartismi != "")
        {
            if (UstKonum[2].GetComponent<CardScript>().kartismi[1] == 'J')
            {
                Debug.Log("JOKER ELINDE VAR");
                kart_oyna(UstKonum[2]);
                oynadi_mi = true;
                return;
            }
        }
        if (UstKonum[3].GetComponent<CardScript>().kartismi != "")
        {
            if (UstKonum[3].GetComponent<CardScript>().kartismi[1] == 'J')
            {
                Debug.Log("JOKER ELINDE VAR");
                kart_oyna(UstKonum[3]);
                oynadi_mi = true;
                return;
            }
        }

        if (!oynadi_mi)
        {
            while (true)
            {
                int randkonum = Random.Range(0, 4);
                if (UstKonum[randkonum].GetComponent<CardScript>().kartismi == "")
                {
                    continue;
                }
                kart_oyna(UstKonum[randkonum]);
                break;

            }
        }



    }
    void GetMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - 10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.GetComponent<CardScript>() && hit.collider.CompareTag("botlar") && hit.collider.GetComponent<CardScript>().kartismi != "")
                {
                    kart_oyna(hit.collider.gameObject);

                }

            }
        }
    }
    void kart_oyna(GameObject kart)
    {

        if (alabilirmi(kart))
        {
            if (OyunSirasi == 0)
            {

                AltKonumHazinesi.Add(kart.GetComponent<CardScript>().kartismi);
                foreach (string item in OrtaKonum.GetComponent<centerPosManager>().kartlar)
                {
                    AltKonumHazinesi.Add(item);

                }
                OrtaKonum.GetComponent<centerPosManager>().kartlar.Clear();
                kart.GetComponent<CardScript>().kartismi = "";
            }
            else if (OyunSirasi == 1)
            {
                UstKonumHazinesi.Add(kart.GetComponent<CardScript>().kartismi);
                foreach (string item in OrtaKonum.GetComponent<centerPosManager>().kartlar)
                {
                    UstKonumHazinesi.Add(item);

                }
                OrtaKonum.GetComponent<centerPosManager>().kartlar.Clear();
                kart.GetComponent<CardScript>().kartismi = "";
            }
        }
        else
        {
            OrtaKonum.GetComponent<centerPosManager>().kartyuzeyi = kart.GetComponent<CardScript>().kartyuzeyleri[0];
            OrtaKonum.GetComponent<centerPosManager>().kartlar.Add(kart.GetComponent<CardScript>().kartismi);
            kart.GetComponent<CardScript>().kartismi = "";
        }





        el_degistir();

    }
    bool alabilirmi(GameObject kart)
    {
        char ortadakikart;
        centerPosManager centerscript = OrtaKonum.GetComponent<centerPosManager>();
        if (centerscript.kartlar.Count == 0)
        {
            ortadakikart = 'x';
        }
        else
        {
            ortadakikart = centerscript.kartlar[centerscript.kartlar.Count - 1][1];

        }

        if (kart.GetComponent<CardScript>().kartismi[1] == ortadakikart)
        {
            if (centerscript.kartlar.Count == 1)
            {
                if (OyunSirasi == 0)
                {
                    AltKonumPisti++;
                }
                else if (OyunSirasi == 1)
                {
                    UstKonumPisti++;
                }
            }
            return true;
        }
        else if (kart.GetComponent<CardScript>().kartismi[1] == 'J')
        {
            return true;
        }
        return false;
    }
    void el_degistir()
    {
        if (OyunSirasi == 1)
        {
            OyunSirasi = 0;
        }
        else
        {
            OyunSirasi = 1;
        }
    }
}