// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;

public interface SetBomb:RhythmObservable
{
	//capacity of bomb at a time
	int MaxNum {
		get;
		set;
	}

	//already used number of bomb
	int CurrNum {
		get;
		set;
	}

//	int BombLifeTime {
//		get;
//		set;
//	}

	void notifyExplosion ();

	//change bomb type
	void setBomb(GameObject bombType);
	void installBomb();
}


