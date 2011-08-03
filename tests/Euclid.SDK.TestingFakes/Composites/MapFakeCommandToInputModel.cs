using System;
using System.Linq;
using System.Web;
using Euclid.Composites.Maps;
using Euclid.Composites.Models;
using Euclid.Framework.Metadata.Extensions;

namespace Euclid.SDK.TestingFakes.Composites
{
    public class MapFakeCommandToInputModel : IMapper<FakeCommand, IInputModel>
    {
        public Type Source
        {
            get { return typeof (FakeCommand); }
        }

        public Type Destination
        {
            get { return typeof(IInputModel); }
        }

        public IInputModel Map(FakeCommand commandMetadata)
        {
            var inputModel = Activator.CreateInstance(typeof(InputModel)) as InputModel;

            if (inputModel == null)
            {
                throw new CannotCreateInputModelException(Destination);
            }

            inputModel.CommandType = typeof (FakeCommand);
            inputModel.AgentSystemName = Destination.Assembly.GetAgentInfo().SystemName;
            inputModel.SubmittedByUser = HttpContext.Current.User.Identity.Name;
            Destination.GetEuclidMetadata().Properties.ToList().ForEach(inputModel.Properties.Add);

            return inputModel;
        }

        public object Map(object source)
        {
            var fakeCommand = source as FakeCommand;

            if (fakeCommand == null)
            {
                throw new ArgumentException("MapFakCommandToInputModel.Map requires the source parameter to be a FakeCommand", "source");
            }

            return Map(fakeCommand);
        }
    }
}