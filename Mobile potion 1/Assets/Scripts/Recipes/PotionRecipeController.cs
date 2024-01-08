using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PotionRecipeController : MonoBehaviour, IProductReceiver, IProductContainer
{
    [SerializeField] private List<PotionConfig> allPotionConfigs;
    [SerializeField] private PotionConfig unstablePotionConfig;

    [SerializeField] private Button makeRecipeButton;
    [SerializeField] private Image ingredientImage;

    private List<ProductWithState> currentIngredientsInPotion = new ();
    private PotionConfig createdPotion;

    private void Awake()
    {
        makeRecipeButton.onClick.AddListener(CompleteRecipe);
    }

    //TODO: remove placeholder
    public PotionConfig GetRandomPotionConfig()
    {
        return allPotionConfigs[Random.Range(0, allPotionConfigs.Count)];
    }

    public void CompleteRecipe()
    {
        if(currentIngredientsInPotion.Count == 0)
        {
            return;
        }

        if (!TryGetPotionThatHasSameIngredients(out createdPotion))
        {
            createdPotion = unstablePotionConfig;
        }

        currentIngredientsInPotion.Clear();

        Debug.Log($"### Made Potion: {createdPotion.Name}");
        makeRecipeButton.gameObject.SetActive(false);
        ingredientImage.gameObject.SetActive(true);
        ingredientImage.sprite = createdPotion.Sprite;
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

    public bool TryTakeProduct(out ProductWithState productData)
    {
        productData = default;

        if (createdPotion == null)
        {
            return false;
        }

        PotionConfig currConfig = createdPotion;
        createdPotion = null;
        productData = new ProductWithState { config = currConfig, state = ProductState.Brewed };
        ingredientImage.gameObject.SetActive(false);

        return true;
    }
}