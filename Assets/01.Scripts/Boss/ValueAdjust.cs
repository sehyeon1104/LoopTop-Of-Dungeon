using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueAdjust : MonoBehaviour
{
  
    [SerializeField]Sprite currentSprite;  
    private Material m_Material;
    [SerializeField]
    float value;
    private void Awake()
    {
        m_Material = GetComponent<Renderer>().material;
        currentSprite = GetComponent<SpriteRenderer>().sprite;
    }
 
  
    // Update is called once per frame
    void Update()
    {
        print(m_Material.GetFloat("_Value"));
        m_Material.SetFloat("_Value", 0.5f);
        m_Material.SetTexture("_Texture2D", currentSprite.texture);
    }
}
