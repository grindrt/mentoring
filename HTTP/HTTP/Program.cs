using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HTTP.Enums;

namespace HTTP
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://beanbaggy.by/";
            Task t = new Task(() => Do(url, 2, DomainLimitation.Without, new List<FileExtension> { FileExtension.JPG }));
            t.Start();

            Console.ReadKey();
        }

        public static async void Do(string url, int deep, DomainLimitation domainLimitation, List<FileExtension> extentions)
        {
            var markups = new List<Markup>();

            var mainPage = await GetHtml(url);
            markups.Add(new Markup { Url = new Uri(url), Html = mainPage });
            if (deep > 0)
            {
                var markupsByLink = new List<Markup>();
                var level = 0;
				while (level < deep)
                {
                    if (markupsByLink.Any())
                    {
                        var parentCount = markupsByLink.Count;
                        for (int index = 0; index < parentCount; index++)
                        {
                            try
                            {
                                var item = markupsByLink[index];
								var pages = await GetChildPages(item.Html, markups.Select(x => x.Url));
                                if (pages.Any())
                                {
                                    markupsByLink.AddRange(pages.Distinct());
                                }
                            }
                            catch (Exception e)
                            {
                              //  Console.WriteLine(e);
                            }
                        }
                        markupsByLink.RemoveRange(0, parentCount);
						markups.AddRange(markupsByLink);
                    }
                    else
                    {
                        markupsByLink.AddRange(await GetChildPages(mainPage, markups.Select(x => x.Url)));
						markups.AddRange(markupsByLink);
                    }
                    level++;
                }

            }

            foreach (var markup in markups)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(markup.Html);
                Console.WriteLine(markup.Url);

                using (var client = new WebClient())
                {
                    var path = markup.Url.AbsolutePath.Replace(markup.Url.Segments.Last(), string.Empty).Replace('/', '\\');

                    var filename = markup.Url.Segments.Last().Equals("/")
                        ? "index"
                        : markup.Url.Segments.Last().Remove('/');

                    var split = filename.Split('.');
                    var extension = split.Length > 1 ? split.Last() : "html";

                    client.DownloadFile(markup.Url.AbsoluteUri,
                        string.Format(@"E:\test\{0}{1}.{2}", path, filename, extension));

                    if (extentions.Any())
                    {
                        foreach (var image in Extras(markup.Url, doc, extentions))
                        {
                            if (!Directory.Exists(@"E:\test"))
                            {
                                Directory.CreateDirectory(@"E:\test");
                            }
                            client.DownloadFile(image.Url, string.Format(@"E:\test\{0}", image.Url.AbsolutePath));
                        }
                    }
                }
            }
        }

        private static async Task<List<Markup>> GetChildPages(string sourcePage, IEnumerable<Uri> storedUrls)
        {
			var pages = new List<Markup>();
			var links = GetLinks(sourcePage, storedUrls);
            foreach (var link in links)
            {
                try
                {
                    pages.Add(new Markup { Url = new Uri(link), Html = await GetHtml(link) });
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            return pages.Distinct().ToList();
        }

        private static List<string> GetLinks(string mainPage, IEnumerable<Uri> storedUrls)
        {
            var linksArray = new List<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(mainPage);
			var nodes = doc.DocumentNode.SelectNodes("//a");

			var domain = storedUrls.FirstOrDefault().AbsoluteUri;
			domain = !string.IsNullOrEmpty(domain) && domain.EndsWith("/") ? domain.Remove(domain.Length - 1) : domain;

            foreach (var node in nodes
                .Select(node => node.Attributes["href"].Value)
                .Where(link => !string.IsNullOrEmpty(link) && !linksArray.Contains(link))
                .Where(link => !storedUrls.Any(x => x.ToString().Equals(link) || x.ToString().EndsWith(link)))
                .Where(link => link.Length > 1))
            {
//#if DEBUG
                if (node.Contains("vk") || node.Contains("twitter")) continue;
//#endif

                if (node.StartsWith("mail")) continue;
				var link = node.StartsWith("/") && !string.IsNullOrEmpty(domain) ? string.Format("{0}{1}", domain, node) : node;
                if (!linksArray.Contains(link))
                {
                    linksArray.Add(link);
                }
            }
            return linksArray;
        }

	    private static IEnumerable<Extra> Extras(Uri url, HtmlDocument doc, List<FileExtension> extensions)
	    {
		    var htmlNodes = doc.DocumentNode.Descendants("img");
	        foreach (
	            var img in
	                htmlNodes.Where(
	                    x =>
	                        x.Attributes["src"] != null &&
	                        extensions.Any(e => IsApprovedExtension(x.Attributes["src"].Value, e))))
	        {
	            var source = img.Attributes["src"] != null ? img.Attributes["src"].Value : string.Empty;
	            if (source.StartsWith("/") || !source.StartsWith("http")) source = string.Format("{0}{1}", url, source);
	            yield return new Extra {Url = new Uri(source)};
	        }
	    }

	    private static bool IsApprovedExtension(string source, FileExtension approvedExtension)
	    {
		    var sourceExtension = Path.GetExtension(source);
		    if (sourceExtension == null) return false;
		    var upper = sourceExtension.Remove(0, sourceExtension.Length > 0 ? 1 : 0).ToUpper();
		    return approvedExtension.ToString().ToUpper().Equals(upper);
	    }

	    private static async Task<string> GetHtml(string url)
        {
            Console.WriteLine(url);
            if (!url.StartsWith("http")) url = string.Format("http://{0}", url);
            string result;
            using (var client = new HttpClient())
            {
                using (var responseMessage = await client.GetAsync(url))
                {
                    using (var content = responseMessage.Content)
                    {
                        result = await content.ReadAsStringAsync();
                    }
                }
            }
            return result;
        }
    }

	public class Markup
	{
		public Uri Url { get; set; }
		public string Html { get; set; }
	}

    public class Extra
    {
        public Uri Url;
    }
}
