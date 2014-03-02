using UnityEngine;
using System.Collections;

public class HexElement{
    public GameObject obj {set; get;}
    public bool canPass = true;
    public Color color;

    public HexElement(Color? colorValue = null)
    {
        if (colorValue.HasValue)
        {
            color = colorValue.Value;
        }
        else
        {
            color = Color.white;
        }
    }

    public void SetNotPass()
    {
        canPass = false;
        obj.renderer.material.color = Color.black;
        color = Color.black;
    }
}
