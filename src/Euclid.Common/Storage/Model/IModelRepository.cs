namespace Euclid.Common.Storage.Model
{
	/// <summary>
	/// 	Marker interface
	/// </summary>
	public interface IModelRepository<TModel>
		where TModel : class, IModel
	{
	}
}