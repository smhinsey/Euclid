using System.Collections.Generic;

namespace Euclid.Common.Configuration
{
	/// <summary>
	/// 	An overridable setting which contains a list of items. This can be used when an app requires an
	/// 	arbitrarily long list of settings, such as a list of message processor types.
	/// </summary>
	/// <typeparam name = "TSettingType">The type contained by the list.</typeparam>
	public interface IOverridableSettingList<TSettingType> : IOverridableSetting<IList<TSettingType>>
	{
		/// <summary>
		/// 	Adds a new item to the setting list.
		/// </summary>
		/// <param name = "newListItem">The new item.</param>
		void Add(TSettingType newListItem);
	}
}