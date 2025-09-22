using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform), typeof(Button))]
public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Settings")]
    [Range(0, 1)] public float clickChance = 0.3f;
    public Vector2 padding = new Vector2(50, 50);
    public float moveCooldown = 0.5f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 screenSize;
    private bool isClickable;
    private bool canMove = true;
    private Button button;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        button = GetComponent<Button>();
        UpdateScreenSize();

        // Ensure button remains visually interactive
        button.interactable = true;
    }

    void UpdateScreenSize()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        screenSize = canvasRect.rect.size;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canMove) return;

        if (Random.value < clickChance)
        {
            // Become clickable
            isClickable = true;
        }
        else
        {
            // Move and become unclickable
            MoveButton();
            isClickable = false;
            StartCoroutine(Cooldown());
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            QuitGame();
        }
    }

    void MoveButton()
    {
        UpdateScreenSize();

        Vector2 buttonSize = rectTransform.rect.size;
        float maxX = (screenSize.x / 2) - (buttonSize.x / 2) - padding.x;
        float minX = (-screenSize.x / 2) + (buttonSize.x / 2) + padding.x;
        float maxY = (screenSize.y / 2) - (buttonSize.y / 2) - padding.y;
        float minY = (-screenSize.y / 2) + (buttonSize.y / 2) + padding.y;

        rectTransform.anchoredPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    IEnumerator Cooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(moveCooldown);
        canMove = true;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}