using System;
using System.ComponentModel;
using Euclid.Framework.Cqrs;

namespace Euclid.SDK.TestingFakes.Composites
{
	public class FakeCommand : ICommand, IFakeMarker
	{
		[Fake(Description = "Euclid.SDK.TestingFakes.FakeAttribute")]
		[Description("System.DescriptionAttribute")]
		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }
		public Guid Identifier { get; set; }
		public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
	}
}