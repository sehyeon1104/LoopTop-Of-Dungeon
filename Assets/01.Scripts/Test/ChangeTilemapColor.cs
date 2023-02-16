using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTilemapColor : MonoBehaviour
{
    private Tilemap tileMap;
    public Vector3Int position;
    private GridLayout gridLayout;
    private Vector3Int cellPos;

    private Vector2 pos;

    private void Start()
    {
        tileMap = GetComponent<Tilemap>();
    }

    public void ChangeTileColor(Transform tileTransform, Color color)
    {
        pos = tileTransform.position;
        cellPos = gridLayout.WorldToCell(pos);

        tileMap.SetTileFlags(cellPos, TileFlags.None);
        tileMap.SetColor(cellPos, color);
    }
}
