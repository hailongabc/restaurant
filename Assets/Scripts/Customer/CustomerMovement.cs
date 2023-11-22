using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CustomerMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public MainCharacterController _mainCharacterController;
    private Vector3 distanceToWalkPoint;
    bool isDes1;
    bool isDes2;
    private float randomValue01;
    private float randomValue23;
    public Animator _animator;
    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        randomValue01 = Random.Range(0f, 1f);
        if (!_mainCharacterController.ghe1)
        {
            randomValue23 = 2f;
            _mainCharacterController.ghe1 = true;
        }
        else
        {
            randomValue23 = 3f;
        }



        Debug.Log("randomValue01 " + Mathf.RoundToInt(randomValue01));
        Debug.Log("randomValue23 " + Mathf.RoundToInt(randomValue23));

    }
    // Update is called once per frame
    void Update()
    {
        if (!isDes1)
        {
            Move1();
        }
    }

    private void Move1()
    {
        if (Mathf.RoundToInt(randomValue23) == 2)
        {
            Vector3 distanceToWalkPoint = Vector3.zero;
            if (!isDes2)
            {
                agent.SetDestination(_mainCharacterController.listDestination[Mathf.RoundToInt(randomValue01)].transform.position);
                distanceToWalkPoint = transform.position - _mainCharacterController.listDestination[Mathf.RoundToInt(randomValue01)].transform.position;
            }
            if (distanceToWalkPoint.magnitude < 1f)
            {
                Debug.Log("vo day ko");
                isDes2 = true;
                agent.SetDestination(_mainCharacterController.listDestination[Mathf.RoundToInt(randomValue23)].transform.position);
                Vector3 distanceToWalkPoint2 = transform.position - _mainCharacterController.listDestination[Mathf.RoundToInt(randomValue23)].transform.position;
                Debug.Log(distanceToWalkPoint2.magnitude);
                if (distanceToWalkPoint2.magnitude < 1.5f)
                {

                    Vector3 targetRotation = Vector3.zero;
                    transform.DOLocalRotate(targetRotation, 1f);
                    _animator.SetBool("IsSit", true);
                    isDes1 = true;
                    Debug.Log("xin day");
                }
            }
        }
        else
        {
            if (!isDes2)
            {
                agent.SetDestination(_mainCharacterController.listDestination[Mathf.RoundToInt(randomValue01)].transform.position);
                distanceToWalkPoint = transform.position - _mainCharacterController.listDestination[Mathf.RoundToInt(randomValue01)].transform.position;
            }

            if (distanceToWalkPoint.magnitude < 1f)
            {
                isDes2 = true;
                agent.SetDestination(_mainCharacterController.listDestination[Mathf.RoundToInt(randomValue23)].transform.position);
                Vector3 distanceToWalkPoint2 = transform.position - _mainCharacterController.listDestination[Mathf.RoundToInt(randomValue23)].transform.position;
                if (distanceToWalkPoint2.magnitude < 1.5f)
                {
                    Vector3 targetRotation = new Vector3(0f, 180f, 0f);
                    transform.DOLocalRotate(targetRotation, 0.5f);
                    _animator.SetBool("IsSit", true);
                    isDes1 = true;
                    Debug.Log("vcvcv");
                }
            }
        }
    }

    IEnumerable SitDown()
    {
        yield return new WaitForSeconds(1f);

        yield return null; 
    }
}
