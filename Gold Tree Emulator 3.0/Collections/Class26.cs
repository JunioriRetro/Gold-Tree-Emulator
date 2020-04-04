using System;
using System.Collections;
namespace GoldTree.Collections
{
	internal sealed class Class26 : Hashtable, IDisposable
	{
		private bool bool_0;
		public Class26(Hashtable hashtable_0) : base(hashtable_0)
		{
			this.bool_0 = false;
		}
		public void Dispose()
		{
			this.method_0(true);
			GC.SuppressFinalize(this);
		}
		private void method_0(bool bool_1)
		{
			if (!this.bool_0)
			{
				this.bool_0 = true;
				if (bool_1)
				{
					base.Clear();
				}
			}
		}
	}
}
