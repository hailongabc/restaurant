using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCCarrierScript : MonoBehaviour
{
    [SerializeField] private ProgressCircleUIController _actionScript;
    [SerializeField] private Transform startSpawnPointBeef;
    [SerializeField] private Transform startSpawnPointVegetable;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject changeParent;
    [SerializeField] private GameObject spawnParent;
    [SerializeField] private List<GameObject> listFood = new List<GameObject>();
    public float arcHeight = 1f;
    public float throwDuration = 2f;
    private int index;
    private bool isBin = false;
    public float delayTime;

    void Start()
    {

    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FoodSpawner"))
        {
            StartCoroutine(GetFood(other));

        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        isBin = false;
        Debug.Log("exit");
        _actionScript.StopAction();
        StopCoroutine(GetFood(other));
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            StartCoroutine(DropFood());
        }
    }
    IEnumerator GetFood(Collider other)
    {
        if (listFood.Count < 4)
        {
            _actionScript.StartAction();
            yield return new WaitForSeconds(_actionScript._duration);

            var kitchenObject = other.GetComponent<KitchenObject>();
            switch (kitchenObject.itemType)
            {
                case KitchenObject.ItemType.Vegetable:
                    GameObject beef = Instantiate(kitchenObject.vegetablePf, startSpawnPointVegetable.position, Quaternion.identity);
                    beef.transform.parent = changeParent.transform;
                    beef.transform.position = Vector3.zero;
                    listFood.Add(beef);
                    ThrowObjectOnArc(beef);
                    break;
                case KitchenObject.ItemType.Beef:
                    GameObject vegetable = Instantiate(kitchenObject.beefPf, startSpawnPointBeef.position, Quaternion.identity);
                    vegetable.transform.parent = changeParent.transform;
                    vegetable.transform.position = Vector3.zero;
                    listFood.Add(vegetable);
                    ThrowObjectOnArc(vegetable);
                    break;
            }
        }
        yield return null;
    }

    void ThrowObjectOnArc(GameObject obj)
    {
        // Coroutine để ném vật thể theo đường cung
        StartCoroutine(ThrowCoroutine(obj));
    }

    IEnumerator ThrowCoroutine(GameObject obj)
    {

        // Chờ một khoảng thời gian để đảm bảo vật thể đã được instantiate
        yield return new WaitForSeconds(0.05f);

        float elapsedTime = 0f;
        while (elapsedTime < throwDuration)
        {
            // Tính toán thời gian đã trôi qua kể từ khi bắt đầu
            elapsedTime += Time.deltaTime;

            // Tính toán vị trí trên đường cung
            float t = elapsedTime / throwDuration;
            Vector3 currentPos = CalculateArcPoint(t, obj);

            // Di chuyển vật thể đến vị trí hiện tại
            obj.transform.position = currentPos;

            yield return null;
        }
        if (isBin)
        {
            Destroy(obj.gameObject);
        }
        // Hoặc bạn có thể thực hiện các công việc khác sau khi ném vật thể hoàn thành
        // Ví dụ: Hủy bỏ vật thể, hiển thị hiệu ứng, v.v.
    }
    Vector3 CalculateArcPoint(float t, GameObject obj)
    {
        // Sử dụng hàm Lerp để tính toán vị trí trên đường cung
        if (!isBin)
        {
            if (obj.transform.name == "beef(Clone)")
            {
                float x = Mathf.Lerp(startSpawnPointBeef.position.x, changeParent.transform.position.x, t);
                float y = Mathf.Lerp(startSpawnPointBeef.position.y, changeParent.transform.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight;
                float z = Mathf.Lerp(startSpawnPointBeef.position.z, changeParent.transform.position.z, t);
                return new Vector3(x, y, z);
            }
            else
            {
                float x = Mathf.Lerp(startSpawnPointVegetable.position.x, changeParent.transform.position.x, t);
                float y = Mathf.Lerp(startSpawnPointVegetable.position.y, changeParent.transform.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight;
                float z = Mathf.Lerp(startSpawnPointVegetable.position.z, changeParent.transform.position.z, t);
                return new Vector3(x, y, z);
            }
        }
        else
        {
            float x = Mathf.Lerp(obj.transform.position.x, dropPoint.position.x, t);
            float y = Mathf.Lerp(obj.transform.position.y, dropPoint.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight / 10;
            float z = Mathf.Lerp(obj.transform.position.z, dropPoint.position.z, t);
            return new Vector3(x, y, z);
        }

    }

    IEnumerator  DropFood()
    {
        yield return new WaitForSeconds(_actionScript._duration);

        index = listFood.Count - 1;
        Debug.Log("index " + index);
        isBin = true;
        if (listFood.Count > 0)
        {
            // Destroy(transform.GetChild(0).GetChild(index).gameObject);
            ThrowObjectOnArc(listFood[index].gameObject);
            listFood.RemoveAt(index);

            index--;
        }
        yield return null;
    }
}
