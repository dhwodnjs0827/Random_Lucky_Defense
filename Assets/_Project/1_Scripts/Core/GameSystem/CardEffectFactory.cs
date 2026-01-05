using System.Collections.Generic;
using Generated;
using UnityEngine;

public class CardEffectFactory : MonoBehaviour
{
    private List<BuffCardData> globalBuffCardDatas;
    
    public CardEffectFactory()
    {
        
    }

    public BuffCardContainer[] GetRandomCards()
    {
        
        
        return null;
    }

    private void InitializeData()
    {
        var cardDatas = ResourceManager.Instance.LoadAll<BuffCardDataSO>("");
        var cardLevelDatas = ResourceManager.Instance.LoadAll<BuffCardLevelDataSO>("");
    }
}

public struct BuffCardContainer
{
    public string Name;
    public string Description;
    public BuffEffectType EffectType;
    public int Level;
    public float[] EffectValue;
}