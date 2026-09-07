using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] PlayerScript player;
    [SerializeField] TextMeshProUGUI healthTesxt;

    void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slider != null && player != null)
        {
            slider.value = player.health;
            healthTesxt.text = player.health.ToString()+" / 100";

        }
    }
}
