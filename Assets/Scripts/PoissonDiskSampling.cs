using System.Collections.Generic;
using UnityEngine;

public class PoissonDiskSamples
{
    /// 
    /// 
    ///

    private float _areaWidth;
    private float _minimumDist;
    private int _numSamplesPer;

    private float _cellSize;
    private int _numCellsWide;

    private List<Vector2> _finalList = new List<Vector2>();
    private List<Vector2> _activeList = new List<Vector2>();

    private int[,] _spatialGrid;


    public PoissonDiskSamples(int areaHalfWidth, float minimumDist, int numSamplesPer = 30)
    {
        _areaWidth = areaHalfWidth*2;
        _minimumDist = minimumDist;
        _numSamplesPer = numSamplesPer;

        _cellSize = _minimumDist / Mathf.Sqrt(2);
        _numCellsWide = (int)Mathf.Ceil(_areaWidth / _cellSize);
        //ensure even number cells
        _numCellsWide = _numCellsWide % 2 == 0 ? _numCellsWide : _numCellsWide + 1;

        Debug.Log("Cell size: " + _cellSize);
        Debug.Log("Number Cells Wide: " + _numCellsWide);
    }






}