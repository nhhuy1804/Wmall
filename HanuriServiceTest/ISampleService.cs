using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

using System.Text;
using System.ServiceModel.Web;
using System.Xml.Serialization;


namespace HanuriServiceTest
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
          ResponseFormat = WebMessageFormat.Xml)]
        CompositeType GetCompositeType();
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations
    [DataContract]
    [XmlRoot(ElementName = "CompositeType")]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember, XmlAttribute(Namespace = "")]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember, XmlAttribute(Namespace = "")]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
