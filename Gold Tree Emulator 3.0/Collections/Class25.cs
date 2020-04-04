using System;
using System.Collections;
namespace GoldTree.Collections
{
	internal sealed class Class25 : Hashtable, IDisposable
	{
		private bool bool_0;
		internal Class26 Class26_0
		{
			get
			{
				return new Class26(base.Clone() as Hashtable);
			}
		}
		public Class25()
		{
			this.bool_0 = false;
		}
		internal void method_0()
		{
			this.Dispose();
		}
		public void Dispose()
		{
			this.method_1(true);
			GC.SuppressFinalize(this);
		}
		private void method_1(bool bool_1)
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
