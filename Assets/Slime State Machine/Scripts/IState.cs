using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public interface IState<T>
{
	void PerformUpdate(T context);
	void PerformFixedUpdate(T context);
	IState<T> CheckTransition(T context);
}
