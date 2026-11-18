using System;
using Floor33;
public class Verify {
 static int n;static void Check(bool b,string m){n++;if(!b)throw new Exception(m);}
 public static void Main(){
  Game g=new Game(1);g.Start(0);Check(g.Touch(0,true)==0&&g.Score==0,"Countdown blocks touch");g.Touch(0,false);g.Tick(3);
  for(int i=0;i<396;i++){Check(g.Touch(i,true)==1,"New press counts");Check(g.Cells[i]==Cell.Safe&&g.TouchCounts[i]==1,"Colour green and tile count");Check(g.Touch(i,true)==0,"Held press ignored");Address a=Layout.At(i);Check(Layout.Index(a.Controller,a.Port,a.Position)==i,"Mapping unchanged");}
  Check(g.Score==396&&g.UniqueTouched==396,"All tile touches counted independently");
  g.Touch(0,false);g.Touch(0,true);Check(g.Cells[0]==Cell.Purple&&g.Score==397&&g.TouchCounts[0]==2,"Second press purple");g.Touch(0,false);g.Touch(0,true);Check(g.Cells[0]==Cell.Orange&&g.Score==398,"Third press orange");g.Touch(0,false);g.Touch(0,true);Check(g.Cells[0]==Cell.Blue&&g.Score==399,"Fourth press blue");
  g.Paused=true;double left=g.SecondsLeft;g.Tick(4);g.Touch(0,false);Check(g.Touch(0,true)==0&&g.Score==399&&g.SecondsLeft==left,"Pause blocks count and time");g.Paused=false;g.Tick(30);Check(g.Finished&&!g.Active,"30 seconds expiry");g.Touch(1,false);Check(g.Touch(1,true)==0&&g.Score==399,"No post-game count");
  int[] saved=(int[])g.TouchCounts.Clone();g.Start(0);Check(g.Score==0&&g.UniqueTouched==0&&g.TouchCounts[0]==0&&g.Cells[0]==Cell.Blue,"New player reset");Check(saved[0]==4,"Saved tile counts independent");
  Check(g.Touch(-1,true)==0&&g.Touch(396,true)==0,"Invalid address ignored");foreach(int duration in new int[]{30,60,90}){var timed=new Game(4);timed.Start(0,duration);Check(timed.DurationSeconds==duration&&timed.SecondsLeft==duration,"Selected duration");timed.Tick(3);timed.Tick(duration-0.5);Check(timed.Active&&!timed.Finished,"Not early expiry");timed.Tick(.5);Check(timed.Finished&&timed.SecondsLeft==0,"Selected duration expires");}Console.WriteLine("PASS "+n+" touch-counter checks: all 396 tiles, per-tile/total counts, colour cycle, hold/release, pause, expiry and reset.");
 }
}
