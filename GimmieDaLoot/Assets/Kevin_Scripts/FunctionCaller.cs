using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionCaller : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void Call()
    {
        SceneManager.LoadScene(sceneName);
    }
}
