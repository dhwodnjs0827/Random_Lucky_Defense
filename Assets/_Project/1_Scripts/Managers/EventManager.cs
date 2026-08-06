using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 이벤트 매니저 - 전역 이벤트 시스템
/// </summary>
public static class EventManager
{
    private static readonly IDictionary<GameEventType, Action> events = new Dictionary<GameEventType, Action>();
    private static readonly IDictionary<GameEventType, Delegate> genericEvents = new Dictionary<GameEventType, Delegate>();

    #region Subscribe

    /// <summary>
    /// 이벤트 구독 (매개변수 없음)
    /// </summary>
    [HideInCallstack]
    public static void Subscribe(GameEventType gameEventType, Action action)
    {
        if (events.TryGetValue(gameEventType, out var existingAction))
        {
            // 중복 체크
            if (existingAction.GetInvocationList().Contains(action))
            {
                CDebug.LogWarning($"[EventManager] 중복된 이벤트를 등록하려고 했습니다. GameEventType: {gameEventType}");
                return;
            }

            events[gameEventType] = existingAction + action;
        }
        else
        {
            events[gameEventType] = action;
        }
    }

    /// <summary>
    /// 이벤트 구독 (매개변수 있음)
    /// </summary>
    [HideInCallstack]
    public static void Subscribe<T>(GameEventType gameEventType, Action<T> action)
    {
        if (genericEvents.TryGetValue(gameEventType, out var existingAction))
        {
            // 중복 체크
            if (existingAction.GetInvocationList().Contains(action))
            {
                CDebug.LogWarning($"[EventManager] 중복된 이벤트를 등록하려고 했습니다. GameEventType: {gameEventType}");
                return;
            }

            genericEvents[gameEventType] = Delegate.Combine(existingAction, action);
        }
        else
        {
            genericEvents[gameEventType] = action;
        }
    }

    #endregion

    #region Unsubscribe

    /// <summary>
    /// 이벤트 구독 해제 (매개변수 없음)
    /// </summary>
    [HideInCallstack]
    public static void Unsubscribe(GameEventType gameEventType, Action action)
    {
        if (!events.TryGetValue(gameEventType, out var existingAction))
        {
            CDebug.LogWarning($"[EventManager] 등록된 이벤트가 없습니다. GameEventType: {gameEventType}");
            return;
        }

        // 등록된 메서드인지 체크
        if (!existingAction.GetInvocationList().Contains(action))
        {
            CDebug.LogWarning($"[EventManager] 등록되지 않은 이벤트를 해제하려고 했습니다. GameEventType: {gameEventType}");
            return;
        }

        var newAction = existingAction - action;

        // 남은 구독자가 없으면 딕셔너리에서 제거
        if (newAction == null)
        {
            events.Remove(gameEventType);
        }
        else
        {
            events[gameEventType] = newAction;
        }
    }

    /// <summary>
    /// 이벤트 구독 해제 (매개변수 있음)
    /// </summary>
    [HideInCallstack]
    public static void Unsubscribe<T>(GameEventType gameEventType, Action<T> action)
    {
        if (!genericEvents.TryGetValue(gameEventType, out var existingAction))
        {
            CDebug.LogWarning($"[EventManager] 등록된 이벤트가 없습니다. GameEventType: {gameEventType}");
            return;
        }

        // 등록된 메서드인지 체크
        if (!existingAction.GetInvocationList().Contains(action))
        {
            CDebug.LogWarning($"[EventManager] 등록되지 않은 이벤트를 해제하려고 했습니다. GameEventType: {gameEventType}");
            return;
        }

        var newAction = Delegate.Remove(existingAction, action);

        // 남은 구독자가 없으면 딕셔너리에서 제거
        if (newAction == null)
        {
            genericEvents.Remove(gameEventType);
        }
        else
        {
            genericEvents[gameEventType] = newAction;
        }
    }

    #endregion

    #region Dispatch

    /// <summary>
    /// 이벤트 발행 (매개변수 없음)
    /// </summary>
    public static void Dispatch(GameEventType gameEventType)
    {
        if (events.TryGetValue(gameEventType, out var action))
        {
            action?.Invoke();
        }
    }

    /// <summary>
    /// 이벤트 발행 (매개변수 있음)
    /// </summary>
    public static void Dispatch<T>(GameEventType gameEventType, T eventData)
    {
        if (genericEvents.TryGetValue(gameEventType, out var action))
        {
            (action as Action<T>)?.Invoke(eventData);
        }
    }

    #endregion
    
    #region Clear

    /// <summary>
    /// 모든 이벤트 초기화
    /// </summary>
    public static void Clear()
    {
        events.Clear();
        genericEvents.Clear();
    }

    /// <summary>
    /// 특정 이벤트 초기화
    /// </summary>
    public static void Clear(GameEventType gameEventType)
    {
        events.Remove(gameEventType);
        genericEvents.Remove(gameEventType);
    }

    #endregion
}