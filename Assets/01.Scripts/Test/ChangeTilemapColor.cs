using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTilemapColor : MonoBehaviour
{
    public Transform Boss;
    public Tilemap tileMap;
    private Grid grid;
    private Vector3Int cellPos;

    private Vector3 pos;

    private void Start()
    {
        grid = GetComponent<Grid>();
        ChangeTileColor(Boss);
    }

    public void CellChange()
    {
        tileMap.SetTileFlags(cellPos, TileFlags.None);
        tileMap.SetColor(cellPos, Color.red);
    }
        
    public void ChangeTileColor(Transform tileTransform)
    {
        pos = tileTransform.position;
        cellPos = grid.WorldToCell(pos);

        cellPos.x -= 1;
        cellPos.y -= 1;

        cellPos.x -= 5; cellPos.y -= 2; CellChange();
        cellPos.y += 1;CellChange();
        cellPos.y += 1; CellChange();
        cellPos.y += 1; CellChange();


    }
}
