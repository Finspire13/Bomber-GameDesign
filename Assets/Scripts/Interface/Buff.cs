using System;


public interface Buff
{
	int LifeTime{
		get;
		set;
	}
	int Value {
		get;
		set;
	}
	void effect();


}


