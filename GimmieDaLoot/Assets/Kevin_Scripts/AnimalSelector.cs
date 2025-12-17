// using UnityEngine;

public class AnimalSelector : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public Transform previewSpawn;

    [Header("Menu Camera Selection")]
    public string menuCameraName = "FrontView";

    private int currentIndex = 0;
    private GameObject previewInstance;

    void Start() => Show(currentIndex);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
        if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
    }

//     public void Next()
//     {
//         currentIndex = (currentIndex + 1) % animalPrefabs.Length;
//         Show(currentIndex);
//     }

//     public void Prev()
//     {
//         currentIndex = (currentIndex - 1 + animalPrefabs.Length) % animalPrefabs.Length;
//         Show(currentIndex);
//     }

    // private void Show(int index)
    // {
    //     if (Camera.main != null) Camera.main.enabled = false;

    //     if (previewInstance != null) Destroy(previewInstance);

    //     previewInstance = Instantiate(animalPrefabs[index], previewSpawn.position, previewSpawn.rotation);

    //     // Disable all cameras in the spawned animal, then enable only the front view
    //     var cams = previewInstance.GetComponentsInChildren<Camera>(true);
    //     foreach (var cam in cams) cam.enabled = false;

    //     Transform front = previewInstance.transform.Find(menuCameraName);
    //     if (front != null)
    //     {
    //         var frontCam = front.GetComponent<Camera>();
    //         if (frontCam != null) frontCam.enabled = true;
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"Menu camera '{menuCameraName}' not found on {previewInstance.name}. Enable one manually or rename it.");
    //     }
    // }
    private void Show(int index)
{
    // 1) Remove old preview
    if (previewInstance != null)
        Destroy(previewInstance);

    // 2) Spawn new preview
    previewInstance = Instantiate(animalPrefabs[index], previewSpawn.position, previewSpawn.rotation);
        var animators = previewInstance.GetComponentsInChildren<Animator>(true);
foreach (var a in animators)
{
    a.applyRootMotion = false;
    a.speed = 0f; // freezes animation completely (menu preview)
}

// 2) Disable NavMeshAgents (they move transforms)
var agents = previewInstance.GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>(true);
foreach (var ag in agents)
{
    ag.enabled = false;
}

// 3) Disable CharacterControllers (if present)
var controllers = previewInstance.GetComponentsInChildren<CharacterController>(true);
foreach (var cc in controllers)
{
    cc.enabled = false;
}

// 4) Disable ANY Rigidbody just in case (some child may have one)
var rbs = previewInstance.GetComponentsInChildren<Rigidbody>(true);
foreach (var rb in rbs)
{
    rb.useGravity = false;
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
    rb.isKinematic = true;
}


    



    // 3) Disable CameraSwitcher on the spawned prefab (menu only)
    var switcher = previewInstance.GetComponentInChildren<CameraSwitcher>(true);
    if (switcher != null)
        switcher.enabled = false;

    // 4) Disable the scene main camera (menu scene camera)
    if (Camera.main != null)
        Camera.main.enabled = false;

    // 5) Disable all prefab cameras
    var cams = previewInstance.GetComponentsInChildren<Camera>(true);
    foreach (var cam in cams)
        cam.enabled = false;

    // 6) Enable ONLY the camera tagged "FrontView"
    foreach (var cam in cams)
    {
        if (cam.CompareTag("FrontView"))
        {
            cam.enabled = true;
            return;
        }
    }

    Debug.LogWarning("No camera tagged 'FrontView' found on " + previewInstance.name);
}
}
