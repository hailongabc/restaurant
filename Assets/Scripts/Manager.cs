using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager ins;
    public Text textHeader;
    public GameObject header;
    public GameObject cash;
    public Text textCash;
    private void Awake()
    {
        ins = this;
        textCash.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
