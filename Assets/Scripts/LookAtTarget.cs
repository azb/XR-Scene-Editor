
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (Camera.main != null)
        {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        }    
    }
}
