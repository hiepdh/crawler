using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Crawler2
{
    public class crawler
    {
        public Site siteInfo;
        public CrawlerEntities db;

        HtmlWeb web = new HtmlWeb();
        HtmlDocument docPage = null;
        HtmlDocument docPageDetail = null;

        public crawler(Site siteInfo, CrawlerEntities crawlerEntities)
        {
            this.siteInfo = siteInfo;
            this.db = crawlerEntities;
        }

        public void start()
        {
            for (int i = 0; i < siteInfo.MaxPage; i++)
            {
                try
                {
                    docPage = web.Load(String.Format(siteInfo.PageUrl, i + 1));
                    HtmlNodeCollection nodes = docPage.DocumentNode.SelectNodes(siteInfo.PageRootXpath);
                    if (nodes != null)
                    {

                        List<Site_Attribute> lstAttr = db.Site_Attribute.Where(pt => pt.SiteGroupId == siteInfo.GroupId).ToList();
                        List<Post_Attribue> lstPostAttrs = new List<Post_Attribue>();
                        Post_Attribue postAttr = null;
                        int postIdx = 0;
                        foreach (HtmlNode node in nodes)
                        {
                            //Load detail post
                            HtmlNode detailNode = node.SelectSingleNode(siteInfo.PageDetailXpath);
                            if (detailNode == null)
                                continue;
                            //
                            postIdx++;

                            string postUrl = detailNode.Attributes["href"].Value;
                            HtmlDocument detailDoc = web.Load(postUrl);

                            foreach (Site_Attribute attr in lstAttr)
                            {
                                postAttr = new Post_Attribue();
                                postAttr.PostId = postIdx;
                                postAttr.PostUrl = postUrl;

                                GetSiteAttribute(detailDoc, postAttr, attr);
                                //lstPostAttrs.Add(postAttr);
                                db.AddToPost_Attribue(postAttr);
                            }
                            //db.SaveChanges();
                        }

                        //Save changes sau mỗi page
                        db.SaveChanges();
                    }
                    //
                    Console.WriteLine(String.Format("{0} - Page {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), i + 1, siteInfo.PageUrl));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void GetSiteAttribute(HtmlDocument doc, Post_Attribue postAttr, Site_Attribute attr)
        {
            if (doc == null)
                return;
            //
            postAttr.SiteGroupId = Convert.ToInt32(siteInfo.GroupId);
            postAttr.SiteId = siteInfo.Id;
            postAttr.AttribueKey = attr.AttributeKey;
            postAttr.AttribueValue = "UNKNOWN";

            try
            {
                var nodeVal = doc.DocumentNode.SelectSingleNode(attr.XPath);
                if (nodeVal != null)
                    postAttr.AttribueValue = RemoveSpecialCharacter(nodeVal.InnerHtml);
            }
            catch (Exception)
            {

            }
        }

        private string RemoveSpecialCharacter(string input)
        {
            input = input.Trim();
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            return rRemScript.Replace(input, "");
        }
    }
}
