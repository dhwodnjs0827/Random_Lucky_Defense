using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 닫기 버튼용 클래스
/// </summary>
[RequireComponent(typeof(Button))]
public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private BaseUI targetUI; // 닫을 UI (없으면 부모에서 찾음)

    private void Awake()
    {
        button ??= GetComponent<Button>();
        targetUI ??= GetComponentInParent<BaseUI>();

        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        // AudioManager.Instance.PlaySFX("UI_Click");
        UIManager.Instance.Close(targetUI);
    }
}
