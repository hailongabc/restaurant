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
    public List<GameObject> listFood = new List<GameObject>();
    public float arcHeight = 1f;
    public float throwDuration = 2f;
    private int index;
    private Coroutine getFood;
    private Coroutine dropFood;
    private Coroutine cleanFood;
    private GameObject vegetable;
    private GameObject beef;

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
            getFood = StartCoroutine(GetFood(other));

        }
        if (other.CompareTag("Trash"))
        {
            dropFood = StartCoroutine(DropFood());
        }
        if (other.CompareTag("Customer"))
        {
            if (listFood.Count > 0)
            {
                if (!other.GetComponent<CustomerMovement>().isEated)
                {
                    DropFoodCustomer(other);
                }
            }
        }
        if (other.CompareTag("Clean"))
        {
            if (other.transform.Find("diaban(Clone)"))
            {
                cleanFood = StartCoroutine(Clean(other));
            }
        }
        if (other.CompareTag("Bill"))
        {
            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        _actionScript.StopAction();

        if (other.CompareTag("Trash"))
        {
            StopCoroutine(dropFood);

        }
        if (other.CompareTag("FoodSpawner"))
        {
            StopCoroutine(getFood);
        }
        if (other.CompareTag("Clean"))
        {
            if (other.transform.Find("diaban(Clone)"))
            {
                StopCoroutine(cleanFood);
            }
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
                    listFood.Add(beef);
                    StartCoroutine(ThrowCoroutine(beef));
                    break;
                case KitchenObject.ItemType.Beef:
                    GameObject vegetable = Instantiate(kitchenObject.beefPf, startSpawnPointBeef.position, Quaternion.identity);
                    listFood.Add(vegetable);
                    StartCoroutine(ThrowCoroutine(vegetable));
                    break;
            }
            getFood = StartCoroutine(GetFood(other));

        }
        yield return null;
    }


    IEnumerator ThrowCoroutine(GameObject obj)
    {
        if (obj.transform.name == "beef(Clone)")
        {
            // Chờ một khoảng thời gian để đảm bảo vật thể đã được instantiate
            obj.transform.SetParent(changeParent.transform);
            yield return new WaitForSeconds(0.05f);

            float elapsedTime = 0f;
            while (elapsedTime < throwDuration)
            {
                // Tính toán thời gian đã trôi qua kể từ khi bắt đầu
                elapsedTime += Time.deltaTime;

                // Tính toán vị trí trên đường cung
                float t = elapsedTime / throwDuration;

                Vector3 currentPos = CalculateArcPointBeef(t);
                // Di chuyển vật thể đến vị trí hiện tại
                obj.transform.position = currentPos;
                yield return null;
            }
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            if (listFood.Count > 1)
            {
                obj.transform.localPosition = new Vector3(listFood[listFood.Count - 2].transform.localPosition.x, listFood[listFood.Count - 2].transform.localPosition.y + 0.145f, listFood[listFood.Count - 2].transform.localPosition.z);
            }
            // Hoặc bạn có thể thực hiện các công việc khác sau khi ném vật thể hoàn thành
            // Ví dụ: Hủy bỏ vật thể, hiển thị hiệu ứng, v.v.
        }
        else
        {
            // Chờ một khoảng thời gian để đảm bảo vật thể đã được instantiate
            obj.transform.SetParent(changeParent.transform);
            yield return new WaitForSeconds(0.05f);

            float elapsedTime = 0f;
            while (elapsedTime < throwDuration)
            {
                // Tính toán thời gian đã trôi qua kể từ khi bắt đầu
                elapsedTime += Time.deltaTime;

                // Tính toán vị trí trên đường cung
                float t = elapsedTime / throwDuration;

                Vector3 currentPos = CalculateArcPointVegetable(t);
                // Di chuyển vật thể đến vị trí hiện tại
                obj.transform.position = currentPos;
                yield return null;
            }
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            if (listFood.Count > 1)
            {
                obj.transform.localPosition = new Vector3(listFood[listFood.Count - 2].transform.localPosition.x, listFood[listFood.Count - 2].transform.localPosition.y + 0.145f, listFood[listFood.Count - 2].transform.localPosition.z);
            }
        }

    }

    IEnumerator ThrowCoroutineDropFood(GameObject obj)
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
            Vector3 currentPos = CalculateArcPointDropFood(t, obj);

            // Di chuyển vật thể đến vị trí hiện tại
            obj.transform.position = currentPos;
            yield return null;
        }
        Destroy(obj.gameObject);
        // Hoặc bạn có thể thực hiện các công việc khác sau khi ném vật thể hoàn thành
        // Ví dụ: Hủy bỏ vật thể, hiển thị hiệu ứng, v.v.

    }

    IEnumerator ThrowCoroutineDropFoodCustomer(GameObject obj, Collider other)
    {
        obj.transform.SetParent(other.transform);
       
            // Chờ một khoảng thời gian để đảm bảo vật thể đã được instantiate
            yield return new WaitForSeconds(0.05f);

            float elapsedTime = 0f;
            while (elapsedTime < throwDuration)
            {
                // Tính toán thời gian đã trôi qua kể từ khi bắt đầu
                elapsedTime += Time.deltaTime;

                // Tính toán vị trí trên đường cung
                float t = elapsedTime / throwDuration;
                Vector3 currentPos = CalculateArcPointDropFoodChair(t, obj, other);

                // Di chuyển vật thể đến vị trí hiện tại
                obj.transform.position = currentPos;
                yield return null;
                other.GetComponent<CustomerMovement>().orderView.SetActive(false);

            }
            // Hoặc bạn có thể thực hiện các công việc khác sau khi ném vật thể hoàn thành
            // Ví dụ: Hủy bỏ vật thể, hiển thị hiệu ứng, v.v.
     

    }
    Vector3 CalculateArcPointBeef(float t)
    {
        // Sử dụng hàm Lerp để tính toán vị trí trên đường cung
        float x = Mathf.Lerp(startSpawnPointBeef.position.x, changeParent.transform.position.x, t);
        float y = Mathf.Lerp(startSpawnPointBeef.position.y, changeParent.transform.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight;
        float z = Mathf.Lerp(startSpawnPointBeef.position.z, changeParent.transform.position.z, t);
        return new Vector3(x, y, z);
    }

    Vector3 CalculateArcPointVegetable(float t)
    {
        // Sử dụng hàm Lerp để tính toán vị trí trên đường cung
        float x = Mathf.Lerp(startSpawnPointVegetable.position.x, changeParent.transform.position.x, t);
        float y = Mathf.Lerp(startSpawnPointVegetable.position.y, changeParent.transform.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight;
        float z = Mathf.Lerp(startSpawnPointVegetable.position.z, changeParent.transform.position.z, t);
        return new Vector3(x, y, z);
    }

    Vector3 CalculateArcPointDropFood(float t, GameObject obj)
    {
        // Sử dụng hàm Lerp để tính toán vị trí trên đường cung
        float x = Mathf.Lerp(obj.transform.position.x, dropPoint.position.x, t);
        float y = Mathf.Lerp(obj.transform.position.y, dropPoint.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight / 10;
        float z = Mathf.Lerp(obj.transform.position.z, dropPoint.position.z, t);
        return new Vector3(x, y, z);
    }

    Vector3 CalculateArcPointDropFoodChair(float t, GameObject obj, Collider other)
    {
        // Sử dụng hàm Lerp để tính toán vị trí trên đường cung
        float x = Mathf.Lerp(obj.transform.position.x, other.transform.GetComponent<CustomerMovement>().dropPointFood.position.x, t);
        float y = Mathf.Lerp(obj.transform.position.y, other.transform.GetComponent<CustomerMovement>().dropPointFood.position.y, t) + Mathf.Sin(t * Mathf.PI) * arcHeight / 10;
        float z = Mathf.Lerp(obj.transform.position.z, other.transform.GetComponent<CustomerMovement>().dropPointFood.position.z, t);
        return new Vector3(x, y, z);
    }

    IEnumerator DropFood()
    {
        yield return new WaitForSeconds(_actionScript._duration / 2);
        if (listFood.Count > 0)
        {
            index = listFood.Count - 1;
            // Destroy(transform.GetChild(0).GetChild(index).gameObject);
            StartCoroutine(ThrowCoroutineDropFood(listFood[index].gameObject));
            listFood.RemoveAt(index);
            dropFood = StartCoroutine(DropFood());
        }
        yield return null;
    }

    private void DropFoodCustomer(Collider other)
    {
        var getOrderView = other.GetComponent<CustomerMovement>().orderView.GetComponent<OrderScript>();
        var getCustomerChair = other.GetComponent<CustomerMovement>();
        if (getOrderView.spriteRenderer.sprite.name == "Vegetable")
        {
            var searchValue = "vegetable(Clone)";
            for (int i = listFood.Count - 1; i >= 0; i--)
            {
                if (listFood[i].transform.name == searchValue)
                {
                    vegetable = listFood[i];
                    listFood.RemoveAt(i);
                    break;
                }
                else
                {
                    vegetable = null;
                }
            }
            if (vegetable != null)
            {
                getCustomerChair.isEated = true;
                getCustomerChair.Eat();
                StartCoroutine(ThrowCoroutineDropFoodCustomer(vegetable, other));
            }
        }
        else if (getOrderView.spriteRenderer.sprite.name == "BeefSteak")
        {
            var searchValue = "beef(Clone)";
            for (int i = listFood.Count - 1; i >= 0; i--)
            {
                if (listFood[i].transform.name == searchValue)
                {
                    beef = listFood[i];
                    listFood.RemoveAt(i);
                    break;
                }
                else
                {
                    beef = null;
                }
            }
            if (beef != null)
            {
                getCustomerChair.isEated = true;
                getCustomerChair.Eat();
                StartCoroutine(ThrowCoroutineDropFoodCustomer(beef, other));
            }

        }
    }

    IEnumerator Clean(Collider other)
    {
        _actionScript.StartAction();
        yield return new WaitForSeconds(_actionScript._duration);
        Destroy(other.transform.Find("diaban(Clone)").gameObject);
        yield return null;
    }
}
