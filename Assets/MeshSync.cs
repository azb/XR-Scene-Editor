using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MeshSync : MonoBehaviourPun, IPunObservable
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Make sure to own the PhotonView for the object
        if (photonView.IsMine)
        {
            // Enable mesh modifications only for the owner
            meshFilter.mesh.MarkDynamic();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (meshFilter.mesh != null)
            {
                if (photonView.IsMine)
                {
                    // Modify mesh data locally
                    // For example, you can deform the mesh based on user input
                    // You can modify vertices, UVs, etc.
                    // ...

                    // Call the method to send updates over the network
                    SendMeshData();
                }
            }
        }
    }

    void SendMeshData()
    {
        photonView.RPC("UpdateMeshData", RpcTarget.Others, meshFilter.mesh.vertices, meshFilter.mesh.uv);
    }

    [PunRPC]
    void UpdateMeshData(Vector3[] vertices, Vector2[] uv)
    {
        Debug.Log("PunRPC UpdateMeshData called");
        if (meshFilter.mesh == null)
        {
            meshFilter.mesh = new Mesh();
        }

        Debug.Log("vertices.Length = " + vertices.Length);

        for (int i = 0 ; i < vertices.Length ; i++)
        {
            Debug.Log("Adding vertex: "+ vertices[i]);
        }
        // Update the mesh on other clients
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.uv = uv;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Serialize and deserialize custom mesh data if needed
        if (stream.IsWriting)
        {
            // Write custom data to the stream
            // For example, if you have additional mesh data to sync
            // stream.SendNext(customData);
        }
        else
        {
            // Read custom data from the stream
            // For example, if you have additional mesh data to sync
            // customData = (CustomDataType)stream.ReceiveNext();
        }
    }
}
