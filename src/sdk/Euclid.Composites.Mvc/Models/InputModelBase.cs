using System;
using System.ComponentModel;
using System.Web.Mvc;
using Euclid.Framework.Models;

namespace Euclid.Composites.Mvc.Models
{
	public abstract class InputModelBase : IInputModel
	{
		[HiddenInput(DisplayValue = false)]
		public string AgentSystemName { get; set; }

		[HiddenInput(DisplayValue = false)]
		public string CommandName
		{
			get { return CommandType != null ? CommandType.Name : string.Empty; }
		}

		public Type CommandType { get; set; }
	}
}