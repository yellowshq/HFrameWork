using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameWork.Core
{
    public struct Message
    {
        private int ID;
        private int DataLen;
        private byte[] Data;

        public Message(int id,byte[] data)
        {
            ID = id;
            DataLen = data.Length;
            Data = data;
        }

        public int GetDataLen()
        {
            return DataLen;
        }

        public int GetMsgID()
        {
            return ID;
        }

        public byte[] GetData()
        {
            return Data;
        }

        public void SetDataLen(int len)
        {
            DataLen = len;
        }

        public void SetMsgID(int id)
        {
            ID = id;
        }

        public void SetData(byte[] data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return "ID:" + ID + " Data:" + System.Text.Encoding.UTF8.GetString(Data);
        }
    }
}

