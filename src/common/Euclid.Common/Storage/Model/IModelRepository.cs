namespace Euclid.Common.Storage.Model
{
	// SELF i think this should be combined with ISimpleRepository

	/// <summary>
	/// 	Marker interface
	/// </summary>
	/// <typeparam name = "TModel">Type of the model to be managed by the repository.</typeparam>
	public interface IModelRepository<TModel>
		where TModel : class, IModel
	{
	}
}