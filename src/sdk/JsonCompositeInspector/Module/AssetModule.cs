using Nancy;

namespace JsonCompositeInspector.Module
{
	public class AssetModule : NancyModule
	{
		public AssetModule()
		{
			Get["/js/{file}"] = p =>
			{
				var resx = string.Format("JsonCompositeInspector.Assets.Scripts.{0}", ((string)p.file).Replace("/", "."));
				var s = GetType().Assembly.GetManifestResourceStream(resx);
				return Response.FromStream(s, "text/javascript");
			};

			Get["/css/{file}"] = p =>
			{
				var resx = string.Format("JsonCompositeInspector.Assets.Styles.{0}", ((string)p.file).Replace("/", "."));
				var s = GetType().Assembly.GetManifestResourceStream(resx);
				return Response.FromStream(s, "text/css");
			};

			Get["/image/{file}"] = p =>
			{
				var resx = string.Format("JsonCompositeInspector.Assets.Images.{0}", ((string)p.file).Replace("/", "."));
				var extension = resx.Substring(resx.Length - 3, 3);
				var contentType = string.Format("image/{0}", extension);
				var s = GetType().Assembly.GetManifestResourceStream(resx);
				return Response.FromStream(s, contentType);
			};
		}
	}
}