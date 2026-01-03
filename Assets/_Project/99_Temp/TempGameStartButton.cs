using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TempGameStartButton : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;

    private void Awake()
    {
        if (gameStartButton != null)
        {
            gameStartButton.onClick.AddListener(GameStart);
        }
    }

    private void GameStart()
    {
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.GameScene).Forget();
    }
}
