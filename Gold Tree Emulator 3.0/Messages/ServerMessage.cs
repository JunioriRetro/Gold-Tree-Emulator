using System;
using System.Collections.Generic;
using System.Text;
using GoldTree.Util;
using System.IO;
namespace GoldTree.Messages
{
	public sealed class ServerMessage
	{
		private uint MessageId;
		private List<byte> Body;
		public uint Id
		{
			get
			{
				return this.MessageId;
			}
		}
		public string Header
		{
			get
			{
				return GoldTree.GetDefaultEncoding().GetString(Base64Encoding.Encodeuint(this.MessageId, 2));
			}
		}
		public int Length
		{
			get
			{
				return this.Body.Count;
			}
		}
		public ServerMessage()
		{
		}
		public ServerMessage(uint _MessageId)
		{
			this.Init(_MessageId);
		}
		public override string ToString()
		{
			return this.Header + GoldTree.GetDefaultEncoding().GetString(this.Body.ToArray());
		}
		public string ToBodyString()
		{
			return GoldTree.GetDefaultEncoding().GetString(this.Body.ToArray());
		}
		public void Clear()
		{
			this.Body.Clear();
		}
		public void Init(uint _MessageId)
		{
			this.MessageId = _MessageId;
			this.Body = new List<byte>();
		}
		public void AppendByte(byte b)
		{
			this.Body.Add(b);
		}
		public void AppendBytes(byte[] Data)
		{
			if (Data != null && Data.Length != 0)
			{
				this.Body.AddRange(Data);
			}
		}
		public void AppendString(string s, Encoding Encoding)
		{
			if (s != null && s.Length != 0)
			{
				this.AppendBytes(Encoding.GetBytes(s));
			}
		}
		public void AppendString(string s)
		{
			this.AppendString(s, GoldTree.GetDefaultEncoding());
		}
		public void AppendStringWithBreak(string s)
		{
			this.AppendStringWithBreak(s, 2);
		}
		public void AppendStringWithBreak(string s, byte BreakChar)
		{
			this.AppendString(s);
			this.AppendByte(BreakChar);
		}
		public void AppendInt32(int i)
		{
			this.AppendBytes(WireEncoding.EncodeInt32(i));
		}
		public void AppendRawInt32(int i)
		{
			this.AppendString(i.ToString(), Encoding.ASCII);
		}
		public void AppendUInt(uint i)
		{
			this.AppendInt32((int)i);
		}
		public void AppendRawUInt(uint i)
		{
			this.AppendRawInt32((int)i);
		}
		public void AppendBoolean(bool Bool)
		{
			if (Bool)
			{
				this.Body.Add(73);
			}
			else
			{
				this.Body.Add(72);
			}
		}
        public byte[] GetBytes()
        {
            byte[] Data = new byte[this.Length + 3];
            byte[] Header = Base64Encoding.Encodeuint(this.MessageId, 2);
            Data[0] = Header[0];
            Data[1] = Header[1];
            for (int i = 0; i < this.Length; i++)
            {
                Data[i + 2] = this.Body[i];
            }
            Data[Data.Length - 1] = 1;
            return Data;
        }
	}
}
