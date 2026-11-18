using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using KLGame;

namespace Floor33 {
 [Serializable] public class Settings {
  public bool hardwareEnabled=false;
  public bool mappingVerified=false;
  public string localAddress="192.168.4.1";
  public string broadcastAddress="192.168.4.255";
  public int receivePort=7800,sendPort=4626;
  public int[] receiverWireIds=new int[]{0};
  public int[] controllerWireIds=new int[]{1};
  public int brightness=80;
  public string note="33 columns x 12 rows = 396 tiles. P1-P6=66; controller 1 ports 1-6 active, ports 7-8 unused. Confirm receiver ID and controller address during commissioning. Output uses supplier SDK framing; multi-controller/packet sequencing requires physical verification.";
 }
 public static class Packets {
  static void U16(List<byte> b,int v){b.Add((byte)(v>>8));b.Add((byte)v);}
  public static byte[] Make(int controller,int kind,int flag,byte[] payload){
   var b=new List<byte>();b.Add(0x75);b.Add((byte)kind);b.Add((byte)kind);U16(b,8+payload.Length);b.Add(2);b.Add((byte)controller);b.Add(0);
   b.Add(kind==0x11?(byte)0x33:kind==0x44?(byte)0x55:(byte)0x88);b.Add(kind==0x11?(byte)0x44:kind==0x44?(byte)0x66:(byte)0x77);
   U16(b,flag);U16(b,payload.Length);b.AddRange(payload);U16(b,b.Count);
   byte sum=0;foreach(byte v in b)sum=unchecked((byte)(sum+v));b.Add(InteractionLight.gyhd_pass_word[sum]);return b.ToArray();
  }
  public static List<byte[]> Frame(int controller,int[] lengths,byte[] rgb){
   if(lengths.Length!=8||rgb.Length!=8*Layout.MaxPerPort*3)throw new ArgumentException("8 independent ports of 66 padded GRB samples required");
   var packets=new List<byte[]>();packets.Add(Make(controller,0x11,0,new byte[0]));
   var counts=new List<byte>();foreach(int n in lengths)U16(counts,n);packets.Add(Make(controller,0x22,0xfff0,counts.ToArray()));
   int max=0;foreach(int n in lengths)max=Math.Max(max,n);
   // 40 samples x 8 ports x 3 channels stays below 1000 bytes per UDP datagram.
   for(int start=0,seq=0;start<max;start+=40,seq++){int count=Math.Min(40,max-start);var p=new byte[count*24];Buffer.BlockCopy(rgb,start*24,p,0,p.Length);packets.Add(Make(controller,0x33,seq,p));}
   packets.Add(Make(controller,0x44,0,new byte[0]));return packets;
  }
  public static int Decode(byte[] b,int[] ids,Action<int,bool> callback,Action<int> seen){
   if(b==null||b.Length<174||b[0]!=0x88)return 0;
   int receiver=Array.IndexOf(ids,(int)b[1]);if(receiver<0)return 0;
   int ports=Math.Min(8,(b.Length-3)/171),n=0;if(ports==0)return 0;
   for(int port=0;port<ports;port++)for(int pos=0;pos<170;pos++){
    byte state=b[3+port*171+pos];int i=Layout.Index(receiver+1,port+1,pos+1);if(i<0)continue;
    if(state!=0xcc&&state!=0xba&&state!=0)continue;callback(i,state==0xcc);n++;
   }
   if(n>0&&ports==8)seen(receiver);return n;
  }
 }
 public sealed class Network : IDisposable {
  readonly Settings cfg;UdpClient socket;IPEndPoint target;double retryAt,nextSend;
  public readonly double[] Seen=new double[]{-100};
  public string Status="SIMULATION - physical output off";
  public long Received,Sent; public string LastPacket="No packets yet";
  public Network(Settings c){cfg=c;}
  public bool Healthy(double now){return socket!=null&&now-Seen[0]<2;}
  public void Update(double now,Cell[] cells,bool lit,int selected,Action<int,bool> touch,bool dimSafe=false){
   if(!cfg.hardwareEnabled)return;
   if(!cfg.mappingVerified){Status="HARDWARE BLOCKED - verify CAD mapping and wire IDs in FloorConfig.json";return;}
   if(socket==null&&now>=retryAt){try{socket=new UdpClient(AddressFamily.InterNetwork);socket.ExclusiveAddressUse=true;socket.Client.Bind(new IPEndPoint(IPAddress.Parse(cfg.localAddress),cfg.receivePort));socket.EnableBroadcast=true;socket.Client.Blocking=false;target=new IPEndPoint(IPAddress.Parse(cfg.broadcastAddress),cfg.sendPort);Status="Waiting for receiver 1";Seen[0]=-100;}catch(Exception e){Status="CONNECT: "+e.Message;Dispose();retryAt=now+2;return;}}
   if(socket==null)return;
   try{
    for(int k=0;k<128&&socket.Available>0;k++){IPEndPoint from=null;byte[] b=socket.Receive(ref from);Received++;LastPacket="RX "+from.Address+" | "+b.Length+" bytes | ID "+(b.Length>1?b[1].ToString():"?");Packets.Decode(b,cfg.receiverWireIds,touch,delegate(int c){Seen[c]=now;});}
    Status=Healthy(now)?"LIVE - receiver 1 responding":"LINK WAIT - receiver 1 "+(now-Seen[0]<2?"OK":"missing");
    if(now<nextSend)return;nextSend=now+1.0/30;
    for(int controller=1;controller<=1;controller++){
     byte[] rgb=new byte[Layout.MaxPerPort*24];int[] lengths=new int[8];for(int port=0;port<8;port++)lengths[port]=(controller-1)*8+port<Layout.ChainCount?Layout.MaxPerPort:0;
     for(int pos=1;pos<=Layout.MaxPerPort;pos++)for(int port=1;port<=8;port++){
      int i=Layout.Index(controller,port,pos);if(i<0||!lit)continue;int o=(pos-1)*24+port-1;
      int v=Math.Max(1,Math.Min(255,cfg.brightness));
      if(selected>=0){if(i==selected){rgb[o]=rgb[o+8]=rgb[o+16]=(byte)v;}continue;}
      Cell c=cells[i];if(c==Cell.Off)continue;if(c==Cell.Purple){rgb[o+8]=(byte)v;rgb[o+16]=(byte)v;}else if(c==Cell.Orange){rgb[o+8]=(byte)v;rgb[o]=(byte)(v/3);}else if(c==Cell.Red)rgb[o+8]=(byte)v;else if(c==Cell.Blue)rgb[o+16]=(byte)v;else rgb[o]=(byte)(dimSafe&&c==Cell.Safe?Math.Max(12,v/4):v);
     }
     foreach(byte[] p in Packets.Frame(cfg.controllerWireIds[controller-1],lengths,rgb)){socket.Send(p,p.Length,target);Sent++;}
    }
   }catch(SocketException e){Status="NETWORK: "+e.SocketErrorCode;Dispose();retryAt=now+2;}catch(Exception e){Status="NETWORK: "+e.Message;Dispose();retryAt=now+2;}
  }
  public void Dispose(){if(socket!=null){socket.Close();socket=null;}}
 }
}
