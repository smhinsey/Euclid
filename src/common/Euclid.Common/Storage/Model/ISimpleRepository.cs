using System;
using System.Collections.Generic;

namespace Euclid.Common.Storage.Model
{
	/// <summary>
	/// 	ISimpleRepository provides basic repository services for the supplied model type.
	/// </summary>
	/// <typeparam name = "TModel">The model type managed by this repository.</typeparam>
	public interface ISimpleRepository<TModel> : IModelRepository<TModel>
		where TModel : class, IModel
	{
		/// <summary>
		/// 	Deletes the supplied model instance.
		/// </summary>
		/// <param name = "model">The model to delete.</param>
		void Delete(TModel model);

		/// <summary>
		/// 	Deletes the model identified by the supplied identifier.
		/// </summary>
		/// <param name = "identifier">The identifier of the model to be deleted.</param>
		void Delete(Guid identifier);

		/// <summary>
		/// 	Finds models by their creation date.
		/// </summary>
		/// <param name = "specificDate">Find models created on this date.</param>
		/// <returns>Models created on specificDate.</returns>
		IList<TModel> FindByCreationDate(DateTime specificDate);

		/// <summary>
		/// 	Finds models created within a range of dates.
		/// </summary>
		/// <param name = "begin">The begining of the creation date range.</param>
		/// <param name = "end">The end of the creation date range.</param>
		/// <returns>Models created between begin and end.</returns>
		IList<TModel> FindByCreationDate(DateTime begin, DateTime end);

		/// <summary>
		/// 	Find a model by its identifier.
		/// </summary>
		/// <param name = "identifier">The model's identifier.</param>
		/// <returns>The model matching identifier.</returns>
		TModel FindById(Guid identifier);

		/// <summary>
		/// 	Finds models by their modification date.
		/// </summary>
		/// <param name = "specificDate">Find models modified on this date.</param>
		/// <returns>Models modified on specificDate.</returns>
		IList<TModel> FindByModificationDate(DateTime specificDate);

		/// <summary>
		/// 	Finds models modified within a range of dates.
		/// </summary>
		/// <param name = "begin">The begining of the modification date range.</param>
		/// <param name = "end">The end of the modification date range.</param>
		/// <returns>Models modified between begin and end.</returns>
		IList<TModel> FindByModificationDate(DateTime begin, DateTime end);

		/// <summary>
		/// 	Saves the supplied model instance.
		/// </summary>
		/// <param name = "model">The model to be saved.</param>
		/// <returns>The saved model.</returns>
		TModel Save(TModel model);

		/// <summary>
		/// 	Updates the supplied model instance.
		/// </summary>
		/// <param name = "model">The model to be updated.</param>
		/// <returns>The updated model.</returns>
		TModel Update(TModel model);
	}
}