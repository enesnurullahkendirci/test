using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool top = false;
    public string tip;
    public int deger;
    public int row;
    public bool onYuz = false;
    //public bool inDeckPile = false;
    private string degerString;

    void Start()
    {
        if (CompareTag("Kart"))
        {
            tip = transform.name[0].ToString();
            //Debug.Log(tip + "deger");
            for (int i = 1; i < transform.name.Length; i++)
            {
                char c = transform.name[i];
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
        }
    }

    void Update()
    {
        
    }
}
