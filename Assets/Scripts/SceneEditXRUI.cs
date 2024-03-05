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
    public GameObject CreatePanel;

    public Mesh CubeMesh;
    public Mesh SphereMesh;
    public Mesh CapsuleMesh;
    public Mesh CylinderMesh;
    public Mesh PlaneMesh;

    public Material DefaultMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateCube()
    {
        CreateSyncedMesh(CubeMesh);
    }

    public void CreateSphere()
    {
        CreateSyncedMesh(SphereMesh);
    }

    public void CreateCapsule()
    {
        CreateSyncedMesh(CapsuleMesh);
    }

    public void CreateCylinder()
    {
        CreateSyncedMesh(CylinderMesh);
    }

    public void CreatePlane()
    {
        CreateSyncedMesh(PlaneMesh);
    }

    public void CreateSyncedMesh(Mesh mesh)
    {
        Debug.Log("CreateSyncedMesh");
        GameObject newGameObject = PhotonNetwork.Instantiate(syncedMeshPrefab.name, spawnPoint.position, Quaternion.identity);
        newGameObject.GetComponentInChildren<MeshFilter>().mesh = mesh;
        newGameObject.GetComponentInChildren<MeshRenderer>().material = DefaultMaterial;
        CreatePanel.SetActive(false);

    }

    public void ClearButtonPressed()
    {
        Debug.Log("ClearButtonPressed");
        MeshSync[] meshSyncs = FindObjectsOfType<MeshSync>();
        for(int i = 0; i < meshSyncs.Length; i++)
        {
            PhotonNetwork.Destroy(meshSyncs[i].gameObject);
        }
    }
    public void CreateButtonPressed()
    {
        Debug.Log("CreateButtonPressed");
        CreatePanel.SetActive(!CreatePanel.activeSelf);
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
