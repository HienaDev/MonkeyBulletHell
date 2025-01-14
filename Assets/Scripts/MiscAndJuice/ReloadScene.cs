using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private UnityEvent doOnRestart;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            doOnRestart.Invoke();
        }
    }

    public void ReloadCurrentScene()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the active scene
        SceneManager.LoadScene(currentScene.name);
    }
}
