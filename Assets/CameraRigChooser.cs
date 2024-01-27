using UnityEngine;
using UnityEngine.XR;

public class CameraRigChooser : MonoBehaviour
{
    public GameObject VRCameraRig;
    public GameObject PCCameraRig;
    public Canvas UI;

    public enum Mode { VR, PC };
    public Mode mode = Mode.VR;

    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isEditor)
        {
            if (XRSettings.isDeviceActive)
            {
                string activeDeviceName = XRSettings.loadedDeviceName;
                Debug.Log("VR headset is connected: " + activeDeviceName);
                mode = Mode.VR;
            }
            else
            {
                Debug.Log("No VR headset is connected.");
                mode = Mode.PC;
                if (UI != null)
                {
                    UI.renderMode = RenderMode.ScreenSpaceOverlay;
                }
            }
        }
        else
        {
            if (mode == Mode.PC)
            {
                if (UI != null)
                {
                    UI.renderMode = RenderMode.ScreenSpaceOverlay;
                }
            }
        }

        Debug.Log("Mode = "+mode);

        VRCameraRig.SetActive(mode == Mode.VR);
        PCCameraRig.SetActive(mode == Mode.PC);
    }
}
