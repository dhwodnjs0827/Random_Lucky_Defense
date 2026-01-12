using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public partial class PlayerDataManager
{
    private Dictionary<CurrencyType, int> currency;
    
    public IDictionary<CurrencyType, int> Currency => currency;

    private void InitializeCurrencyData(CurrencySaveData saveData)
    {
        currency = new Dictionary<CurrencyType, int>
        {
            [CurrencyType.Gold] = saveData.Gold,
            [CurrencyType.Gem] = saveData.Gem,
            [CurrencyType.Diamond] = saveData.Diamond
        };
    }

    /// <summary>
    /// 재화량 수정 (+: 증가, -: 감소)
    /// </summary>
    public bool ChangeCurrency(CurrencyType type, int amount, bool isAutoSave = true)
    {
        var currentAmount = currency[type];
        if (currentAmount + amount < 0)
        {
            return false;
        }
        currency[type] += amount;
        
        if (isAutoSave)
        {
            SaveCurrencyData();
        }
        return true;
    }

    public void SaveCurrencyData()
    {
        var saveData = SaveLoadManager.Instance.SaveData;
        saveData.CurrencyData.Gold = currency[CurrencyType.Gold];
        saveData.CurrencyData.Gem = currency[CurrencyType.Gem];
        saveData.CurrencyData.Diamond = currency[CurrencyType.Diamond];
        SaveLoadManager.Instance.SaveAsync(saveData).Forget();
    }
}