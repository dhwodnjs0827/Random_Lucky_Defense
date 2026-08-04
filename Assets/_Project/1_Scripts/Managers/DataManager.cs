using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Generated;

public class DataManager : MonoSingleton<DataManager>
{
    protected override bool isInitialized { get; set; }

    private List<AbilityDataSO> abilityDataList = new();
    private List<AbilityLevelDataSO> abilityLevelDataList = new();
    private List<DamageRateByClassDataSO> damageRateByClassData = new();
    private List<EnemyDataSO> enemyDataList = new();
    private List<HeroDataSO> heroDataList = new();
    private List<InGameHeroLevelUpDataSO> inGameHeroLevelUpDataList = new();
    private List<SummonDataSO> summonDataList = new();
    private List<WaveDataSO> waveDataList = new();
    
    public IList<AbilityDataSO> AbilityDataList => abilityDataList;
    public IList<AbilityLevelDataSO> AbilityLevelDataList => abilityLevelDataList;
    public IList<DamageRateByClassDataSO> DamageRateByClassDataList => damageRateByClassData;
    public IList<EnemyDataSO> EnemyDataList => enemyDataList;
    public IList<HeroDataSO> HeroDataList => heroDataList;
    public IList<InGameHeroLevelUpDataSO> InGameHeroLevelUpDataList => inGameHeroLevelUpDataList;
    public IList<SummonDataSO> SummonDataList => summonDataList;
    public IList<WaveDataSO> WaveDataList => waveDataList;
    
    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        await LoadAbilityDataAsync();
        await LoadAbilityLevelDataAsync();
        await LoadDamageRateByClassDataAsync();
        await LoadEnemyDataAsync();
        await LoadHeroDataAsync();
        await LoadInGameHeroLevelUpDataAsync();
        await LoadSummonDataAsync();
        await LoadWaveDataAsync();
        
        isInitialized = true;
    }

    private async UniTask LoadAbilityDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<AbilityDataSO>(AddressableLabels.ABILITY_DATA);
        abilityDataList = loadedData.ToList();
    }
    
    private async UniTask LoadAbilityLevelDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<AbilityLevelDataSO>(AddressableLabels.ABILITY_LEVEL_DATA);
        abilityLevelDataList = loadedData.ToList();
    }
    
    private async UniTask LoadDamageRateByClassDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<DamageRateByClassDataSO>(AddressableLabels.DAMAGE_RATE_BY_CLASS_DATA);
        damageRateByClassData = loadedData.ToList();
    }
    
    private async UniTask LoadEnemyDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<EnemyDataSO>(AddressableLabels.ENEMY_DATA);
        enemyDataList = loadedData.ToList();
    }
    
    private async UniTask LoadHeroDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<HeroDataSO>(AddressableLabels.HERO_DATA);
        heroDataList = loadedData.ToList();
    }
    
    private async UniTask LoadInGameHeroLevelUpDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<InGameHeroLevelUpDataSO>(AddressableLabels.INGAME_HERO_LEVEL_UP_DATA);
        inGameHeroLevelUpDataList = loadedData.ToList();
    }
    
    private async UniTask LoadSummonDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<SummonDataSO>(AddressableLabels.SUMMON_DATA);
        summonDataList = loadedData.ToList();
    }
    
    private async UniTask LoadWaveDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<WaveDataSO>(AddressableLabels.WAVE_DATA);
        waveDataList = loadedData.OrderBy(i => i.WaveIndex).ToList();
    }
}
