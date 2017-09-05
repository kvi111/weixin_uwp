using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace weixin_uwp
{
    public static class Utils
    {
        //private static Gson gson = new GsonBuilder().disableHtmlEscaping().create();

        //    private Utils()
        //    {
        //    }

        public static Dictionary<String, Object> createMap(Object[] values)
        {
            Dictionary<String, Object> map = new Dictionary<String, Object>(values.Length / 2);
            for (int i = 0; i < values.Length; i++)
            {
                map.Add(values[i].ToString(), values[i + 1]);
                ++i;
            }
            return map;
        }

        public static String emptyOr(String str1, String str2)
        {
            if (string.IsNullOrEmpty(str1))
            {
                return str2;
            }
            return str1;
        }

        //    public static void sleep(long timeout)
        //    {
        //        try
        //        {
        //            TimeUnit.MILLISECONDS.sleep(timeout);
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //    }

        public static String match(String p, String str)
        {
            MatchCollection mc = Regex.Matches(p, str);
            if (mc.Count > 0)
            {
                return mc[0].Groups[1].Value;
            }
            return "";
        }

        //    public static void closeQuietly(Closeable closeable)
        //    {
        //        try
        //        {
        //            closeable.close();
        //        }
        //        catch (Exception e)
        //        {
        //            e.printStackTrace();
        //        }
        //    }

        public static bool isBlank(String str)
        {
            return null == str || "" == str.Trim();
        }

        //    public static boolean isNotBlank(String str)
        //    {
        //        return !isBlank(str) && !"null".equalsIgnoreCase(str);
        //    }

        /// <summary>
        /// 得到指定位数的随机数
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String getRandomNumber(int size)
        {
            //String num = "";
            //for (int i = 0; i < size; i++)
            //{
            //    Random rnd = new Random();
            //    double a = rnd.NextDouble() * 9;
            //    a = Math.Ceiling(a);
            //    int randomNum = (int)a;
            //    num += (int)a;
            //}
            //return num;
            Random rnd = new Random();
            int min = (int)Math.Pow(10, size - 1);
            int max = (int)Math.Pow(10, size) - 1;
            return rnd.Next(min, max).ToString();
        }

        /**
         * obj转json
         */
        public static String toJson(Object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o);
                //return gson.toJson(o);
            }
            catch
            {
                //WriteLog("Json序列化失败", e);
            }
            return "";
        }

        //    /**
        //     * json转obj
        //     */
        //    public static <T> T fromJson(String json, Class<T> classOfT)
        //    {
        //        try
        //        {
        //            return gson.fromJson(json, classOfT);
        //        }
        //        catch (Exception e)
        //        {
        //            WriteLog("Json反序列化失败", e);
        //        }
        //        return null;
        //    }

        //    public static String utf8ToUnicode(String inStr)
        //    {
        //        char[] myBuffer = inStr.toCharArray();
        //        StringBuffer sb = new StringBuffer();
        //        for (int i = 0; i < inStr.length(); i++)
        //        {
        //            Character.UnicodeBlock ub = Character.UnicodeBlock.of(myBuffer[i]);
        //            if (ub == Character.UnicodeBlock.BASIC_LATIN)
        //            {
        //                //英文及数字等
        //                sb.append(myBuffer[i]);
        //            }
        //            else if (ub == Character.UnicodeBlock.HALFWIDTH_AND_FULLWIDTH_FORMS)
        //            {
        //                //全角半角字符
        //                int j = (int)myBuffer[i] - 65248;
        //                sb.append((char)j);
        //            }
        //            else
        //            {
        //                //汉字
        //                short s = (short)myBuffer[i];
        //                String hexS = Integer.toHexString(s);
        //                String unicode = "\\u" + hexS;
        //                sb.append(unicode.toLowerCase());
        //            }
        //        }
        //        return sb.toString();
        //    }

        public static String unicodeToUtf8(String string1)
        {
            try
            {
                if (string1 == null) return "";

                if (string1.IndexOf("\\u") == -1)
                {
                    return string1;
                }
                //byte[] utf8 = string1.ToString( .getBytes("UTF-8");
                //// Convert from UTF-8 to Unicode
                //return new String(utf8, "UTF-8");

                Encoding unicode = Encoding.Unicode;
                Encoding utf8 = Encoding.UTF8;

                byte[] unicodeBytes = unicode.GetBytes(string1);
                byte[] utf8Bytes = Encoding.Convert(unicode, utf8, unicodeBytes);
                return utf8.GetString(utf8Bytes);
            }
            catch
            {

            }
            return string1;
        }

        public static String getCookie(List<String> cookies)
        {
            StringBuilder sBuffer = new StringBuilder();
            foreach (String value in cookies)
            {
                if (value == null)
                {
                    continue;
                }
                String cookie = value.Substring(0, value.IndexOf(";") + 1);
                sBuffer.Append(cookie);
            }
            return sBuffer.ToString();
        }

        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long currentTimeMillis()
        {
            return (long)((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
        }
        public static async Task<string> PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                byte[] responseData = await PostWebRequestBytesAsync(postUrl, paramData);
                ret = Encoding.UTF8.GetString(responseData);//解码
            }
            catch
            {

            }
            return ret;
        }
        public static async Task<byte[]> PostWebRequestBytesAsync(string postUrl, string paramData)
        {
            byte[] responseData = new byte[0];
            try
            {
                //byte[] postData = Encoding.UTF8.GetBytes(paramData);
                //WebClient webClient = new WebClient();
                List<KeyValuePair<string, string>> kvp = GetParamDataList(paramData);
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.PostAsync(postUrl, new FormUrlEncodedContent(kvp)).Result;//得到返回字符流
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch
            {

            }
            return responseData;
        }
        public static List<KeyValuePair<string, string>> GetParamDataList(string paramData)
        {
            var kvp = new List<KeyValuePair<string, string>>();
            string[] arrList = paramData.Split('&');
            foreach (string str in arrList)
            {
                string[] arr = str.Split('=');
                if (arr.Length == 2)
                {
                    kvp.Add(new KeyValuePair<string, string>(arr[0], arr[1]));
                }
            }
            return kvp;
        }
        //public static byte[] PostWebRequestBytes1(string postUrl, string paramData)
        //{
        //    byte[] responseData = new byte[0];
        //    try
        //    {
        //        byte[] postData = Encoding.UTF8.GetBytes(paramData);
        //        HttpWebRequest webClient = (HttpWebRequest)HttpWebRequest.Create(postUrl);
        //        webClient.Headers.Add("Cookie", "");
        //        responseData = webClient.UploadData(postUrl, "POST", postData);//得到返回字符流
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //    }
        //    return responseData;
        //}

        //public static ImageSource BytesToImage(byte[] buffer)
        //{
        //    MemoryStream ms = new MemoryStream(buffer);
        //    //Windows.Graphics.Imaging.ImageStream
        //    Image image;// = System.Drawing.Image.FromStream(ms);

        //    return image;
        //}

        public static async Task<ImageSource> BytesToImage(byte[] imageBuffer)
        {
            ImageSource imageSource = null;
            using (MemoryStream stream = new MemoryStream(imageBuffer))
            {
                var ras = stream.AsRandomAccessStream();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ras); //BitmapDecoder.JpegDecoderId,
                var provider = await decoder.GetPixelDataAsync();
                byte[] buffer = provider.DetachPixelData();
                WriteableBitmap bitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                await bitmap.PixelBuffer.AsStream().WriteAsync(buffer, 0, buffer.Length);
                imageSource = bitmap;
            }
            return imageSource;
        }

        public static async Task<byte[]> doPost1(String url)
        {
            using (HttpClient client = new HttpClient())
            {
                //if (null != cookie)
                //{
                //    client.DefaultRequestHeaders.Add("Cookie", cookie);
                //}
                ByteArrayContent bac = new ByteArrayContent(new byte[] { });
                //if (null != obj)
                //{
                //    String bodyJson = Utils.toJson(obj);
                //    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(bodyJson);
                //    bac = new ByteArrayContent(bytes);
                //}

                HttpResponseMessage response = await client.PostAsync(url, bac);//得到返回字符流
                try
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                catch (Exception e)
                {
                    //WriteError("doPost异常：{0}_{1}", e.Message, e.StackTrace);
                    return new byte[] { };
                }
            }
        }

        public static async Task<BitmapImage> ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return null;
            }

            BitmapImage bmp = new BitmapImage();
            try
            {
                MemoryStream mStream = new MemoryStream(byteArray);
                await bmp.SetSourceAsync(mStream.AsRandomAccessStream());
                return bmp;
            }
            catch (Exception ex)
            {
                bmp = null;
            }

            return bmp;
        }

        public static async Task<WriteableBitmap> GetWriteableBitmapAsync(byte[] buffer)
        {
            try
            {
                //byte[] buffer = await doPost1(url);
                if (buffer != null)
                {
                    BitmapImage bi = new BitmapImage();
                    WriteableBitmap wb = null; Stream stream2Write;
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {

                        stream2Write = stream.AsStreamForWrite();

                        await stream2Write.WriteAsync(buffer, 0, buffer.Length);

                        await stream2Write.FlushAsync();
                        stream.Seek(0);

                        await bi.SetSourceAsync(stream);

                        wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
                        stream.Seek(0);
                        await wb.SetSourceAsync(stream);

                        return wb;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        //public static async Task<ImageSource> BytesToImage(byte[] imageBuffer, int width, int height)
        //{
        //    WriteableBitmap wb = new WriteableBitmap(width, height);
        //    using (Stream _stream = wb.PixelBuffer.AsStream())
        //    {
        //        await _stream.WriteAsync(imageBuffer, 0, imageBuffer.Length);
        //    }
        //    return wb;
        //}

        /// <summary>
        /// 字符串形式的 Emoji 16进制Unicode编码  转换成 UTF16字符串 能够直接输出到客户端
        /// </summary>
        /// <param name="EmojiCode">1f604</param>
        /// <returns></returns>
        public static string EmojiCodeToUTF16String(string EmojiCode)
        {
            if (EmojiCode.Length != 4 && EmojiCode.Length != 5)
            {
                throw new ArgumentException("错误的 EmojiCode 16进制数据长度.一般为4位或5位");
            }
            //1f604
            int EmojiUnicodeHex = int.Parse(EmojiCode, System.Globalization.NumberStyles.HexNumber);

            //1f604对应 utf16 编码的int
            Int32 EmojiUTF16Hex = EmojiToUTF16(EmojiUnicodeHex, true);             //这里字符的低位在前.高位在后.
            //Response.Write(Convert.ToString(lon, 16)); Response.Write("<br/>"); //这里字符的低位在前.高位在后. 
            var emojiBytes = BitConverter.GetBytes(EmojiUTF16Hex);                     //把整型值变成Byte[]形式. Int64的话 丢掉高位的空白0000000   

            return Encoding.Unicode.GetString(emojiBytes);
        }

        /// <summary>
        /// EmoJi U+字符串对应的 int 值 转换成UTF16字符编码的值
        /// </summary>
        /// <param name="V">EmojiU+1F604 转成计算机整形以后的值V=0x1F604 </param>
        /// <param name="LowHeight">低字节在前的顺序.(默认)</param>
        /// <remarks>
        ///参考  
        ///http://blog.csdn.net/fengsh998/article/details/8668002
        ///http://punchdrunker.github.io/iOSEmoji/table_html/index.html
        /// V  = 0x64321
        /// Vx = V - 0x10000
        ///    = 0x54321
        ///    = 0101 0100 0011 0010 0001
        ///
        /// Vh = 01 0101 0000 // Vx 的高位部份的 10 bits
        /// Vl = 11 0010 0001 // Vx 的低位部份的 10 bits
        /// wh = 0xD800 //結果的前16位元初始值
        /// wl = 0xDC00 //結果的後16位元初始值
        ///
        /// wh = wh | Vh
        ///    = 1101 1000 0000 0000
        ///    |        01 0101 0000
        ///    = 1101 1001 0101 0000
        ///    = 0xD950
        ///
        /// wl = wl | Vl
        ///    = 1101 1100 0000 0000
        ///    |        11 0010 0001
        ///    = 1101 1111 0010 0001
        ///    = 0xDF21
        /// </remarks>
        /// <returns>EMOJI字符对应的UTF16编码16进制整形表示</returns>
        public static Int32 EmojiToUTF16(Int32 V, bool LowHeight = true)
        {

            Int32 Vx = V - 0x10000;

            Int32 Vh = Vx >> 10;//取高10位部分
            Int32 Vl = Vx & 0x3ff; //取低10位部分
            //Response.Write("Vh:"); Response.Write(Convert.ToString(Vh, 2)); Response.Write("<br/>"); //2进制显示
            //Response.Write("Vl:"); Response.Write(Convert.ToString(Vl, 2)); Response.Write("<br/>"); //2进制显示

            Int32 wh = 0xD800; //結果的前16位元初始值,这个地方应该是根据Unicode的编码规则总结出来的数值.
            Int32 wl = 0xDC00; //結果的後16位元初始值,这个地方应该是根据Unicode的编码规则总结出来的数值.
            wh = wh | Vh;
            wl = wl | Vl;

            if (LowHeight)
            {
                return wl << 16 | wh;   //低位左移16位以后再把高位合并起来 得到的结果是UTF16的编码值   //适合低位在前的操作系统 
            }
            else
            {
                return wh << 16 | wl; //高位左移16位以后再把低位合并起来 得到的结果是UTF16的编码值   //适合高位在前的操作系统
            }
        }

        /// <summary>
        /// 处理表情消息
        /// </summary>
        /// <param name="strEmoji"></param>
        /// <returns></returns>
        public static string SetEmoji(string strEmoji)
        {
            //strEmoji = "asdf[呲牙]asdf";
            string tempStr = "";
            string[] arrStr = strEmoji.Split(new string[] { "[", "]", "\"></span>", "<span class=\"" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in arrStr)
            {
                string str1 = str.Trim();
                if (string.IsNullOrEmpty(str1)) continue;

                if (Const.dictEmoji.ContainsKey("[" + str1 + "]"))
                {
                    tempStr += Utils.EmojiCodeToUTF16String(Const.dictEmoji["[" + str1 + "]"]);
                }
                else if (str1.StartsWith("emoji emoji")) //表情
                {
                    //使用emoji字符集来显示
                    tempStr += Utils.EmojiCodeToUTF16String(str1.Replace("emoji emoji", ""));
                }
                else
                {
                    tempStr += str;
                }
            }
            return tempStr;
        }
    }
}
