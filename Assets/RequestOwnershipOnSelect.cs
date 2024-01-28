
using Photon.Pun;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RequestOwnershipOnSelect : MonoBehaviour
{
    public bool RequestOwnershipNow;

    void Update()
    {
        if (RequestOwnershipNow)
        {
            RequestOwnershipNow = false;
            RequestPhotonOwnership();
        }
        // Check if the GameObject is selected in the Unity Editor
#if UNITY_EDITOR
        if (IsObjectSelectedInEditor(gameObject))
        {
            // Request Photon ownership when selected
            RequestPhotonOwnership();
        }
#endif
    }

#if UNITY_EDITOR
    bool IsObjectSelectedInEditor(GameObject obj)
    {
        // Check if the object is selected in the Unity Editor
        return Selection.activeGameObject == obj;
    }
#endif

    void RequestPhotonOwnership()
    {
        // Request Photon ownership logic goes here
        // For example, if this GameObject has a PhotonView, request ownership
        PhotonView photonView = GetComponent<PhotonView>();
        if (photonView != null)
        {
            photonView.RequestOwnership();
        }
    }
}
