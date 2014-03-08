using UnityEngine;
using System.Collections;

public class Player{
	public GameObject Obj { get; set; }
    public GameObject RealObj { get; set; }
	public int Row { get; set; }
	public int Col { get; set; }

    public int MoveAbility { get; set; }


	public Player(GameObject obj, int row, int col, int moveAbility = 3){
		Obj = obj;
		Row = row;
		Col = col;
        MoveAbility = moveAbility;
	}


}
