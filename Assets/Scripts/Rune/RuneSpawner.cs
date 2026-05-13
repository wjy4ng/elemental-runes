using UnityEngine;
using System.Collections.Generic;

public class RuneSpawner : MonoBehaviour
{
    public static RuneSpawner Instance { get; private set; }

    [Header("룬 설정")]
    public GameObject runePrefab;

    private RuneController[,] runeGrid;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        runeGrid = new RuneController[
            GridManager.Instance.columns,
            GridManager.Instance.rows
        ];
    }

    public void SpawnRune()
    {
        // 빈 슬롯 목록 전체 수집
        List<Vector2Int> emptySlots = GetAllEmptySlots();

        if (emptySlots.Count == 0)
        {
            Debug.Log("빈 슬롯 없음");
            return;
        }

        // 랜덤한 빈 슬롯 선택
        Vector2Int slot = emptySlots[Random.Range(0, emptySlots.Count)];
        int x = slot.x;
        int y = slot.y;

        // 숫자는 항상 1, 색상만 랜덤
        RuneColor randomColor = (RuneColor)Random.Range(0, System.Enum.GetValues(typeof(RuneColor)).Length);
        RuneData data = new RuneData(1, randomColor);

        // 슬롯 위치에 룬 생성
        GameObject slotObj = GridManager.Instance.GetSlot(x, y);
        GameObject runeObj = Instantiate(runePrefab, slotObj.transform);

        RectTransform rt = runeObj.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(180, 180);

        RuneController rune = runeObj.GetComponent<RuneController>();
        rune.Setup(data, new Vector2Int(x, y));

        runeGrid[x, y] = rune;
    }

    // 빈 슬롯 전체 리스트 반환
    private List<Vector2Int> GetAllEmptySlots()
    {
        List<Vector2Int> emptySlots = new List<Vector2Int>();

        for (int y = 0; y < GridManager.Instance.rows; y++)
        {
            for (int x = 0; x < GridManager.Instance.columns; x++)
            {
                if (runeGrid[x, y] == null)
                    emptySlots.Add(new Vector2Int(x, y));
            }
        }

        return emptySlots;
    }

    public void RemoveRune(int x, int y)
    {
        if (runeGrid[x, y] != null)
        {
            Destroy(runeGrid[x, y].gameObject);
            runeGrid[x, y] = null;
        }
    }

    // 특정 위치 룬 교체
    public void ReplaceRune(int x, int y, RuneData newData)
    {
        // 기존 룬 삭제
        RemoveRune(x, y);

        // 새 룬 생성
        GameObject slotObj = GridManager.Instance.GetSlot(x, y);
        GameObject runeObj = Instantiate(runePrefab, slotObj.transform);

        RectTransform rt = runeObj.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(180, 180);

        RuneController rune = runeObj.GetComponent<RuneController>();
        rune.Setup(newData, new Vector2Int(x, y));

        runeGrid[x, y] = rune;
    }

    public RuneController GetRune(int x, int y) => runeGrid[x, y];
}