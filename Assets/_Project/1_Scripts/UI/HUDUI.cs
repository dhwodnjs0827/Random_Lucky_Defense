using UnityEngine;
using UnityEngine.UI;

public class HUDUI : UIBase
{
    [Header("Buttons")]
    [SerializeField] private Button spawnButton;
    [SerializeField] private Button magicianLevelUpButton;
    [SerializeField] private Button archerLevelUpButton;
    [SerializeField] private Button warriorLevelUpButton;
    [SerializeField] private Button exchangeButton;
    [SerializeField] private Button sellButton;

    protected override void Opened(params object[] args)
    {
        InitializeButtons();
        SubscribeEvents();
    }

    protected override void Closed(params object[] args)
    {
        ClearButtons();
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        
    }

    private void UnsubscribeEvents()
    {
        
    }

    private void InitializeButtons()
    {
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(OnClickSpawnButton);
        }

        if (magicianLevelUpButton != null)
        {
            magicianLevelUpButton.onClick.AddListener(OnClickMagicianLevelUpButton);
        }

        if (archerLevelUpButton != null)
        {
            archerLevelUpButton.onClick.AddListener(OnClickArcherLevelUpButton);
        }

        if (warriorLevelUpButton != null)
        {
            warriorLevelUpButton.onClick.AddListener(OnClickWarriorLevelUpButton);
        }

        if (exchangeButton != null)
        {
            exchangeButton.onClick.AddListener(OnClickExchangeButton);
        }

        if (sellButton != null)
        {
            sellButton.onClick.AddListener(OnClickSellButton);
        }
    }

    private void ClearButtons()
    {
        if (spawnButton != null)
        {
            spawnButton.onClick.RemoveAllListeners();
        }

        if (magicianLevelUpButton != null)
        {
            magicianLevelUpButton.onClick.RemoveAllListeners();
        }

        if (archerLevelUpButton != null)
        {
            archerLevelUpButton.onClick.RemoveAllListeners();
        }

        if (warriorLevelUpButton != null)
        {
            warriorLevelUpButton.onClick.RemoveAllListeners();
        }

        if (exchangeButton != null)
        {
            exchangeButton.onClick.RemoveAllListeners();
        }

        if (sellButton != null)
        {
            sellButton.onClick.RemoveAllListeners();
        }
    }
    
    private void OnClickSpawnButton()
    {
        CDebug.Log("[HUDUI] 영웅 소환 버튼 클릭");
    }

    private void OnClickMagicianLevelUpButton()
    {
        CDebug.Log("[HUDUI] 마법사 레벨 업 버튼 클릭");
    }
    
    private void OnClickArcherLevelUpButton()
    {
        CDebug.Log("[HUDUI] 궁수 레벨 업 버튼 클릭");
    }
    
    private void OnClickWarriorLevelUpButton()
    {
        CDebug.Log("[HUDUI] 전사 레벨 업 버튼 클릭");
    }
    
    private void OnClickExchangeButton()
    {
        CDebug.Log("[HUDUI] 영웅 교환 버튼 클릭");
    }

    private void OnClickSellButton()
    {
        CDebug.Log("[HUDUI] 영웅 판매 버튼 클릭");
    }
}