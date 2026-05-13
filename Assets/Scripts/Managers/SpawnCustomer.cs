using UnityEngine;

public class SpawnCustomer : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;

    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(CustomerSpawn), 2f, spawnInterval);
    }

    void CustomerSpawn()
    {
        if (GameManager.instance.customerExists)
        return;

        int randomIndex = Random.Range(0, customerPrefabs.Length);

        Instantiate(
            customerPrefabs[randomIndex],
            spawnPoint.position,
            Quaternion.identity
        );
        GameManager.instance.customerExists = true;
    }
    
}