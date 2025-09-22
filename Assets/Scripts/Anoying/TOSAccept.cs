using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TOSAccept : MonoBehaviour
{
    public ScrollRect tosScroll;
    public Button acceptButton;
    public Text buttonText;
    public float requiredScroll = 1.0f;
    public float cooldownTime = 5f;
    public GameObject TOSPanel;
    public GameObject OptionsPanel;

    private bool isCooldown;
    private Color originalColor;
    private bool timerCompleted; // New flag

    void Start()
    {
        TOSPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        acceptButton.interactable = false;
        originalColor = acceptButton.image.color;
        acceptButton.onClick.AddListener(OnAcceptClicked);
        timerCompleted = false;
    }

    void Update()
    {
        if (!isCooldown && !timerCompleted)
        {
            bool scrolledEnough = tosScroll.verticalNormalizedPosition <= (1 - requiredScroll);
            acceptButton.interactable = scrolledEnough;
            UpdateButtonAppearance(scrolledEnough);
        }
    }

    void UpdateButtonAppearance(bool isActive)
    {
        acceptButton.image.color = isActive ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        buttonText.text = isActive ? "ACCEPT" : "SCROLL MORE";
    }

    void OnAcceptClicked()
    {
        if (!timerCompleted)
        {
            // First click - start timer
            StartCoroutine(StartCooldown());
        }
        else
        {
            // Second click - toggle UI
            TOSPanel.SetActive(false);
            OptionsPanel.SetActive(true);
            timerCompleted = false;
        }
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        acceptButton.interactable = false;

        // Phase 1: Countdown
        float timer = cooldownTime;
        while (timer > 0)
        {
            buttonText.text = "WAIT " + Mathf.CeilToInt(timer) + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        // Phase 2: Enable confirmation
        timerCompleted = true;
        acceptButton.interactable = true;
        buttonText.text = "ACCEPT";
        isCooldown = false;
    }

    public void ReturnToTOS()
    {
        TOSPanel.SetActive(true);
        OptionsPanel.SetActive(false);
        tosScroll.verticalNormalizedPosition = 1;
        timerCompleted = false;
        acceptButton.interactable = false;
        UpdateButtonAppearance(false);
    }
}