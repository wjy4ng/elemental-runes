using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    public static BossController Instance { get; private set; }

    [Header("보스 스탯")]
    public int maxHp = 80000;
    public int currentHp;

    [Header("UI 연결")]
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public Image bossImage;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateHpUI();
    }

    // 데미지 받기
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(0, currentHp);
        UpdateHpUI();

        Debug.Log($"보스 피격! 데미지: {damage}, 남은 HP: {currentHp}");

        if (currentHp <= 0)
        {
            OnBossDead();
        }
    }

    void UpdateHpUI()
    {
        if (hpSlider != null)
            hpSlider.value = (float)currentHp / maxHp;

        if (hpText != null)
            hpText.text = $"{currentHp} / {maxHp}";
    }

    void OnBossDead()
    {
        Debug.Log("보스 처치!");
        GameManager.Instance.ChangeState(GameManager.GameState.Clear);
    }
}