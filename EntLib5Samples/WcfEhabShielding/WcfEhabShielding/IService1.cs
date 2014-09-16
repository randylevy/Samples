using System;
using System.ServiceModel;

namespace WcfEhabShielding
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [FaultContract(typeof(MyFaultContract))]
        string GetData(int value);
    }
}
