using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 시작 버튼용 UI 클래스
/// </summary>
public class GameStartButtonComponent : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    
    private bool isGameStartClicked = false;

    private void Awake()
    {
        if (gameStartButton != null)
        {
            gameStartButton.onClick.AddListener(GameStart);
        }
    }

    private void GameStart()
    {
        if (isGameStartClicked)
        {
            return;
        }
        isGameStartClicked = true;
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.GameScene).Forget();
    }
}
