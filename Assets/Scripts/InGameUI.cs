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
    [SerializeField] Slider mageBossSlider;
    [SerializeField] BossMageScript mageBoss;
    private bool mageBossBattleStarted;

    void Awake()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerScript>();
        }

        if (mageBoss == null)
        {
            mageBoss = FindFirstObjectByType<BossMageScript>();
        }

        if (mageBossSlider != null)
        {
            mageBossSlider.gameObject.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mageBossBattleStarted && mageBoss != null && mageBossSlider != null
            && mageBoss.stateMachine.currentState == mageBoss.mageBattleState)
        {
            mageBossSlider.value = mageBoss.health;
            mageBossSlider.gameObject.SetActive(true);
            mageBossBattleStarted = true;
        }

        if (mageBossBattleStarted && mageBoss != null && mageBossSlider != null)
        {
            mageBossSlider.value = mageBoss.health;
        }

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
