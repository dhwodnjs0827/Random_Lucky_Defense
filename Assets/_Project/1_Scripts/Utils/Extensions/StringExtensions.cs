using System;
using System.Text;
using UnityEngine;

/// <summary>
/// string 확장 메서드
/// </summary>
public static class StringExtensions
{
    private static readonly StringBuilder StrBuilder = new();

    /// <summary>
    /// int로 변환 (실패 시, 기본값 반환)
    /// </summary>
    public static int ToInt(this string str, int defaultValue = 0)
    {
        return int.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// float으로 변환 (실패 시, 기본값 반환)
    /// </summary>
    public static float ToFloat(this string str, float defaultValue = 0f)
    {
        return float.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// bool로 변환 (실패 시, 기본값 반환)
    /// </summary>
    public static bool ToBool(this string str, bool defaultValue = false)
    {
        return bool.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// Enum으로 변환 (실패 시, 기본값 반환)
    /// </summary>
    public static T ToEnum<T>(this string str, T defaultValue = default) where T : struct, Enum
    {
        return Enum.TryParse<T>(str, true, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// string에 color 추가
    /// </summary>
    public static string Color(this string s, Color color)
    {
        StrBuilder.Length = 0;
        return StrBuilder.Append($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>").Append(s)
            .Append("</color>").ToString();
    }

    /// <summary>
    /// 플레이스홀더를 실제 값으로 치환
    /// </summary>
    /// <example>
    /// "공격력이 {value}% 증가".ReplaceValues(("{value}", 10));
    /// "공격력 {atk}, 방어력 {def}".ReplaceValues(("{atk}", 100), ("{def}", 50));
    /// </example>
    public static string ReplaceValues(this string str, params (string key, object value)[] replacements)
    {
        foreach (var (key, value) in replacements)
        {
            str = str.Replace(key, value.ToString());
        }
        return str;
    }
}