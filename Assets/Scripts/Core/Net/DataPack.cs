using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HFrameWork.Core
{
    public class DataPack
    {
        public static byte[] Pack(Message message)
        {
            byte[] lenBytes = BitConverter.GetBytes(message.GetDataLen());
            int lenL = lenBytes.Length;

            byte[] idBytes = BitConverter.GetBytes(message.GetMsgID());
            int idL = idBytes.Length;

            byte[] dataBytes = message.GetData();
            int dataL = message.GetDataLen();

            byte[] dataBuff = new byte[lenL+idL+dataL];
            Array.Copy(lenBytes, 0, dataBuff, 0, lenL);
            Array.Copy(idBytes, 0, dataBuff, lenL, idL);
            Array.Copy(dataBytes, 0, dataBuff, lenL + idL, dataL);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataBuff);
            }

            return dataBuff;
        }

        public static int GetPackDataLen(byte[] data)
        {
            int lenInt32 = sizeof(UInt32);
            byte[] lenBytes = new byte[lenInt32];
            Array.Copy(data, lenBytes, lenInt32);
            int len = BitConverter.ToInt32(lenBytes, 0);
            return len;
        }
        public static Message Unpack(byte[] data)
        {

            int lenInt32 = sizeof(UInt32);
            //byte[] lenBytes = new byte[lenInt32];
            //Array.Copy(data, lenBytes, lenInt32);
            int len = GetPackDataLen(data);

            byte[] idBytes = new byte[lenInt32];
            Array.Copy(data, lenInt32, idBytes,0, lenInt32);
            int id = BitConverter.ToInt32(idBytes, 0);

            int dataL = len;
            byte[] dataBytes = new byte[dataL];
            Array.Copy(data, lenInt32+lenInt32, dataBytes, 0, dataL);

            Message message = new Message(id,dataBytes);
            return message;
        }
    }
}

