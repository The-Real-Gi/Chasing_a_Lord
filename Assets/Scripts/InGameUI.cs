using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider energySlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI energyText;

    void Awake()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerScript>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (hpSlider != null)
        {
            hpSlider.value = player.health;
            healthText.text= player.health.ToString()+" / 100";
        }

        if (energySlider != null)
        {
            energySlider.value = player.energyValue;
            energyText.text = player.energyValue.ToString()+" / 100";

        }
    }
}
