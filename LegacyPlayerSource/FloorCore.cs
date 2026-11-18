using System;
using System.Collections.Generic;

namespace Floor33 {
 public struct Address { public int Chain,Controller,Port,Position; }
 public static class Layout {
  public const int Width=33,Height=12,Count=Width*Height,ChainCount=6,MaxPerPort=66;
  public static Address At(int index) {
   if(index<0||index>=Count)throw new ArgumentOutOfRangeException("index");
   int row=index/33,col=index%33,chain=row/2,localRow=row-chain*2;
   return new Address{Chain=chain+1,Controller=chain/8+1,Port=chain%8+1,Position=localRow*33+(localRow%2==0?col:32-col)+1};
  }
  public static int Index(int controller,int port,int position) {
   if(controller<1||controller>1||port<1||port>8)return -1;
   int chain=(controller-1)*8+port-1;
   if(chain>=ChainCount||position<1||position>MaxPerPort)return -1;
   int r=(position-1)/33,c=(position-1)%33;
   return (chain*2+r)*33+(r%2==0?c:32-c);
  }
 }
 public enum Cell { Safe,Blue,Red,Cleared,Purple,Orange,Off }
 public sealed class Game {
  public readonly Cell[] Cells=new Cell[Layout.Count];
  public readonly bool[] Held=new bool[Layout.Count];
  public readonly int[] TouchCounts=new int[Layout.Count];
  public int Score,Hits,UniqueTouched,LastTile=-1,Difficulty,Errors;
  public double SecondsLeft,Countdown,StageSeconds; public int DurationSeconds=30;
  public int GameMode,Combo,BestCombo,Wave,Targets,Lives=3; public bool LavaActive;  readonly Random random; int laneX=3,direction=1;
  public bool Active,Finished,Paused;
  public string Feedback="Touch any tile to change its colour";
  static readonly bool[] startLetters=BuildStartLetters();
  static bool[] BuildStartLetters(){var mask=new bool[Layout.Count];string[] glyphs={"01111|10000|10000|01110|00001|00001|11110","11111|00100|00100|00100|00100|00100|00100","01110|10001|10001|11111|10001|10001|10001","11110|10001|10001|11110|10100|10010|10001","11111|00100|00100|00100|00100|00100|00100"};for(int letter=0;letter<5;letter++){string[] rows=glyphs[letter].Split('|');for(int y=0;y<7;y++)for(int x=0;x<5;x++)if(rows[y][x]=='1')mask[(y+2)*33+2+letter*6+x]=true;}return mask;}
  public bool IsStartTile(int i){return i>=0&&i<Layout.Count&&startLetters[i];}
  public bool StartPressed(int i,bool down){return IsStartTile(i)&&!Active&&down&&!Held[i];}
  public void ShowIdleStart(){if(Active)return;for(int i=0;i<Cells.Length;i++)Cells[i]=startLetters[i]?Cell.Safe:Cell.Off;}
  public Game(int seed){random=new Random(seed);ShowIdleStart();}
  public void Start(int difficulty,int duration=30,int gameMode=0){if(duration!=30&&duration!=60&&duration!=90)throw new ArgumentOutOfRangeException("duration");GameMode=gameMode;DurationSeconds=duration;Score=Hits=UniqueTouched=Errors=Combo=BestCombo=Wave=0;LastTile=-1;Difficulty=difficulty;SecondsLeft=DurationSeconds;Countdown=3;Finished=Paused=false;Active=true;Lives=3;LavaActive=false;laneX=3;direction=1;Array.Clear(TouchCounts,0,TouchCounts.Length);for(int i=0;i<Cells.Length;i++)Cells[i]=Cell.Blue;if(GameMode==1)NewStage();if(GameMode==2)NewLavaRound();Feedback=GameMode==2?"8 SINGLE SAFE TILES - LAVA WAVES MOVE IN FROM RANDOM SIDES":GameMode==1?"START NEAR COLUMN 1. CHASE BLUE ALONG THE FLOOR.":"Get ready. Every new touch counts +1.";}
  public void Tick(double dt){if(!Active||Paused)return;dt=Math.Max(0,dt);if(Countdown>0){double used=Math.Min(dt,Countdown);Countdown-=used;dt-=used;}if(Countdown>0)return;SecondsLeft=Math.Max(0,SecondsLeft-dt);if(SecondsLeft<=0){Active=false;Finished=true;Feedback="TIME UP - SCORE "+Score;return;}if(GameMode==2){TickLava(dt);return;}if(GameMode==1){StageSeconds-=dt;if(StageSeconds<=0)NewStage();}}
  public int Touch(int i,bool down){if(i<0||i>=Layout.Count)return 0;bool rising=down&&!Held[i];Held[i]=down;if(!rising||!Active||Paused||Countdown>0)return 0;
   LastTile=i;if(TouchCounts[i]==0)UniqueTouched++;TouchCounts[i]++;
   if(GameMode==2){if(LavaActive&&Cells[i]==Cell.Red)return LavaDamage();if(Cells[i]==Cell.Safe)Feedback="IN GREEN - STAY SAFE WHEN LAVA RISES";return 0;}
   if(GameMode==1){
    if(Cells[i]==Cell.Red){Score-=2;Errors++;Combo=0;Feedback="RED -2 | COMBO RESET";return -2;}
    if(Cells[i]!=Cell.Blue&&Cells[i]!=Cell.Purple)return 0;
    int points=Cells[i]==Cell.Purple?3:1;Cells[i]=Cell.Cleared;Targets--;Hits++;Combo++;BestCombo=Math.Max(BestCombo,Combo);if(Combo%5==0)points+=3;Score+=points;Feedback="TARGET +"+points+" | COMBO "+Combo;if(Targets==0){StageSeconds=Math.Min(StageSeconds,.7);Feedback="STAGE CLEAR! NEXT TARGETS AHEAD";}return points;
   }
   Score++;Hits++;
   Cells[i]=Cells[i]==Cell.Blue?Cell.Safe:Cells[i]==Cell.Safe?Cell.Purple:Cells[i]==Cell.Purple?Cell.Orange:Cell.Blue;
   Feedback="TOUCH +1 | R"+(i/Layout.Width+1)+" C"+(i%Layout.Width+1)+" | TILE COUNT "+TouchCounts[i];return 1;
  }
  public void Stop(){Active=false;Paused=false;Finished=false;}
  public sealed class LavaFront { public int Side,SpanStart,SpanWidth;public double Position,Speed,Warning; }
  public readonly List<LavaFront> LavaFronts=new List<LavaFront>();
  public bool LavaResting;public int ActiveSpots,PassedWaves;double damageCooldown,spawnIn;int previousSide=-1;
  readonly bool[] safeMask=new bool[Layout.Count];readonly int[] safeTiles=new int[8];readonly double[] departingWarning=new double[Layout.Count];
  public double SafeMoveSeconds=5;public int SafeMoveCount;
  public string WaveLabel { get { return "WAVES "+LavaFronts.Count+" / SAFE MOVES "+Math.Ceiling(SafeMoveSeconds)+"s"; } }
  void NewLavaRound(){Wave=1;LavaActive=false;LavaResting=false;StageSeconds=2;damageCooldown=0;spawnIn=0;previousSide=-1;PassedWaves=0;LavaFronts.Clear();Array.Clear(safeMask,0,safeMask.Length);
   SafeMoveSeconds=5;SafeMoveCount=0;Array.Clear(departingWarning,0,departingWarning.Length);int safeIndex=0;foreach(int y in new int[]{3,8})foreach(int x in new int[]{4,12,20,28}){safeTiles[safeIndex++]=y*33+x;safeMask[y*33+x]=true;}
   PaintLavaWave();Feedback="8 SINGLE SAFE TILES - RANDOM MOVING WAVES IN 2s";
  }
  void MoveSafeTiles(){Array.Clear(safeMask,0,safeMask.Length);
   for(int k=0;k<safeTiles.Length;k++){int old=safeTiles[k],ox=old%33,oy=old/33,sectorX=k%4,sectorY=k/4;var candidates=new List<int>();
    for(int dy=-1;dy<=1;dy++)for(int dx=-1;dx<=1;dx++){if(dx==0&&dy==0)continue;int x=ox+dx,y=oy+dy;if(x<1+sectorX*8||x>7+sectorX*8||y<(sectorY==0?1:7)||y>(sectorY==0?4:10))continue;candidates.Add(y*33+x);}
    int next=candidates[random.Next(candidates.Count)];safeTiles[k]=next;safeMask[next]=true;departingWarning[old]=1;
   }SafeMoveCount++;Feedback="GREEN TILES MOVED - FOLLOW GREEN; OLD TILES WARN ORANGE";
  }
  void SpawnLavaFront(){if(LavaFronts.Count>=(Wave>=3?3:2))return;int side;do{side=random.Next(4);}while(side==previousSide);previousSide=side;
   int cross=side<2?Layout.Height:Layout.Width,length=side<2?Layout.Width:Layout.Height,width=random.Next(4,8);
   LavaFronts.Add(new LavaFront{Side=side,SpanStart=random.Next(cross-width+1),SpanWidth=width,Position=-2,Speed=(length+4)/(7.5+random.NextDouble()*3.5)*Math.Min(1.3,1+(Wave-1)*.05),Warning=1.1});
   Feedback="WAVE FROM "+new string[]{"LEFT","RIGHT","TOP","BOTTOM"}[side]+" - FIND GREEN";
  }
  void PaintLavaWave(){ActiveSpots=0;for(int y=0;y<Layout.Height;y++)for(int x=0;x<Layout.Width;x++){
   int i=y*33+x;if(safeMask[i]){Cells[i]=Cell.Safe;continue;}if(departingWarning[i]>0){Cells[i]=Cell.Orange;continue;}Cell c=Cell.Blue;
   foreach(LavaFront f in LavaFronts){int cross=f.Side<2?y:x;if(cross<f.SpanStart||cross>=f.SpanStart+f.SpanWidth)continue;int z=f.Side<2?x:y,length=f.Side<2?Layout.Width:Layout.Height;if(f.Side%2==1)z=length-1-z;
    double bend=.7*Math.Sin((cross-f.SpanStart)*Math.PI/(f.SpanWidth-1)),front=f.Position+bend;
    bool red=f.Warning<=0&&z>=front-2&&z<=front;
    bool orange=f.Warning>0?z<2:z>front&&z<=front+2;
    if(red)c=Cell.Red;else if(orange&&c!=Cell.Red)c=Cell.Orange;
   }Cells[i]=c;if(c==Cell.Red)ActiveSpots++;
  }}
  bool InLavaSafeZone(){bool green=false;for(int i=0;i<Held.Length;i++)if(Held[i]){if(Cells[i]==Cell.Red)return false;if(safeMask[i]||departingWarning[i]>0)green=true;}return green;}
  bool OnRed(){for(int i=0;i<Held.Length;i++)if(Held[i]&&Cells[i]==Cell.Red)return true;return false;}
  int LavaDamage(){if(damageCooldown>0||!Active)return 0;damageCooldown=1;Lives--;Errors++;Score-=2;Feedback="LAVA WAVE HIT -2 | LIVES "+Lives;if(Lives<=0){Active=false;Finished=true;Feedback="OUT OF LIVES - FINAL SCORE "+Score;}return -2;}
  void TickLava(double dt){damageCooldown=Math.Max(0,damageCooldown-dt);StageSeconds-=dt;
   for(int i=0;i<departingWarning.Length;i++)departingWarning[i]=Math.Max(0,departingWarning[i]-dt);
   SafeMoveSeconds-=dt;while(SafeMoveSeconds<=0){MoveSafeTiles();SafeMoveSeconds+=5;}
   if(!LavaActive){if(StageSeconds<=0){LavaActive=true;StageSeconds=9;SpawnLavaFront();spawnIn=1.5;}PaintLavaWave();return;}
   for(int k=LavaFronts.Count-1;k>=0;k--){LavaFront f=LavaFronts[k];double move=dt;if(f.Warning>0){double used=Math.Min(move,f.Warning);f.Warning-=used;move-=used;}f.Position+=move*f.Speed;int length=f.Side<2?Layout.Width:Layout.Height;if(f.Position>length+3){LavaFronts.RemoveAt(k);PassedWaves++;}}
   spawnIn-=dt;if(spawnIn<=0){SpawnLavaFront();spawnIn=1.3+random.NextDouble()*.9;}
   PaintLavaWave();if(OnRed())LavaDamage();if(!Active)return;
   if(StageSeconds<=0){if(InLavaSafeZone()){Score+=5;Hits++;Feedback="SAFE CHECKPOINT +5";}else LavaDamage();if(!Active)return;Wave++;StageSeconds=9;}
  }
  void NewStage(){Wave++;StageSeconds=Math.Max(4,7-(Wave-1)/4);
   if(Wave>1){laneX+=direction*4;if(laneX>=27){laneX=27;direction=-1;}if(laneX<=3){laneX=3;direction=1;}}
   for(int y=0;y<Layout.Height;y++)for(int x=0;x<Layout.Width;x++){int i=y*Layout.Width+x;bool route=y==5||y==6||Math.Abs(x-laneX)<=1;Cells[i]=!route&&!Held[i]&&random.NextDouble()<.14?Cell.Red:Cell.Safe;}
   Targets=0;int top=Wave%2==1?2:7;
   for(int y=top;y<top+3;y++)for(int x=laneX-2;x<laneX+3;x++){int i=y*Layout.Width+x;if(!Held[i]){Cells[i]=Cell.Blue;Targets++;}}
   int bonus=(top+1)*Layout.Width+laneX;if(!Held[bonus])Cells[bonus]=Cell.Purple;
   Feedback="STAGE "+Wave+" | TARGETS NEAR COLUMN "+(laneX+1);
  }
 }
}
