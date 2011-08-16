using System;
using AutoMapper;
using Euclid.Composites.Conversion;
using Euclid.Framework.Agent.Extensions;
using Euclid.Framework.Cqrs;
using Euclid.Framework.TestingFakes.Cqrs;
using Euclid.Framework.TestingFakes.InputModels;
using NUnit.Framework;

namespace Euclid.Composite.InputModelMapping
{
	[TestFixture]
	public class InputModelMappingTests
	{
		private InputModelFakeCommand4 GetInputModel()
		{
			return new InputModelFakeCommand4
			       	{
			       		AgentSystemName = typeof (FakeCommand4).Assembly.GetAgentMetadata().SystemName,
			       		BirthDay = DateTime.Now,
			       		CommandType = typeof (FakeCommand4),
			       		Password = DateTime.Now.Ticks.ToString()
			       	};
		}

		public class InputToFakeCommand4Converter : IInputToCommandConverter
		{
			public Type CommandType
			{
				get { return typeof (FakeCommand4); }
			}

			public Type InputModelType
			{
				get { return typeof (InputModelFakeCommand4); }
			}

			public ICommand Convert(ResolutionContext context)
			{
				var source = context.SourceValue as InputModelFakeCommand4;
				if (source == null)
				{
					throw new CannotCreateInputModelException(typeof (FakeCommand4).GetMetadata().Name);
				}

				var command = Activator.CreateInstance<FakeCommand4>();

				command.YourBirthday = source.BirthDay;

				command.PasswordHash = string.Format("hashed: {0}", source.Password);

				command.PasswordSalt = string.Format("salted: {0}", source.Password);

				return command;
			}
		}

		[Test]
		public void TestConverterConfig()
		{
			Mapper.CreateMap(typeof (InputModelFakeCommand4), typeof (FakeCommand4)).ConvertUsing(typeof (InputToFakeCommand4Converter));
			Mapper.AssertConfigurationIsValid();

			var model = GetInputModel();
			var command = Activator.CreateInstance(typeof (FakeCommand4));
			var dest = Mapper.Map(model, command, model.GetType(), command.GetType()) as FakeCommand4;

			Assert.NotNull(dest);
			Assert.AreEqual(dest.GetType(), typeof (FakeCommand4));
			Assert.AreEqual(model.BirthDay, dest.YourBirthday);
			Assert.AreEqual("hashed: " + model.Password, dest.PasswordHash);
			Assert.AreEqual("salted: " + model.Password, dest.PasswordSalt);
		}

		[Test]
		public void TestGetCommand()
		{
			var started = DateTime.Now;
			IInputModelTransfomerRegistry r = new InputModelToCommandTransformerRegistry();

			var metadata = typeof (FakeCommand4).GetMetadata();

			r.Add(metadata.Name, new InputToFakeCommand4Converter());

			var m = r.GetInputModel(metadata.Name);

			Assert.NotNull(m);

			Assert.AreEqual(typeof (InputModelFakeCommand4), m.GetType());

			var instanceOfModel = GetInputModel();

			var instanceOfCommand = r.GetCommand(instanceOfModel) as FakeCommand4;

			Assert.NotNull(instanceOfCommand);

			Assert.AreEqual("hashed: " + instanceOfModel.Password, instanceOfCommand.PasswordHash);
			Assert.AreEqual("salted: " + instanceOfModel.Password, instanceOfCommand.PasswordSalt);
			Assert.AreEqual(instanceOfCommand.YourBirthday, instanceOfModel.BirthDay);
		}

		[Test]
		public void TestGetInputModel()
		{
			IInputModelTransfomerRegistry r = new InputModelToCommandTransformerRegistry();

			var metadata = typeof (FakeCommand4).GetMetadata();

			r.Add(metadata.Name, new InputToFakeCommand4Converter());

			var m = r.GetInputModel(metadata.Name);
			Assert.NotNull(m);

			Assert.AreEqual(typeof (InputModelFakeCommand4), m.GetType());
		}
	}
}