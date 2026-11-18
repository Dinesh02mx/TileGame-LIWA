using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace KLGame
{
    public class InteractionLight
    {
        
        public static readonly byte[] gyhd_pass_word = new byte[256]
{
0x23,0x3f,0xbb,0x45,0x6b,0xb2,0x5c,0x4c,0x27,0x45,0xcd,0x25,0xdf,0xff,0xa5,0xe7,
0x10,0xdc,0x63,0x3d,0x19,0xcb,0xcb,0x9b,0x6b,0x1e,0x5c,0x90,0xda,0xc2,0xe2,0x58,
0xc4,0xbe,0x43,0xc3,0x9f,0xb9,0xd1,0x18,0xa3,0x41,0x19,0xac,0x7e,0x3f,0xe0,0x3d,
0xa0,0x50,0x7d,0x5b,0xef,0x90,0x19,0x8d,0xb7,0xcc,0xab,0xbc,0xff,0xa2,0x68,0xe1,
0xba,0x5b,0xe8,0x03,0x64,0xd0,0x31,0xd3,0x25,0xc0,0x14,0x63,0x1b,0x5c,0x93,0x98,
0x56,0xb1,0x35,0x99,0x5e,0xb1,0xc8,0x21,0xaf,0xc3,0x0f,0xe4,0xf7,0x12,0xf4,0x96,
0xa5,0xe5,0xd4,0x60,0x54,0xc8,0xa8,0xbf,0x26,0x70,0xab,0x74,0x79,0xba,0x93,0xcb,
0x1e,0x76,0x73,0x9f,0xee,0x8b,0x3c,0x39,0xeb,0xd5,0x9f,0xc6,0xa0,0x32,0x61,0xc9,
0xfd,0xf2,0xf0,0x4d,0x66,0x0c,0xb7,0xeb,0xf3,0xf7,0x4b,0x5a,0x0d,0xec,0x38,0x85,
0x96,0x80,0x8a,0xbe,0x8c,0x0d,0xd5,0x12,0x07,0x75,0xff,0x2d,0x45,0xd6,0xb3,0x32,
0x1c,0x42,0x7b,0xef,0xbe,0x49,0x8e,0xda,0xfd,0x05,0xd4,0xae,0x98,0x4b,0xe2,0xe2,
0xac,0x4e,0x23,0x5d,0xfa,0xee,0x13,0x20,0xf7,0xdf,0x59,0x7b,0x56,0x8a,0x96,0x92,
0xd6,0xc0,0x5d,0x98,0x9c,0xd3,0x43,0x33,0xc3,0xa5,0x42,0x0a,0x0a,0x1f,0x01,0xc6,
0xea,0x87,0x22,0x80,0xd0,0xc8,0xd5,0xa9,0xee,0x4a,0xdd,0xd0,0x68,0xaa,0xa6,0x24,
0x4c,0xb1,0xc4,0x03,0x8d,0xa7,0x7f,0x38,0xb1,0xcb,0x2d,0x6b,0x2e,0x52,0xd9,0x8b,
0xa8,0x2d,0xc6,0x06,0x2b,0x0b,0x39,0x58,0xb6,0x54,0xbd,0x1d,0x23,0x8f,0x8a,0xab
};

        public static readonly byte[] frzm_pass_word = new byte[256]
{
0x44,0xaa,0x1f,0xbf,0x6a,0x43,0x37,0x47,0x0a,0xb9,0x92,0xe3,0x9e,0xcf,0xef,0x0c,
0xe0,0xb3,0xad,0xfd,0x98,0x3e,0xd3,0x9d,0x02,0x7f,0x79,0xeb,0xf9,0xbc,0xe0,0xd1,
0x4b,0x30,0x16,0x1d,0xbe,0xd1,0x3a,0xb6,0x26,0x64,0xdf,0xa7,0x61,0x01,0xdb,0xdb,
0x1d,0x39,0x0d,0xc1,0x9b,0xb5,0xc6,0x04,0xd1,0x2c,0x18,0x5c,0xea,0xad,0x13,0x48,
0x6b,0x83,0x72,0x93,0xd3,0x99,0x0d,0xeb,0x39,0xce,0x01,0x47,0xc1,0x6e,0x34,0xb9,
0x75,0x4a,0xcc,0xfe,0xf7,0x6e,0x42,0x45,0x42,0x59,0x34,0x5d,0xba,0xb3,0x9b,0x76,
0xf9,0x0c,0x35,0xfb,0xcd,0x98,0x11,0xf0,0xc0,0x3b,0xd1,0x53,0xf4,0x68,0x04,0x0e,
0x9b,0x47,0x3b,0x80,0x46,0xb8,0x7c,0xe7,0xf4,0x43,0xbf,0xa8,0x1b,0x0b,0xfd,0x50,
0x93,0x05,0x92,0x3e,0xd9,0x2e,0xec,0xf0,0x5a,0xdd,0x49,0x53,0x46,0x2d,0x5b,0x61,
0x13,0x73,0xd0,0x34,0xda,0x95,0xd4,0x72,0xe2,0x39,0x13,0x63,0x95,0xff,0x5d,0x3b,
0xff,0xae,0x1a,0x71,0x91,0xd3,0x28,0x62,0x01,0x9b,0x10,0xf8,0x59,0xfa,0xd1,0x7c,
0xe2,0x8a,0x4b,0x81,0x53,0x19,0xc3,0x5b,0x6b,0x44,0x11,0xdd,0xb2,0xad,0xcf,0x69,
0xf0,0xb2,0x32,0x97,0xec,0xeb,0x7b,0xc9,0xe8,0xa9,0x66,0x29,0x09,0xc2,0x4b,0x15,
0x2e,0x91,0x34,0xfa,0x6e,0xdc,0x56,0xf1,0x83,0xa3,0x42,0xb6,0xc4,0x56,0x83,0x7d,
0xb9,0x59,0x2a,0xbb,0x48,0xac,0x6a,0x62,0xa6,0x4c,0x33,0xfb,0xc9,0x19,0xe9,0x51,
0x70,0x9f,0xdd,0x7f,0x69,0xce,0x58,0x48,0x39,0xaf,0x1a,0x7a,0xd3,0xac,0x23,0xed
};
        //灯具类型   frzm=0   gyhd=1
        public static int m_s_nLightType = 1;

        //灯具控制发送数据给PC电脑   电脑接受数据的端口
        public static int m_nRecivePort = 7800;//丰荣照明8200   光影互动7800

        //灯具控制器接受数据端口   发送数据改变灯的颜色
        public static int m_nSendPort = 4626;//丰荣照明4628   光影互动4626

        //灯具被感应的标志    有灯具被按下或者脚踩了识别标志值
        public static byte m_bLightPressFlag = 0xcc;//丰荣照明ab   光影互动cc

        public static byte m_bReciveCmdHead = 0x88;//丰荣照明96   光影互动88

        public static byte m_bSendCmdHead = 0x75;//丰荣照明90   光影互动75

        public static byte Get_crc(byte[] Pdat, int len)
        {
            byte dat = 0;
            int i = 0;
            for (i = 0; i < len; i++)
            {
                dat += Pdat[i];
            }
            return dat;
        }


        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have an even number of characters.");
            }

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        public static byte[] GetNetMask()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            byte[] maskArr=null;
            foreach (NetworkInterface ni in networkInterfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    UnicastIPAddressInformationCollection unicastAddresses = ni.GetIPProperties().UnicastAddresses;

                    foreach (UnicastIPAddressInformation address in unicastAddresses)
                    {
                        if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            string ipAddress = address.Address.ToString();
                            string ipMask = address.IPv4Mask.ToString();
                            maskArr = address.IPv4Mask.GetAddressBytes();

                        }
                    }
                }
            }
            return maskArr;


         }

        public static byte[] GetControIpCmd(string strDeviceName,string strMark)
        {
            List<byte> list = new List<byte>();

            list.Add(0x56);//命令类型 // Command type
            list.Add(0x55);//2个字节  的自定义命令标志 // Custom command flag (2 bytes)
            list.Add(0x55);

            list.Add(0x00);//2个字节   命令长度   数据位的数据长度 // Command length (2 bytes, length of data)
            list.Add(0x0E);

            list.Add(0x02);//PC/控制机 // PC/Controller



            for (int i = 0; i < strDeviceName.Length; i++)
            {
                list.Add(Convert.ToByte(strDeviceName[i]));
            }

            if(strMark == "HC01")
            {
                list.Add(Convert.ToByte(1));
            }
            else if (strMark == "HC02")
            {
                list.Add(Convert.ToByte(2));
            }
            else if (strMark == "HC04")
            {
                list.Add(Convert.ToByte(3));
            }
            else if (strMark == "HC08")
            {
                list.Add(Convert.ToByte(4));
            }

            

            if (m_s_nLightType == 0)
            {
                list.Add(0x01);   //第9个数据  品牌标识 0x01。 // Brand identifier (0x01)
            }
            else if (m_s_nLightType == 1)
            {
                list.Add(0x00);   //第9个数据  品牌标识 0x01。 // Brand identifier (0x01)
            }
            

            list.Add(0x00);




            byte[] addressMadkBytes = GetNetMask();
            if (addressMadkBytes != null)
            {
                for (int i = 0; i < addressMadkBytes.Length; i++)
                {
                    list.Add(addressMadkBytes[i]);
                }
            }



            //前面所有数据的长度  // Add total data length to the command
            UInt16 dataLen = Convert.ToUInt16(list.Count);
            list.Add(Convert.ToByte(dataLen>>8));
            list.Add(Convert.ToByte(dataLen &0xFF));


            byte Crc = Get_crc(list.ToArray(), list.Count);
            if (m_s_nLightType == 0)
            {
                Crc = frzm_pass_word[Crc];
            }
            else if (m_s_nLightType == 1)
            {
                Crc = gyhd_pass_word[Crc];
            }

            
            list.Add(Crc);

            return list.ToArray();
        }

        //获取发送数据头包的字节  // Generate the header packet for sending data
        public static byte[] GetSendHead( byte ControlCount)
        {
            List<byte> list = new List<byte>();

            list.Add(m_bSendCmdHead);//命令类型 // Command type 
            list.Add(0x11);//2个字节  的自定义命令标志 // Custom command flag (2 bytes)
            list.Add(0x11);

            list.Add(0x00);//2个字节   命令长度   数据位的数据长度    1024 // Command length (2 bytes, length of data)
            list.Add(0x08);

            list.Add(0x02);//PC/控制机 // PC/Controller

            list.Add(ControlCount);//控制器的序号，如果有多个控制器，则需要分按照序号发送 // Controller index (if multiple controllers, send in order)
            list.Add(0x00);//电脑主控序号 // PC master index

            // ========== Start data section ============================================
            //==========  开始数据   ============================================
            // Data type (data frame: 0x88, 0x77; start frame: 0x33, 0x44; end frame: 0x55, 0x66)
            //发送的数据类型，数据帧0x88,0x77,开始头帧0x33,0x44,结束帧0x55,0x66 
            list.Add(0x033);

            list.Add(0x044);

            list.Add(0x00);//通知控制前   每个端口控制多少灯的标志，发送头这里不用就是0 // Flag for number of lights per port (0 for header)
            list.Add(0x00);

            list.Add(0x00);//灯像素控制的数据长度 // Length of pixel control data
            list.Add(0x00);//

            //数据总长度 // Add total data length
            UInt16 dataLen = Convert.ToUInt16(list.Count);
            list.Add(Convert.ToByte(dataLen >> 8));
            list.Add(Convert.ToByte(dataLen & 0xFF));


            byte Crc = Get_crc(list.ToArray(), list.Count);
            if (m_s_nLightType == 0)
            {
                Crc = frzm_pass_word[Crc];
            }
            else if (m_s_nLightType == 1)
            {
                Crc = gyhd_pass_word[Crc];
            }
            list.Add(Crc);


            return list.ToArray();
        }



        //获取发送数据   通知控制器，每路端口控制多少个灯
        // Generate the command to notify the controller about the number of lights per port
        public static byte[] GetSendControlLightCount(byte ControlCount, byte m_nControlPort,UInt16 ControlLightCount)
        {
            List<byte> list = new List<byte>();

            list.Add(m_bSendCmdHead);//命令类型 // Command type
            list.Add(0x22);//2个字节  的自定义命令标志 // Custom command flag (2 bytes)
            list.Add(0x22);

            // Command length = controller index (1 byte) + PC master index (1 byte) + flag for number of lights per port (2 bytes) + pixel control data length (2 bytes) + N ports * 2
            //命令长度 =  控制器序号1位 + 电脑主控序号1位 +  每个端口控制多少灯的标志2位 + 像素控制数据长度2位  + N路端口*2
            int TempLen = 1 + 1 + 2 + 2 + 8 * 2;
            UInt16 Len1 = Convert.ToUInt16(TempLen);
            list.Add(Convert.ToByte(Len1 >> 8));
            list.Add(Convert.ToByte(Len1 & 0xFF));

            list.Add(0x02);//PC/控制机 // PC/Controller

            // Controller serial number. If there are multiple controllers, send them according to the serial number
            list.Add(ControlCount);//控制器的序号，如果有多个控制器，则需要分按照序号发送 

            list.Add(0x00);//电脑主控序号 // PC master index


            //==========  Start data section   ============================================
            //==========  开始数据   ============================================
            // Data type (data frame: 0x88, 0x77; start frame: 0x33, 0x44; end frame: 0x55, 0x66)
            //发送的数据类型，数据帧0x88,0x77,开始头帧0x33,0x44,结束帧0x55,0x66   
            list.Add(0x088);
            list.Add(0x077);//电脑主控序号 // PC master index

            list.Add(0xFF);//通知控制前   每个端口控制多少灯的标志 //A sign notifying how many lights each port controls before control
            list.Add(0xF0);

            // Data length = 8 ports * number of lights per port (2 bytes)
            //数据长度  这个数据长度 = 8路端口* 每个端口控制灯的数量(占用2个字节)
            TempLen = m_nControlPort * 2;
            UInt16 Len2 = Convert.ToUInt16(TempLen);
            list.Add(Convert.ToByte(Len2 >> 8));
            list.Add(Convert.ToByte(Len2 & 0xFF));

            // Here are 8 controllers, each one controlling the same number of lights, the number of controls on each one is expressed in 2 bytes
            //这里是8路控制器，每一路控制一样多的灯就行    每一路的控制数量用2个字节表示
            for (byte i = 0; i < m_nControlPort; i++)
            {
                list.Add(Convert.ToByte(ControlLightCount >> 8));
                list.Add(Convert.ToByte(ControlLightCount & 0xFF));
            }


            // Add total data length
            //数据总长度
            UInt16 dataLen = Convert.ToUInt16(list.Count);
            list.Add(Convert.ToByte(dataLen >> 8));
            list.Add(Convert.ToByte(dataLen & 0xFF));


            byte Crc = Get_crc(list.ToArray(), list.Count);
            if (m_s_nLightType == 0)
            {
                Crc = frzm_pass_word[Crc];
            }
            else if (m_s_nLightType == 1)
            {
                Crc = gyhd_pass_word[Crc];
            }
            list.Add(Crc);


            return list.ToArray();
        }


        public static byte[]  SenddData(byte G, byte R , byte B, byte ControlCount, UInt16 ControlLightCount)
        {
            List<byte> list = new List<byte>();

            list.Add(m_bSendCmdHead);//命令类型 // Command type
            list.Add(0x33);//2个字节  的自定义命令标志 // Custom command flag (2 bytes)
            list.Add(0x33);


            // Command length = controller index (1 byte) + PC master index (1 byte) + flag for number of lights per port (2 bytes) + pixel control data length (2 bytes) + number of ports * number of lights * 3
            //命令长度 =  控制器序号1位 + 电脑主控序号1位 +  每个端口控制多少灯的标志2位 + 像素控制数据长度2位 +  N路端口*M个灯*3
            int TempLen = 1 + 1 + 2 + 2 + 8 * Convert.ToInt32(ControlLightCount) * 3;
            UInt16 Len1 = Convert.ToUInt16(TempLen);
            list.Add(Convert.ToByte(Len1 >> 8));
            list.Add(Convert.ToByte(Len1 & 0xFF));

            list.Add(0x02);//PC/控制机 // PC/Controller

            //Controller serial number. If there are multiple controllers, send them according to the serial number
            //控制器的序号，如果有多个控制器，则需要分按照序号发送
            list.Add(ControlCount);
            list.Add(0x00);//电脑主控序号 // PC master index

            // ==========  Start data section  ============================================
            //==========  开始数据   ============================================
            // Data type (data frame: 0x88, 0x77; start frame: 0x33, 0x44; end frame: 0x55, 0x66)
            //发送的数据类型，数据帧0x88,0x77,开始头帧0x33,0x44,结束帧0x55,0x66  
            list.Add(0x88); 
            list.Add(0x77);//电脑主控序号 // PC master index

            list.Add(0x00);//通知控制前   每个端口控制多少灯的标志 //A sign notifying how many lights each port controls before control
            list.Add(0x00);

            // Data length This data length = 8 ports * Number of control lights per port * 3
            //数据长度  这个数据长度 = 8路端口* 每个端口控制灯的数量* 3
            TempLen = Convert.ToInt32(ControlLightCount) * 24;
            UInt16 Len2 = Convert.ToUInt16(TempLen);
            list.Add(Convert.ToByte(Len2 >> 8));
            list.Add(Convert.ToByte(Len2 & 0xFF));


            byte[] arrRgb = new byte[24];
            //灯光的颜色控制
            // Add RGB color data for each light
            for (int i = 0; i < ControlLightCount; i++)//灯的数量 // Number of lights
            {
                for (int k = 0; k < 8; k++)//8路端口 // 8 ports
                {
                    arrRgb[k] =R;
                    arrRgb[8 + k] = G;
                    arrRgb[16 + k] = B;
                }
                list.AddRange(arrRgb);
            }



            //数据总长度  // Add total data length
            UInt16 dataLen = Convert.ToUInt16(list.Count);
            list.Add(Convert.ToByte(dataLen >> 8));
            list.Add(Convert.ToByte(dataLen & 0xFF));


            byte Crc = Get_crc(list.ToArray(), list.Count);
            if (m_s_nLightType == 0)
            {
                Crc = frzm_pass_word[Crc];
            }
            else if (m_s_nLightType == 1)
            {
                Crc = gyhd_pass_word[Crc];
            }
            list.Add(Crc);

            return list.ToArray();
        }

        //一帧数据的结束包数据 // End packet data of a frame
        public static byte[] GetSendEnd(byte ControlCount)
        {
            List<byte> list = new List<byte>();

            list.Add(m_bSendCmdHead);//命令类型//Command type
            list.Add(0x44);//2个字节  的自定义命令标志 // A 2-byte custom command flag
            list.Add(0x44);

            list.Add(0x00);//2个字节   命令长度   数据位的数据长度  //2 bytes Command Length Indicates the data length of the data bit
            list.Add(0x08);

            list.Add(0x02);//PC/控制机//PC/ Controller

            list.Add(ControlCount);//控制器的序号，如果有多个控制器，则需要分按照序号发送//Controller serial number. If there are multiple controllers, send them according to the serial number
            list.Add(0x00);//电脑主控序号// PC master index


            //==========  Start data section   ============================================
            //==========  开始数据   ============================================
            // Data type (data frame: 0x88, 0x77; start frame: 0x33, 0x44; end frame: 0x55, 0x66)
            //发送的数据类型，数据帧0x88,0x77,开始头帧0x33,0x44,结束帧0x55,0x66   
            list.Add(0x55);
            list.Add(0x66);
            //The symbol of how many lights each port controls before the notification control, the sending header here does not need to be 0
            //通知控制前   每个端口控制多少灯的标志，发送头这里不用就是0
            list.Add(0x00);
            list.Add(0x00);

            list.Add(0x00);//灯像素控制的数据长度//The light pixel controls the data length
            list.Add(0x00);//

            //数据总长度  // Add total data length
            UInt16 dataLen = Convert.ToUInt16(list.Count);
            list.Add(Convert.ToByte(dataLen >> 8));
            list.Add(Convert.ToByte(dataLen & 0xFF));


            byte Crc = Get_crc(list.ToArray(), list.Count);
            if (m_s_nLightType == 0)
            {
                Crc = frzm_pass_word[Crc];
            }
            else if (m_s_nLightType == 1)
            {
                Crc = gyhd_pass_word[Crc];
            }
            list.Add(Crc);


            return list.ToArray();
        }
    }
}
