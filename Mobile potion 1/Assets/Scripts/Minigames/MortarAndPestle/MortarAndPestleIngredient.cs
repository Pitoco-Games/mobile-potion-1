using UnityEngine;

public class MortarAndPestleIngredient : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private MortarAndPestleIngredient ingredientPrefab;
    [SerializeField] private float splitMoveSpeed = 2f;

    public bool CanDetectCollisions => canDetectCollisions;

    private IngredientConfig config;
    private int currProgressionStage;

    private float collisionDetectionDelay = 0.5f;
    private float elapsedTimeSinceSpawn;
    private bool canDetectCollisions;

    public void Setup(IngredientConfig config, int currProgressionStage, Vector2 ingredientSizeScale)
    {
        this.config = config;
        this.currProgressionStage = currProgressionStage;
        transform.localScale = ingredientSizeScale;

        spriteRenderer.sprite = GetCorrectSprite(config);
        
        if (currProgressionStage == config.requiredMortarAndPestleStages)
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.excludeLayers = LayerMask.GetMask("Player");
        }
    }

    private Sprite GetCorrectSprite(IngredientConfig config)
    {
        if(currProgressionStage == 0)
        {
            return config.Sprite;

        }
        if(currProgressionStage == config.requiredMortarAndPestleStages)
        {
            return config.fullyMashedSprite;
        }

        return config.mashedProgressSprite;
    }

    private void Update()
    {
        if(elapsedTimeSinceSpawn < collisionDetectionDelay)
        {
            elapsedTimeSinceSpawn += Time.deltaTime;
        }
        else if(!canDetectCollisions)
        {
            canDetectCollisions = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canDetectCollisions || currProgressionStage == config.requiredMortarAndPestleStages)
        {
            return;
        }

        var pestleController = collision.gameObject.GetComponent<PestleController>();
        if (pestleController == null)
        {
            return;
        }

        SplitIngredient();
    }

    private void SplitIngredient()
    {
        InstantiateAndSetupNewIngredient(new Vector2(-0.5f, 0.5f));

        if(currProgressionStage + 1 < config.requiredMortarAndPestleStages)
        {
            InstantiateAndSetupNewIngredient(new Vector2(0.5f, 0.5f));
        }

        Destroy(gameObject);
    }

    private void InstantiateAndSetupNewIngredient(Vector2 moveDirection)
    {
        MortarAndPestleIngredient ingredientInstance;

        Vector2 scaleToUse = currProgressionStage + 1 < config.requiredMortarAndPestleStages ? transform.localScale / 2 : transform.localScale;

        ingredientInstance = Instantiate(ingredientPrefab, transform.position, Quaternion.identity);
        ingredientInstance.Setup(config, currProgressionStage + 1, scaleToUse);

        ingredientInstance.SetSplitDirection(moveDirection);
    }

    private void SetSplitDirection(Vector2 moveDirection)
    {
        rigidbody2D.velocity = moveDirection * splitMoveSpeed;
    }
}
