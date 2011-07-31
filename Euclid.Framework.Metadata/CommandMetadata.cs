//using System;
//using Euclid.Framework.Cqrs;

//namespace Euclid.Framework.Metadata
//{
//    public class CommandMetadata : ICommandMetadata
//    {
//        public Type CommandType { get; private set; }

//        public CommandMetadata(Type commandType)
//        {
//            CommandType = commandType;
//        }

//        public ICommand Create()
//        {
//            return Activator.CreateInstance(CommandType) as ICommand;
//        }
//    }
//}