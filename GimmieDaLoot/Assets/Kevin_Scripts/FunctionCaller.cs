using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionCaller : MonoBehaviour
{
    [Header("Scene to load when Space is pressed or button clicked")]
    [SerializeField] private string sceneName;

    [Header("Enable Spacebar activation?")]
    public bool enableSpaceActivation = true;

    void Start()
    {
        // Make sure cursor is visible & unlocked for UI scenes
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (enableSpaceActivation && Input.GetKeyDown(KeyCode.Space))
        {
            Call();    // same function the button calls
        }
    }

    public void Call()
    {
        SceneManager.LoadScene(sceneName);
    }
}
