// using UnityEngine;

// public class AnimalSelector : MonoBehaviour
// {
//     public GameObject[] animalPrefabs;
//     public Transform previewSpawn;

//     [Header("Menu Camera Selection")]
//     public string menuCameraName = "FrontView";

//     private int currentIndex = 0;
//     private GameObject previewInstance;

//     void Start() => Show(currentIndex);

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
//         if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
//     }

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

    
//     private void Show(int index)
// {
//     // 1) Remove old preview
//     if (previewInstance != null)
//         Destroy(previewInstance);

//     // 2) Spawn new preview
//     previewInstance = Instantiate(animalPrefabs[index], previewSpawn.position, previewSpawn.rotation);
//         var animators = previewInstance.GetComponentsInChildren<Animator>(true);
// foreach (var a in animators)
// {
//     a.applyRootMotion = false;
//     a.speed = 0f; // freezes animation completely (menu preview)
// }

// // 2) Disable NavMeshAgents (they move transforms)
// var agents = previewInstance.GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>(true);
// foreach (var ag in agents)
// {
//     ag.enabled = false;
// }

// // 3) Disable CharacterControllers (if present)
// var controllers = previewInstance.GetComponentsInChildren<CharacterController>(true);
// foreach (var cc in controllers)
// {
//     cc.enabled = false;
// }

// // 4) Disable ANY Rigidbody just in case (some child may have one)
// var rbs = previewInstance.GetComponentsInChildren<Rigidbody>(true);
// foreach (var rb in rbs)
// {
//     rb.useGravity = false;
//     rb.velocity = Vector3.zero;
//     rb.angularVelocity = Vector3.zero;
//     rb.isKinematic = true;
// }


    



//     // 3) Disable CameraSwitcher on the spawned prefab (menu only)
//     var switcher = previewInstance.GetComponentInChildren<CameraSwitcher>(true);
//     if (switcher != null)
//         switcher.enabled = false;

//     // 4) Disable the scene main camera (menu scene camera)
//     if (Camera.main != null)
//         Camera.main.enabled = false;

//     // 5) Disable all prefab cameras
//     var cams = previewInstance.GetComponentsInChildren<Camera>(true);
//     foreach (var cam in cams)
//         cam.enabled = false;

//     // 6) Enable ONLY the camera tagged "FrontView"
//     foreach (var cam in cams)
//     {
//         if (cam.CompareTag("FrontView"))
//         {
//             cam.enabled = true;
//             return;
//         }
//     }

//     Debug.LogWarning("No camera tagged 'FrontView' found on " + previewInstance.name);
// }
// }


using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalSelector : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public Transform previewSpawn;

    [Header("Menu Camera Selection")]
    public string menuCameraName = "FrontView";

    private int currentIndex = 0;
    private GameObject previewInstance;

    // Key used to store the selection
    private const string PREF_KEY = "SelectedAnimalIndex";

    void Start()
    {
        // Load saved selection (default 0)
        currentIndex = PlayerPrefs.GetInt(PREF_KEY, 0);

        // Clamp in case your prefab array size changed
        if (animalPrefabs != null && animalPrefabs.Length > 0)
            currentIndex = Mathf.Clamp(currentIndex, 0, animalPrefabs.Length - 1);
        else
            currentIndex = 0;

        Show(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();
        if (Input.GetKeyDown(KeyCode.RightArrow)) Next();

        // OPTIONAL: press Enter to "confirm" selection (still just saves here)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SaveSelection();
            SceneManager.LoadScene("Marcelina_MapScene"); // if you want to load gameplay
        }
    }

    public void Next()
    {
        if (animalPrefabs == null || animalPrefabs.Length == 0) return;

        currentIndex = (currentIndex + 1) % animalPrefabs.Length;
        SaveSelection();
        Show(currentIndex);
    }

    public void Prev()
    {
        if (animalPrefabs == null || animalPrefabs.Length == 0) return;

        currentIndex = (currentIndex - 1 + animalPrefabs.Length) % animalPrefabs.Length;
        SaveSelection();
        Show(currentIndex);
    }

    private void SaveSelection()
    {
        PlayerPrefs.SetInt(PREF_KEY, currentIndex);
        PlayerPrefs.Save();
    }

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

        // Disable NavMeshAgents (they move transforms)
        var agents = previewInstance.GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>(true);
        foreach (var ag in agents)
            ag.enabled = false;

        // Disable CharacterControllers (if present)
        var controllers = previewInstance.GetComponentsInChildren<CharacterController>(true);
        foreach (var cc in controllers)
            cc.enabled = false;

        // Disable ANY Rigidbody just in case (some child may have one)
        var rbs = previewInstance.GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in rbs)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Disable CameraSwitcher on the spawned prefab (menu only)
        var switcher = previewInstance.GetComponentInChildren<CameraSwitcher>(true);
        if (switcher != null)
            switcher.enabled = false;

        // Disable the scene main camera (menu scene camera)
        if (Camera.main != null)
            Camera.main.enabled = false;

        // Disable all prefab cameras
        var cams = previewInstance.GetComponentsInChildren<Camera>(true);
        foreach (var cam in cams)
            cam.enabled = false;

        // Enable ONLY the camera tagged "FrontView"
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
