
using Photon.Pun;
using UnityEngine;

public class TransformSync : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Vector3 networkScale;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable control on objects that are not owned by this client
            // For example, disable player input or other scripts.
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // Smoothly lerp towards the network position and rotation
            transform.localPosition = Vector3.Lerp(transform.localPosition, networkPosition, Time.deltaTime * 10f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, networkRotation, Time.deltaTime * 10f);
            transform.localScale = Vector3.Lerp(transform.localScale, networkScale, Time.deltaTime * 10f);
        }
    }

    #region IPunObservable Implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // This client owns the PhotonView
            // Send data to others (networkPosition, networkRotation, etc.)
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            stream.SendNext(transform.localScale);
        }
        else
        {
            // Network client, receive data
            // Update networkPosition, networkRotation, etc.
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            networkScale = (Vector3)stream.ReceiveNext();
        }
    }

    #endregion
}