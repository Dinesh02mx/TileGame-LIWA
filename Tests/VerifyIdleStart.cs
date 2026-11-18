using System;
using System.Net;
using System.Net.Sockets;
using Floor33;
public class VerifyIdleStart {
 static int n;static void C(bool b,string s){n++;if(!b)throw new Exception(s);}
 public static void Main(){
  var g=new Game(1);int lit=0,first=-1;for(int i=0;i<396;i++){if(g.Cells[i]==Cell.Safe){lit++;if(first<0)first=i;C(i%33>=2&&i%33<=30&&i/33>=2&&i/33<=8,"Word fits centred 29x7 area");}else C(g.Cells[i]==Cell.Off,"Background dark");C(g.IsStartTile(i)==(g.Cells[i]==Cell.Safe),"Floor start hit map matches green letters");}
  C(lit>60&&lit<100,"Recognizable multi-letter message");foreach(int col in new int[]{7,13,19,25})for(int row=2;row<9;row++)C(g.Cells[row*33+col]==Cell.Off,"Letter spacing dark");
  C(g.StartPressed(first,true),"New step on green requests start");g.Touch(first,true);C(!g.StartPressed(first,true),"Held letter does not repeat start");g.Touch(first,false);C(g.StartPressed(first,true),"Release and step can start");C(!g.StartPressed(0,true),"Dark background does not start");g.Start(0,30,2);C(!g.StartPressed(first,true),"No idle start during play");g.ShowIdleStart();C(g.Active&&g.Countdown==3,"Idle draw cannot interrupt game");g.Tick(33);g.ShowIdleStart();C(g.Finished&&g.Cells[first]==Cell.Safe&&g.Cells[0]==Cell.Off,"START returns after game over");
  using(var capture=new UdpClient(new IPEndPoint(IPAddress.Loopback,0)))using(var reservation=new UdpClient(new IPEndPoint(IPAddress.Loopback,0))){var cfg=new Settings{hardwareEnabled=true,mappingVerified=true,localAddress="127.0.0.1",broadcastAddress="127.0.0.1",receivePort=((IPEndPoint)reservation.Client.LocalEndPoint).Port,sendPort=((IPEndPoint)capture.Client.LocalEndPoint).Port};reservation.Close();capture.Client.ReceiveTimeout=1000;
   using(var net=new Network(cfg)){net.Update(1,g.Cells,true,-1,delegate(int i,bool b){});C(net.Sent==5,"Idle word frame sent");for(int packet=0;packet<5;packet++){IPEndPoint from=null;byte[] b=capture.Receive(ref from);if(b[1]!=0x33)continue;int start=(b[10]*256+b[11])*40,samples=(b.Length-17)/24;for(int s=0;s<samples;s++)for(int port=1;port<=8;port++){int i=Layout.Index(1,port,start+s+1),o=14+s*24+port-1;bool green=i>=0&&g.Cells[i]==Cell.Safe;C(b[o]==(green?80:0)&&b[o+8]==0&&b[o+16]==0,"Correct green START and black background across hardware ports");}}}
  }
  Console.WriteLine("PASS "+n+" idle START checks: centred letters, dark background, press/release start detection, return after finish, no play interruption and actual UDP colour output.");
 }
}
