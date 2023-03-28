using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class LayerAlphaSetting : MonoBehaviour
{
    public Tilemap tilemap, tilemap1;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("TileMapPlayerCol"))
        {
            tilemap.color = new Color(1, 1, 1, 0.5f);
            tilemap1.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TileMapPlayerCol"))
        {
            tilemap.color = new Color(1, 1, 1, 1);
            tilemap1.color = new Color(1, 1, 1, 1);
        }
    }
}
