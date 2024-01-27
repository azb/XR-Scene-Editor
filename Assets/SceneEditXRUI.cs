using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEditXRUI : MonoBehaviour
{
    public GameObject syncedMeshPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSyncedMesh()
    {
        PhotonNetwork.Instantiate(syncedMeshPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
