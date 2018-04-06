using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace crawler
{
    public static class util
    {
        public static Dictionary<string, object> ToDictionary(this object myObj)
        {
            return myObj.GetType()
                .GetProperties()
                .Select(pi => new { Name = pi.Name, Value = pi.GetValue(myObj, null) })
                .Union(
                    myObj.GetType()
                    .GetFields()
                    .Select(fi => new { Name = fi.Name, Value = fi.GetValue(myObj) })
                 )
                .ToDictionary(ks => ks.Name, vs => vs.Value);
        }

        public static string RemoveVietnamese(string Input)
        {
            try
            {
                char[] Thga = "aáàạảãâấầậẩẫăắằặẳẵ".ToCharArray();
                char[] Thge = "eéèẹẻẽêếềệểễeeeeee".ToCharArray();
                char[] Thgo = "oóòọỏõôốồộổỗơớờợởỡ".ToCharArray();
                char[] Thgu = "uúùụủũưứừựửữuuuuuu".ToCharArray();
                char[] Thgi = "iíìịỉĩiiiiiiiiiiii".ToCharArray();
                char[] Thgd = "dđdddddddddddddddd".ToCharArray();
                char[] Thgy = "yýỳỵỷỹyyyyyyyyyyyy".ToCharArray();
                char[] HoaA = "AÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ".ToCharArray();
                char[] HoaE = "EÉÈẸẺẼÊẾỀỆỂỄEEEEEE".ToCharArray();
                char[] HoaO = "OÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ".ToCharArray();
                char[] HoaU = "UÚÙỤỦŨƯỨỪỰỬỮUUUUUU".ToCharArray();
                char[] HoaI = "IÍÌỊỈĨIIIIIIIIIIII".ToCharArray();
                char[] HoaD = "DĐDDDDDDDDDDDDDDDD".ToCharArray();
                char[] HoaY = "YÝỲỴỶỸYYYYYYYYYYYY".ToCharArray();
                for (int i = 1; i < Thga.Length; i++)
                {
                    Input = Input.Replace(Thga[i], Thga[0]);
                    Input = Input.Replace(Thge[i], Thge[0]);
                    Input = Input.Replace(Thgo[i], Thgo[0]);
                    Input = Input.Replace(Thgu[i], Thgu[0]);
                    Input = Input.Replace(Thgi[i], Thgi[0]);
                    Input = Input.Replace(Thgd[i], Thgd[0]);
                    Input = Input.Replace(Thgy[i], Thgy[0]);
                    Input = Input.Replace(HoaA[i], HoaA[0]);
                    Input = Input.Replace(HoaE[i], HoaE[0]);
                    Input = Input.Replace(HoaO[i], HoaO[0]);
                    Input = Input.Replace(HoaU[i], HoaU[0]);
                    Input = Input.Replace(HoaI[i], HoaI[0]);
                    Input = Input.Replace(HoaD[i], HoaD[0]);
                    Input = Input.Replace(HoaY[i], HoaY[0]);
                }

                Input = Input.ToLower().Trim();
                Input = Input.Replace(":", "");
                Input = Input.Replace("?", "");
                Input = Input.Replace("%", "");
                Input = Input.Replace(",", "");
                Input = Input.Replace(".", "");
                Input = Input.Replace(":", "");
                Input = Input.Replace("”", "");
                Input = Input.Replace("“", "");
                Input = Input.Replace("&", "");
                Input = Input.Replace("#", "");
                Input = Input.Replace("$", "");
                Input = Input.Replace("@", "");
                Input = Input.Replace("!", "");
                Input = Input.Replace("(", "");
                Input = Input.Replace(")", "");
                Input = Input.Replace(";", "");
                Input = Input.Replace("<", "");
                Input = Input.Replace(">", "");
                Input = Input.Replace("\"", "");
                Input = Input.Replace("\n", "");
                Input = Input.Replace("\r", "");
                Input = Input.Replace("`", "");
                Input = Input.Replace("'", "");
                Input = Input.Replace("ể", "");
                Input = Input.Replace("/", "-");
                Input = Input.Replace(" - ", "-");
                Input = Input.Replace(" ", "-");
            }
            catch (Exception ex)
            {
                Input = ex.Message;
            }
            return Input;
        }

        public static string HTTP_POST(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            try
            {
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        public static string getQuanHuyen(string quanhuyen)
        {

            try
            {
                //HN
                if (quanhuyen.Equals("quan-ba-dinh"))
                {
                    return "6";
                }
                else if (quanhuyen.Equals("huyen-ba-vi"))
                {
                    return "7";
                }
                else if (quanhuyen.Equals("quan-cau-giay"))
                {
                    return "8";
                }
                else if (quanhuyen.Equals("huyen-chuong-my"))
                {
                    return "9";
                }
                else if (quanhuyen.Equals("huyen-dan-phuong"))
                {
                    return "10";
                }
                else if (quanhuyen.Equals("huyen-dong-anh"))
                {
                    return "11";
                }
                else if (quanhuyen.Equals("quan-dong-da"))
                {
                    return "12";
                }
                else if (quanhuyen.Equals("huyen-gia-lam"))
                {
                    return "13";
                }
                else if (quanhuyen.Equals("quan-ha-dong"))
                {
                    return "14";
                }
                else if (quanhuyen.Equals("quan-hai-ba-trung"))
                {
                    return "15";
                }
                else if (quanhuyen.Equals("huyen-hoai-duc"))
                {
                    return "16";
                }
                else if (quanhuyen.Equals("quan-hoan-kiem"))
                {
                    return "17";
                }
                else if (quanhuyen.Equals("quan-hoang-mai"))
                {
                    return "18";
                }
                else if (quanhuyen.Equals("quan-long-bien"))
                {
                    return "19";
                }
                else if (quanhuyen.Equals("huyen-me-linh"))
                {
                    return "20";
                }
                else if (quanhuyen.Equals("huyen-my-duc"))
                {
                    return "21";
                }
                else if (quanhuyen.Equals("huyen-phu-xuyen"))
                {
                    return "22";
                }
                else if (quanhuyen.Equals("huyen-phuc-tho"))
                {
                    return "23";
                }
                else if (quanhuyen.Equals("huyen-quoc-oai"))
                {
                    return "24";
                }
                else if (quanhuyen.Equals("huyen-soc-son"))
                {
                    return "25";
                }
                else if (quanhuyen.Equals("thi-xa-son-tay"))
                {
                    return "26";
                }
                else if (quanhuyen.Equals("quan-tay-ho"))
                {
                    return "27";
                }
                else if (quanhuyen.Equals("huyen-thach-that"))
                {
                    return "28";
                }
                else if (quanhuyen.Equals("huyen-thanh-oai"))
                {
                    return "29";
                }
                else if (quanhuyen.Equals("huyen-thanh-tri"))
                {
                    return "30";
                }
                else if (quanhuyen.Equals("quan-thanh-xuan"))
                {
                    return "31";
                }
                else if (quanhuyen.Equals("huyen-thuong-tin"))
                {
                    return "32";
                }
                else if (quanhuyen.Equals("quan-bac-tu-liem"))
                {
                    return "33";
                }
                else if (quanhuyen.Equals("quan-nam-tu-liem"))
                {
                    return "60";
                }
                else if (quanhuyen.Equals("huyen-ung-hoa"))
                {
                    return "34";
                }

                //HCM
                else if (quanhuyen.Equals("huyen-binh-chanh"))
                {
                    return "36";
                }
                else if (quanhuyen.Equals("huyen-can-gio"))
                {
                    return "37";
                }
                else if (quanhuyen.Equals("huyen-cu-chi"))
                {
                    return "38";
                }
                else if (quanhuyen.Equals("huyen-hoc-mon"))
                {
                    return "39";
                }
                else if (quanhuyen.Equals("huyen-nha-be"))
                {
                    return "40";
                }
                else if (quanhuyen.Equals("quan-1"))
                {
                    return "41";
                }
                else if (quanhuyen.Equals("quan-1"))
                {
                    return "41";
                }
                else if (quanhuyen.Equals("quan-2"))
                {
                    return "42";
                }
                else if (quanhuyen.Equals("quan-3"))
                {
                    return "43";
                }
                else if (quanhuyen.Equals("quan-4"))
                {
                    return "44";
                }
                else if (quanhuyen.Equals("quan-5"))
                {
                    return "45";
                }
                else if (quanhuyen.Equals("quan-6"))
                {
                    return "46";
                }
                else if (quanhuyen.Equals("quan-7"))
                {
                    return "47";
                }
                else if (quanhuyen.Equals("quan-8"))
                {
                    return "48";
                }
                else if (quanhuyen.Equals("quan-9"))
                {
                    return "49";
                }
                else if (quanhuyen.Equals("quan-10"))
                {
                    return "50";
                }
                else if (quanhuyen.Equals("quan-11"))
                {
                    return "51";
                }
                else if (quanhuyen.Equals("quan-12"))
                {
                    return "52";
                }
                else if (quanhuyen.Equals("quan-binh-tan"))
                {
                    return "53";
                }
                else if (quanhuyen.Equals("quan-binh-thanh"))
                {
                    return "54";
                }
                else if (quanhuyen.Equals("quan-go-vap"))
                {
                    return "55";
                }
                else if (quanhuyen.Equals("quan-phu-nhuan"))
                {
                    return "56";
                }
                else if (quanhuyen.Equals("quan-tan-binh"))
                {
                    return "57";
                }
                else if (quanhuyen.Equals("quan-tan-phu"))
                {
                    return "58";
                }
                else if (quanhuyen.Equals("quan-thu-duc"))
                {
                    return "59";
                }


                //DN
                else if (quanhuyen.Equals("quan-hai-chau"))
                {
                    return "62";
                }
                else if (quanhuyen.Equals("quan-thanh-khe"))
                {
                    return "63";
                }
                else if (quanhuyen.Equals("quan-son-tra"))
                {
                    return "64";
                }
                else if (quanhuyen.Equals("quan-ngu-hanh-son"))
                {
                    return "65";
                }
                else if (quanhuyen.Equals("quan-lien-chieu"))
                {
                    return "66";
                }
                else if (quanhuyen.Equals("huyen-hoa-vang"))
                {
                    return "67";
                }
                else if (quanhuyen.Equals("quan-cam-le"))
                {
                    return "68";
                }
                else if (quanhuyen.Equals("huyen-hoang-sa"))
                {
                    return "69";
                }

                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                return "";
            }
        }

        internal static JObject HTTP_POST_JSON(string url, string apiKey, string jsonContent)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string Out = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.Method = "POST";
            request.Headers.Add("Authorization", apiKey);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    System.IO.Stream ReceiveStream = response.GetResponseStream();
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                    {
                        Char[] read = new Char[256];
                        int count = sr.Read(read, 0, 256);

                        while (count > 0)
                        {
                            String str = new String(read, 0, count);
                            Out += str;
                            count = sr.Read(read, 0, 256);
                        }
                    }
                }
                //
                //if (Out != "")
                //{
                request = null;
                JObject retVal = JObject.Parse(Out);
                return retVal;

                //}
            }
            catch (WebException ex)
            {
                //           MessageBox.Show(ex.Message);
                request = null;
                return null;
            }
        }

        internal static JObject HTTP_PUT_JSON(string url, string apiKey, string jsonContent)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string Out = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.Method = "PUT";
            request.Headers.Add("Authorization", apiKey);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    System.IO.Stream ReceiveStream = response.GetResponseStream();
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                    {
                        Char[] read = new Char[256];
                        int count = sr.Read(read, 0, 256);

                        while (count > 0)
                        {
                            String str = new String(read, 0, count);
                            Out += str;
                            count = sr.Read(read, 0, 256);
                        }
                    }
                }
                //
                //if (Out != "")
                //{
                request = null;
                JObject retVal = JObject.Parse(Out);
                return retVal;

                //}
            }
            catch (WebException ex)
            {
                //           MessageBox.Show(ex.Message);
                request = null;
                return null;
            }
        }

        public static string getQuanHuyenHN(string quanhuyen)
        {
            try
            {
                if (quanhuyen.Equals("quan-ba-dinh"))
                {
                    return "6";
                }
                else if (quanhuyen.Equals("huyen-ba-vi"))
                {
                    return "7";
                }
                else if (quanhuyen.Equals("quan-cau-giay"))
                {
                    return "8";
                }
                else if (quanhuyen.Equals("huyen-chuong-my"))
                {
                    return "9";
                }
                else if (quanhuyen.Equals("huyen-dan-phuong"))
                {
                    return "10";
                }
                else if (quanhuyen.Equals("huyen-dong-anh"))
                {
                    return "11";
                }
                else if (quanhuyen.Equals("quan-dong-da"))
                {
                    return "12";
                }
                else if (quanhuyen.Equals("huyen-gia-lam"))
                {
                    return "13";
                }
                else if (quanhuyen.Equals("quan-ha-dong"))
                {
                    return "14";
                }
                else if (quanhuyen.Equals("quan-hai-ba-trung"))
                {
                    return "15";
                }
                else if (quanhuyen.Equals("huyen-hoai-duc"))
                {
                    return "16";
                }
                else if (quanhuyen.Equals("quan-hoan-kiem"))
                {
                    return "17";
                }
                else if (quanhuyen.Equals("quan-hoang-mai"))
                {
                    return "18";
                }
                else if (quanhuyen.Equals("quan-long-bien"))
                {
                    return "19";
                }
                else if (quanhuyen.Equals("huyen-me-linh"))
                {
                    return "20";
                }
                else if (quanhuyen.Equals("huyen-my-duc"))
                {
                    return "21";
                }
                else if (quanhuyen.Equals("huyen-phu-xuyen"))
                {
                    return "22";
                }
                else if (quanhuyen.Equals("huyen-phuc-tho"))
                {
                    return "23";
                }
                else if (quanhuyen.Equals("huyen-quoc-oai"))
                {
                    return "24";
                }
                else if (quanhuyen.Equals("huyen-soc-son"))
                {
                    return "25";
                }
                else if (quanhuyen.Equals("thi-xa-son-tay"))
                {
                    return "26";
                }
                else if (quanhuyen.Equals("quan-tay-ho"))
                {
                    return "27";
                }
                else if (quanhuyen.Equals("huyen-thach-that"))
                {
                    return "28";
                }
                else if (quanhuyen.Equals("huyen-thanh-oai"))
                {
                    return "29";
                }
                else if (quanhuyen.Equals("huyen-thanh-tri"))
                {
                    return "30";
                }
                else if (quanhuyen.Equals("quan-thanh-xuan"))
                {
                    return "31";
                }
                else if (quanhuyen.Equals("huyen-thuong-tin"))
                {
                    return "32";
                }
                else if (quanhuyen.Equals("quan-bac-tu-liem"))
                {
                    return "33";
                }
                else if (quanhuyen.Equals("quan-nam-tu-liem"))
                {
                    return "60";
                }
                else if (quanhuyen.Equals("huyen-ung-hoa"))
                {
                    return "34";
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string getQuanHuyenHCM(string quanhuyen)
        {

            try
            {
                if (quanhuyen.Equals("huyen-binh-chanh"))
                {
                    return "36";
                }
                else if (quanhuyen.Equals("huyen-can-gio"))
                {
                    return "37";
                }
                else if (quanhuyen.Equals("huyen-cu-chi"))
                {
                    return "38";
                }
                else if (quanhuyen.Equals("huyen-hoc-mon"))
                {
                    return "39";
                }
                else if (quanhuyen.Equals("huyen-nha-be"))
                {
                    return "40";
                }
                else if (quanhuyen.Equals("quan-1"))
                {
                    return "41";
                }
                else if (quanhuyen.Equals("quan-1"))
                {
                    return "41";
                }
                else if (quanhuyen.Equals("quan-2"))
                {
                    return "42";
                }
                else if (quanhuyen.Equals("quan-3"))
                {
                    return "43";
                }
                else if (quanhuyen.Equals("quan-4"))
                {
                    return "44";
                }
                else if (quanhuyen.Equals("quan-5"))
                {
                    return "45";
                }
                else if (quanhuyen.Equals("quan-6"))
                {
                    return "46";
                }
                else if (quanhuyen.Equals("quan-7"))
                {
                    return "47";
                }
                else if (quanhuyen.Equals("quan-8"))
                {
                    return "48";
                }
                else if (quanhuyen.Equals("quan-9"))
                {
                    return "49";
                }
                else if (quanhuyen.Equals("quan-10"))
                {
                    return "50";
                }
                else if (quanhuyen.Equals("quan-11"))
                {
                    return "51";
                }
                else if (quanhuyen.Equals("quan-12"))
                {
                    return "52";
                }
                else if (quanhuyen.Equals("quan-binh-tan"))
                {
                    return "53";
                }
                else if (quanhuyen.Equals("quan-binh-thanh"))
                {
                    return "54";
                }
                else if (quanhuyen.Equals("quan-go-vap"))
                {
                    return "55";
                }
                else if (quanhuyen.Equals("quan-phu-nhuan"))
                {
                    return "56";
                }
                else if (quanhuyen.Equals("quan-tan-binh"))
                {
                    return "57";
                }
                else if (quanhuyen.Equals("quan-tan-phu"))
                {
                    return "58";
                }
                else if (quanhuyen.Equals("quan-thu-duc"))
                {
                    return "59";
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                return "";
            }
        }


        public static DataTable Excel_To_DataTable(string pRutaArchivo, int pHojaIndex)
        {
            // --------------------------------- //
            /* REFERENCIAS:
             * NPOI.dll
             * NPOI.OOXML.dll
             * NPOI.OpenXml4Net.dll */
            // --------------------------------- //
            /* USING:
             * using NPOI.SS.UserModel;
             * using NPOI.HSSF.UserModel;
             * using NPOI.XSSF.UserModel; */
            // AUTOR: Ing. Jhollman Chacon R. 2015
            // --------------------------------- //
            DataTable Tabla = null;
            try
            {
                if (System.IO.File.Exists(pRutaArchivo))
                {

                    IWorkbook workbook = null;  //IWorkbook determina si es xls o xlsx              
                    ISheet worksheet = null;
                    string first_sheet_name = "";

                    using (FileStream FS = new FileStream(pRutaArchivo, FileMode.Open, FileAccess.Read))
                    {
                        workbook = WorkbookFactory.Create(FS);          //Abre tanto XLS como XLSX
                        worksheet = workbook.GetSheetAt(pHojaIndex);    //Obtener Hoja por indice
                        first_sheet_name = worksheet.SheetName;         //Obtener el nombre de la Hoja

                        Tabla = new DataTable(first_sheet_name);
                        Tabla.Rows.Clear();
                        Tabla.Columns.Clear();

                        // Leer Fila por fila desde la primera
                        for (int rowIndex = 0; rowIndex <= worksheet.LastRowNum; rowIndex++)
                        {
                            try
                            {
                                DataRow NewReg = null;
                                IRow row = worksheet.GetRow(rowIndex);
                                IRow row2 = null;
                                IRow row3 = null;

                                if (rowIndex == 0)
                                {
                                    row2 = worksheet.GetRow(rowIndex + 1); //Si es la Primera fila, obtengo tambien la segunda para saber el tipo de datos
                                    row3 = worksheet.GetRow(rowIndex + 2); //Y la tercera tambien por las dudas
                                }

                                if (row != null) //null is when the row only contains empty cells 
                                {
                                    if (rowIndex > 0) NewReg = Tabla.NewRow();

                                    int colIndex = 0;
                                    //Leer cada Columna de la fila
                                    foreach (ICell cell in row.Cells)
                                    {
                                        object valorCell = null;
                                        string cellType = "";
                                        string[] cellType2 = new string[2];

                                        if (rowIndex == 0) //Asumo que la primera fila contiene los titlos:
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                ICell cell2 = null;
                                                if (i == 0) { cell2 = row2.GetCell(cell.ColumnIndex); }
                                                else { cell2 = row3.GetCell(cell.ColumnIndex); }

                                                if (cell2 != null)
                                                {
                                                    switch (cell2.CellType)
                                                    {
                                                        case CellType.Blank: break;
                                                        case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                        case CellType.String: cellType2[i] = "System.String"; break;
                                                        case CellType.Numeric:
                                                            if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                            else
                                                            {
                                                                cellType2[i] = "System.Double";  //valorCell = cell2.NumericCellValue;
                                                            }
                                                            break;

                                                        case CellType.Formula:
                                                            bool continuar = true;
                                                            switch (cell2.CachedFormulaResultType)
                                                            {
                                                                case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                                case CellType.String: cellType2[i] = "System.String"; break;
                                                                case CellType.Numeric:
                                                                    if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                                    else
                                                                    {
                                                                        try
                                                                        {
                                                                            //DETERMINAR SI ES BOOLEANO
                                                                            if (cell2.CellFormula == "TRUE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                            if (continuar && cell2.CellFormula == "FALSE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                            if (continuar) { cellType2[i] = "System.Double"; continuar = false; }
                                                                        }
                                                                        catch { }
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            cellType2[i] = "System.String"; break;
                                                    }
                                                }
                                            }

                                            //Resolver las diferencias de Tipos
                                            if (cellType2[0] == cellType2[1]) { cellType = cellType2[0]; }
                                            else
                                            {
                                                if (cellType2[0] == null) cellType = cellType2[1];
                                                if (cellType2[1] == null) cellType = cellType2[0];
                                                if (cellType == "") cellType = "System.String";
                                            }

                                            //Obtener el nombre de la Columna
                                            string colName = "Column_{0}";
                                            try { colName = cell.StringCellValue; }
                                            catch { colName = string.Format(colName, colIndex); }

                                            //Verificar que NO se repita el Nombre de la Columna
                                            foreach (DataColumn col in Tabla.Columns)
                                            {
                                                if (col.ColumnName == colName) colName = string.Format("{0}_{1}", colName, colIndex);
                                            }

                                            //Agregar el campos de la tabla:
                                            if (cellType != null)
                                            {
                                                DataColumn codigo = new DataColumn(colName, System.Type.GetType(cellType));
                                                Tabla.Columns.Add(codigo); colIndex++;
                                            }
                                        }
                                        else
                                        {
                                            //Las demas filas son registros:
                                            if (cellType != null)
                                            {
                                                switch (cell.CellType)
                                                {
                                                    case CellType.Blank: valorCell = DBNull.Value; break;
                                                    case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                    case CellType.String: valorCell = cell.StringCellValue; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                        else { valorCell = cell.NumericCellValue; }
                                                        break;
                                                    case CellType.Formula:
                                                        switch (cell.CachedFormulaResultType)
                                                        {
                                                            case CellType.Blank: valorCell = DBNull.Value; break;
                                                            case CellType.String: valorCell = cell.StringCellValue; break;
                                                            case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                            case CellType.Numeric:
                                                                if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                                else { valorCell = cell.NumericCellValue; }
                                                                break;
                                                        }
                                                        break;
                                                    default: valorCell = cell.StringCellValue; break;
                                                }
                                            }
                                            //Agregar el nuevo Registro
                                            if (cell.ColumnIndex <= Tabla.Columns.Count - 1) NewReg[cell.ColumnIndex] = valorCell;
                                        }
                                    }

                                }
                                if (rowIndex > 0) Tabla.Rows.Add(NewReg);

                            }
                            catch (Exception ex)
                            {
                                string s = ex.ToString();
                            }
                        }

                        Tabla.AcceptChanges();
                    }
                }
                else
                {
                    throw new Exception("ERROR 404: El archivo especificado NO existe.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tabla;
        }


        private static ISheet GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }

        public static DataTable GetRequestsDataFromExcel(string fullFilePath)
        {
            try
            {
                var sh = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < currentRow.Cells.Count; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    dr[j] = DateUtil.IsCellDateFormatted(cell)
                                        ? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
