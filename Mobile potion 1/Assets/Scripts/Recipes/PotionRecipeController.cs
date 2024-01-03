using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PotionRecipeController : MonoBehaviour, IProductReceiver
{
    [SerializeField] private List<PotionConfig> allPotionConfigs;
    [SerializeField] private PotionConfig unstablePotionConfig;

    [SerializeField] private Button makeRecipeButton;

    private List<ProductWithState> currentIngredientsInPotion = new ();

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
        PotionConfig potionMade;
        if (!TryGetPotionThatHasSameIngredients(out potionMade))
        {
            potionMade = unstablePotionConfig;
        }

        currentIngredientsInPotion.Clear();

        Debug.Log($"### Made Potion: {potionMade.Name}");

        // Instantiate potion object
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

    public bool ReceiveProduct(ProductObject product)
    {
        currentIngredientsInPotion.Add(new ProductWithState{config = product.ProductConfig as IngredientConfig, state = product.State});
        return true;
    }
}