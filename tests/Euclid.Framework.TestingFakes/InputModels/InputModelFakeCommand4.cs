using System;
using System.Collections.Specialized;
using Euclid.Framework.Models;

namespace Euclid.Framework.TestingFakes.InputModels
{
    public class InputModelFakeCommand4 : IInputModel
    {
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public string AgentSystemName { get; set; }
        public Type CommandType { get; set; }
        public string CommandName { get; set; }
    }
}