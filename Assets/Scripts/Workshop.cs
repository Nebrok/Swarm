using System.Collections.Generic;
using UnityEngine;

public class Workshop : Building
{

    [Header("Workshop")]
    [SerializeField]
    BuildingStorage _storage;

    List<Resource> _receipeMaterials = new List<Resource>();

    [SerializeField]
    RecipeSO _recipeSO;


    void Start()
    {
        SetTransformToGridPos();

    }

    void Update()
    {
        
    }
}
