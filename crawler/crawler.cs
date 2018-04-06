using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using Newtonsoft.Json.Linq;
using ReverseMarkdown;
using System.Web;

namespace crawler
{
    public class crawler
    {
        public pageInfo pageInfo;
        HtmlWeb web = new HtmlWeb();
        HtmlDocument docPage = null;
        HtmlDocument docPageDetail = null;

        public crawler(pageInfo pageInfo)
        {
            this.pageInfo = pageInfo;
        }

        public void start()
        {
            pageInfo.MaxPage = GetMaxPage(pageInfo.FirstPageUrl);
            for (int i = 0; i < pageInfo.MaxPage; i++)
            {
                try
                {
                    if (i == 0)
                        docPage = web.Load(pageInfo.FirstPageUrl);
                    else
                        docPage = web.Load(String.Format(pageInfo.PageURL, i + 1));

                    //Loop each page
                    pageInfo.PostList = new List<postInfo>();
                    //Load post content
                    HtmlNodeCollection nodes = docPage.DocumentNode.SelectNodes("//div[@id='colbds']//div[@class='row']");
                    postInfo post = null;
                    foreach (HtmlNode node in nodes)
                    {
                        post = new postInfo();
                        if (node.SelectNodes(".//h3") != null)
                        {
                            post.Title = node.SelectNodes(".//h3").First().InnerText.Trim();
                            post.Url = node.SelectNodes(".//h3/a").First().Attributes["href"].Value;
                            //Load detail
                            docPageDetail = web.Load(post.Url);
                            if (docPageDetail.DocumentNode.SelectNodes(".//div[@id='colbds']//p") != null)
                                post.Content = docPageDetail.DocumentNode.SelectNodes(".//div[@id='colbds']//p").First().InnerText;
                            if (docPageDetail.DocumentNode.SelectNodes(".//*[@id='colbds']//div[2]//div[2]//span") != null)
                                post.Price = docPageDetail.DocumentNode.SelectNodes("//*[@id='colbds']//div[2]//div[2]//span").First().InnerText;
                            if (docPageDetail.DocumentNode.SelectNodes(".//*[@id='colbds']//p[3]") != null)
                                post.Phone = docPageDetail.DocumentNode.SelectNodes(".//*[@id='colbds']//p[3]").First().InnerText;
                            if (post.Phone != null && post.Phone.Contains(":"))
                            {
                                post.Phone = post.Phone.Split(new string[] { ":" }, StringSplitOptions.None)[1];
                            }
                            post.Province = docPageDetail.DocumentNode.SelectNodes(".//*[@id='colbds']//ol//li[3]//a").First().InnerText;
                            post.District = docPageDetail.DocumentNode.SelectNodes(".//*[@id='colbds']//ol//li[4]//a").First().InnerText;
                            if (!String.IsNullOrEmpty(post.Phone))
                                post.Title = String.Format("{0} - LH: {1}", post.Title, post.Phone);
                            //
                            pageInfo.PostList.Add(post);
                        }
                    }

                    Console.WriteLine(String.Format("{0} - Page {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), i + 1, pageInfo.PageURL));
                    Post2Site(pageInfo.PostList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        //kế toán thiên ưng
        public void start_kttu()
        {

            pageInfo.MaxPage = GetMaxPage(pageInfo.FirstPageUrl);
            for (int i = 0; i < pageInfo.MaxPage; i++)
            {
                try
                {
                    if (i == 0)
                        docPage = web.Load(pageInfo.FirstPageUrl);
                    else
                        docPage = web.Load(String.Format(pageInfo.PageURL, i + 1));

                    string queryString = new System.Uri(pageInfo.FirstPageUrl).Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

                    string categoryId = "14";//Thu nộp BHXH,YT,TN

                    //Loop each page
                    pageInfo.PostList = new List<postInfo>();
                    //Load post content

                    //*[@id="nc"]/div[2]/a[1]

                    HtmlNodeCollection nodes = docPage.DocumentNode.SelectNodes("//*[@id='nc']//a[@class='name']");
                    postInfo post = null;
                    if (nodes != null)
                        foreach (HtmlNode node in nodes)
                        {
                            post = new postInfo();
                            if (node.Attributes["href"].Value != null)
                            {
                                post.Title = node.InnerText.Trim();
                                post.Url = node.Attributes["href"].Value;
                                post.SiteId = "http://ketoanthienung.org";
                                post.CategoryId = categoryId;
                                //Load detail
                                docPageDetail = web.Load(post.Url);

                                //
                                //docPageDetail.DocumentNode.Descendants()
                                //  .Where(n => n.Name == "script" || n.Name == "style")
                                //  .ToList()
                                //  .ForEach(n => n.Remove());

                                //
                                //queryString = new System.Uri(post.Url).Query;
                                //queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                                string postId = "0";
                                //
                                post.PostId = postId;
                                post.Content = "";
                                if (docPageDetail.DocumentNode.SelectNodes("//div[@id='nd']//div[@class='details']") != null)
                                    post.Content = docPageDetail.DocumentNode.SelectNodes("//div[@id='nd']//div[@class='details']").First().InnerText;

                                post.ContentReply = "";

                                //
                                if (post.Content != "")
                                    pageInfo.PostList.Add(post);
                            }
                        }

                    Console.WriteLine(String.Format("{0} - Page {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), i + 1, pageInfo.PageURL));

                    //Post2Site(pageInfo.PostList);
                    if (pageInfo.PostList.Count > 0)
                        Post2Site_Alo(pageInfo.PostList, categoryId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        //bhxhhn.com.vn
        public void start_bhhn()
        {
            pageInfo.MaxPage = GetMaxPage(pageInfo.FirstPageUrl);
            for (int i = 0; i < pageInfo.MaxPage; i++)
            {
                try
                {
                    if (i == 0)
                        docPage = web.Load(pageInfo.FirstPageUrl);
                    else
                        docPage = web.Load(String.Format(pageInfo.PageURL, i + 1));

                    //docPage.DocumentNode.Descendants()
                    //    .Where(n => n.Name == "script" || n.Name == "style")
                    //    .ToList()
                    //    .ForEach(n => n.Remove());

                    //pkcm=2
                    string queryString = new System.Uri(pageInfo.FirstPageUrl).Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

                    string categoryId = queryDictionary["pkcm"].ToString();

                    switch (categoryId)
                    {
                        //<option value="1">Bảo hiểm y tế</option>
                        //<option value="2">Bảo hiểm xã hội</option>
                        //<option value="3">Bảo hiểm thất nghiệp</option>
                        //<option value="4">Hỏi đáp các vấn đề khác</option>
                        //<option selected="selected" value="5">Hưu trí</option>
                        //<option value="6">Tử tuất</option>
                        //<option value="7">Ốm đau thai sản</option>
                        //<option value="8">Tai nạn lao động, bệnh nghề nghiệp</option>
                        //<option value="9">Sổ BHXH</option>
                        //<option value="10">Thẻ BHYT</option>
                        //<option value="11">KCB BHYT</option>
                        //<option value="12">BHXH 1 lần</option>

                        //Y tế
                        case "1":
                            categoryId = "12";
                            break;
                        case "2":
                            categoryId = "19";
                            break;
                        case "3":
                            categoryId = "16";
                            break;
                        case "4":
                            categoryId = "6";
                            break;
                        case "5":
                            categoryId = "10";
                            break;
                        case "6":
                            categoryId = "10";
                            break;
                        case "7":
                            categoryId = "8";
                            break;
                        case "8":
                            categoryId = "9";
                            break;
                        case "9":
                            categoryId = "15";
                            break;
                        case "10":
                            categoryId = "15";
                            break;
                        case "11":
                            categoryId = "15";
                            break;
                        case "12":
                            categoryId = "14";
                            break;
                    }


                    //Loop each page
                    pageInfo.PostList = new List<postInfo>();
                    //Load post content
                    HtmlNodeCollection nodes = docPage.DocumentNode.SelectNodes("//table[@id='dnn_ctr1675_FE_View_All_view_gvHoiDap']//tr");
                    postInfo post = null;
                    foreach (HtmlNode node in nodes)
                    {
                        post = new postInfo();
                        if (node.SelectNodes(".//td[2]//a") != null)
                        {
                            post.Title = node.SelectNodes(".//td[2]//a").First().InnerText.Trim();
                            post.Url = node.SelectNodes(".//td[2]//a").First().Attributes["href"].Value;
                            post.SiteId = "bhxhhn.com.vn";
                            post.CategoryId = categoryId;
                            //Load detail
                            docPageDetail = web.Load(post.Url);

                            //
                            //docPageDetail.DocumentNode.Descendants()
                            //  .Where(n => n.Name == "script" || n.Name == "style")
                            //  .ToList()
                            //  .ForEach(n => n.Remove());

                            //
                            queryString = new System.Uri(post.Url).Query;
                            queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                            string postId = queryDictionary["hdId"].ToString();
                            //
                            post.PostId = postId;

                            if (docPageDetail.DocumentNode.SelectNodes("//div[@id='dnn_ContentPane']//p[@class='noidungcauhoi']") != null)
                                post.Content = docPageDetail.DocumentNode.SelectNodes("//div[@id='dnn_ContentPane']//p[@class='noidungcauhoi']").First().InnerText;

                            //if (docPageDetail.DocumentNode.SelectNodes("//*[@id='dnn_ctr1726_ModuleContent']/div/div/div[2]/div[3]") != null)
                            //    post.Content = docPageDetail.DocumentNode.SelectNodes("//*[@id='dnn_ctr1726_ModuleContent']/div/div/div[2]/div[3]").First().InnerText;
                            post.ContentReply = docPageDetail.DocumentNode.SelectNodes("//div[@id='dnn_ContentPane']//div[@class='contenttraloi']").First().InnerHtml;

                            //
                            pageInfo.PostList.Add(post);
                        }
                    }

                    Console.WriteLine(String.Format("{0} - Page {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), i + 1, pageInfo.PageURL));
                    //Post2Site(pageInfo.PostList);
                    Post2Site_Alo(pageInfo.PostList, categoryId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        internal void start_bhxhgovvn()
        {
            //pageInfo.MaxPage = 111;
            pageInfo.MaxPage = GetMaxPage(pageInfo.FirstPageUrl);
            for (int i = 0; i < pageInfo.MaxPage; i++)
            {
                try
                {
                    string webUrl = "";
                    if (i == 0)
                    {
                        webUrl = pageInfo.FirstPageUrl;
                        docPage = web.Load(webUrl);
                    }
                    else
                    {
                        webUrl = String.Format(pageInfo.PageURL, i + 1);
                        docPage = web.Load(webUrl);
                    }

                    string queryString = new System.Uri(pageInfo.FirstPageUrl).Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

                    string categoryId = "7";//Thu nộp BHXH,YT,TN
                    

                    //Loop each page
                    pageInfo.PostList = new List<postInfo>();
                    //Load post content

                    HtmlNodeCollection nodes = docPage.DocumentNode.SelectNodes("//div[contains(@class, 'list-tin') and not(contains(@class, 'tintuc-mobile')) ]");
                    postInfo post = null;
                    if (nodes != null)
                        foreach (HtmlNode node in nodes)
                        {
                            post = new postInfo();
                            if (node.SelectSingleNode(".//a").Attributes["href"] != null)
                            {
                                post.Title = node.SelectSingleNode(".//p[contains(@class, 'tieude')]").InnerText.Trim();
                                post.Url = webUrl + node.SelectSingleNode(".//a").Attributes["href"].Value;
                                post.SiteId = "baohiemxahoi.gov.vn";
                                post.CategoryId = categoryId;
                                //Load detail
                                docPageDetail = web.Load(post.Url);

                                string postId = "0";

                                queryString = new System.Uri(post.Url).Query;
                                queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                                postId = queryDictionary["ItemID"].ToString();

                                //
                                post.PostId = postId;
                                post.Content = "";

                                //if (docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]/text()") != null)
                                //    post.Content = docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]/text()").First().InnerText;

                                //post.ContentReply = docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]/span[1]").First().InnerText;

                                post.Content = docPageDetail.DocumentNode.SelectNodes(".//div[contains(@class, 'tinchitiet')]/div[2]/div[2]").First().InnerHtml;

                                //
                                if (post.Content != "")
                                    pageInfo.PostList.Add(post);
                            }
                        }

                    Console.WriteLine(String.Format("{0} - Page {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), i + 1, pageInfo.PageURL));

                    //Post2Site(pageInfo.PostList);
                    if (pageInfo.PostList.Count > 0)
                        Post2Site_Alo(pageInfo.PostList, categoryId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        internal void start_bhdn()
        {
            //pageInfo.MaxPage = 111;
            pageInfo.MaxPage = GetMaxPage(pageInfo.FirstPageUrl);
            for (int i = 0; i < pageInfo.MaxPage; i++)
            {
                try
                {
                    if (i == 0)
                        docPage = web.Load(pageInfo.FirstPageUrl);
                    else
                        docPage = web.Load(String.Format(pageInfo.PageURL, i + 1));

                    string queryString = new System.Uri(pageInfo.FirstPageUrl).Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

                    string categoryId = "13";//Thu nộp BHXH,YT,TN

                    //Loop each page
                    pageInfo.PostList = new List<postInfo>();
                    //Load post content

                    //*[@id="nc"]/div[2]/a[1]

                    HtmlNodeCollection nodes = docPage.DocumentNode.SelectNodes("//*[@id='SearchResults']//tr[contains(@class, 'sectiontableentry1') or contains(@class, 'sectiontableentry1')]");
                    postInfo post = null;
                    if (nodes != null)
                        foreach (HtmlNode node in nodes)
                        {
                            post = new postInfo();
                            if (node.SelectSingleNode(".//a").Attributes["href"] != null)
                            {
                                post.Title = node.SelectSingleNode(".//a//b").InnerText.Trim();
                                post.Url = "http://www.bhxhdanang.gov.vn/" + node.SelectSingleNode(".//a").Attributes["href"].Value;
                                post.SiteId = "bhxhdanang.gov.vn";
                                post.CategoryId = categoryId;
                                //Load detail
                                docPageDetail = web.Load(post.Url);

                                string postId = "0";
                                //
                                post.PostId = postId;
                                post.Content = "";

                                //if (docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]/text()") != null)
                                //    post.Content = docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]/text()").First().InnerText;

                                //post.ContentReply = docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]/span[1]").First().InnerText;

                                post.Content = docPageDetail.DocumentNode.SelectNodes("//*[@id=\"body\"]/div[1]/div[2]").First().InnerHtml;

                                //
                                if (post.Content != "")
                                    pageInfo.PostList.Add(post);
                            }
                        }

                    Console.WriteLine(String.Format("{0} - Page {1} - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), i + 1, pageInfo.PageURL));

                    //Post2Site(pageInfo.PostList);
                    if (pageInfo.PostList.Count > 0)
                        Post2Site_Alo(pageInfo.PostList, categoryId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private int GetMaxPage(string firstPageUrl)
        {
            int maxPage = 5;
            //
            return maxPage;
        }

        public void Post2Site_Alo(List<postInfo> posts, string categoryId)
        {
            //return;

            SQLiteHelper sh = new SQLiteHelper();
            sh = sh.Init();
            sh.BeginTransaction();

            //Luu DB local - SQLLITE, de check trung
            //string root = "http://localhost:4567/api/v2";

            string root = "https://alobhxh.com/api/v2";

            string postUrl = "/topics";
            string replyUrl = "";

            //string apiKey = "Bearer 94cdf3b2-7b9c-4c67-9b98-e0e20682a5fb"; //Master
            string apiKey = String.Format("Bearer {0}",Program.API_MASTER_KEY);// 15baf25c-5d82-47c8-a660-71b4abb7b675"; //Master

            //string apiKey = "Bearer 02ca7aea-28e5-4f04-b6c6-02861c1c6d36";
            //string apiKey = "Bearer f440c24f-a7b1-401e-a788-31c1dc1df19c";

            string cid = categoryId, title = "", content = "";
            string url = root + postUrl;

            foreach (postInfo post in posts)
            {
                string sqlExist = "SELECT COUNT(*) FROM POSTS WHERE siteId = '" + post.SiteId + "' AND postId='" + post.PostId + "'";
                if (post.PostId == "0" || post.PostId == "")
                    sqlExist = "SELECT COUNT(*) FROM POSTS WHERE siteId = '" + post.SiteId + "' AND title='" + post.Title + "'";
                if (Convert.ToInt32(sh.ExecuteScalar(sqlExist)) > 0)
                    continue;

                //1. Insert local DB
                sh.Insert("POSTS", post.ToDictionary());

                if (post.SiteId == "bhxhhn.com.vn" && Program.lstExistPost!= null)
                {
                    string checkExist = String.Format("{0}|{1}", post.CategoryId, post.Title);
                    if (Program.lstExistPost.Contains(checkExist))
                        continue;
                }
                //2. Post site

                //string strData = "api_key=" + apiKey + "&api_username=" + api_username + "&title=" + post.Title + "&category=" + category + "&raw=" + post.Content + "";

                try
                {
                    var converter = new Converter();

                    JObject o = new JObject();
                    o["cid"] = categoryId;
                    o["title"] = post.Title;
                    //
                    string contentMD = post.Content;
                    contentMD = converter.Convert(contentMD).Trim();
                    contentMD = contentMD.Replace("\\r\\n\\","");
                    //
                    string contentRepMD = post.ContentReply;
                    if (!String.IsNullOrEmpty(contentRepMD))
                    {
                        contentRepMD = converter.Convert(contentRepMD).Trim();
                        contentRepMD = contentRepMD.Replace("\\r\\n\\", "");
                    }
                    
                    //Nếu bài dài quá, thì cắt làm 2
                    if (contentMD.Length > 32000 && String.IsNullOrEmpty(post.ContentReply))
                    {
                        string initContent = contentMD;
                        contentMD = initContent.Substring(0, 32000);
                        contentRepMD = initContent.Substring(32001, 32000);
                    }

                    o["content"] = contentMD;
                    string userId = userhelper.GetRandomUID(post.SiteId);
                    o["_uid"] = userId;

                    //var jsonContent = String.Format("{'cid': '{0}','title': '{1}', 'content': '{2}'}", cid, post.Title, post.Content);
                    string jsonContent = o.ToString();

                    JObject obj = util.HTTP_POST_JSON(url, apiKey, jsonContent);

                    if (obj != null && obj["code"].ToString() == "ok" && !String.IsNullOrEmpty(contentRepMD))
                    {
                        string tId = obj["payload"]["topicData"]["tid"].ToString();
                        if (!String.IsNullOrEmpty(tId))
                        {
                            string url2 = url + "/" + tId;

                            o = new JObject();
                            //contentMD = post.ContentReply;
                            // 
                            //contentMD = converter.Convert(post.ContentReply);

                            o["content"] = contentRepMD.Trim();
                            o["_uid"] = userhelper.GetRandomUIDReply(post.SiteId, userId);
                            jsonContent = o.ToString();
                            //
                            if (!String.IsNullOrEmpty(contentRepMD))
                            {
                                string modAPI = apiKey;
                                obj = util.HTTP_POST_JSON(url2, modAPI, jsonContent);
                                if (obj["code"].ToString() == "ok")
                                {
                                    Console.WriteLine("OK: " + post.Title);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.ToString();
                }
            }

            sh.Commit();
            sh.Close();
        }

        public void Post2Site(List<postInfo> posts)
        {
            SQLiteHelper sh = new SQLiteHelper();
            sh = sh.Init();
            sh.BeginTransaction();

            //Luu DB local - SQLLITE, de check trung
            string strUrrl = "https://bds5s.com/posts";
            string apiKey = "a82841644696ebb6381bd74b41c81fb4d5bddc0b91de59a932da5a9fb87ce9be";
            string user = "", catid = "", category = "";

            foreach (postInfo post in posts)
            {
                if (Convert.ToInt32(sh.ExecuteScalar("SELECT COUNT(*) FROM POSTS WHERE title='" + post.Title + "'")) > 0)
                    continue;

                //1. Insert local DB
                sh.Insert("POSTS", post.ToDictionary());

                //2. Post site
                string purl = util.RemoveVietnamese(post.Province);
                if (purl.Equals("ha-noi"))
                {
                    user = "bds2930";
                    catid = util.getQuanHuyen(util.RemoveVietnamese(post.District));
                    if (catid.Equals(""))
                    {
                        category = "5";
                    }
                    else
                    {
                        category = catid;
                    }
                    //
                }
                else if (purl.Equals("ho-chi-minh"))
                {
                    user = "bds2930";
                    catid = util.getQuanHuyen(util.RemoveVietnamese(post.District));
                    if (catid.Equals(""))
                    {
                        category = "35";
                    }
                    else
                    {
                        category = catid;
                    }
                }
                else if (purl.Equals("da-nang"))
                {
                    user = "bds2930";
                    catid = util.getQuanHuyen(util.RemoveVietnamese(post.District));
                    if (catid.Equals(""))
                    {
                        category = "61";
                    }
                    else
                    {
                        category = catid;
                    }
                }

                if (!category.Equals(""))
                {
                    string api_username = user;
                    string strData = "api_key=" + apiKey + "&api_username=" + api_username + "&title=" + post.Title + "&category=" + category + "&raw=" + post.Content + "";
                    util.HTTP_POST(strUrrl, strData);
                }
            }

            sh.Commit();
            sh.Close();
        }
    }
}
