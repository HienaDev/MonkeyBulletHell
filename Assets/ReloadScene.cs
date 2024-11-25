using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            ReloadCurrentScene();
    }

    public void ReloadCurrentScene()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the active scene
        SceneManager.LoadScene(currentScene.name);
    }
}
