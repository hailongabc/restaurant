using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public ItemType itemType;
    public  GameObject beefPf;
    public  GameObject vegetablePf;
    public enum ItemType
    {
        Vegetable,
        Beef,
        TrashBin,
        Wine
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
      
    }

    private void OnTriggerExit(Collider other)
    {
      
    }

  

}
