using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEditXRUI : MonoBehaviour
{
    public GameObject syncedMeshPrefab;
    public Transform spawnPoint;
    public Transform GameBoard;
    public GameObject DebugPanel;

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
        Debug.Log("CreateSyncedMesh");
        PhotonNetwork.Instantiate(syncedMeshPrefab.name, spawnPoint.position, Quaternion.identity);
    }

    public void ClearButtonPressed()
    {
        Debug.Log("ClearButtonPressed");

    }
    public void CreateButtonPressed()
    {
        Debug.Log("CreateButtonPressed");

    }
    public void NoteButtonPressed()
    {
        Debug.Log("NoteButtonPressed");

    }
    public void DebugLogButtonPressed()
    {
        Debug.Log("DebugLogButtonPressed");
        DebugPanel.SetActive(!DebugPanel.activeSelf);
    }

}
