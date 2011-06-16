﻿using System;
using Euclid.Common.Transport;

namespace Euclid.Common.Storage
{
	public interface IBasicRecordRepository<TRecord>
	{
		TRecord Create(Uri messageLocation, Type messageType);
		TRecord Delete(Guid id);
		TRecord Retrieve(Guid id);
		TRecord Update(TRecord record);
	}
}