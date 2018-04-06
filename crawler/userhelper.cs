using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace crawler
{
    public class userhelper
    {
        public static List<string> lstUser = null;
        public static string GetRandomUID(string siteId = "")
        {
            if(siteId == "bhxhdanang.gov.vn")
                return Program.API_MOD_ID_DNG;
            else if(siteId == "baohiemxahoi.gov.vn")
                return  "1012";

            string randomUid = "";
            if (lstUser == null)
            {
                SQLiteHelper sh = new SQLiteHelper();
                sh = sh.Init();
                DataTable dtUser = sh.Select("SELECT uid FROM USERS WHERE uid is not null and uid <> ''");
                if (dtUser.Rows.Count > 0)
                {
                    lstUser = new List<string>();
                    foreach (DataRow dr in dtUser.Rows)
                    {
                        lstUser.Add(dr[0].ToString());
                    }
                    //
                    
                }
                //
                sh.Close();
            }
            //
            Random r = new Random();
            int index = r.Next(lstUser.Count - 1);
            randomUid = lstUser[index];

            return randomUid;
        }

        public static string GetRandomUIDReply(string siteId, string userId = "")
        {
            string mod = "";
            switch(siteId.ToLower().Trim())
            {
                case "bhxhhn.com.vn":
                    mod = Program.API_MOD_ID_HN;
                    break;
                case "bhxhdanang.gov.vn":
                    mod = Program.API_MOD_ID_DNG;
                    break;
                case "baohiemxahoi.gov.vn":
                    mod = "1012";
                    break;
                default:
                    mod = userId;
                    break;
            }
            return mod;
        }

        public static void PostUser()
        {
            SQLiteHelper sh = new SQLiteHelper();
            sh = sh.Init();
            DataTable dt =  sh.Select("SELECT * FROM USERS WHERE uid is null or uid = ''");
            sh.BeginTransaction();
            int total = 0;
            foreach(DataRow dr in dt.Rows)
            {
                total++;

                if (total >= 3000)
                    break;

                //string url = "http://localhost:4567/api/v2/users";
                //string apiKey = "Bearer f440c24f-a7b1-401e-a788-31c1dc1df19c";

                string url = "https://alobhxh.com/api/v2/users";
                string apiKey = String.Format("Bearer {0}", Program.API_ADMIN_KEY);

                JObject user = new JObject();
                user["username"] = GetString(dr["username"]);
                user["password"] = user["username"] + "@123";
                user["email"] = user["username"] + "@alobhxh.com";

                string jSONContent = user.ToString();
                //
                JObject obj = util.HTTP_POST_JSON(url, apiKey, jSONContent);
                if (obj!=null && obj["code"].ToString() == "ok")
                {
                    Console.WriteLine("OK: " + user["username"]);
                    //
                    string uid = obj["payload"]["uid"].ToString();
                    sh.Execute("UPDATE USERS SET UID = '" + uid + "' WHERE USERNAME = '" + user["username"] + "'");
                    //
                    url += "/" + uid;
                    //username, email, fullname, website, location, birthday, signature
                    user = new JObject();
                    user["username"] = GetString(dr["username"]);
                    user["email"] = user["username"] + "@alobhxh.com";
                    user["fullname"] = GetString(dr["fullname"]);
                    user["location"] = GetString(dr["address"]);
                    user["birthday"] = GetString(dr["birthday"]);
                    jSONContent = user.ToString();
                    //
                    util.HTTP_PUT_JSON(url, apiKey, jSONContent);
                }
            }
            sh.Commit();
            sh.Close();
        }

        private static string GetString(object v)
        {
            if (v != null)
                return v.ToString();
            else
                return "";
        }

        public static void CreateUser()
        {
            string path = Path.Combine(Application.StartupPath, "Data", "alo_users.xls");
            //DataTable dt = util.Excel_To_DataTable(path,0);
            DataTable dt = util.GetRequestsDataFromExcel(path);

            SQLiteHelper sh = new SQLiteHelper();
            sh = sh.Init();
            sh.BeginTransaction();

            foreach (DataRow dr in dt.Rows)
            {
                string username = dr["HoTen"].ToString().Replace("'", "");
                username = fullname2username(username).ToLower();
                string mail = username + "@alobhxh.com";
                string birthDay = dr["NgaySinh"].ToString();
                //
                //
                if (Convert.ToInt32(sh.ExecuteScalar("SELECT COUNT(*) FROM USERS WHERE USERNAME = '" + username + "'")) > 0)
                    continue;
                //
                userInfo user = new userInfo();
                user.Username = username.Replace("'", "");
                user.Birthday = birthDay;
                user.Fullname = dr["HoTen"].ToString().Replace("'", "");
                user.Address = dr["DiaChi"].ToString().Replace("'", "");
                user.Sex = dr["GioiTinh"].ToString().Replace("'", "");

                //
                sh.Insert("USERS", user.ToDictionary());
                //
            }

            sh.Commit();
            sh.Close();
        }


        public static string fullname2username(string fullName)
        {
            fullName = convertToUnSign2(fullName);
            string[] names = fullName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            string name = names[names.Length - 1];

            if (names.Length > 1)
                for (int i = 0; i < names.Length - 1; i++)
                {
                    name += names[i].Substring(0, 1);
                }

            return name;
        }

        public static string convertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

    }
}
