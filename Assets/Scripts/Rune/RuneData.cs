using UnityEngine;

public enum RuneColor
{
    Green,   // 베기
    Red,     // 불
    Blue,    // 얼음
    Yellow,  // 전기
    Light,   // 빛
    Dark,    // 어둠
    Neutral  // 무속성
}

[System.Serializable]
public class RuneData
{
    public int number;
    public RuneColor color;

    public RuneData(int number, RuneColor color)
    {
        this.number = number;
        this.color = color;
    }
}