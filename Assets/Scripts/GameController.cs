using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public GameObject hexPrism;
    public Material material;

    //private GameObject[,] _map;
    private Map _map;
    
    void Start()
    {
        _map = new Map(hexPrism);
        _map.GenerateHexPrism(this);
    }

    public GameObject MakeObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        return (GameObject)Instantiate(obj, position, rotation);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100000.0f))
            {
                for (int i = 0; i < _map.Width; ++i)
                {
                    for (int j = 0; j < _map.Height; ++j)
                    {
                        _map.Hexes[i, j].obj.renderer.material.color = _map.Hexes[i,j].color;
                    }
                }

                int row, col;
                _map.SearchMap(hit.collider.gameObject, out row, out col);
                _map.Hexes[row, col].obj.renderer.material.color = Color.blue;
                foreach (var neighbor in _map.GetNeighborsByLength(row, col, 3, new bool[10,10]))
                {
                    _map.Hexes[(int)neighbor.x, (int)neighbor.y].obj.renderer.material.color = Color.red;
                }

            }
        }
    }
}