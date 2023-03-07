using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeTilemapColor : MonoBehaviour
{
    public Transform Boss;
    public Tilemap tileMap;
    private Grid grid;
    private Vector3Int RC; //RangeCell
    private Vector3Int RCF; //RangeCellFill

    private Vector3 pos;

    WaitForSeconds RCFTime = new WaitForSeconds(0.3f);

    private void Start()
    {
        grid = GetComponent<Grid>();
    }

    public void RCChange()
    {
        tileMap.SetTileFlags(RC, TileFlags.None);
        tileMap.SetColor(RC, new Color(255,0,0,1));
    }

    public void RCFChange()
    {
        tileMap.SetTileFlags(RCF, TileFlags.None);
        tileMap.SetColor(RCF, new Color(255, 0, 0, 1));
    }
        

}

