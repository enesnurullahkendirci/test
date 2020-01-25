using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerPosManager : MonoBehaviour
{
    public List<string> kartlar;
    public Sprite kartyuzeyi;
    public Sprite defaultyuzey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        kartrenderer();
    }
    void kartrenderer()
    {
        if (kartlar.Count==0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = defaultyuzey;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = kartyuzeyi;
        }
    }


}
