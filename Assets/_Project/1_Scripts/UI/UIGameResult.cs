using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 게임 결과창 Popup UI 클래스
/// </summary>
public class UIGameResult : UIBase
{
    [SerializeField] private Button exitGameButton;
    
    private bool isClickedExitGame = false;
    
    protected override void Opened(params object[] args)
    {
        if (args[0] is not InGameFinishEventData)
        {
            CDebug.LogError("[UIGameResult] 잘못된 매개변수 전달!");
            return;
        }
        InitializeButtons();
        InGameManager.Instance.PauseGame();
        //TODO: 임시로 보상 주는거임
        RewardCurrency();
    }

    protected override void Closed(params object[] args)
    {
    }

    private void InitializeButtons()
    {
        if (exitGameButton != null)
        {
            exitGameButton.onClick.AddListener(OnClickExitGame);
        }
    }

    private void OnClickExitGame()
    {
        if (isClickedExitGame)
        {
            return;
        }
        
        isClickedExitGame = true;
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene).Forget();
    }

    //TODO: 임시 재화 보상
    private void RewardCurrency()
    {
        PlayerDataManager.Instance.ChangeCurrency(CurrencyType.Gold, 1000);
    }
}
