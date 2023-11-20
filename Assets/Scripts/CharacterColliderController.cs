using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter" + other.gameObject.tag);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit" + other.gameObject.tag);

    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    void Update()
    {
    }

 
}
