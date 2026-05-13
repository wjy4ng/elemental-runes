using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("그리드 설정")]
    public int columns = 5;
    public int rows = 3;

    [Header("슬롯 설정")]
    public GameObject slotPrefab;
    public float slotSize = 180f;
    public float spacing = 10f;

    [Header("여백 설정")]
    public float paddingLeft = 40f;
    public float paddingRight = 40f;
    public float paddingTop = 40f;
    public float paddingBottom = 40f;

    private GameObject[,] slots;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        slots = new GameObject[columns, rows];

        float totalWidth = columns * slotSize + (columns - 1) * spacing;
        float totalHeight = rows * slotSize + (rows - 1) * spacing;

        float startX = -totalWidth / 2 + slotSize / 2;
        float startY = totalHeight / 2 - slotSize / 2;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 pos = new Vector2(
                    startX + x * (slotSize + spacing),
                    startY - y * (slotSize + spacing)
                );

                GameObject slot = Instantiate(slotPrefab, transform);
                slot.GetComponent<RectTransform>().anchoredPosition = pos;
                slot.name = $"Slot({x},{y})";

                slots[x, y] = slot;
            }
        }
    }

    public GameObject GetSlot(int x, int y)
    {
        if (x < 0 || x >= columns || y < 0 || y >= rows) return null;
        return slots[x, y];
    }

    public bool IsSlotEmpty(int x, int y)
    {
        return true;
    }
}