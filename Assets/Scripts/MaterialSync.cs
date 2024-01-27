using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.XR;

public class MaterialSync : MonoBehaviourPun
{
    [SerializeField]
    MeshRenderer meshRenderer;
    public Shader shader;
    MeshSync meshSync;

    Material previousMaterial;
    Texture previousTexture;
    bool allowDownloadMesh;

    private void Start()
    {
        meshSync = GetComponent<MeshSync>();
        allowDownloadMesh = meshSync.allowDownloadMesh;
        previousMaterial = null;
        previousTexture = null;
        meshRenderer = GetComponent<MeshRenderer>();
        //Invoke("UpdateTimer", 1f);
    }

    private void Update()
    {
        //if on xr device don't push mesh updates to other clients
        if (!allowDownloadMesh && XRSettings.isDeviceActive) //(!photonView.IsMine)
            return;
        if (meshRenderer != null)
        {
            //if (meshRenderer.sharedMaterial != null)
            //{
                if (meshRenderer.sharedMaterial != previousMaterial)
                    //|| meshRenderer.sharedMaterial.mainTexture != previousTexture)
                {
                    previousMaterial = meshRenderer.sharedMaterial;
                    //previousTexture = meshRenderer.sharedMaterial.mainTexture;
                    Debug.Log("Updating material");
                    UpdateTimer();
                }
            //}
        }
    }

    void UpdateTimer()
    {
        // Make sure to own the PhotonView for the object
        //if (photonView.IsMine)
        //{
        if (meshRenderer.material != null)
        {
            if (meshRenderer.material.mainTexture != null)
            {
                // Call the method to send the material's texture over the network
                Texture2D textureToSend = (Texture2D)meshRenderer.material.mainTexture;
                byte[] textureData = Texture2DToByteArray(textureToSend);
                SendMaterialTexture(textureData);
            }
            else
            {
                SendSetTextureToNull();
                //Debug.LogError("Can't sync because meshRenderer.material.mainTexture is null");
            }
        }
        else
        {
            SendSetMaterialToNull();
            //Debug.LogError("Can't sync because mesh is null");
        }
        //}
        //Invoke("UpdateTimer", 1f);
    }

    void SendSetMaterialToNull()
    {
        photonView.RPC("ReceiveSetMaterialToNull", RpcTarget.Others);
    }

    [PunRPC]
    void ReceiveSetMaterialToNull()
    {
        meshRenderer.material = null;
    }

    void SendSetTextureToNull()
    {
        photonView.RPC("ReceiveSetTextureToNull", RpcTarget.Others);
    }

    [PunRPC]
    void ReceiveSetTextureToNull()
    {
        meshRenderer.material.mainTexture = null;
    }

    void SendMaterialTexture(byte[] textureData)
    {
        photonView.RPC("ReceiveMaterialTexture", RpcTarget.Others, textureData);
    }

    [PunRPC]
    void ReceiveMaterialTexture(byte[] receivedTextureData)
    {
        // Process the received texture data
        Texture2D receivedTexture = ByteArrayToTexture2D(receivedTextureData);

        if (meshRenderer.material == null)
        {
            meshRenderer.material = new Material(shader);
        }

        // Apply the received texture to the material
        meshRenderer.material.mainTexture = receivedTexture;
    }

    byte[] Texture2DToByteArray(Texture2D texture)
    {
        return texture.EncodeToPNG();
    }

    Texture2D ByteArrayToTexture2D(byte[] textureData)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(textureData);

        return texture;
    }
}
