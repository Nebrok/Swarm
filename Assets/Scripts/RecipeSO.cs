using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipes/New Recipe")]
public class RecipeSO : ScriptableObject
{
    [SerializeField]
    private List<Resource> _ingredients = new List<Resource>();

    [SerializeField]
    private GameObject _product;


    public GameObject Product
    {
        get { return _product; }
    }

    public List<Resource> ingredients
    {
        get { return _ingredients; }
    }
}
