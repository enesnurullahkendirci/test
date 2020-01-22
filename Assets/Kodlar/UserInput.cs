using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    public MasaYoneticisiKod masaYoneticisiKod;
    

    public List<string> mid = new List<string>();///Tüm homojen arraylistleri List<T> ile değiştir Arralist x = new Arraylist();
    public List<string> bot = new List<string>();
    public GameObject[] temizlik;

    //Botun kartlarını bir diziye atıp oradan kontrol etmek daha mantıklı geldi:
    public List<int> topDeger = new List<int>() ;
    // tamamı deneysel kendi yöremizin ürünü
    //private int* p1;
    public GameObject[] deneme;

    void Start()
    {
        slot1 = this.gameObject;
        
        mid.Add("Z0");
    }

    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - 10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.CompareTag("Deste"))
                {
                    Deste();
                }
                else if (hit.collider.CompareTag("Kart"))
                {
                    Kart(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Mid"))
                {
                    Mid(hit.collider.gameObject);
                }
            }
        }
    }

    void Deste()
    {
        Debug.Log("deste click");

    }
    
    void Kart(GameObject selected)
    {

        if (selected.GetComponent<Selectable>().onYuz)
        {
            Debug.Log("kart click");
            
            slot1 = selected;
            Stack2(selected);
        }
        //if (slot1 == this.gameObject ) 
        //{
        //    slot1 = selected;

        //}
        //else if (slot1 != selected )
        //{
        //    Debug.Log("elseif");
        //    if (Stackable(selected))
        //    {
        //        Stack(selected);
        //    }
        //    else
        //    {
        //        slot1 = selected;
        //    }
            
            
        //}
        //if tıklanmış kart tersidönükse bir şey yapmıycak
        //if kart middense bir şey yapmayacak
        //if kart yüzü dönükse mide göndericek ,yapamazsam 2. tıkla çalışacak.
        ///eğer iki tıkla çalışırsa
            // kart seçili değilse yeni kart seçicek
            // seçiliyse ve ikinci tıklanan yere kartı göndericek
                //eğer ikinci tıklanan yer bottansa, kartların yerleri değişecek
        //eğer middeki kart aynı sayıda kart ise middeki kart listesinin tamamı botun skor listesine gönderilir
            //eğer midde tek kart varsa ve üstüne gelen kart aynı sayıda ise static pistBot/pistiTop++
        //Vale atılırsa listenin tamamı botun veya topun listesine alınır.

    }
    void Mid(GameObject selected)
    {
        Debug.Log("Mid click");
        
    }
    bool Stackable(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        //Debug.Log("S2s2s2s2s2s2"+s2);
        
        if (s1.deger == s2.deger && s1.deger == 11)
        {
            //Debug.Log("Kartı mide taşı");
            return false;
        }
        else
        {
            //Debug.Log("Kartı mide taşı");
            //Debug.Log("Middeki kartları ganimete taşı");
            return true;
        }
    }
    
                                           //////
    void Stack2(GameObject selected)                                            /////
    {                                                                           ////
        
        slot1.transform.position = new Vector3(0, 0, 0);
        slot1.transform.parent = selected.transform;

        Selectable s1 = slot1.GetComponent<Selectable>();
        //Debug.Log("Slot1 ismi: " + slot1.name);
        string sondeger = mid[mid.Count - 1];
        int sondegerInt = DegerDondurme(sondeger);
        
        
        deneme = GameObject.FindGameObjectsWithTag("Mid");//önceki kartları görünmez yapıyor
        foreach (GameObject item in deneme)//önceki kartları görünmez yapıyor
        {
            item.GetComponent<SpriteRenderer>().enabled = false;//önceki kartları görünmez yapıyor
        }//önceki kartları görünmez yapıyor
        
        mid.Add(slot1.name);
        int slot1Int = DegerDondurme(slot1.name);
        
        //Debug.Log("eleman sayısı: "+mid.Count);
        //Bu arayı

        //
        slot1.tag = "Mid";//şimdilik çift tıklama sorununu çözmek için yaptım
        if (sondegerInt == slot1Int || slot1.name == "CJ" || slot1.name == "DJ" || slot1.name == "HJ" || slot1.name == "SJ")
            kartlariAlma();
        
        slot1 = this.gameObject;
        botTurn();
        
    }
    void botTurn()
    {
        //Debug.Log("botTurn çalıştı");
        topDegerEkleme();
        Debug.Log(masaYoneticisiKod.top[0]);

    }
    void topDegerEkleme()
    {
        for (int i = 0; i < 4; i++)
        {
            topDeger.Insert(i,i);
            Debug.Log(topDeger[i]);
        }
    }
    void kartlariAlma()
    {
        //Oyuncu İçin
        temizlik = GameObject.FindGameObjectsWithTag("Mid");
        for (int i = mid.Count-1; i > 0; i--)
        {

            bot.Add(mid[i]);
            mid.RemoveAt(i);
            
        }
        for (int i = 0; i < temizlik.Length; i++)
        {
            Destroy(temizlik[i]);
        }
        
    }

    int DegerDondurme(string isim)
    {
        string degerString = "";
        int deger = 0;
        for (int i = 1; i < isim.Length; i++)
        {
            char c = isim[i];
            //Debug.Log(c + " " + i);
            degerString += c.ToString();
        }
        if (degerString == "A")
        {
            deger = 1;
        }
        if (degerString == "2")
        {
            deger = 2;
        }
        if (degerString == "3")
        {
            deger = 3;
        }
        if (degerString == "4")
        {
            deger = 4;
        }
        if (degerString == "5")
        {
            deger = 5;
        }
        if (degerString == "6")
        {
            deger = 6;
        }
        if (degerString == "7")
        {
            deger = 7;
        }
        if (degerString == "8")
        {
            deger = 8;
        }
        if (degerString == "9")
        {
            deger = 9;
        }
        if (degerString == "10")
        {
            deger = 10;
        }
        if (degerString == "J")
        {
            deger = 11;
        }
        if (degerString == "Q")
        {
            deger = 12;
        }
        if (degerString == "K")
        {
            deger = 13;
        }
        return deger;
    }

    
}
