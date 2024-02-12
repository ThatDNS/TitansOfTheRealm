using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Player;

/// <summary>
/// Manages the UI representation of player currencies, specifically Soulstones.
/// </summary>
public class CurrencyUIManager : MonoBehaviour
{
    #region Fields and Properties
    public Player player;
    public GameObject currencyUIPrefab;
    public Transform contentPanel;

    // Dictionary to map currency data to their UI elements.
    private Dictionary<StonesDataSO, GameObject> currencyUIMap = new Dictionary<StonesDataSO, GameObject>();
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        EnsureContentPanelIsSet();
    }

    private void Start()
    {
        InitializeUI();
    }

    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<SoulstoneUpdatedEvent>(OnSoulstoneUpdated);
        }
    }
    #endregion

    #region Initialization
    /// <summary>
    /// Ensures necessary references are set, specifically the content panel.
    /// </summary>
    private void EnsureContentPanelIsSet()
    {
        if (contentPanel == null)
        {
            contentPanel = this.transform;
        }
    }

    /// <summary>
    /// Initializes the UI elements for each currency. Subscribes to currency update events.
    /// </summary>
    private void InitializeUI()
    {
        FindPlayerReference();
        InitializeCurrencyUIElements();
        SubscribeToCurrencyUpdates();
    }

    private void FindPlayerReference()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
    }

    private void InitializeCurrencyUIElements()
    {
        foreach (var soulstoneCache in player.GetStones())
        {
            if (soulstoneCache.soulstoneData != null)
            {
                currencyUIMap[soulstoneCache.soulstoneData] = GetCurrencyUIElement(soulstoneCache);
            }
        }
    }

    private void SubscribeToCurrencyUpdates()
    {
        if(EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<SoulstoneUpdatedEvent>(OnSoulstoneUpdated);
        }
    }

    private void OnSoulstoneUpdated(object eventData)
    {
        var soulstoneUpdatedEvent = (SoulstoneUpdatedEvent)eventData;
        UpdateCurrencyUI(soulstoneUpdatedEvent.UpdatedStones);
    }
    #endregion

    #region UI Updates
    /// <summary>
    /// Updates the UI element for a specific currency based on the provided currency data.
    /// </summary>
    /// <param name="updatedStone">The currency data that was updated.</param>
    private void UpdateCurrencyUI(SoulstoneCache updatedStone)
    {
        if (currencyUIMap.TryGetValue(updatedStone.soulstoneData, out GameObject currencyUI))
        {
            UpdateCurrencyElementUI(currencyUI, updatedStone);
        }
    }

    private void UpdateCurrencyElementUI(GameObject currencyUI, SoulstoneCache updatedStone)
    {
        TextMeshProUGUI currencyText = currencyUI.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = currencyUI.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        currencyText.text = updatedStone.soulstoneData.stoneName;
        amountText.text = updatedStone.quantity.ToString();
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Creates or retrieves the UI element (GameObject) associated with the provided currency data.
    /// If the UI element doesn't exist, it creates one and sets up its properties and event listeners.
    /// </summary>
    /// <param name="stone">The currency data for which to retrieve or create the UI element.</param>
    /// <returns>The UI element (GameObject) associated with the provided currency data.</returns>
    private GameObject GetCurrencyUIElement(SoulstoneCache stone)
    {
        if (currencyUIMap.ContainsKey(stone.soulstoneData))
            return currencyUIMap[stone.soulstoneData];
        GameObject currencyUI = Instantiate(currencyUIPrefab, contentPanel);
        TextMeshProUGUI currencyText = currencyUI.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = currencyUI.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        Image currencyImage = currencyUI.transform.Find("Icon").GetComponent<Image>();
        Button addButton = currencyUI.transform.Find("Add").GetComponent<Button>();
        Button removeButton = currencyUI.transform.Find("Remove").GetComponent<Button>();
        currencyText.text = stone.soulstoneData.stoneName;
        amountText.text = stone.quantity.ToString();
        currencyImage.sprite = stone.soulstoneData.stoneSprite;

        addButton.onClick.AddListener(() => player.UpdateCurrencyAmount(stone.soulstoneData, 10)); // Add 10 as an example
        removeButton.onClick.AddListener(() => player.UpdateCurrencyAmount(stone.soulstoneData, -10)); // Remove 10
        return currencyUI;
    }

    #endregion
}
