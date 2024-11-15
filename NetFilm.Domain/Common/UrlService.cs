namespace NetFilm.Domain.Common
{
	public static class UrlService
	{
		private static readonly string DISTRIBUTION_DOMAIN_VIDEO = "https://dqg1h1bamqrgk.cloudfront.net";
		private static readonly string DISTRIBUTION_DOMAIN_IMAGE = "https://d2iq2ti9djrztg.cloudfront.net";
		private static readonly string DISTRIBUTION_DOMAIN_SUBTITLE = "https://d1psb6ixmwpbjl.cloudfront.net";

		public static string CreateUrl(this string key)
		{
			List<string> imageExtensions = new List<string>
			{
				".jpg",
				".jpeg",
				".png",
				".gif",
				".bmp",
				".tiff",
				".tif",
				".webp",
				".svg",
				".ico"
			};
			List<string> videoExtensions = new List<string>
			{
				".mp4",
				".webm",
				".ogg",
				".mov",
				".avi",
				".flv",
				".3gp",
				".mkv"
			};
			var extension = Path.GetExtension(key);
			var DistributionDomain = "";

			if (imageExtensions.Contains(extension))
			{
				DistributionDomain = DISTRIBUTION_DOMAIN_IMAGE;
			}
			if (videoExtensions.Contains(extension))
			{
				DistributionDomain = DISTRIBUTION_DOMAIN_VIDEO;
			}
			if (extension == ".srt")
			{
				DistributionDomain = DISTRIBUTION_DOMAIN_SUBTITLE;
			}
			string url = $"{DistributionDomain}/{key}";
			return url;
		}
	}
}
