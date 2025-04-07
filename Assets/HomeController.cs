using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleSound();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    bool audioOff = false;
    public void ToggleSound()
    {
        audioOff = !audioOff;

        AudioListener.pause = audioOff;
    }
}
