using System;
using System.Web.Mvc;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Models
{
	public abstract class InputModelBase : IInputModel
	{
		[HiddenInput(DisplayValue = false)]
		public string AgentSystemName { get; set; }

		public Type CommandType { get; set; }

		[HiddenInput(DisplayValue = false)]
		public string PartName
		{
			get
			{
				return CommandType != null ? CommandType.Name : string.Empty;
			}
		}
	}
}