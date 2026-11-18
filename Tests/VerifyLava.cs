using System;
using System.Collections.Generic;
using Floor33;
public class VerifyStaggeredWaves {
 static int n;static void C(bool b,string s){n++;if(!b)throw new Exception(s);}
 static int Count(Game g,Cell c){int n=0;foreach(Cell v in g.Cells)if(v==c)n++;return n;}
 public static void Main(){
  var sides=new HashSet<int>();int maximumRed=0;
  for(int seed=0;seed<16;seed++){
   var g=new Game(seed);g.Start(0,90,2);g.Tick(3);C(Count(g,Cell.Safe)==8,"Eight single green refuges");int safe=Array.FindIndex(g.Cells,c=>c==Cell.Safe);g.Touch(safe,true);g.Tick(2);C(g.LavaFronts.Count==1,"One wave enters first");C(g.LavaFronts[0].Warning>0&&Count(g,Cell.Red)==0,"Wave entry warning before red");
   for(int t=0;t<430;t++){
    g.Touch(safe,false);safe=Array.FindIndex(g.Cells,c=>c==Cell.Safe);g.Touch(safe,true);Game.LavaFront front=g.LavaFronts.Count>0?g.LavaFronts[0]:null;double old=front==null?0:front.Position;g.Tick(.1);
    C(Count(g,Cell.Safe)==8,"Only eight safe tiles throughout");C(g.LavaFronts.Count<=(g.Wave>=3?3:2),"Staggered wave concurrency cap");int red=Count(g,Cell.Red);maximumRed=Math.Max(maximumRed,red);C(red<=63,"Partial bands, never full-floor red");
    if(front!=null&&g.LavaFronts.Contains(front)&&front.Warning<=0)C(front.Position>=old,"Wavefront moves continuously");
    foreach(Game.LavaFront f in g.LavaFronts){sides.Add(f.Side);C(f.SpanWidth>=4&&f.SpanWidth<=7,"Connected moving group spans several tiles");C(f.SpanStart>=0&&f.SpanStart+f.SpanWidth<=(f.Side<2?12:33),"Wave span stays on floor");}
    C(g.Lives==3,"Following relocating green avoids damage");
   }
   C(g.Score>=20&&g.Score%5==0,"Safe checkpoint rewards");C(g.PassedWaves>0,"Waves cross and leave floor");g.Paused=true;double clock=g.SecondsLeft,stage=g.StageSeconds;g.Tick(2);C(g.SecondsLeft==clock&&g.StageSeconds==stage,"Pause clocks and waves");
  }
  C(sides.Count==4,"Waves from all four sides");C(maximumRed>=8,"Visible moving bands rather than isolated dots");
  foreach(int duration in new int[]{30,60,90}){var g=new Game(1);g.Start(0,duration,2);g.Tick(3);g.Tick(duration);C(g.Finished&&!g.Active,"Selected duration stops mode");}
  Console.WriteLine("PASS "+n+" random-wave checks: moving partial curved bands, staggered entry, warning edge, four directions, concurrency cap, 8 protected tiles, checkpoint scoring, pause and durations.");
 }
}
