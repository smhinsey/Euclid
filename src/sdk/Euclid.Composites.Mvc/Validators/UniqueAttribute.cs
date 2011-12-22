using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using Euclid.Framework.Cqrs;

namespace Euclid.Composites.Mvc.Validators
{
	public class UniqueAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly IQuery _query;
		private readonly MethodInfo _queryMethod;
		public UniqueAttribute(Type queryType, string nameOfMethodThatReturnsIList)
		{
			if (typeof(IQuery).IsAssignableFrom(queryType))
			{
				throw new InvalidTypeException(string.Format("The specified type '{0}' does not implement IQuery", queryType.Name));
			}

			_query = DependencyResolver.Current.GetService(queryType) as IQuery;
			if (_query == null)
			{
				throw new TypeNotRegisteredException(
					string.Format("The type '{0}' is not registered, and cannot be used to validate the uniqueness of a property", queryType.Name));
			}

			_queryMethod = queryType.GetMethod(nameOfMethodThatReturnsIList);
			if (_queryMethod == null)
			{
				throw new InvalidMethodException(string.Format("The method '{0}.{1}' could not be found", queryType, nameOfMethodThatReturnsIList));
			}

			if (!typeof(IList).IsAssignableFrom(_queryMethod.ReturnType))
			{
				throw new InvalidMethodException(
					string.Format(
						"Really?! The method '{0}' assigned to nameOfMethodThatReturnsIList does not return an IEnumerable",
						nameOfMethodThatReturnsIList));
			}
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var methodParams = _queryMethod.GetParameters();
			if (methodParams.Length != 1 && methodParams[0].ParameterType != validationContext.ObjectType)
			{
				throw new InvalidMethodException(string.Format("The method '{0}' needs to accept a single parameter of type {1} in order to validate {2} for uniqeness", _queryMethod.Name, validationContext.ObjectType, validationContext.DisplayName));
			}

			var results = _queryMethod.Invoke(_query, new [] {value}) as IList;

			if (results == null)
			{
				return new ValidationResult("Unable to retrieve query results");
			}

			return results.Count <= 0
			       	? ValidationResult.Success
			       	: new ValidationResult(ErrorMessage);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var clientValidationRule = new ModelClientValidationRule()
			{
				ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
				ValidationType = "isUnique"
			};

			clientValidationRule.ValidationParameters.Add("queryType", _query.GetType().Name);
			clientValidationRule.ValidationParameters.Add("queryName", _queryMethod.Name);

			return new[] { clientValidationRule };
		}
	}

	public class InvalidMethodException : Exception
	{
		public InvalidMethodException(string message) : base(message)
		{
		}
	}

	public class TypeNotRegisteredException : Exception
	{
		public TypeNotRegisteredException(string message) : base(message)
		{
		}
	}

	public class InvalidTypeException : Exception
	{
		public InvalidTypeException(string message) : base(message)
		{
		}
	}
}