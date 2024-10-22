using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    public Button speedButton;
    public Slider speedSlider;

    private void Start()
    {
        speedSlider.gameObject.SetActive(false);
        
        speedButton.onClick.AddListener(ToggleSpeedSlider);
        
        speedSlider.onValueChanged.AddListener(ChangeGameSpeed);
    }

    private void ToggleSpeedSlider()
    {
        speedSlider.gameObject.SetActive(!speedSlider.gameObject.activeSelf);
    }

    private void ChangeGameSpeed(float value)
    {
        Time.timeScale = value;
    }
}
