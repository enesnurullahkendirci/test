using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MasaYoneticisiKod : MonoBehaviour
{
    //UserInput userInput = new UserInput();
    public List<string> deste;//desteyi tutacak arraylist
    public Sprite[] kartYuzeyi;//Kartların önyüzlerini unity üzerinden atabileceğim liste
    public GameObject kartPrefab;
    public GameObject[] botPos;
    public GameObject[] topPos;
    

    public static string[] tip = new string[] { "C", "D", "H", "S" };//Kart tipleri (Türkçede harfler çakışıyor)
    public static string[] degerler = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string>[] bots;
    public List<string>[] tops;
    

    private List<string> bot0 = new List<string>();//*List<string> yerine ilerde string diye düzeltilebilir ve yeni bots ve tops List<string> olabilir.
    private List<string> bot1 = new List<string>();
    private List<string> bot2 = new List<string>();
    private List<string> bot3 = new List<string>();

     ///***

    private List<string> top0 = new List<string>();
    private List<string> top1 = new List<string>();
    private List<string> top2 = new List<string>();
    private List<string> top3 = new List<string>();//*

    //deneme
    //olmadı
    public List<string> top = new List<string>();

    void elemanSil (List<string> liste)
    {

    }

    void Start()
    {
        bots = new List<string>[] { bot0, bot1, bot2, bot3 };
        tops = new List<string>[] { top0, top1, top2, top3 };
        iskambil();// 40
    }

    void Update()
    {
        
    }
    public void iskambil()
    {
        deste = DesteOlusturma();//desteyi sıralı oluşturur
        Karistir(deste);//desteyi karıştırır

        /*foreach(string kart in deste)//debug
        {
            print(kart);
        }/*/
        destedenSilme();
        StartCoroutine(dagitma());
    }

    public static List<string> DesteOlusturma()
    {
        List<string> yeniDeste = new List<string>();
        foreach (string t in tip)
        {
            foreach(string d in degerler)
            {
                yeniDeste.Add(t + d); // C2, C3, C4 gibi gibi stringleri listede topla
            }
        }
        return yeniDeste;
    }

    void Karistir<T>(List<T> list) 
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
    IEnumerator dagitma()
    {
        UserInput userInput = new UserInput();
        for (int i = 0; i < 4; i++)
        {
            foreach (string kart in bots[i])
            {
                yield return new WaitForSeconds(0.05f);
                GameObject yeniKart = Instantiate(kartPrefab, new Vector3(botPos[i].transform.position.x, botPos[i].transform.position.y, botPos[i].transform.position.z), Quaternion.identity, botPos[i].transform);
                yeniKart.name = kart;
                yeniKart.GetComponent<Selectable>().row = i;
                yeniKart.GetComponent<Selectable>().onYuz = true;
                
                //yeniKart.GetComponent<Selectable>().inDeckPile = true;//
            }
            foreach (string kart in tops[i])
            {
                yield return new WaitForSeconds(0.05f);
                GameObject yeniKart = Instantiate(kartPrefab, new Vector3(topPos[i].transform.position.x, topPos[i].transform.position.y, topPos[i].transform.position.z), Quaternion.identity, topPos[i].transform);
                yeniKart.name = kart;
                top.Add(yeniKart.name);
                
                
                //yeniKart.GetComponent<Selectable>().onYuz = true;
            }
        }
    }
    void destedenSilme()
    {
        for (int i = 0; i < 4; i++)
        {
            bots[i].Add(deste.Last<string>());
            deste.RemoveAt(deste.Count - 1);
            tops[i].Add(deste.Last<string>());
            deste.RemoveAt(deste.Count - 1);
        }
    }


}
