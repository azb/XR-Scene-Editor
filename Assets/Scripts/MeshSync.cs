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
        Invoke("UpdateTimer", .1f);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Updating mesh");
            UpdateTimer();
        }
        */
    }

    void UpdateTimer()
    {
        if (photonView.IsMine)
        {
            if (meshFilter.mesh != null)
            {
                // Modify mesh data locally
                // For example, you can deform the mesh based on user input
                // You can modify vertices, UVs, etc.
                // ...

                // Call the method to send updates over the network
                SendMeshData();
                if (trianglesPosition < meshFilter.mesh.triangles.Length - 1)
                {
                    Invoke("UpdateTimer", .01f);
                }
            }
            else
            {
                SendSetMeshToNull();
                Invoke("UpdateTimer", 1f);
            }
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

    int verticesPosition = 0;
    int trianglesPosition = 0;

    void SendMeshData()
    {
        if (meshFilter.mesh != null && meshFilter.mesh.vertices.Length > 0)
        {
            int verticesLength = meshFilter.mesh.vertices.Length;
            int trianglesLength = meshFilter.mesh.triangles.Length;

            if (verticesPosition < meshFilter.mesh.vertices.Length - 1)
            {
                verticesPosition++;
            }
            else
            {
                //verticesPosition = 0;
            }

            if (trianglesPosition < meshFilter.mesh.triangles.Length - 1)
            {
                trianglesPosition++;
            }
            else
            {
                //trianglesPosition = 0;
                if (trianglesPosition == meshFilter.mesh.triangles.Length)
                {
                    return;
                }
            }

            photonView.RPC("UpdateMeshData", RpcTarget.Others,
                meshFilter.mesh.vertices[verticesPosition],
                meshFilter.mesh.uv[verticesPosition],
                meshFilter.mesh.triangles[trianglesPosition],
                meshFilter.mesh.normals[verticesPosition],
                verticesPosition,
                trianglesPosition,
                verticesLength,
                trianglesLength
                );
        }
        /*
    Debug.Log("meshFilter.mesh.vertices.Length = " + meshFilter.mesh.vertices.Length);
    Debug.Log("meshFilter.mesh.uv.Length = " + meshFilter.mesh.uv.Length);
    Debug.Log("meshFilter.mesh.triangles.Length = " + meshFilter.mesh.triangles.Length);
    Debug.Log("meshFilter.mesh.normals.Length = " + meshFilter.mesh.normals.Length);*/
    }

    Vector3[] vertexBuffer;
    Vector2[] uvBuffer;
    int[] triangleBuffer;
    Vector3[] normalBuffer;

    [PunRPC]
    void UpdateMeshData(
        Vector3 vertex,
        Vector2 uv,
        int triangle,
        Vector3 normal,
        int verticesPosition,
        int trianglesPosition,
        int verticesLength,
        int trianglesLength
    )
    {
        if (photonView.IsMine)
            return;
        Debug.Log("PunRPC UpdateMeshData called");
        if (meshFilter.mesh == null)
        {
            Debug.Log("Creating new mesh because current one is null");
            meshFilter.mesh = new Mesh();
        }

        if (meshFilter.mesh.vertices == null || meshFilter.mesh.vertices.Length != verticesLength)
        {
            Debug.Log("Creating new mesh vertices because current one is null");
            meshFilter.mesh.vertices = new Vector3[verticesLength];
            meshFilter.mesh.uv = new Vector2[verticesLength];
            meshFilter.mesh.normals = new Vector3[verticesLength];

            vertexBuffer = new Vector3[verticesLength];
            uvBuffer = new Vector2[verticesLength];
            normalBuffer = new Vector3[verticesLength];
        }

        if (meshFilter.mesh.triangles == null || meshFilter.mesh.triangles.Length != trianglesLength)
        {
            Debug.Log("Creating new mesh triangles because current one is null");
            meshFilter.mesh.triangles = new int[trianglesLength];
            triangleBuffer = new int[trianglesLength];
        }

        Debug.Log("verticesPosition = " + verticesPosition + " / " + verticesLength + "\ntrianglesPosition = " + trianglesPosition + " / " + trianglesLength);

        // Update the mesh on other clients
        vertexBuffer[verticesPosition] = vertex;
        uvBuffer[verticesPosition] = uv;
        triangleBuffer[trianglesPosition] = triangle;
        normalBuffer[verticesPosition] = normal;

        meshFilter.mesh.vertices = vertexBuffer;
        meshFilter.mesh.uv = uvBuffer;
        meshFilter.mesh.triangles = triangleBuffer;
        meshFilter.mesh.normals = normalBuffer;
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
