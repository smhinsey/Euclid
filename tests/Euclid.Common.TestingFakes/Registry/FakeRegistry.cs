using System.Collections.Generic;
using System.Linq;
using System.Text;
using Euclid.Common.Registry;

namespace Euclid.Common.TestingFakes.Registry
{
    public class FakeRegistry : InMemoryRegistry<FakeRecord, FakeMessage>
    {
    }
}
