using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitSettings : MonoBehaviour, IPointerClickHandler
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        QuitGame();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}