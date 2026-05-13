using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("전투 설정")]
    public float attackInterval = 2f;
    public float gameDuration = 180f; // 3분
    private float timeRemaining;
    private bool isBattling = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = gameDuration;
        StartBattle();
    }

    void Update()
    {
        if (!isBattling) return;

        timeRemaining -= Time.deltaTime;

        // 타이머 UI 업데이트
        TimerManager.Instance?.UpdateTimer(timeRemaining);

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            OnTimeUp();
        }
    }

    public void StartBattle()
    {
        isBattling = true;
        StartCoroutine(BattleLoop());
    }

    public void StopBattle()
    {
        isBattling = false;
        StopAllCoroutines();
    }

    IEnumerator BattleLoop()
    {
        while (isBattling)
        {
            yield return new WaitForSeconds(attackInterval);
            Attack();
        }
    }

    void Attack()
    {
        int totalDamage = 0;

        for (int y = 0; y < GridManager.Instance.rows; y++)
        {
            for (int x = 0; x < GridManager.Instance.columns; x++)
            {
                RuneController rune = RuneSpawner.Instance.GetRune(x, y);
                if (rune != null)
                {
                    totalDamage += CalculateDamage(rune.GetData());
                }
            }
        }

        if (totalDamage > 0)
        {
            BossController.Instance.TakeDamage(totalDamage);
            Debug.Log($"총 데미지: {totalDamage}");
        }
    }

    // 데미지 계산: 2^(숫자-1) × 10
    public int CalculateDamage(RuneData data)
    {
        int baseDamage = (int)(Mathf.Pow(2, data.number - 1) * 10);
        return baseDamage;
    }

    // 다른 색 합치기 시 속성 보너스 데미지
    public int CalculateMergeBonusDamage(RuneData data)
    {
        float multiplier = GetColorMultiplier(data.color);
        int baseDamage = CalculateDamage(data);
        return (int)(baseDamage * multiplier);
    }

    float GetColorMultiplier(RuneColor color)
    {
        switch (color)
        {
            case RuneColor.Red:     return 2.0f;
            case RuneColor.Blue:    return 1.5f;
            case RuneColor.Green:   return 1.8f;
            case RuneColor.Yellow:  return 2.2f;
            case RuneColor.Light:   return 2.5f;
            case RuneColor.Dark:    return 2.3f;
            case RuneColor.Neutral: return 1.3f;
            default: return 1.0f;
        }
    }

    void OnTimeUp()
    {
        StopBattle();
        if (BossController.Instance.currentHp > 0)
        {
            Debug.Log("시간 초과! 실패");
            GameManager.Instance.ChangeState(GameManager.GameState.Fail);
        }
    }
}