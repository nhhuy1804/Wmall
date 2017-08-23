using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HanuriServiceTest
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class SampleService : ISampleService
    {
        public CompositeType GetCompositeType()
        {
            return new CompositeType();
        }
    }
}
