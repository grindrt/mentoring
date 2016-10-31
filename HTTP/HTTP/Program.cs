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
	        markups.Add(new Markup {Url = url, Html = mainPage});
            if (deep > 0)
            {
                var markupsByLink = new List<Markup>();
                var level = 0;
				while (level < deep)
                {
                    if (markupsByLink.Any())
                    {
                        //var buf = new List<Markup>();
                        var parentCount = markupsByLink.Count;
                        for (int index = 0; index < parentCount; index++)
                        {
                            try
                            {
                                var item = markupsByLink[index];
								var pages = await GetChildPages(item.Html, markups.Select(x => x.Url));
                                if (pages.Any())
                                {
                                    //buf.AddRange(pages);
                                    markupsByLink.AddRange(pages.Distinct());
                                }
                            }
                            catch (Exception e)
                            {
                              //  Console.WriteLine(e);
                            }
                        }
                        markupsByLink.RemoveRange(0, parentCount);
                        //markupsByLink.Clear();
                        //markupsByLink.AddRange(buf);
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

                if (extentions.Any())
                {
					foreach (var image in Extras(url, doc, extentions))
                    {
                        Console.WriteLine(image.Url.Segments.Last() + " " + image.Title);
                        using (var wc = new WebClient())
                        {
                            //if (!Directory.Exists(@"E:\test"))
                            //{
                            //    Directory.CreateDirectory(@"E:\test");
                            //}
                            //wc.DownloadFile(url, @"E:\test\test.html");
                            //wc.DownloadFile(image.Url, String.Format(@"E:\test\{0}", image.Url.AbsolutePath));
                        }
                    }
                }
            }
        }

        private static async Task<List<Markup>> GetChildPages(string sourcePage, IEnumerable<string> storedUrls)
        {
			var pages = new List<Markup>();
			var links = GetLinks(sourcePage, storedUrls);
            foreach (var link in links)
            {
                try
                {
                    pages.Add(new Markup { Url = link, Html = await GetHtml(link) });
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            return pages.Distinct().ToList();
        }

        private static List<string> GetLinks(string mainPage, IEnumerable<string> storedUrls)
        {
            var linksArray = new List<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(mainPage);
            var nodes = doc.DocumentNode.SelectNodes("//a");
            foreach (var node in nodes
                .Select(node => node.Attributes["href"].Value)
                .Where(link => !string.IsNullOrEmpty(link) && !linksArray.Contains(link))
                .Where(link => !storedUrls.Any(x => x.Equals(link)))
                .Where(link => link.Length > 1))
            {
//#if DEBUG
                if (node.Contains("vk") || node.Contains("twitter")) continue;
//#endif

                if (node.StartsWith("mail")) continue;
                var domain = storedUrls.FirstOrDefault();
                domain = domain != null && domain.EndsWith("/") ? domain.Remove(domain.Length - 1) : domain;
                var link = node.StartsWith("/") && domain != null ? string.Format("{0}{1}", domain, node) : node;
                if (!linksArray.Contains(link))
                {
                    linksArray.Add(link);
                }
            }
            return linksArray;
        }

	    private static IEnumerable<Img> Extras(string url, HtmlDocument doc, List<FileExtension> extensions)
	    {
		    var htmlNodes = doc.DocumentNode.Descendants("img");
	        foreach (
	            var img in
	                htmlNodes.Where(
	                    x =>
	                        x.Attributes["src"] != null &&
	                        extensions.Any(e => IsApprovedExtension(x.Attributes["src"].Value, e))))
	        {
	            var title = img.ParentNode.Attributes["title"];
	            var source = img.Attributes["src"] != null ? img.Attributes["src"].Value : string.Empty;
	            if (source.StartsWith("/") || !source.StartsWith("http")) source = string.Format("{0}{1}", url, source);
	            yield return new Img {Url = new Uri(source), Title = (title != null ? title.Value : "")};
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
		public string Url { get; set; }
		public string Html { get; set; }
	}

	public enum DomainLimitation
    {
        Without,
        CurrentDomain,
        SourceURL
    }

    public enum FileExtension
    {
        GIF,
        JPEG,
        JPG,
        PDF,
	    PNG
    }

    public class Get
    {
        public async Task Do()
        {
            HttpClient client = new HttpClient();
            string responseBodyAsText = await client.GetStringAsync("www.contoso.com");
            var s = responseBodyAsText;
        }
    }

    public class Img
    {
        public string Title;
        public Uri Url;
    }
}
