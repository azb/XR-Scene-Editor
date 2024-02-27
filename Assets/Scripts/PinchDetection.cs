
using Photon.Pun;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    public Transform GameBoard;
    void Start()
    {

    }

    void Update()
    {/*
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            // Pinch event detected
            Debug.Log("Pinch detected!");

            // Add your pinch-related actions or interactions here
        }
        // || OVRInput.Get(OVRInput.Button.One)


        if (OVRInput.Get(OVRInput.Button.Two))
        {
            GameBoard.position = hand.transform.position + hand.transform.forward * .5f;

            float rotation = hand.transform.rotation.eulerAngles.y;

            GameBoard.rotation = Quaternion.Euler(0, rotation, 0);
        }

        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            // Pinch event detected
            Debug.Log("Pinch detected!");

            GameBoard.position = hand.transform.position - hand.transform.right * .5f;

            float rotation = hand.transform.rotation.eulerAngles.y + 90;

            GameBoard.rotation = Quaternion.Euler(0, rotation, 0);

            // Add your pinch-related actions or interactions here
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        Debug.Log("OnTriggerEnter for gameObject " + gameObject.name + " collided with: " + other.name);
        if (gameObject.activeInHierarchy)
        {
            Grabbable grabbable = other.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                Debug.Log("Requesting ownership of " + other.gameObject.name);
                PhotonView photonView = other.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    photonView.RequestOwnership();
                }
                else
                {
                    Debug.Log("Other trigger's photonview is null. This: " + gameObject.name + " other: " + other.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Game object " + other.gameObject.name + " does not have a Grabbable");
            }
        }
        else
        {
            Debug.Log("Game object is not active in the hierarchy " + gameObject.name);
        }
        */
    }
}
