using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    public static Manager ins;
    public Text textHeader;
    public GameObject header;
    public GameObject cash;
    public Text textCash;
    public List<CustomerMovement> listCustomer = new List<CustomerMovement>();
    public CustomerMovement customerPf;
    public GameObject containerObject;
    public Button button;
    public Transform endPos3;
    public Transform endPos4;
    public GameObject Banghe2;
    public List<CustomerMovement> listCustomerReceipt = new List<CustomerMovement>();
    public Transform exitPoint;
    private void Awake()
    {
        ins = this;
        textCash.text = "0";
    }

    private void Start()
    {
        SpawnCustomer();
        button.onClick.AddListener(Button);
    }

    public void SpawnCustomer()
    {

        for (int i = 0; i < MainCharacterController.ins.listDestinationEndPoint.Count; i++)
        {
            CustomerMovement customer = Instantiate(customerPf, containerObject.transform);
            listCustomer.Add(customer);
            if (i < 2)
            {
                customer.customerId = 1;
            }
            else
            {
                customer.customerId = 2;
            }
        }
    }
    public void checkEatDone(CustomerMovement customerMovement)
    {
        for (int i = 0; i < listCustomer.Count; i++)
        {
            if (listCustomer[i] != customerMovement)
            {
                if (listCustomer[i].customerId == customerMovement.customerId)
                {
                    if (listCustomer[i].isEatDone && customerMovement.isEatDone)
                    {
                        customerMovement.ShowDiaBan();
                        listCustomer[i].BillPlease();
                        customerMovement.BillPlease();
                    }
                }
            }
        }
    }

    private void Button()
    {
        MainCharacterController.ins.listDestinationEndPoint.Add(endPos3);
        MainCharacterController.ins.listDestinationEndPoint.Add(endPos4);
        for(int i = 0; i < MainCharacterController.ins.listDestinationEndPoint.Count; i++)
        {
            if(i > 1)
            {
                CustomerMovement customer = Instantiate(customerPf, containerObject.transform);
                listCustomer.Add(customer);
                if (i < 2)
                {
                    customer.customerId = 1;
                }
                else
                {
                    customer.customerId = 2;
                }
            }
        }
        Banghe2.SetActive(true);
        button.interactable = false;
    }

}
