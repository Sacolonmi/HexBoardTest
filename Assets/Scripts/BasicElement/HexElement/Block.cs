using UnityEngine;
using System.Collections;

public class Block : HexElement {
    public Block() : base(9999, Color.black) {
        canPass = false;
        height = 9.0f;
    }
}
