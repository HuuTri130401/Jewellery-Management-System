using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THT.JMS.Utilities
{
    public class JMSException : Exception
    {
        public JMSException() : base() { }
        public JMSException(string message) : base(message)
        {
        }
        public JMSException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
