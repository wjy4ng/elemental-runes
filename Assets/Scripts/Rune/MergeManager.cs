using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance { get; private set; }

    private RuneController draggingRune; // 드래그 중인 룬

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 드래그 시작
    public void OnBeginDrag(RuneController rune)
    {
        draggingRune = rune;
        Debug.Log($"드래그 시작: {rune.GetData().color} {rune.GetData().number}");
    }

    // 드래그 끝 - 어느 슬롯에 놓았는지 판별
    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingRune == null) return;

        // 마우스/터치 위치에서 룬 찾기
        RuneController targetRune = GetRuneAtPosition(eventData.position);

        if (targetRune == null || targetRune == draggingRune)
        {
            draggingRune = null;
            return;
        }

        TryMerge(draggingRune, targetRune);
        draggingRune = null;
    }

    // 화면 위치에서 룬 찾기
    private RuneController GetRuneAtPosition(Vector2 screenPos)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = screenPos;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            RuneController rune = result.gameObject.GetComponent<RuneController>();
            if (rune != null && rune != draggingRune)
                return rune;

            // 부모에서도 찾기
            rune = result.gameObject.GetComponentInParent<RuneController>();
            if (rune != null && rune != draggingRune)
                return rune;
        }

        return null;
    }

    // 합치기 시도
    private void TryMerge(RuneController from, RuneController to)
    {
        RuneData fromData = from.GetData();
        RuneData toData = to.GetData();

        // 숫자가 다르면 합치기 불가
        if (fromData.number != toData.number)
        {
            Debug.Log("숫자가 달라서 합치기 불가");
            return;
        }

        Vector2Int toPos = to.GetGridPosition();
        Vector2Int fromPos = from.GetGridPosition();

        if (fromData.color == toData.color)
        {
            // 같은 색 + 같은 숫자 → 같은 색, 숫자 +1
            RuneData newData = new RuneData(toData.number + 1, toData.color);
            RuneSpawner.Instance.ReplaceRune(toPos.x, toPos.y, newData);
            Debug.Log($"같은 색 합치기! {toData.color} {toData.number} → {newData.number}");
        }
        else
        {
            // 다른 색 + 같은 숫자 → 랜덤 색, 숫자 유지 + 보너스 데미지 즉시 적용
            RuneColor randomColor = (RuneColor)Random.Range(0,
                System.Enum.GetValues(typeof(RuneColor)).Length);
            RuneData newData = new RuneData(toData.number, randomColor);
            RuneSpawner.Instance.ReplaceRune(toPos.x, toPos.y, newData);

            // 속성 폭발 데미지 즉시 보스에게 적용
            int bonusDamage = BattleManager.Instance.CalculateMergeBonusDamage(newData);
            BossController.Instance.TakeDamage(bonusDamage);
            Debug.Log($"속성 폭발! 보너스 데미지: {bonusDamage}");
        }

        // 출발 룬 삭제
        RuneSpawner.Instance.RemoveRune(fromPos.x, fromPos.y);
    }
}