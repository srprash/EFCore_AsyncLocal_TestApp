using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    public class AsyncLocalContext
    {
        public static AsyncLocal<string> _asyncLocalString = new AsyncLocal<string>();
    }
}
