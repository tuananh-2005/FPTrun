using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadHomeScene()
    {
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void outGame()
    {
        Application.Quit();
    }
}
