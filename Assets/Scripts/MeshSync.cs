using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MeshSync : MonoBehaviourPun, IPunObservable
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    private int chunkSize = 100; // Adjust the chunk size based on your needs
    private int currentChunkIndex = 0;

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
        InvokeRepeating("UpdateTimer", 5f, 0.1f); // Adjust the interval between chunks
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Updating mesh");
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        if (meshFilter.mesh != null)
        {
            // Modify mesh data locally
            // For example, you can deform the mesh based on user input
            // You can modify vertices, UVs, etc.
            // ...

            // Call the method to send updates over the network
            SendMeshData();
        }
        else
        {
            SendSetMeshToNull();
        }
    }

    void SendSetMeshToNull()
    {
        photonView.RPC("ReceiveSetMeshToNull", RpcTarget.Others);
    }

    [PunRPC]
    void ReceiveSetMeshToNull()
    {
        meshFilter.mesh = null;
    }

    void SendMeshData()
    {
        int totalChunks = Mathf.CeilToInt(meshFilter.mesh.vertices.Length / (float)chunkSize);

        if (currentChunkIndex < totalChunks)
        {
            int startIndex = currentChunkIndex * chunkSize;
            int endIndex = Mathf.Min((currentChunkIndex + 1) * chunkSize, meshFilter.mesh.vertices.Length);

            Vector3[] chunkVertices = new Vector3[endIndex - startIndex];
            Vector2[] chunkUVs = new Vector2[endIndex - startIndex];
            int[] chunkTriangles = new int[meshFilter.mesh.triangles.Length]; // Send all triangles every time
            Vector3[] chunkNormals = new Vector3[endIndex - startIndex];

            System.Array.Copy(meshFilter.mesh.vertices, startIndex, chunkVertices, 0, endIndex - startIndex);
            System.Array.Copy(meshFilter.mesh.uv, startIndex, chunkUVs, 0, endIndex - startIndex);
            System.Array.Copy(meshFilter.mesh.normals, startIndex, chunkNormals, 0, endIndex - startIndex);

            photonView.RPC("UpdateMeshData", RpcTarget.Others, chunkVertices, chunkUVs, chunkTriangles, chunkNormals);

            currentChunkIndex++;
        }
        else
        {
            CancelInvoke("UpdateTimer"); // Stop the repeating update if all chunks are sent
        }
    }

    [PunRPC]
    void UpdateMeshData(Vector3[] vertices, Vector2[] uv, int[] triangles, Vector3[] normals)
    {
        if (meshFilter.mesh == null)
        {
            meshFilter.mesh = new Mesh();
        }

        // Update the mesh on other clients
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.uv = uv;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.normals = normals;
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
