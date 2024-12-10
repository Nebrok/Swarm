using System.Collections.Generic;
using UnityEngine;

public class Workshop : Building
{

    [Header("Workshop")]
    [SerializeField]
    BuildingStorage _storage;

    List<Resource> _recipeMaterials = new List<Resource>();
    GameObject _recipeProduct;

    [SerializeField]
    RecipeSO _recipeSO;


    void Start()
    {
        SetTransformToGridPos();

    }

    void Update()
    {
        
    }

    void OpenNewRecipe(RecipeSO newRecipe)
    {
        _recipeSO = newRecipe;
        _recipeMaterials = new List<Resource>(newRecipe.ingredients);
        _recipeProduct = newRecipe.Product;
    }
}
