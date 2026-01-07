using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
