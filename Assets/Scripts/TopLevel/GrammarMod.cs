using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrammarMod : IModule
{
    public GrammarMod() : base(4) { }

    public override void Gen() {}
    public int Rand(int floor, int ceiling) => RandOvr(floor, ceiling); 
}
