using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crawler2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while(!exit)
            {
                using (CrawlerEntities db = new CrawlerEntities())
                {
                    foreach(Site site in db.Sites.Where(pt=>pt.IsActive == true).ToList())
                    {
                        crawler robot = new crawler(site, db);
                        robot.start();
                    }
                    Thread.Sleep(1000);//
                }
            }
        }
    }
}
