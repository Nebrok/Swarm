using System;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiskSamples
{
    /// 
    /// A possible solution to drone spacing
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

        _spatialGrid = new int[_numCellsWide, _numCellsWide];
        for (int x = 0; x < _numCellsWide; x++)
        {
            for (int y = 0; y < _numCellsWide; y++)
            {
                _spatialGrid[x, y] = -1;
            }
        }

        Vector2 initialPoint = new Vector2(0, 0);//new Vector2(UnityEngine.Random.Range(-areaHalfWidth, areaHalfWidth), UnityEngine.Random.Range(-areaHalfWidth, areaHalfWidth));
        Vector2Int initialPointGridPos = GetGridPosition(initialPoint, _areaWidth, _numCellsWide);

        _finalList.Add(initialPoint);
        _activeList.Add(initialPoint);
        _spatialGrid[initialPointGridPos.x, initialPointGridPos.y] = 0;

        for (int sanityCheck = 0; sanityCheck < 1000; sanityCheck++)
        {
            Vector2 currentPoint = _activeList[0];
            Vector2Int currentGridPos = GetGridPosition(currentPoint, _areaWidth, _numCellsWide);

            for (int k = 0; k < numSamplesPer; k++)
            {
                float randomXOffset = UnityEngine.Random.Range(-2 * minimumDist, 2 * minimumDist);
                float randomYOffset = UnityEngine.Random.Range(-2 * minimumDist, 2 * minimumDist);

                Vector2 sample = new Vector2(currentPoint.x + randomXOffset, currentPoint.y + randomYOffset);
                Vector2Int sampleGridPos = GetGridPosition(sample, _areaWidth, _numCellsWide);

                //Skip if outside grid
                if (sampleGridPos.x == -1 || sampleGridPos.y == -1)
                {
                    continue;
                }

                //Skip if something already in cell
                if (_spatialGrid[sampleGridPos.x, sampleGridPos.y] != -1)
                {
                    continue;
                }


                //NEED TO CHECK IF SAMPLE POINT IS minimumDist AWAY FROM SURROUNDING POINTS TOO

                float DistTweenPts = Vector2.Distance(currentPoint, sample);

                if (DistTweenPts > 2 * minimumDist || DistTweenPts < minimumDist)
                {
                    continue;
                }

                _finalList.Add(sample);
                _activeList.Add(sample);
                _spatialGrid[sampleGridPos.x, sampleGridPos.y] = _finalList.Count - 1;
                break;
            }

            _activeList.Remove(currentPoint);
            
            //Break sanityLoop
            if (_activeList.Count <= 0)
            {
                break;
            }
        }
    }

    public Vector2Int GetGridPosition(Vector2 position, float areaWidth, float gridWidth)
    {
        Vector2 translatedPosition = new Vector2(position.x + areaWidth/2, position.y + areaWidth/2);
        //Checks if outside grid bounds
        if (translatedPosition.x < 0 || translatedPosition.y < 0)
        {
            return new Vector2Int(-1, -1);
        }
        if (translatedPosition.x > areaWidth || translatedPosition.y > areaWidth)
        {
            return new Vector2Int(-1, -1);

        }

        float cellWidth = areaWidth / gridWidth;

        int gridXPos = (int)Math.Floor(translatedPosition.x / cellWidth);
        int gridYPos = (int)Math.Floor(translatedPosition.y / cellWidth);
        


        return new Vector2Int(gridXPos, gridYPos);
    }

    public List<Vector2Int> GetSurroundingGridCells(Vector2Int cellPos, int gridWidth, int neighbourhoodReach)
    {
        List<Vector2Int> outList = new List<Vector2Int>();

        int xMin = cellPos.x - neighbourhoodReach < 0 ? 0 : cellPos.x - neighbourhoodReach;
        int xMax = cellPos.x + neighbourhoodReach > gridWidth ? gridWidth : cellPos.x + neighbourhoodReach;
        int yMin = cellPos.y - neighbourhoodReach < 0 ? 0 : cellPos.y - neighbourhoodReach;
        int yMax = cellPos.y + neighbourhoodReach > gridWidth ? gridWidth : cellPos.y + neighbourhoodReach;

        for (int i = xMin; i <= xMax; i++)
        {
            for (int j = yMin; j <= yMax; j++)
            {
                outList.Add(new Vector2Int(i, j));
            }
        }

        return outList;
    }

    public List<Vector2> PoissonDiscSample
    {
        get { return _finalList; }
    }


}