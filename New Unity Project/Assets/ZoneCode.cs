using UnityEngine;

public class ZoneCode : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Hit!");
    }
}
