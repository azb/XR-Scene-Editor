using UnityEngine;
using Photon.Pun;
using System.IO;

public class MaterialSync : MonoBehaviourPun
{
    [SerializeField]
    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Make sure to own the PhotonView for the object
            if (photonView.IsMine)
            {
                // Call the method to send the material's texture over the network
                Texture2D textureToSend = (Texture2D)meshRenderer.material.mainTexture;
                byte[] textureData = Texture2DToByteArray(textureToSend);
                SendMaterialTexture(textureData);
            }
        }
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
