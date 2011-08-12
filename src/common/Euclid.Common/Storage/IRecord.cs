﻿using System;
using Euclid.Common.Messaging;

namespace Euclid.Common.Storage
{
	/// <summary>
	/// 	A small piece of information which needs to be persisted to an arbitrary medium.
	/// </summary>
	// SELF at some point these should be split apart. the notion of a record has no conceptual connection to a message at this level
	// although a message can contain a record and a record can persist a message, it should be clear that the former is about 
	// transport and the latter persistence
	public interface IRecord : IMessage
	{
		DateTime Modified { get; }
	}
}