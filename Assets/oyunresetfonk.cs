using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oyunresetfonk : MonoBehaviour
{
    public GameObject oyun;
    public GameObject buton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void oyun_calistir()
    {
        if(DeskManager.singleton != null)
        {
            DestroyImmediate(GameObject.Find("MasaYoneticisi").gameObject);

        }
        DeskManager.singleton = null;
        Debug.Log(DeskManager.singleton);
        Debug.Log("BURADASIN");
        DeskManager.GetSingleton();
        buton.transform.position=new Vector3(10000,0,0);

    }

}
