using System;
using System.Collections.Generic;
using System.Threading;
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

        _spatialGrid = new int[_numCellsWide, _numCellsWide];
        for (int x = 0; x < _numCellsWide; x++)
        {
            for (int y = 0; y < _numCellsWide; y++)
            {
                _spatialGrid[x, y] = -1;
            }
        }

        Vector2 initialPoint = new Vector2(areaHalfWidth, areaHalfWidth);//new Vector2(UnityEngine.Random.Range(-areaHalfWidth, areaHalfWidth), UnityEngine.Random.Range(-areaHalfWidth, areaHalfWidth));
        Vector2Int initialPointGridPos = GetGridPosition(initialPoint, _areaWidth, _numCellsWide);

        _finalList.Add(initialPoint);
        _activeList.Add(initialPoint);
        _spatialGrid[initialPointGridPos.x, initialPointGridPos.y] = 0;

        int checkCount = 0;
        while (_activeList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _activeList.Count);
            Vector2 currentPoint = _activeList[randomIndex];
            Vector2Int currentGridPos = GetGridPosition(currentPoint, _areaWidth, _numCellsWide);

            bool wasFound = false;
            for (int k = 0; k < numSamplesPer; k++)
            {
                float coinFlip = UnityEngine.Random.Range(0, 1.0f) < 0.5 ? -1 : 1;
                float randomRadius = UnityEngine.Random.Range(minimumDist, 2 * minimumDist);
                float randomXOffset = UnityEngine.Random.Range(-randomRadius, randomRadius);
                float randomYOffset = coinFlip * (float)Math.Sqrt(randomRadius * randomRadius - randomXOffset * randomXOffset);
                //float randomYOffset = UnityEngine.Random.Range(-2 * minimumDist, 2 * minimumDist);

                Vector2 sample = new Vector2(currentPoint.x + randomXOffset, currentPoint.y + randomYOffset);
                Vector2Int sampleGridPos = GetGridPosition(sample, _areaWidth, _numCellsWide);

                //Skip if outside grid
                if (sampleGridPos.x == -1 || sampleGridPos.y == -1)
                {
                    continue;
                }

                /*
                //Skip if something already in cell
                if (_spatialGrid[sampleGridPos.x, sampleGridPos.y] != -1)
                {
                    continue;
                }
                */

                bool invalidPos = false;
                List<Vector2Int> sampleGridNeighbours = GetSurroundingGridCells(sampleGridPos, _numCellsWide, 2);
                foreach (Vector2Int cell in sampleGridNeighbours)
                {
                    int neighbourIndex = _spatialGrid[cell.x, cell.y];
                    if (neighbourIndex == -1)
                    {
                        continue;
                    }
                    Vector2 neighbouringPoint = _finalList[neighbourIndex];

                    float distTweenPts = Vector2.Distance(neighbouringPoint, sample);

                    if (distTweenPts < minimumDist)
                    {
                        invalidPos = true;
                        break;
                    }
                }

                //Skip if One of neighbouring cells is too close
                if (invalidPos) continue;

                float distToCurrent = Vector2.Distance(currentPoint, sample);

                if (distToCurrent > 2 * minimumDist)
                {
                    continue;
                }

                wasFound = true;
                _finalList.Add(sample);
                _activeList.Add(sample);
                _spatialGrid[sampleGridPos.x, sampleGridPos.y] = _finalList.Count - 1;
                break;
            }

            if (!wasFound) _activeList.Remove(currentPoint);

            checkCount++;
            //Break sanityLoop
            if (checkCount > 100000)
            {
                break;
            }
        }
    }

    public Vector2Int GetGridPosition(Vector2 position, float areaWidth, float gridWidth)
    {
        //Checks if outside grid bounds
        if (position.x < 0 || position.y < 0)
        {
            return new Vector2Int(-1, -1);
        }
        if (position.x > areaWidth || position.y > areaWidth)
        {
            return new Vector2Int(-1, -1);

        }

        float cellWidth = areaWidth / gridWidth;

        int gridXPos = (int)Math.Floor(position.x / cellWidth);
        int gridYPos = (int)Math.Floor(position.y / cellWidth);
        
        return new Vector2Int(gridXPos, gridYPos);
    }

    public List<Vector2Int> GetSurroundingGridCells(Vector2Int cellPos, int gridWidth, int neighbourhoodReach)
    {
        List<Vector2Int> outList = new List<Vector2Int>();

        int xMin = cellPos.x - neighbourhoodReach < 0 ? 0 : cellPos.x - neighbourhoodReach;
        int xMax = cellPos.x + neighbourhoodReach >= gridWidth ? gridWidth -1 : cellPos.x + neighbourhoodReach;
        int yMin = cellPos.y - neighbourhoodReach < 0 ? 0 : cellPos.y - neighbourhoodReach;
        int yMax = cellPos.y + neighbourhoodReach >= gridWidth ? gridWidth - 1 : cellPos.y + neighbourhoodReach;

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

    public float CellSize
    {
        get { return _cellSize; }
    }

    public int GridWidth
    {
        get { return _numCellsWide; }
    }
}