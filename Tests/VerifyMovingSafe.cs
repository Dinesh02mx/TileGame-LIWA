using System;
using System.Collections.Generic;
using Floor33;
public class VerifyMovingSafe {
 static int n;static void C(bool b,string m){n++;if(!b)throw new Exception(m);}
 static HashSet<int> Safe(Game g){var a=new HashSet<int>();for(int i=0;i<396;i++)if(g.Cells[i]==Cell.Safe)a.Add(i);return a;}
 public static void Main(){
  for(int seed=0;seed<30;seed++){
   var g=new Game(seed);g.Start(0,90,2);g.Tick(3);var old=Safe(g);C(old.Count==8,"Initial eight safe tiles");g.Tick(4.9);C(g.SafeMoveCount==0&&Safe(g).SetEquals(old),"No early movement");g.Tick(.1);C(g.SafeMoveCount==1,"Move at 5 seconds");var next=Safe(g);C(next.Count==8,"Exactly eight new greens");foreach(int i in old){C(!next.Contains(i),"Each old green relocates");C(g.Cells[i]==Cell.Orange,"Old tile gets orange departure warning");}
   foreach(int i in next){bool near=false;foreach(int o in old)if(Math.Abs(i%33-o%33)<=1&&Math.Abs(i/33-o/33)<=1)near=true;C(near,"Reachable one-tile move");C(i%33>0&&i%33<32&&i/33>0&&i/33<11,"Interior position");C(i%33==0||g.Cells[i-1]!=Cell.Safe,"No joined horizontal safe tiles");C(i/33==0||g.Cells[i-33]!=Cell.Safe,"No joined vertical safe tiles");}
   g.Paused=true;double timer=g.SafeMoveSeconds;g.Tick(4);C(g.SafeMoveSeconds==timer&&g.SafeMoveCount==1,"Safe relocation pauses on disconnect");g.Paused=false;g.Tick(.1);C(g.SafeMoveCount==1,"No extra immediate move");
   // Exercise many relocations independently of survival scoring/lives.
   for(int k=0;k<10;k++){g.Lives=100;g.SecondsLeft=90;g.Tick(5);C(Safe(g).Count==8,"Eight greens after repeated relocation");}
  }
  Console.WriteLine("PASS "+n+" safe-relocation checks: exact 5-second timing, eight isolated interior greens, one-step moves, departure warning, pause and repeat relocation.");
 }
}
