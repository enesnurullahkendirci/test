using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public string kartismi;
    public bool kartyuzu;
    public List<Sprite> kartyuzeyleri;
    public Sprite emptypos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        kartrender();
    }
    void kartrender()
    {
        if (kartismi == "")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = emptypos;
        }
        else
        {
          

            if (kartyuzu == true)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = kartyuzeyleri[0];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = kartyuzeyleri[1];
            }
        }
       
    }
}
