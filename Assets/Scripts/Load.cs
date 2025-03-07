using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
