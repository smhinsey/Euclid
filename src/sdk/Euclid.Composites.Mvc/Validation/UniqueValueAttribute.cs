using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Euclid.Composites.Mvc.Validation
{
	/// <summary>
	/// ensures that the value of the decorated property is unique, by executing the specified query with the value of the property as the sole argument
	/// if the query returns null, the value is assumed to be unique
	/// </summary>
	public class UniqueValueAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly string _queryName;
		private readonly string _methodName;
		private readonly string _argumentName;
		private readonly ICompositeApp _compositeApp;
		
		public UniqueValueAttribute(string queryName, string methodName)
		{
			_compositeApp = DependencyResolver.Current.GetService<ICompositeApp>();
			_queryName = queryName;
			_methodName = methodName;

			var query = _compositeApp
				.Queries
				.Where(q => q.Name.Equals(_queryName, StringComparison.InvariantCultureIgnoreCase))
				.FirstOrDefault();
			if (query == null)
			{
				throw new QueryNotPresentInCompositeException(queryName);
			}

			var method =
				query.Type.GetMethods().Where(m => m.Name.Equals(_methodName, StringComparison.InvariantCulture) && m.GetParameters().Count() == 1).FirstOrDefault();
			if (method == null)
			{
				throw new MissingMethodException(methodName);
			}

			_argumentName = method.GetParameters()[0].Name;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var results = _compositeApp.ExecuteQuery(_queryName, _methodName, 1, paramName => value.ToString());

			return (results == null) 
				? ValidationResult.Success
				: new ValidationResult(ErrorMessage, new [] {validationContext.DisplayName});
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var rule= new ModelClientValidationRule
			{
				ErrorMessage = ErrorMessage,
				ValidationType = "uniquevalue"
			};

			rule.ValidationParameters.Add("query", _queryName);
			rule.ValidationParameters.Add("method", _methodName);
			rule.ValidationParameters.Add("argument", _argumentName);

			yield return rule;
		}
	}
}