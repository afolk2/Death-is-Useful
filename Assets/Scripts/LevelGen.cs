using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGen : MonoBehaviour
{
    // Start is called before the first frame update
    public Grid _grid;
    public Tilemap _tileMap;
    public RuleTile _tile;
    public TileBase[] _tiles;

    public Vector3Int[] _positions;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _tileMap = GetComponentInChildren<Tilemap>();
    }
    void Start()
    {
        _tileMap.ClearAllTiles();


        foreach (Vector3Int vector3Int in _positions)
        {
            _tileMap.SetTile(vector3Int, _tile);
        }
    }
}
