using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CustomerMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    private Vector3 distanceToWalkPoint;
    private Transform endPoint;
    bool isDes1;
    bool isDes2;
    private float randomValue01;
    private float randomValue23;
    public Animator _animator;
    public GameObject orderView;
    public bool isGhe1;
    public bool isEated;
    public bool isEatDone;
    private bool isEndPoint = true;
    public Transform dropPointFood;
    public int customerId;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        randomValue01 = Random.Range(0f, 3f);
        for (int i = 0; i < MainCharacterController.ins.listDestinationEndPoint.Count; i++)
        {
            if (!MainCharacterController.ins.listDestinationEndPoint[i].GetComponent<CheckReception>().haveCustomer)
            {
                MainCharacterController.ins.listDestinationEndPoint[i].GetComponent<CheckReception>().haveCustomer = true;
                endPoint = MainCharacterController.ins.listDestinationEndPoint[i];
                break;
            }
        }

    }
    void Update()
    {
        //if (!isDes1)
        //{
        //    Move();
        //}
        //if (MainCharacterController.ins.doneEatCount > 1)
        //{
        //    if (isEatDone)
        //    {
        //        MainCharacterController.ins.billCount++;
        //        if (MainCharacterController.ins.billCount >= 2)
        //        {
        //            MainCharacterController.ins.doneEatCount = 0;
        //            BillPlease();
        //        }
        //    }
        //}
        if (!isDes1)
        {
        MoveTempPoint(MainCharacterController.ins.listDestinationTempPoint[Mathf.RoundToInt(randomValue01)].transform);

        }

        //if (!isEndPoint)
        //{
        //    GotoReceipt();
        //}

    }

    private void Move()
    {
        if (Mathf.RoundToInt(randomValue23) == 5)
        {
            Vector3 distanceToWalkPoint = Vector3.zero;
            if (!isDes2)
            {
                agent.SetDestination(MainCharacterController.ins.listDestinationTempPoint[Mathf.RoundToInt(randomValue01)].transform.position);
                distanceToWalkPoint = transform.position - MainCharacterController.ins.listDestinationTempPoint[Mathf.RoundToInt(randomValue01)].transform.position;
            }
            if (distanceToWalkPoint.magnitude < 1f)
            {
                isDes2 = true;
                agent.SetDestination(MainCharacterController.ins.listDestinationEndPoint[Mathf.RoundToInt(randomValue23)].transform.position);
                Vector3 distanceToWalkPoint2 = transform.position - MainCharacterController.ins.listDestinationEndPoint[Mathf.RoundToInt(randomValue23)].transform.position;
                if (distanceToWalkPoint2.magnitude < 1.1f)
                {
                    Vector3 targetRotation = Vector3.zero;
                    transform.DOLocalRotate(targetRotation, 0.5f);
                    StartCoroutine(SitDown());
                    isDes1 = true;
                }
            }
        }
        else
        {
            if (!isDes2)
            {
                agent.SetDestination(MainCharacterController.ins.listDestinationTempPoint[Mathf.RoundToInt(randomValue01)].transform.position);
                distanceToWalkPoint = transform.position - MainCharacterController.ins.listDestinationTempPoint[Mathf.RoundToInt(randomValue01)].transform.position;
            }

            if (distanceToWalkPoint.magnitude < 1f)
            {
                isDes2 = true;
                agent.SetDestination(MainCharacterController.ins.listDestinationEndPoint[Mathf.RoundToInt(randomValue23)].transform.position);
                Vector3 distanceToWalkPoint2 = transform.position - MainCharacterController.ins.listDestinationEndPoint[Mathf.RoundToInt(randomValue23)].transform.position;
                if (distanceToWalkPoint2.magnitude < 1.1f)
                {
                    Vector3 targetRotation = new Vector3(0f, 180f, 0f);
                    transform.DOLocalRotate(targetRotation, 0.5f);
                    StartCoroutine(SitDown());
                    isDes1 = true;
                }
            }
        }
    }

    private void MoveTempPoint(Transform tempPoint)
    {
        if (!isDes2)
        {
            agent.SetDestination(tempPoint.position);
            distanceToWalkPoint = transform.position - MainCharacterController.ins.listDestinationTempPoint[Mathf.RoundToInt(randomValue01)].transform.position;
        }
        if (distanceToWalkPoint.magnitude < 1f)
        {
            isDes2 = true;
            agent.SetDestination(endPoint.position);
            Vector3 distanceToWalkEndPoint = transform.position - endPoint.position;
            if (distanceToWalkEndPoint.magnitude < 1.5f)
            {
                transform.DOLocalRotate(endPoint.GetComponent<CheckReception>().customerRotate, 0.5f);
                StartCoroutine(SitDown());
                isDes1 = true;
            }
        }


    }
    IEnumerator SitDown()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).transform.DOLocalMoveY(0.798f, 0.1f);
        _animator.SetBool("IsSit", true);
        orderView.SetActive(true);
        yield return null;
    }
    public void Eat()
    {
        _animator.SetBool("IsEat", true);
        StartCoroutine(EatDone());
    }

    IEnumerator EatDone()
    {
        yield return new WaitForSeconds(MainCharacterController.ins.timeEat);
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        _animator.SetBool("IsEat", false);
        isEatDone = true;
        Manager.ins.checkEatDone(this);

        yield return null;
    }

    public void ShowDiaBan()
    {
        Instantiate(MainCharacterController.ins.diaban, endPoint.GetComponent<CheckReception>().diabanPoint.position, Quaternion.identity, endPoint.GetComponent<CheckReception>().diabanParent);
    }
    public void BillPlease()
    {
        //MainCharacterController.ins.ghe1 = false;
        transform.GetChild(0).transform.DOLocalMoveY(0f, 0.1f)
            .OnComplete(() =>
            {
                _animator.SetBool("IsSit", false);
                isEndPoint = false;
            });
        GotoReceipt();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LineUp"))
        {
            RotateCustomer();
        }
    }
    private void GotoReceipt()
    {
        for (int i = 0; i < MainCharacterController.ins.listReception.Count; i++)
        {
            if (!MainCharacterController.ins.listReception[i].GetComponent<CheckReception>().haveCustomer)
            {
                agent.SetDestination(MainCharacterController.ins.listReception[i].position);
                MainCharacterController.ins.listReception[i].GetComponent<CheckReception>().haveCustomer = true;
                break;
            }
        }


    }
    private void RotateCustomer()
    {
        {
            _animator.SetBool("IsSit", true);
            Vector3 targetRotation = new Vector3(0f, 180f, 0f);
            transform.DOLocalRotate(targetRotation, 0.5f);
            isEndPoint = true;
        }
    }
}
