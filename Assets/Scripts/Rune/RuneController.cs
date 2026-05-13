using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RuneController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("컴포넌트")]
    public Image runeImage;
    public TextMeshProUGUI numberText;

    private RuneData data;
    private Vector2Int gridPosition; // 그리드 상 내 위치

    private Color GetRuneColor(RuneColor color)
    {
        switch (color)
        {
            case RuneColor.Green:   return new Color(0.2f, 0.8f, 0.2f);
            case RuneColor.Red:     return new Color(0.9f, 0.15f, 0.15f);
            case RuneColor.Blue:    return new Color(0.2f, 0.5f, 1f);
            case RuneColor.Yellow:  return new Color(1f, 0.85f, 0.1f);
            case RuneColor.Light:   return new Color(1f, 1f, 0.85f);
            case RuneColor.Dark:    return new Color(0.35f, 0.05f, 0.5f);
            case RuneColor.Neutral: return new Color(0.6f, 0.5f, 0.4f);
            default: return Color.white;
        }
    }

    public void Setup(RuneData runeData, Vector2Int pos)
    {
        data = runeData;
        gridPosition = pos;
        runeImage.color = GetRuneColor(data.color);
        numberText.text = data.number.ToString();
    }

    public RuneData GetData() => data;
    public Vector2Int GetGridPosition() => gridPosition;

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        MergeManager.Instance.OnBeginDrag(this);
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData) { }

    // 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        MergeManager.Instance.OnEndDrag(eventData);
    }
}