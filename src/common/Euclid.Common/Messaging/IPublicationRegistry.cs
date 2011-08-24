using System;
using System.Collections.Generic;

namespace Euclid.Common.Messaging
{
	// SELF I'm not sure what to do about TPublicationRecord. 
	// It's not used for anything, but it allows an implementation to specify a concrete type 
	// for TRecordContract which can be used to create new instances. Since the declaration is 
	// otherwise unused, I'm not sure about this approach, but for now, it solves a problem that 
	// resulted in an ugly implementation for CommandRegistry

	/// <summary>
	/// 	A publication registry is used by an IPublisher to manage publication of a message.
	/// </summary>
	/// <typeparam name = "TPublicationRecord">The concrete type of the publication record used to track 
	/// 	the state of the message after it has been published.</typeparam>
	/// <typeparam name = "TPublicationRecordContract">An interface which constrains 
	/// 	TPublicationRecord</typeparam>
	public interface IPublicationRegistry<out TPublicationRecord, out TPublicationRecordContract>
		where TPublicationRecord : class, TPublicationRecordContract, IPublicationRecord
		where TPublicationRecordContract : IPublicationRecord
	{
		// SELF what's up with this method signature? It seems like it should probably take Guid identifier
		// It seems like a given IPublicationRegistry implementation is likely to want to hide the specific
		// methods it uses for interacting with its record storage mechanism. Based on this method's usages, 
		// such a refactoring should be straightforward.
		IMessage GetMessage(Uri messageLocation, Type messageType);

		/// <summary>
		/// 	Returns a current copy of a publication record. This is useful to determine if a parallel actor has already processed the message
		/// 	in the case of processing backlogs, or for any other scenario where a caller needs the most up to date record possible.
		/// </summary>
		/// <param name = "identifier">The identifier of the record to be retrieved.</param>
		/// <returns>The current version of the record specified by identifier.</returns>
		TPublicationRecordContract GetPublicationRecord(Guid identifier);

		/// <summary>
		/// 	Indicates that processing has completed without error and that there is no further work 
		/// 	to be done in relation to the message associated with the specified record.
		/// </summary>
		/// <param name = "identifier">The identifier of the record to be marked as complete.</param>
		/// <returns>The updated record.</returns>
		TPublicationRecordContract MarkAsComplete(Guid identifier);

		/// <summary>
		/// 	Indicates that the processing of a message encountered an error of some kind. Optionally, details of the error
		/// 	may be added to the record.
		/// </summary>
		/// <param name = "identifier">The identifier of the record to be marked as failed.</param>
		/// <param name = "message">A message associated with the error.</param>
		/// <param name = "callStack">The callstack of the error.</param>
		/// <returns>The updated record.</returns>
		TPublicationRecordContract MarkAsFailed(Guid identifier, string message = null, string callStack = null);

		// SELF we probably need to differentiate between "no dispatcher available" and "dispatcher blew up" below

		/// <summary>
		/// 	Indicates that a message could not be dispatched. This may indicate a failure in the dispatcher or that
		/// 	there was simply no dispatcher available to handle the message.
		/// </summary>
		/// <param name = "identifier">The identifier of the record whose associated message could not be dispatched.</param>
		/// <param name = "isError">Indicates whether the message was unable to dispatch due to an error occurring.</param>
		/// <param name = "message">If an error ocurred, its message.</param>
		/// <returns>The updated record.</returns>
		TPublicationRecordContract MarkAsUnableToDispatch(Guid identifier, bool isError = false, string message = null);

		/// <summary>
		/// 	Called by a publisher. Creates a TPublicationRecord containing all relevant info from the 
		/// 	published message, persists it, and returns it.
		/// </summary>
		/// <param name = "message">The message to be published.</param>
		/// <returns>Once an instance of TPublicationRecordContract is returned, the message is guaranteed 
		/// 	to be processed, although  depending on the configuration of the system, that processing may 
		/// 	not necessarily be timely, if some other part of the publication
		/// 	pipeline fails.</returns>
		TPublicationRecordContract PublishMessage(IMessage message);

	    IList<IPublicationRecord> GetRecords(int pageSize, int offSet);
	}
}