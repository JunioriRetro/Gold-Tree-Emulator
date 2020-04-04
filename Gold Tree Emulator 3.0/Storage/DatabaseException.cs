using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldTree.Storage
{
	class DatabaseException : Exception
	{
        public DatabaseException(string sMessage) : base(sMessage) { }
	}
}
