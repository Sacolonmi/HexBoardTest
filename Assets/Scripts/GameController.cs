using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public GameObject hexPrism;
	public GameObject player;
    public Material material;
    private Player _selectedPlayer;
    private Map _map;
    
    void Start()
    {
        _map = new Map(hexPrism, player, 30, 30);
        _map.GenerateHexPrism(this);
		_map.GeneratePlayer(this);
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
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Player")
                {
                    _map.UpdateAllHexPrismsColor();
                    _map.SearchPlayer(hit.collider.gameObject, out _selectedPlayer);
                    if (_selectedPlayer != null)
                    {
                        _map.Hexes[_selectedPlayer.Row, _selectedPlayer.Col].obj.renderer.material.color = Color.blue;
                        foreach (var neighbor in _map.GetNeighborsByLength(_selectedPlayer.Row, _selectedPlayer.Col, _selectedPlayer.MoveAbility, new bool[_map.Width, _map.Height]))
                        {
                            _map.Hexes[(int)neighbor.x, (int)neighbor.y].obj.renderer.material.color = Color.red;
                        }
                    }
                    /*
                    int row, col;
                    _map.SearchMap(hit.collider.gameObject, out row, out col);
                    _map.Hexes[row, col].obj.renderer.material.color = Color.blue;
                    foreach (var neighbor in _map.GetNeighborsByLength(row, col, 3, new bool[_map.Width, _map.Height]))
                    {
                        _map.Hexes[(int)neighbor.x, (int)neighbor.y].obj.renderer.material.color = Color.red;
                    }
                    */
                }
                else if (hit.collider.gameObject.tag == "Map" && _selectedPlayer != null)
                {
                    if (hit.collider.gameObject.renderer.material.color != Color.red)
                    {
                        _map.UpdateAllHexPrismsColor();
                        _selectedPlayer = null;
                    }
                    else
                    {
                        _map.UpdateAllHexPrismsColor();

                        int row, col;
                        _map.SearchMap(hit.collider.gameObject, out row, out col);
                        _selectedPlayer.Row = row;
                        _selectedPlayer.Col = col;
                        _selectedPlayer.RealObj.transform.position = new Vector3(0.0f, 2.0f, 0.0f) + _map.TransformCoordinate(_selectedPlayer.Row, _selectedPlayer.Col);
                    }
                }
            }
        }
    }
}