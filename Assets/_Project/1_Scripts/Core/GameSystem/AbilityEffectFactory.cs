using System;
using System.Collections.Generic;
using System.Linq;
using Generated;
using Random = UnityEngine.Random;

/// <summary>
/// 재능 시스템의 핵심 중개 클래스
/// </summary>
public class AbilityEffectFactory : IEventListener
{
    private readonly Dictionary<AbilityEffectType, List<IAbilityEffect>> effectHandlers = new();

    private AbilityDataSO[] abilityDatas;
    private readonly Dictionary<string, List<AbilityLevelDataSO>> abilityLevelDataDic = new();
    private readonly Dictionary<string, int> currentAbilityLevelDic = new();

    private Action<AbilitySelectEventData> onAbilitySelected;
    
    private const string ABILITY_DATA_SO_PATH = "AbilityData";
    private const string ABILITY_LEVEL_DATA_SO_PATH = "AbilityLevelData";


    public AbilityEffectFactory()
    {
        InitializeData();
    }

    #region IEventListener implementation

    public void SubscribeEvents()
    {
        onAbilitySelected += SelectedAbilityProcess;
        EventManager.Subscribe(GameEventType.AbilitySelected, onAbilitySelected);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.AbilitySelected, onAbilitySelected);
        onAbilitySelected -= SelectedAbilityProcess;
    }

    #endregion

    /// <summary>
    /// 가중치 기반 랜덤 재능 불러오기
    /// <remarks>5레벨 미만 재능만 필터링</remarks>
    /// </summary>
    /// <param name="count">재능 개수(기본값 3장)</param>
    public AbilityContainer[] GetRandomAbilities(int count = 3)
    {
        // 5레벨 미만 재능 필터링
        var availableAbilities = abilityDatas
            .Where(ability => GetAbilityLevel(ability.ID) < GameConstants.ABILITY_MAX_LEVEL).ToList();

        // 가중치 기반 랜덤 선택(중복 없이 count 개수만큼)
        var selectedAbilities = new List<AbilityContainer>();
        for (var i = 0; i < count && availableAbilities.Count > 0; i++)
        {
            var selectedAbility = SelectByWeight(availableAbilities);
            var selectedAbilityCurrentLevel = GetAbilityLevel(selectedAbility.ID);
            selectedAbilities.Add(CreateContainer(selectedAbility, selectedAbilityCurrentLevel));
            availableAbilities.Remove(selectedAbility);
        }

        return selectedAbilities.ToArray();
    }

    /// <summary>
    /// 재능 효과 적용 대상 등록
    /// </summary>
    public void RegisterAbilityEffectHandler(AbilityEffectType type, IAbilityEffect handler)
    {
        if (!effectHandlers.ContainsKey(type))
        {
            effectHandlers[type] = new List<IAbilityEffect>();
        }

        effectHandlers[type].Add(handler);
    }

    /// <summary>
    /// 재능 효과 적용 대상 해제
    /// </summary>
    public void UnregisterAbilityEffectHandler(AbilityEffectType type, IAbilityEffect handler)
    {
        if (effectHandlers.TryGetValue(type, out var handlers))
        {
            handlers.Remove(handler);
        }
    }

    /// <summary>
    /// 재능 및 재능별 레벨 데이터 초기화
    /// </summary>
    private void InitializeData()
    {
        abilityDatas = ResourceManager.Instance.LoadAll<AbilityDataSO>(ABILITY_DATA_SO_PATH);
        foreach (var abilityData in abilityDatas)
        {
            var list = new List<AbilityLevelDataSO>();
            abilityLevelDataDic.TryAdd(abilityData.ID, list);
            currentAbilityLevelDic.TryAdd(abilityData.ID, 0);
        }

        var abilityLevelDatas = ResourceManager.Instance.LoadAll<AbilityLevelDataSO>(ABILITY_LEVEL_DATA_SO_PATH);
        foreach (var abilityLevelData in abilityLevelDatas)
        {
            if (abilityLevelDataDic.TryGetValue(abilityLevelData.AbilityID, out var list))
            {
                list.Add(abilityLevelData);
            }
        }
    }

    /// <summary>
    /// 특정 재능의 현재 레벨 불러오기
    /// </summary>
    /// <param name="abilityID">특정 재능의 ID</param>
    /// <returns>현재 재능 레벨</returns>
    private int GetAbilityLevel(string abilityID)
    {
        return currentAbilityLevelDic.GetValueOrDefault(abilityID, 0);
    }

    /// <summary>
    /// 특정 레벨의 재능 데이터 컨테이너 만들기
    /// </summary>
    /// <param name="abilityData">재능 SO 데이터</param>
    /// <param name="level">재능 레벨</param>
    private AbilityContainer CreateContainer(AbilityDataSO abilityData, int level)
    {
        var levelData = abilityLevelDataDic[abilityData.ID][level];
        var container = new AbilityContainer(abilityData, levelData);
        return container;
    }

    /// <summary>
    /// 가중치 기반 재능 데이터 선별
    /// </summary>
    /// <param name="abilities">선별 가능한 재능 데이터 리스트</param>
    private AbilityDataSO SelectByWeight(List<AbilityDataSO> abilities)
    {
        var totalWeight = abilities.Sum(c => c.Weight);
        var random = Random.Range(0, totalWeight);

        var cumulative = 0;
        foreach (var ability in abilities)
        {
            cumulative += ability.Weight;
            if (random < cumulative)
                return ability;
        }

        return abilities.Last();
    }

    /// <summary>
    /// 재능 선택 프로세스 처리
    /// </summary>
    private void SelectedAbilityProcess(AbilitySelectEventData data)
    {
        if (currentAbilityLevelDic.ContainsKey(data.SelectedAbility.AbilityData.ID))
        {
            currentAbilityLevelDic[data.SelectedAbility.AbilityData.ID]++;
        }

        var effectType = data.SelectedAbility.AbilityData.AbilityEffectType;
        if (effectHandlers.TryGetValue(effectType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                handler.ApplyAbilityEffect(data.SelectedAbility);
            }
        }

        CDebug.Log(
            $"[AbilityEffectFactory] 선택한 재능: {data.SelectedAbility.AbilityData.Name}, 재능 레벨: {currentAbilityLevelDic[data.SelectedAbility.AbilityData.ID]}");
    }

    #region Cheat

#if UNITY_EDITOR

    /// <summary>
    /// 선택 가능한 모든 재능 목록 반환 (치트용)
    /// </summary>
    public AbilityContainer[] CheatGetAvailableAbilities()
    {
        var availableAbilities = abilityDatas
            .Where(ability => GetAbilityLevel(ability.ID) < GameConstants.ABILITY_MAX_LEVEL)
            .ToList();

        var containers = new List<AbilityContainer>();
        foreach (var ability in availableAbilities)
        {
            var currentLevel = GetAbilityLevel(ability.ID);
            containers.Add(CreateContainer(ability, currentLevel));
        }

        return containers.ToArray();
    }

    /// <summary>
    /// 특정 재능 활성화 (치트용)
    /// </summary>
    public void CheatActivateAbility(AbilityContainer ability)
    {
        EventManager.Dispatch(GameEventType.AbilitySelected, new AbilitySelectEventData(ability));
    }

    /// <summary>
    /// 현재 활성화된 재능 목록 반환 (치트용)
    /// </summary>
    public List<(AbilityDataSO Data, int Level)> CheatGetActivatedAbilities()
    {
        var activatedAbilities = new List<(AbilityDataSO Data, int Level)>();

        foreach (var ability in abilityDatas)
        {
            var level = GetAbilityLevel(ability.ID);
            if (level > 0)
            {
                activatedAbilities.Add((ability, level));
            }
        }

        return activatedAbilities;
    }

#endif

    #endregion
}

public struct AbilityContainer
{
    public readonly AbilityDataSO AbilityData;
    public readonly AbilityLevelDataSO AbilityLevelData;

    public AbilityContainer(AbilityDataSO abilityData, AbilityLevelDataSO levelData)
    {
        AbilityData = abilityData;
        AbilityLevelData = levelData;
    }
}