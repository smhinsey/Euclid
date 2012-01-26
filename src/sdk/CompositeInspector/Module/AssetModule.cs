using Nancy;

namespace CompositeInspector.Module
{
	public class AssetModule : NancyModule
	{
		private const string AssetRootNamespace = "CompositeInspector.Assets";

		private const string BaseRoute = "composite";

		private const string CssRoute = "/css/{file}";

		private const string ImageRoute = "/image/{file}";

		private const string JsRoute = "/js/{file}";

		// TODO: we should probably do better than this

		public AssetModule()
			: base(BaseRoute)
		{
			Get[JsRoute] = p =>
				{
					var convertedFilePath = ((string)p.file).Replace("/", ".");

					var resourcePath = string.Format("{0}.Scripts.{1}", AssetRootNamespace, convertedFilePath);

					var resourceStream = GetType().Assembly.GetManifestResourceStream(resourcePath);

					return Response.FromStream(resourceStream, "text/javascript");
				};

			Get[CssRoute] = p =>
				{
					var convertedFilePath = ((string)p.file).Replace("/", ".");

					var resourcePath = string.Format("{0}.Styles.{1}", AssetRootNamespace, convertedFilePath);

					var resourceStream = GetType().Assembly.GetManifestResourceStream(resourcePath);

					return Response.FromStream(resourceStream, "text/css");
				};

			Get[ImageRoute] = p =>
				{
					var convertedFilePath = ((string)p.file).Replace("/", ".");

					var resourcePath = string.Format("{0}.Images.{1}", AssetRootNamespace, convertedFilePath);

					var fileExtension = resourcePath.Substring(resourcePath.Length - 3, 3);

					var contentType = string.Format("image/{0}", fileExtension);

					var resourceStream = GetType().Assembly.GetManifestResourceStream(resourcePath);

					return Response.FromStream(resourceStream, contentType);
				};
		}
	}
}