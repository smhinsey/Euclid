namespace Euclid.Sdk.TestComposite.Converters
{
	//public class FailingInputModelToCommandConverter : IInputToCommandConverter
	//{
	//    public Type CommandType
	//    {
	//        get
	//        {
	//            return typeof(FailingCommand);
	//        }
	//    }

	//    public Type InputModelType
	//    {
	//        get
	//        {
	//            return typeof(FailingInputModel);
	//        }
	//    }

	//    public ICommand Convert(ResolutionContext context)
	//    {
	//        var source = context.SourceValue as FailingInputModel;
	//        var command = context.DestinationValue as FailingCommand;

	//        if (source == null || command == null)
	//        {
	//            throw new CannotCreateInputModelException(InputModelType.Name);
	//        }

	//        return command;
	//    }
	//}
}