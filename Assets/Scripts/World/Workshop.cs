using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ItemTally
{
    SortedDictionary<string, int> _tally = new SortedDictionary<string, int>();

    public SortedDictionary<string, int> Tally { get { return _tally; } }

    public ItemTally(string manifest)
    {
        string[] items = manifest.Split(", ");

        foreach (string item in items)
        {
            if (_tally.ContainsKey(item))
            {
                _tally[item] += 1;
            }
            else
            {
                _tally.Add(item, 1);
            }
        }
    }

    public bool CompareTally(ItemTally other)
    {
        return ToString() == other.ToString();
    }

    public override string ToString()
    {
        string output = string.Empty;

        foreach (string item in Tally.Keys)
        {
            for (int i = 0; i < Tally[item]; i++)
            {
                output += item + ", ";
            }
        }

        output = output.Remove(output.Length - 2, 2);

        return output;
    }

}

public class Workshop : Building
{
    [Header("Workshop")]
    [SerializeField]
    BuildingStorage _storage;
    [SerializeField]
    Transform _spawnPoint;


    List<Resource> _recipeMaterials = new List<Resource>();
    ItemTally _recipeTally = null;
    GameObject _recipeProduct;

    [SerializeField]
    RecipeSO _recipeSO;


    void Start()
    {
        SetTransformToGridPos();
        OpenNewRecipe(_recipeSO);

        if (AreAllResourcesCollected())
        {
            Instantiate(_recipeProduct, _spawnPoint.position, Quaternion.identity);
            _storage.ClearStorage();
        }

    }

    void Update()
    {

    }

    public void DepositItem(IStorable item)
    {
        _storage.AddItem(item);
        if (AreAllResourcesCollected())
        {
            Instantiate(_recipeProduct, _spawnPoint.position, Quaternion.identity);
            _storage.ClearStorage();
        }
    }

    private void OpenNewRecipe(RecipeSO newRecipe)
    {
        _recipeSO = newRecipe;
        _recipeMaterials = new List<Resource>(newRecipe.Ingredients);
        _recipeTally = CreateRecipeItemTally(_recipeMaterials);
        _recipeProduct = newRecipe.Product;
    }

    private bool AreAllResourcesCollected()
    {
        string storageManifest = _storage.GetStorageManifest();
        ItemTally storageItemTally = CreateItemTally(storageManifest);

        return _recipeTally.CompareTally(storageItemTally);
    }

    private ItemTally CreateItemTally(string manifest)
    {
        ItemTally itemTally = new ItemTally(manifest);

        return itemTally;
    }

    private ItemTally CreateRecipeItemTally(List<Resource> recipeIngredients)
    {
        string manifest = string.Empty;

        for (int i = 0; i < recipeIngredients.Count; i++)
        {
            if (i == recipeIngredients.Count - 1)
            {
                manifest += recipeIngredients[i].GetItemName();
                continue;
            }
            manifest += recipeIngredients[i].GetItemName() + ", ";
        }

        ItemTally recipeTally = new ItemTally(manifest);

        return recipeTally;
    }

}
