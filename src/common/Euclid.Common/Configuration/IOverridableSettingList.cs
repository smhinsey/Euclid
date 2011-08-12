using System.Collections.Generic;

namespace Euclid.Common.Configuration
{
	public interface IOverridableSettingList<TSettingType> : IOverridableSetting<IList<TSettingType>>
	{
		void Add(TSettingType newListItem);
	}
}