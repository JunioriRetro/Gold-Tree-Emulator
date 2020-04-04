using System;
using System.Text;
using GoldTree.Util;
namespace GoldTree.Messages
{
	internal sealed class ClientMessage
	{
		private uint MessageId;
		private byte[] Body;
		private int Pointer;

		public uint Id
		{
			get
			{
				return this.MessageId;
			}
		}
		public int Length
		{
			get
			{
				return this.Body.Length;
			}
		}
		public int RemainingLength
		{
			get
			{
				return this.Body.Length - this.Pointer;
			}
		}
		public string Header
		{
			get
			{
				return Encoding.Default.GetString(Base64Encoding.Encodeuint(this.MessageId, 2));
			}
		}
		public ClientMessage(uint _MessageId, byte[] _Body)
		{
			if (_Body == null)
			{
				_Body = new byte[0];
			}
			this.MessageId = _MessageId;
			this.Body = _Body;
			this.Pointer = 0;
		}
		public override string ToString()
		{
			return this.Header + GoldTree.GetDefaultEncoding().GetString(this.Body);
		}
		public void ResetPointer()
		{
			this.Pointer = 0;
		}
		public void AdvancePointer(int i)
		{
			this.Pointer += i;
		}
		public string GetBody()
		{
			return Encoding.Default.GetString(this.Body);
		}
		public byte[] ReadBytes(int Bytes)
		{
			if (Bytes > this.RemainingLength)
			{
				Bytes = this.RemainingLength;
			}
			byte[] data = new byte[Bytes];
			for (int i = 0; i < Bytes; i++)
			{
				data[i] = this.Body[this.Pointer++];
			}
			return data;
		}
		public byte[] PlainReadBytes(int Bytes)
		{
			if (Bytes > this.RemainingLength)
			{
				Bytes = this.RemainingLength;
			}
			byte[] data = new byte[Bytes];
			int x = 0;
			int y = this.Pointer;
			while (x < Bytes)
			{
				data[x] = this.Body[y];
				x++;
				y++;
			}
			return data;
		}
		public byte[] ReadFixedValue()
		{
			int len = Base64Encoding.DecodeInt32(this.ReadBytes(2));
			return this.ReadBytes(len);
		}
		public bool PopBase64Boolean()
		{
			return this.RemainingLength > 0 && this.Body[this.Pointer++] == 65;
		}
		public int PopInt32()
		{
			return Base64Encoding.DecodeInt32(this.ReadBytes(2));
		}
		public uint PopUInt32()
		{
			return (uint)this.PopInt32();
		}
		public string PopFixedString()
		{
			return this.PopFixedString(Encoding.Default);
		}
		public string PopFixedString(Encoding Encoding)
		{
			return Encoding.GetString(this.ReadFixedValue()).Replace(Convert.ToChar(1), ' ');
		}
		public int PopFixedInt32()
		{
			int i = 0;
			string s = this.PopFixedString(Encoding.ASCII);
			int.TryParse(s, out i);
			return i;
		}
		public uint PopFixedUInt32()
		{
			return (uint)this.PopFixedInt32();
		}
		public bool PopWiredBoolean()
		{
			return this.RemainingLength > 0 && this.Body[this.Pointer++] == 73;
		}
		public int PopWiredInt32()
		{
			int i;
			if (this.RemainingLength < 1)
			{
				i = 0;
			}
			else
			{
				byte[] Data = this.PlainReadBytes(6);
				int TotalBytes = 0;
				int num2 = WireEncoding.DecodeInt32(Data, out TotalBytes);
				this.Pointer += TotalBytes;
				i = num2;
			}
			return i;
		}
		public uint PopWiredUInt()
		{
			return (uint)this.PopWiredInt32();
		}
	}
}
