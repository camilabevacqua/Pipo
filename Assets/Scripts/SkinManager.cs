using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [Header("Skins")]
    public RuntimeAnimatorController[] skins;

    [Header("Pipo principal")]
    public Animator pipoAnimator;

    [Header("Preview")]
    public Animator previewAnimator;

    [Header("UI")]
    public Button selectButton;
    public TextMeshProUGUI selectButtonText;

    [Header("Panel")]
    public GameObject skinsPanel;

    private int currentIndex = 0;
    private int selectedSkin = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        selectedSkin =
            PlayerPrefs.GetInt("SelectedSkin", 0);

        currentIndex = selectedSkin;

        ApplySkin();

        UpdatePreview();
    }

    // =========================
    // FLECHAS
    // =========================

    public void NextSkin()
    {
        currentIndex++;

        if (currentIndex >= skins.Length)
            currentIndex = 0;

        UpdatePreview();
    }

    public void PreviousSkin()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = skins.Length - 1;

        UpdatePreview();
    }

    // =========================
    // SELECT
    // =========================

    public void SelectSkin()
    {
        selectedSkin = currentIndex;

        PlayerPrefs.SetInt(
            "SelectedSkin",
            selectedSkin
        );

        PlayerPrefs.Save();

        ApplySkin();

        UpdatePreview();
    }

    // =========================
    // APPLY
    // =========================

    void ApplySkin()
    {
        if (pipoAnimator != null)
        {
            pipoAnimator.runtimeAnimatorController =
                skins[selectedSkin];
        }
    }

    public RuntimeAnimatorController GetSelectedSkin()
    {
        return skins[selectedSkin];
    }

    // =========================
    // PREVIEW
    // =========================

    void UpdatePreview()
    {
        if (skins.Length == 0)
            return;

        if (previewAnimator != null)
        {
            previewAnimator.runtimeAnimatorController =
                skins[currentIndex];
        }

        if (currentIndex == selectedSkin)
        {
            selectButtonText.text = "Selected";

            selectButton.interactable = false;
        }
        else
        {
            selectButtonText.text = "Select";

            selectButton.interactable = true;
        }
    }

    // =========================
    // PANEL
    // =========================

    public void CloseSkinsPanel()
    {
        if (skinsPanel != null)
        {
            skinsPanel.SetActive(false);
        }
    }
}