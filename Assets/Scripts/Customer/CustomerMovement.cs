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
    public GameObject orderView;
    public bool isGhe1;
    public bool isEated;
    private bool isEatDone;
    private int randomIndex;
    private bool isEndPoint = true;
    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        randomValue01 = Random.Range(0f, 3f);
        if (!_mainCharacterController.ghe1)
        {
            randomValue23 = 4f;
            _mainCharacterController.ghe1 = true;
            isGhe1 = false;
        }
        else
        {
            isGhe1 = true;
            randomValue23 = 5f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isDes1)
        {
            Move();
        }
        if (_mainCharacterController.doneEatCount > 1)
        {
            if (isEatDone)
            {
                _mainCharacterController.billCount++;
                if (_mainCharacterController.billCount >= 2)
                {
                    _mainCharacterController.doneEatCount = 0;
                }
                BillPlease();
            }
        }
        if (!isEndPoint)
        {
            GotoReceipt();
        }

    }

    private void Move()
    {
        if (Mathf.RoundToInt(randomValue23) == 5)
        {
            Vector3 distanceToWalkPoint = Vector3.zero;
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
                agent.SetDestination(_mainCharacterController.listDestination[Mathf.RoundToInt(randomValue01)].transform.position);
                distanceToWalkPoint = transform.position - _mainCharacterController.listDestination[Mathf.RoundToInt(randomValue01)].transform.position;
            }

            if (distanceToWalkPoint.magnitude < 1f)
            {
                isDes2 = true;
                agent.SetDestination(_mainCharacterController.listDestination[Mathf.RoundToInt(randomValue23)].transform.position);
                Vector3 distanceToWalkPoint2 = transform.position - _mainCharacterController.listDestination[Mathf.RoundToInt(randomValue23)].transform.position;
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
        yield return new WaitForSeconds(_mainCharacterController.timeEat);
        _mainCharacterController.doneEatCount++;
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        _animator.SetBool("IsEat", false);
        isEatDone = true;
        if (_mainCharacterController.doneEatCount > 1)
        {
            Instantiate(_mainCharacterController.diaban, _mainCharacterController.diabanPos.position, Quaternion.identity, _mainCharacterController.changeParentTable.transform);
        }
        yield return null;
    }
    private void BillPlease()
    {
        randomIndex = Mathf.RoundToInt(Random.Range(0f, 3f));
        _mainCharacterController.ghe1 = false;
        transform.GetChild(0).transform.DOLocalMoveY(0f, 0.1f)
            .OnComplete(() =>
            {
                _animator.SetBool("IsSit", false);
                isEndPoint = false;
            });
        isEatDone = false;

    }

    private void GotoReceipt()
    {
        agent.SetDestination(_mainCharacterController.listReception[randomIndex].position);
        var distanceToEndPoint = transform.position - _mainCharacterController.listReception[randomIndex].position;
        if (distanceToEndPoint.magnitude < 1f)
        {
            _animator.SetBool("IsSit", true);
            Vector3 targetRotation = new Vector3(0f, 180f, 0f);
            transform.DOLocalRotate(targetRotation, 0.5f);
            isEndPoint = true;
            Debug.Log("zxczxczxczxczx");
        }
    }
}
