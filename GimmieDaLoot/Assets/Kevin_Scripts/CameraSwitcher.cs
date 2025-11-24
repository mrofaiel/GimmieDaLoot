using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    public KeyCode switchKey = KeyCode.V;
    private bool usingCamera1 = true;

    void Start()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            usingCamera1 = !usingCamera1;
            camera1.enabled = usingCamera1;
            camera2.enabled = !usingCamera1;
        }
    }
}
