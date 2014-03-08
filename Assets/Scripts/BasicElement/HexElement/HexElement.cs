using UnityEngine;
using System.Collections;

public class HexElement{
    public GameObject obj {set; get;}
    public bool canPass = true;
    public Color color;
	public int cost;

    public float height;

	public HexElement(int costValue, Color? colorValue = null)
    {
        if (colorValue.HasValue) {
            color = colorValue.Value;
        } else {
            color = Color.white;
        }

		cost = costValue;
        height = 1.0f;
    }

    /*
    public void SetNotPass()
    {
        canPass = false;
        obj.renderer.material.color = Color.black;
        color = Color.black;
    }
     */
}
