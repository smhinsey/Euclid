using System;

namespace Euclid.Framework.Cqrs
{
    public class CommandPublicationRecord : ICommandPublicationRecord
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid Identifier { get; set; }
        public string CallStack { get; set; }
        public bool Completed { get; set; }
        public bool Dispatched { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public Uri MessageLocation { get; set; }
        public Type MessageType { get; set; }
    }
}