using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuzey : MonoBehaviour
{
    public Sprite kartYuzu;
    public Sprite kartArkasi;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private MasaYoneticisiKod masaYoneticisiKod;
    private UserInput userInput;



    // Start is called before the first frame update
    void Start()
    {
        List<string> deste = MasaYoneticisiKod.DesteOlusturma();
        masaYoneticisiKod = FindObjectOfType<MasaYoneticisiKod>();
        userInput = FindObjectOfType<UserInput>();

        int i = 0;
        foreach (string kart in deste)
        {
            if (this.name == kart)
            {
                kartYuzu = masaYoneticisiKod.kartYuzeyi[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }

    
    void Update()
    {
        if (selectable.onYuz == true)
        {
            spriteRenderer.sprite = kartYuzu;
        }
        else
        {
            spriteRenderer.sprite = kartArkasi;
        }

        if (userInput.slot1)
        {
            if (name == userInput.slot1.name)
            {
                spriteRenderer.color = Color.yellow;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
}
