using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TempExitGameButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(LoadLobby);
    }

    private void LoadLobby()
    {
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene).Forget();
    }
}
