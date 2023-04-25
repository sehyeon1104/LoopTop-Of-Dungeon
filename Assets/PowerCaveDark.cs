using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class PowerCaveDark : MonoBehaviour
{
    public Tilemap tilemap, dark, silling;

    private bool alphaOn = false;

    private void OnTriggerStay2D(Collider2D collision)
    {


        if (collision.CompareTag("TileMapPlayerCol"))
        {
            if (alphaOn == false)
            {
                
                Color darktargetColor = dark.color;
                darktargetColor.a = 1;
                Color tilemaptargetColor = tilemap.color;
                tilemaptargetColor.r = 0;
                tilemaptargetColor.g = 0;
                tilemaptargetColor.b = 0;
                Color sillingtargetColor = silling.color;
                sillingtargetColor.a = 0;

                DOTween.To(() => dark.color, color => dark.color = color, darktargetColor, 0.7f);
                DOTween.To(() => tilemap.color, color => tilemap.color = color, tilemaptargetColor, 0.7f);
                DOTween.To(() => silling.color, color => silling.color = color, sillingtargetColor, 0.7f);

                alphaOn = true;

                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TileMapPlayerCol"))
        {
            Color darktargetColor = dark.color;
            darktargetColor.a = 0;
            Color tilemaptargetColor = tilemap.color;
            tilemaptargetColor.r = 1;
            tilemaptargetColor.g = 1;
            tilemaptargetColor.b = 1;
            Color sillingtargetColor = silling.color;
            sillingtargetColor.a = 1;

            DOTween.To(() => dark.color, color => dark.color = color, darktargetColor, 0.7f);
            DOTween.To(() => tilemap.color, color => tilemap.color = color, tilemaptargetColor, 0.7f);
            DOTween.To(() => silling.color, color => silling.color = color, sillingtargetColor, 0.7f);
            alphaOn = false;
        }
    }

}
