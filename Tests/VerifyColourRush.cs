using System;
using Floor33;
public class VerifyRush {
 static int n;static void C(bool b,string s){n++;if(!b)throw new Exception(s);}
 public static void Main(){
  for(int seed=0;seed<15;seed++){
   Game g=new Game(seed);g.Start(0,90,1);g.Tick(3);int red=Array.FindIndex(g.Cells,c=>c==Cell.Red),bonus=Array.FindIndex(g.Cells,c=>c==Cell.Purple);C(red>=0&&bonus>=0,"Hazard and bonus exist");C(g.Touch(bonus,true)==3&&g.Score==3,"Purple +3");C(g.Touch(bonus,true)==0,"Held bonus not repeated");g.Touch(bonus,false);
   for(int k=0;k<4;k++){int blue=Array.FindIndex(g.Cells,c=>c==Cell.Blue);C(g.Touch(blue,true)==(k==3?4:1),"Five-hit combo extra3");g.Touch(blue,false);}C(g.Score==10&&g.Combo==5,"Combo sum");C(g.Touch(red,true)==-2&&g.Score==8&&g.Combo==0,"Red penalty resets combo");C(g.Touch(red,true)==0,"Held red not repeated");g.Touch(red,false);
   g.Paused=true;double t=g.SecondsLeft,w=g.StageSeconds;g.Tick(3);C(g.SecondsLeft==t&&g.StageSeconds==w,"Stage and session pause");g.Paused=false;
   int previousCenter=-1,min=99,max=0;
   for(int stage=0;stage<14;stage++){int first=Array.FindIndex(g.Cells,c=>c==Cell.Blue||c==Cell.Purple),center=first%33+2;min=Math.Min(min,center);max=Math.Max(max,center);if(previousCenter>=0)C(Math.Abs(center-previousCenter)<=4,"Length-aware four-column movement");previousCenter=center;
    for(int x=0;x<33;x++){C(g.Cells[5*33+x]!=Cell.Red&&g.Cells[6*33+x]!=Cell.Red,"Green centre route");}
    C(g.StageSeconds>=4&&g.StageSeconds<=7,"Stage timing limits");int wave=g.Wave;g.Tick(g.StageSeconds);C(g.Wave==wave+1,"Pattern advances");}
   C(min==3&&max==27,"Length covered in stages");g.Tick(90);C(g.Finished&&!g.Active,"Session expiry");int score=g.Score;g.Touch(0,true);C(g.Score==score,"No post-time score");
  }
  for(int d=30;d<=90;d+=30){Game g=new Game(1);g.Start(0,d,0);g.Tick(3);C(g.Touch(0,true)==1&&g.Cells[0]==Cell.Safe,"Touch Counter preserved");g.Tick(d);C(g.Finished,"Each selectable time ends");}
  Console.WriteLine("PASS "+n+" Colour Rush checks: scoring, combos, held touches, safe routes, length-aware progression, stage limits, pause, all durations and Touch Counter preservation.");
 }
}
