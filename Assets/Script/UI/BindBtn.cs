using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BindBtn : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    void OnPlayClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
    }
}