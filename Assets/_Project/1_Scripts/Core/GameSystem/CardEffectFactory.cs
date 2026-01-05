using System;
using System.Collections.Generic;
using System.Linq;
using Generated;
using Random = UnityEngine.Random;

public class CardEffectFactory : IEventListener
{
    private List<IBuffCardEffect> effectHandlers = new();
    
    private BuffCardDataSO[] buffCardDatas;
    private readonly Dictionary<int, List<BuffCardLevelDataSO>> buffCardLevelDatas = new();
    private readonly Dictionary<int, int> currentCardLevels = new();
    
    private Action<GameBuffCardSelectEventData> onCardSelected;

    public CardEffectFactory()
    {
        InitializeData();
        SubscribeEvents();
    }
    
    public void SubscribeEvents()
    {
        onCardSelected += SelectedCardProcess;
        EventManager.Subscribe(GameEventType.BuffCardSelected, onCardSelected);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.BuffCardSelected, onCardSelected);
        onCardSelected -= SelectedCardProcess;
    }

    public BuffCardContainer[] GetRandomCards(int count = 3)
    {
        // 5레벨 미만 카드 필터링
        var availableCards = buffCardDatas.Where(card => GetCardLevel(card.ID) < GameConstants.CARD_MAX_LEVEL).ToList();

        // 가중치 기반 랜덤 선택(중복 없이 count 개수만큼)
        var selectedCards = new List<BuffCardContainer>();
        for (var i = 0; i < count && availableCards.Count > 0; i++)
        {
            var selected = SelectByWeight(availableCards);
            var selectedCardCurrentLevel = GetCardLevel(selected.ID);
            selectedCards.Add(CreateContainer(selected, selectedCardCurrentLevel));
            availableCards.Remove(selected);
        }

        return selectedCards.ToArray();
    }

    public void LevelUpCard(int cardId)
    {
        currentCardLevels.TryAdd(cardId, 0);

        currentCardLevels[cardId]++;
    }

    private void InitializeData()
    {
        buffCardDatas = ResourceManager.Instance.LoadAll<BuffCardDataSO>("Data/SO/BuffCardData");
        foreach (var cardData in buffCardDatas)
        {
            var list = new List<BuffCardLevelDataSO>();
            buffCardLevelDatas.TryAdd(cardData.ID, list);
            currentCardLevels.TryAdd(cardData.ID, 0);
        }

        var cardLevelDatas = ResourceManager.Instance.LoadAll<BuffCardLevelDataSO>("Data/SO/BuffCardLevelData");
        foreach (var cardLevelData in cardLevelDatas)
        {
            if (buffCardLevelDatas.TryGetValue(cardLevelData.CardID, out var list))
            {
                list.Add(cardLevelData);
            }
        }
    }
    
    private int GetCardLevel(int cardId)
    {
        return currentCardLevels.GetValueOrDefault(cardId, 0);
    }

    private BuffCardContainer CreateContainer(BuffCardDataSO cardData, int level)
    {
        var levelData = buffCardLevelDatas[cardData.ID][level];
        var container = new BuffCardContainer(cardData, levelData);
        return container;
    }

    private BuffCardDataSO SelectByWeight(List<BuffCardDataSO> cards)
    {
        int totalWeight = cards.Sum(c => c.Weight);
        int random = Random.Range(0, totalWeight);

        int cumulative = 0;
        foreach (var card in cards)
        {
            cumulative += card.Weight;
            if (random < cumulative)
                return card;
        }

        return cards.Last();
    }

    private void SelectedCardProcess(GameBuffCardSelectEventData data)
    {
        if (currentCardLevels.ContainsKey(data.SelectedCard.CardID))
        {
            currentCardLevels[data.SelectedCard.CardID]++;
            CDebug.Log($"[CardEffectFactory] {data.SelectedCard.Name} 선택. 현재 레벨{currentCardLevels[data.SelectedCard.CardID]}");
        }

        foreach (var handler in effectHandlers)
        {
            handler.ApplyEffect(data.SelectedCard);
            CDebug.Log($"[CardEffectFactory] 카드 효과 적용");
        }
    }
}

public struct BuffCardContainer
{
    public int CardID;
    public string Name;
    public string Description;
    public BuffEffectType EffectType;
    public int Level;
    public int CurrentLevel;
    public List<float> EffectValue;

    public BuffCardContainer(BuffCardDataSO cardData, BuffCardLevelDataSO levelData)
    {
        CardID = cardData.ID;
        Name = cardData.Name;
        Description = cardData.Description;
        EffectType = cardData.BuffEffectType;
        Level = levelData.Level;
        CurrentLevel = Level - 1;
        EffectValue = new List<float> { levelData.value, levelData.value1 };
    }
}