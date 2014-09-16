using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace WcfEhabShielding
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    [ExceptionShielding("Policy")]
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            throw new Exception("OOPS!");

            return string.Format("You entered: {0}", value);
        }
    }
}
