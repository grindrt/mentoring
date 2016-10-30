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
            var url = "www.google.com";
            Task t = new Task(() => Do(url, 2, DomainLimitation.Without, new List<FileExtention> { FileExtention.JPEG }));
            t.Start();

            Console.ReadKey();
        }

        public static async void Do(string url, int deep, DomainLimitation domainLimitation, List<FileExtention> extentions)
        {
            if (!url.StartsWith("http")) url = string.Format("https://{0}", url);

            var markups = new List<string>();

            var mainPage = await GetHtml(url);
            markups.Add(mainPage);
            if (deep > 0)
            {
                var markupsByLink = new List<string>();
                var level = 0;
                do
                {
                    if (markupsByLink.Any())
                    {
                        markups.AddRange(markupsByLink);

                        var buf = new List<string>();
                        for (int index = 0; index < markupsByLink.Count; index++)
                        {
                            try
                            {
                                var item = markupsByLink[index];
                                var pages = await GetChildPages(item);
                                if (pages.Any())
                                {
                                    buf.AddRange(pages);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        markupsByLink.Clear();
                        markupsByLink.AddRange(buf);
                    }
                    else
                    {
                        markupsByLink.AddRange(await GetChildPages(mainPage));
                    }
                    level++;
                } while (level < deep);

            }

            foreach (var markup in markups)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(markup);

                if (extentions.Any())
                {
                    foreach (var image in Extras(url, doc))
                    {
                        Console.WriteLine(image.Url.Segments.Last() + " " + image.Title);
                        using (var wc = new WebClient())
                        {
                            //if (!Directory.Exists(@"E:\test"))
                            //{
                            //    Directory.CreateDirectory(@"E:\test");
                            //}
                            //wc.DownloadFile(requestUri, @"E:\test\test.html");
                            //wc.DownloadFile(image.Url, String.Format(@"E:\test\{0}", image.Url.AbsolutePath));
                        }
                    }
                }
            }

        }

        private static async Task<List<string>> GetChildPages(string sourcePage)
        {
            var pages = new List<string>();
            var links = GetLinks(sourcePage);
            foreach (var link in links)
            {
                try
                {
                    var item = await GetHtml(link);
                    pages.Add(item);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            return pages;
        }

        private static List<string> GetLinks(string mainPage)
        {
            var linksArray = new List<string>();


            var doc = new HtmlDocument();
            doc.LoadHtml(mainPage);
            var nodes = doc.DocumentNode.SelectNodes("//a");
            foreach (var link in nodes
                .Select(node => node.Attributes["href"].Value)
                .Where(link => !string.IsNullOrEmpty(link) && !linksArray.Contains(link)))
            {
                linksArray.Add(link);
            }
            return linksArray;
        }

        private static IEnumerable<Img> Extras(string url, HtmlDocument doc)
        {
          //  var reu = new List<Img>();
            var htmlNodes = doc.DocumentNode.Descendants("img");
            foreach (var img in htmlNodes)
            {
                var t = img.ParentNode.Attributes["title"];
                var s = img.Attributes["src"].Value;
                if (s.StartsWith("/") || !s.StartsWith("http")) s = string.Format("{0}{1}", url, s);
                    yield return new Img { Url = new Uri(s), Title = (t != null ? t.Value : "") };
               // reu.Add(new Img { Url = new Uri(s), Title = (t != null ? t.Value : "") });
            }
           // return reu;
        }

        private static async Task<string> GetHtml(string requestUri)
        {
            string result;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.GetAsync(requestUri))
                {
                    using (HttpContent content = responseMessage.Content)
                    {
                        result = await content.ReadAsStringAsync();
                    }
                }
            }
            return result;
        }
    }

    public enum DomainLimitation
    {
        Without,
        CurrentDomain,
        SourceURL
    }

    public enum FileExtention
    {
        GIF,
        JPEG,
        JPG,
        PDF
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
