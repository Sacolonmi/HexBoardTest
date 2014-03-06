using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map{
    private HexElement[,] _hexes;
    private int _width, _height;
    private GameObject _hexPrism;

    public HexElement[,] Hexes{
        set { _hexes = value; }
        get { return _hexes; }
    }

    public int Width
    {
        set { _width = value; }
        get { return _width; }
    }

    public int Height
    {
        set { _height = value; }
        get { return _height; }
    }

    public Map(GameObject hexPrism, int width = 10, int height = 10 )
    {
        _hexPrism = hexPrism;
        _width = width;
        _height = height;

        _hexes = new HexElement[width, height];

        for (int i = 0 ; i < width ; ++i)
        {
            for (int j = 0 ; j < height ; ++j)
            {
                _hexes[i, j] = new HexElement();
            }
        }
    }

    public void GenerateHexPrism(GameController gc, Vector3? originValue = null)
    {
        Vector3 origin = Vector3.zero;
        if (originValue.HasValue) origin = originValue.Value;

        for (int row = 0 ; row < _width ; ++row) {
            for (int col = 0 ; col < _height ; ++col) {
                Vector3 position;
                Quaternion rotation = Quaternion.Euler(270, 0, 0);
                if (row % 2 == 0) {
                    position = origin + new Vector3(col * 3.0f, 0.0f, row * 1.5f * Mathf.Sqrt(3));

                } else {
                    position = origin + new Vector3(col * 3.0f + 1.5f, 0.0f, row * 1.5f * Mathf.Sqrt(3));
                }
                _hexes[row,col].obj = gc.MakeObject(_hexPrism, position, rotation);
            }
        }

        for (int i = 0; i < 10; ++i) {
            if (i != 4) {
                _hexes[5, i].SetNotPass();
            }
        }

        _hexes[4, 1].SetNotPass();
        _hexes[4, 3].SetNotPass();
        _hexes[3, 2].SetNotPass();
        _hexes[3, 1].SetNotPass();
    }

    public void SearchMap(GameObject obj, out int rowOutput, out int colOutput)
    {
        for (int row = 0 ; row < _width ; ++row) {
            for (int col = 0 ; col < _height ; ++col) {
                if (_hexes[row, col].obj == obj) {
                    rowOutput = row;
                    colOutput = col;
                    return;
                }
            }
        }

        rowOutput = -1;
        colOutput = -1;
    }

    public IEnumerable<Vector2> GetNeighborsByLength(int row, int col, int length, bool[,] check)
    {
        if (length <= 0) throw new System.Exception("Length can not be 0.");

        check[row, col] = true;
        foreach(var neighbor in GetNeighbors(row,col)){
            if( !check[(int)neighbor.x,(int)neighbor.y] ) {
                yield return neighbor;
            }
            if (length > 1)
            {
                foreach (var otherNeighbor in GetNeighborsByLength((int)neighbor.x, (int)neighbor.y, length - 1, check)) {
                    yield return otherNeighbor;
                }
            }
        }
    }

    public IEnumerable<Vector2> GetNeighbors( int row, int col )
    {
        if (!isInBoard(row, col)) {
            yield break;
        }

        if (row % 2 == 1) {
            foreach (var v in GetOddNeighbors(row, col)) {
                yield return v;
            }

        } else {
            foreach (var v in GetEvenNeighbors(row, col)) {
                yield return v;
            }
        }

        foreach (var v in GetSharedNeighbors(row, col)) {
            yield return v;
        }
        
    }

    IEnumerable<Vector2> GetOddNeighbors(int row, int col)
    {
        int[,] oddNeighbors = { { row - 1, col + 1 },
                                { row + 1, col + 1 } };

        for (int i = 0; i < oddNeighbors.GetLength(0) ; ++i) {
            int x = oddNeighbors[i, 0], y = oddNeighbors[i, 1];
            if (isGoable(x, y)) {
                yield return new Vector2(x, y);
            }
        }
    }

    IEnumerable<Vector2> GetEvenNeighbors(int row, int col)
    {
        int[,] evenNeighbors = { { row + 1, col - 1 },
                                 { row - 1, col - 1 } };

        for (int i = 0; i < evenNeighbors.GetLength(0) ; ++i) {
            int x = evenNeighbors[i, 0], y = evenNeighbors[i, 1];
            if (isGoable(x, y)) {
                yield return new Vector2(x, y);
            }
        }
    }

    IEnumerable<Vector2> GetSharedNeighbors(int row, int col)
    {
        int[,] neighbors = { { row + 1, col },
                             { row, col + 1 },
                             { row - 1, col },
                             { row, col - 1 } };

        for (int i = 0; i < neighbors.GetLength(0) ; ++i) {
            int x = neighbors[i, 0], y = neighbors[i, 1];
            if (isGoable(x, y)) {
                yield return new Vector2(x, y);
            }
        }
    }

    bool isInBoard(int row, int col)
    {
        return (row >= 0 && col >= 0 &&
            row < Width && col < Height);
    }

    bool isGoable(int row, int col)
    {
        return isInBoard(row, col) &&
            _hexes[row, col].canPass;
    }


    public void UpdateAllHexPrismsColor()
    {
        foreach (var hexPrism in Hexes) {
            hexPrism.obj.renderer.material.color = hexPrism.color;
        }
    }

}
