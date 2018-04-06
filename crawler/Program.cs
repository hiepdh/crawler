using Newtonsoft.Json.Linq;
using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace crawler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        public static List<String> lstExistPost;
        public static string API_MASTER_KEY = "eef9a642-6361-4816-a4d6-2068e4a86695";
        public static string API_ADMIN_KEY = "bec8e3eb-a95b-444f-8731-856358ccd4fc";
        public static string API_MOD_ID_HN = "1009";
        public static string API_MOD_ID_DNG = "1011";

        static string testMarkdown()
        {
            string html = "<div class=\"col - xs - 12 none - padding img -class\"> "
            + "<br>"
            + "<p>Đây là nội dung được đưa ra tại Hội nghị cung cấp thông tin báo chí thường kỳ tháng 3/2018 do BHXH Việt Nam tổ chức tại Hà Nội, chiều ngày 27/3. Phó Tổng Giám đốc BHXH Việt Nam Đào Việt Ánh chủ trì hội nghị. </p>"
            + "<p></p><div class=\"ExternalClass1FDB321B16234854A852EB050018BE46\"> <p style = \"text-align:center;\" ><img alt=\"\" src=\"https://baohiemxahoi.gov.vn:4545/pic/new_BHXH/BHYT/hopbaot31%2028318_20180328050831PM.jpg\" style=\"height:421px;width:632px;\" > </p>  abcs"
            + "<p class=\"tacgia\">PV</p></div> ";

            var converter = new Converter();
            html = converter.Convert(html).Trim();

            return html;
        }

        static void Main(string[] args)
        {
            //testMarkdown();
            //return;

            //string fileDV = Path.Combine(Application.StartupPath, "posts_exist.csv");
            //lstExistPost =  File.ReadAllLines(fileDV).ToList() ;

            //userhelper.CreateUser();
            //userhelper.PostUser();
            //return;

            //string url = "http://localhost:4567/api/v2/topics/142";
            //string apiKey = "Bearer f440c24f-a7b1-401e-a788-31c1dc1df19c";
            //string jsonContent = "{\"content\": \"Nội dung câu trả lời\"}";
            //JObject obj = util.HTTP_POST_JSON(url, apiKey, jsonContent);
            //return;


            List<pageInfo> pages = new List<pageInfo>();

            //< option value = "1" > Bảo hiểm y tế </ option >
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

            pageInfo page;

            page = new pageInfo();
            page.PageId = "baohiemxahoi.gov.vn";
            page.FirstPageUrl = "https://baohiemxahoi.gov.vn/tintuc/Pages/hoat-dong-bhxh-viet-nam.aspx?CateID=136&date=&Page=1";
            page.PageURL = "https://baohiemxahoi.gov.vn/tintuc/Pages/hoat-dong-bhxh-viet-nam.aspx?CateID=136&date=&Page={0}";
            pages.Add(page);

            page = new pageInfo();
            page.PageId = "baohiemxahoi.gov.vn";
            page.FirstPageUrl = "https://baohiemxahoi.gov.vn/tintuc/Pages/hoat-dong-bhxh-viet-nam.aspx?CateID=52&date=&Page=1";
            page.PageURL = "https://baohiemxahoi.gov.vn/tintuc/Pages/hoat-dong-bhxh-viet-nam.aspx?CateID=52&date=&Page={0}";
            pages.Add(page);

            page = new pageInfo();
            page.PageId = "baohiemxahoi.gov.vn";
            page.FirstPageUrl = "https://baohiemxahoi.gov.vn/tintuc/Pages/cai-cach-thu-tuc-hanh-chinh.aspx?CateID=59&date=&Page=1";
            page.PageURL = "https://baohiemxahoi.gov.vn/tintuc/Pages/cai-cach-thu-tuc-hanh-chinh.aspx?CateID=59&date=&Page={0}";
            pages.Add(page);


            page = new pageInfo();
            page.PageId = "baohiemxahoi.gov.vn";
            page.FirstPageUrl = "https://baohiemxahoi.gov.vn/tintuc/pages/luat-bhxh-bhyt-bat-buoc.aspx?CateID=53&date=&Page=1";
            page.PageURL = "https://baohiemxahoi.gov.vn/tintuc/pages/luat-bhxh-bhyt-bat-buoc.aspx?CateID=53&date=&Page={0}";
            pages.Add(page);


            //page = new pageInfo();
            //page.PageId = "bhxhdanang.gov.vn";
            //page.FirstPageUrl = "http://www.bhxhdanang.gov.vn/HoiDap.aspx?&Page=0";
            //page.PageURL = "http://www.bhxhdanang.gov.vn/HoiDap.aspx?&Page={0}";
            //pages.Add(page);

            //Tin tức
            //page = new pageInfo();
            //page.PageId = "ketoanthienung.org";
            //page.FirstPageUrl = "http://ketoanthienung.org/tin-tuc/nhung-diem-moi-ve-bhxh-bhyt-bhtn-kpcd.htm_p1";
            //page.PageURL = "http://ketoanthienung.org/tin-tuc/nhung-diem-moi-ve-bhxh-bhyt-bhtn-kpcd.htm_p{0}";
            //pages.Add(page);

            ////

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=1&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=1&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=2&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=2&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=3&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=3&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=4&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=4&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=5&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=5&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=6&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=6&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=7&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=7&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=8&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=8&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=9&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=9&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=10&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=10&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=11&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=11&numberpage=10";
            //pages.Add(page);

            //page = new pageInfo();
            //page.PageId = "bhxhhn.com.vn";
            //page.FirstPageUrl = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index=1&pkcm=12&numberpage=10";
            //page.PageURL = "http://bhxhhn.com.vn/hoidap/tabid/245/TopMenuId/48/cMenu/48/stParentMenuId/48/Default.aspx?index={0}&pkcm=12&numberpage=10";
            //pages.Add(page);


            //
            Console.WriteLine("Start crawler: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Console.WriteLine("-----");
            
            bool exit = false;
            while (!exit)
            {
                foreach (pageInfo pInfo in pages)
                {
                    crawler robot = new crawler(pInfo);

                    if (pInfo.PageId == "ketoanthienung.org")
                        robot.start_kttu();
                    else if (pInfo.PageId == "bhxhhn.com.vn")
                        robot.start_bhhn();
                    else if (pInfo.PageId == "bhxhdanang.gov.vn")
                        robot.start_bhdn();
                    else if (pInfo.PageId == "baohiemxahoi.gov.vn")
                        robot.start_bhxhgovvn();

                    //string sexit = Console.ReadLine();
                    //if (sexit == "exit")
                    //    exit = true;
                    //else
                    Thread.Sleep(6000);//
                }
            }
        }

    }
}
