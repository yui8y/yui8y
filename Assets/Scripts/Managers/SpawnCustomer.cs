using UnityEngine;

public class SpawnCustomer : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public string[] customerCharIds;  // prefabsと同じ順番でcharIdを入力する

    public Transform spawnPoint;

    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(CustomerSpawn), 2f, spawnInterval);
    }

    void CustomerSpawn()
    {
        if (GameManager.instance.customerExists == true)
        {
            return;
        }

        int randomIndex = Random.Range(0, customerPrefabs.Length);
        GameData.currentCharId = customerCharIds[randomIndex];  // charIdを保存！

        Instantiate(
            customerPrefabs[randomIndex],
            spawnPoint.position,
            Quaternion.identity
        );
        GameManager.instance.customerExists = true;
    }
    
}