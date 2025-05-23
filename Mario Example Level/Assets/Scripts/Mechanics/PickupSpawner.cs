using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject[] pickupPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Length)], transform.position, Quaternion.identity);
}
