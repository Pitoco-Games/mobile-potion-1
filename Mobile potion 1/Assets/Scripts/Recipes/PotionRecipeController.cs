using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PotionRecipeController : MonoBehaviour, IProductReceiver, IProductContainer
{
    [SerializeField] private List<PotionConfig> allPotionConfigs;
    [SerializeField] private PotionConfig unstablePotionConfig;

    [SerializeField] private Button makeRecipeButton;
    [SerializeField] private CauldronMinigame minigamePrefab;
    [SerializeField] private Image potionImage;
    [SerializeField] private Transform minigameParent;
    [SerializeField] private GameObject canvasObject;

    private List<ProductWithState> currentIngredientsInPotion = new ();
    private PotionConfig createdPotion;

    private void Awake()
    {
        makeRecipeButton.onClick.AddListener(StartMinigame);
    }

    private void StartMinigame()
    {
        if (!TryGetPotionThatHasSameIngredients(out createdPotion))
        {
            createdPotion = unstablePotionConfig;
        }

        currentIngredientsInPotion.Clear();

        makeRecipeButton.gameObject.SetActive(false);
        canvasObject.SetActive(false);

        CauldronMinigame minigame = Instantiate(minigamePrefab, minigameParent);
        minigame.StartMinigame(createdPotion, CompleteRecipe);
    }

    public void CompleteRecipe()
    {
        canvasObject.SetActive(true);
        potionImage.gameObject.SetActive(true);
        potionImage.sprite = createdPotion.Sprite;
    }

    private bool TryGetPotionThatHasSameIngredients(out PotionConfig potionFound)
    {
        currentIngredientsInPotion.Sort();

        potionFound = null;
        foreach (PotionConfig potionConfig in allPotionConfigs)
        {
            if (potionConfig.RequiredIngredients.SequenceEqual(currentIngredientsInPotion))
            {
                potionFound = potionConfig;
                return true;
            }
        }

        return false;
    }

    public bool ReceiveProduct(ProductWithState productData)
    {
        makeRecipeButton.gameObject.SetActive(true);

        Debug.Log($"Received {productData.config.Name}");
        currentIngredientsInPotion.Add(new ProductWithState{config = productData.config as IngredientConfig, state = productData.state});
        return true;
    }
    private void OnProductReleasedByDrag(bool wentToNewProductReceiver)
    {
        if (wentToNewProductReceiver)
        {
            createdPotion = null;
            return;
        }

        potionImage.gameObject.SetActive(true);
    }

    public bool TryTakeProduct(out ProductWithState productData, out Action<bool> onProductReleased)
    {
        productData = default;
        onProductReleased = OnProductReleasedByDrag;

        if (createdPotion == null)
        {
            return false;
        }

        PotionConfig currConfig = createdPotion;
        productData = new ProductWithState { config = currConfig, state = ProductState.Brewed };
        potionImage.gameObject.SetActive(false);

        return true;
    }
}