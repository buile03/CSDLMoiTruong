using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace TechLife.Common
{
    public static class Functions
    {
        public static double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }
        public static string GetFullDiaPhuong(string sonha, string diachi, string xa, string huyen, string tinh = "")
        {
            string str = "";
            if (!String.IsNullOrEmpty(sonha))
            {
                str += sonha + " - ";
            }
            if (!String.IsNullOrEmpty(diachi))
            {
                str += diachi + " - ";
            }
            if (!String.IsNullOrEmpty(xa))
            {
                str += xa + " - ";
            }
            if (!String.IsNullOrEmpty(huyen))
            {
                str += huyen;
            }
            if (!String.IsNullOrEmpty(tinh))
            {
                str += " - " + tinh;
            }
            return str;
        }
        public static DateTime ConvertDateToSql(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    return DateTime.MinValue;
                }
                string str = "";
                if (date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                DateTime date_orc = Convert.ToDateTime(str + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                return date_orc.Date;
            }
            catch
            {
                return DateTime.MinValue.Date;
            }

        }
        public static DateTime ConvertDateToSql(DateTime date)
        {
            try
            {
                string str = "";
                if (date.ToString("dd/MM/yyyy").IndexOf("/") > 0)
                {
                    string[] str_split = date.ToString("dd/MM/yyyy").Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                DateTime date_orc = Convert.ToDateTime(str + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                return date_orc;
            }
            catch
            {
                return DateTime.MinValue;
            }

        }
        public static string GetDatetimeToVn(DateTime? date)
        {
            if (date != null)
            {
                if (Convert.ToDateTime(date).Year > 0001)
                    return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                else return String.Empty;
            }
            else return String.Empty;
        }
        public static string GetTimeToVn(DateTime date)
        {
            if (date.Year > 0001)
                return date.ToString("HH:mm dd/MM/yyyy");
            else return String.Empty;
        }
        public static List<T> ToListData<T>(this DataTable dataTable)
        {
            var dataList = new List<T>();
            dataList = JsonConvert.DeserializeObject<List<T>>(ToJsonData(dataTable));
            return dataList;
        }
        public static T ToData<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new T();
            dataList = JsonConvert.DeserializeObject<T>(ToJsonData(dataTable));
            return dataList;
        }
        static string ToJsonData(DataTable dataTable)
        {
            return JsonConvert.SerializeObject(dataTable);
        }
        public static string ConvertDecimalToVnd(decimal value)
        {
            var cul = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            return Convert.ToDouble(value).ToString("#,### vnđ", cul.NumberFormat);

        }
        public static decimal ConvertStringToDecimal(string value)
        {
            decimal result = 0;
            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    result = Convert.ToDecimal(value.Replace(" ", ","));
                }
                catch
                {
                    result = 0;
                }
            }
            return result;

        }
        public static string ConvertToVnd(decimal value)
        {
            if (value == 0) return "0";
            var cul = CultureInfo.GetCultureInfo("en-us");

            return Convert.ToDecimal(value).ToString("#,### ", cul.NumberFormat);

        }

        public static string ConvertToTrieuVnd(decimal value)
        {
            if (value == 0) return "0";

            value = value / 1000000;

            return String.Format("{0:N}", value);
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }
        public static string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        public static string RemoveIllegalCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace("(", string.Empty);
            text = text.Replace(")", string.Empty);
            text = text.Replace("<", string.Empty);
            text = text.Replace(">", string.Empty);

            return text;
        }
        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in
                normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
        private static string RemoveExtraHyphen(string text)
        {
            if (text.Contains("--"))
            {
                text = text.Replace("--", "-");
                return RemoveExtraHyphen(text);
            }
            return text;
        }
        private static string RemoveUnicodePunctuation(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in
                normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.InitialQuotePunctuation &&
                                              CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.FinalQuotePunctuation))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
        static string GetNewPrams(string url)
        {
            return !String.IsNullOrEmpty(url) ? Regex.Replace(url, @"~[^a-zA-Z0-9\-\\_\/\.]+~", string.Empty) : "";
        }
        public static bool CheckUrl(string url)
        {
            string news_url = RemoveIllegalCharacters(url);

            if (url != news_url)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static string NonUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                        "đ",
                                        "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                        "í","ì","ỉ","ĩ","ị",
                                        "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                        "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                        "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                        "d",
                                        "e","e","e","e","e","e","e","e","e","e","e",
                                        "i","i","i","i","i",
                                        "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                        "u","u","u","u","u","u","u","u","u","u","u",
                                        "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static string CompactFileName(this string text, int maxLength = 15)
        {
            if (String.IsNullOrEmpty(text))
                return null;
            if (text.Length <= 15 || text.Length <= maxLength)
                return text;
            text = text.Substring(0, 5) + "..." + text.Substring(text.Length - 7, 7);
            return text;
        }

        /// <summary>
        /// Hàm lấy ngày cuối cùng đã trừ đi các ngày nghỉ thứ 7, cn, các ngày nghỉ lễ
        /// </summary>
        /// <param name="ngayBatDau">Ngày bắt đầu</param>
        /// <param name="ngayNghiLe">Danh sách ngày nghỉ lễ âm lịch, và ngày nghỉ bù vì trùng với với thứ 7, chủ nhật</param>
        /// <param name="soNgay">Số ngày thực hiện</param>
        /// <returns></returns>
        public static DateTime NgayKetThucLamViec(DateTime? tuNgay, List<DateTime> ngayNghiLe = null, int soNgay = 0)
        {
            if (tuNgay != null)
            {
                DateTime ngayBatDau = (DateTime)tuNgay;

                if (ngayNghiLe != null && !ngayNghiLe.Contains(ngayBatDau.Date) && ngayBatDau.DayOfWeek == DayOfWeek.Saturday) ngayBatDau = ngayBatDau.AddDays(1);
                if (ngayNghiLe != null && !ngayNghiLe.Contains(ngayBatDau.Date) && ngayBatDau.DayOfWeek == DayOfWeek.Sunday) ngayBatDau = ngayBatDau.AddDays(1);
                if (ngayNghiLe != null && ngayNghiLe.Contains(ngayBatDau.Date)) ngayBatDau = ngayBatDau.AddDays(1);

                DateTime ngayKetThucChuaNghi = ngayBatDau.AddDays(soNgay);
                DateTime ngayKetThuc = ngayKetThucChuaNghi;
                int soNgayNghiCuoiTuan = 0;
                while (true)
                {

                    for (DateTime d = ngayBatDau; d <= ngayKetThuc; d = d.AddDays(1))
                    {
                        if (ngayNghiLe != null && !ngayNghiLe.Contains(d.Date) && d.DayOfWeek == DayOfWeek.Saturday)// Có ngày thứ 7
                        {
                            soNgayNghiCuoiTuan++;
                        }
                        if (ngayNghiLe != null && !ngayNghiLe.Contains(d.Date) && d.DayOfWeek == DayOfWeek.Sunday)// Có ngày chủ nhật
                        {
                            soNgayNghiCuoiTuan++;
                        }
                        if (ngayNghiLe != null && ngayNghiLe.Contains(d.Date))
                        {
                            soNgayNghiCuoiTuan++;
                        }
                    }
                    if (soNgayNghiCuoiTuan == 0) break;
                    ngayBatDau = ngayKetThuc.AddDays(1);
                    ngayKetThuc = soNgayNghiCuoiTuan > 0 ? ngayKetThuc.AddDays(soNgayNghiCuoiTuan) : ngayKetThuc.AddDays(1);
                    soNgayNghiCuoiTuan = 0;
                }
                if (soNgay > 1) ngayKetThuc = ngayKetThuc.AddDays(-1);

                while ((ngayNghiLe != null && ngayNghiLe.Contains(ngayKetThuc.Date))
                    || ngayKetThuc.DayOfWeek == DayOfWeek.Saturday
                    || ngayKetThuc.DayOfWeek == DayOfWeek.Sunday)
                {
                    ngayKetThuc = ngayKetThuc.AddDays(-1);
                }
                return ngayKetThuc;
            }
            else return DateTime.MinValue;

        }

        public static int SoNgayLamViec(DateTime tuNgay, DateTime denNgay, List<DateTime> ngayNghiLe = null)
        {
            int soNgayNghi = (denNgay - tuNgay).Days + 1;
            int soNgayNghiCuoiTuan = 0;
            for (DateTime d = tuNgay; d <= denNgay; d = d.AddDays(1))
            {
                if (ngayNghiLe != null && !ngayNghiLe.Contains(d.Date) && d.DayOfWeek == DayOfWeek.Saturday)// Có ngày thứ 7
                {
                    soNgayNghiCuoiTuan++;
                }
                if (ngayNghiLe != null && !ngayNghiLe.Contains(d.Date) && d.DayOfWeek == DayOfWeek.Sunday)// Có ngày chủ nhật
                {
                    soNgayNghiCuoiTuan++;
                }
                if (ngayNghiLe != null && ngayNghiLe.Contains(d.Date))
                {
                    soNgayNghiCuoiTuan++;
                }
            }
            return soNgayNghi - soNgayNghiCuoiTuan;
        }
        public static string GetDayOfWeek(DayOfWeek day)
        {
            if (day == DayOfWeek.Monday) return "Thứ 2";
            else if (day == DayOfWeek.Tuesday) return "Thứ 3";
            else if (day == DayOfWeek.Wednesday) return "Thứ 4";
            else if (day == DayOfWeek.Thursday) return "Thứ 5";
            else if (day == DayOfWeek.Friday) return "Thứ 6";
            else if (day == DayOfWeek.Saturday) return "Thứ 7";
            else return "Chủ nhật";
        }

        public static string RemoveMultiplesSpace(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return String.Empty;
            var list = value.Split(' ').Where(s => !String.IsNullOrWhiteSpace(s));
            return string.Join(" ", list);
        }
        public static string VietTat(this string value)
        {
            //string ten = name.Trim();
            //string[] ten_split = ten.Split(' ', '-');
            //string tenviettat = "";
            //foreach (var x in ten_split)
            //{
            //    if (x != "")
            //    {
            //        tenviettat += x[0].ToString().ToUpper();
            //    }
            //}
            //return tenviettat;

            try
            {
                if (String.IsNullOrWhiteSpace(value))
                    return String.Empty;

                var arr = value.Split(' ', '-');
                if (arr.Length == 0) return String.Empty;
                else if (arr.Length == 1)
                {
                    return !string.IsNullOrEmpty(arr[0].Substring(0, 1)) ? arr[0].Substring(0, 1).ToUpper() : "";
                }
                else
                {
                    string str = "";
                    for (int i = 0; i < arr.Length; i++)
                    {
                        str += !string.IsNullOrEmpty(arr[i].Substring(0, 1)) ? arr[i].Substring(0, 1).ToUpper() : "";
                    }
                    return str;
                }
            }
            catch
            {
                return string.Empty;
            }


        }
        public static int ConvertSeconds(int type, int time)
        {
            switch (type)
            {
                case 2: return time * 60;
                case 3: return time * 60 * 60;
                case 4: return time * 60 * 60 * 60;
                case 5: return time * 60 * 60 * 60 * 60;
                default: return time;
            }
        }

        public static string ReplaceFileName(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            text = text.Replace("[", "-");
            text = text.Replace("]", "-");
            text = text.Replace("@", "-");
            text = text.Replace("*", "-");
            text = text.Replace("'", "-");
            text = text.Replace("(", "-");
            text = text.Replace(")", "-");
            text = text.Replace("<", "-");
            text = text.Replace(">", "-");
            text = text.Replace("/", "-");
            text = text.Replace(@"\", "-");
            return text;
        }
        public static string ConvertPathFileToUrl(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            path = path.Replace(@"\\", "/");
            return path;
        }
        public static string CombineUrl(this string[] uriParts)
        {
            string url = "/";
            if (uriParts != null && uriParts.Length > 0)
            {
                for (int i = 0; i < uriParts.Length; i++)
                {
                    if (i == uriParts.Length - 1)
                        url += uriParts[i];
                    else
                        url += uriParts[i] + "/";
                }
            }
            return url;
        }

        public static bool IsValidDateTime(string input, out DateTime dateTime)
        {
            return DateTime.TryParseExact(
                input,
                "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateTime
            );
        }

        public static bool IsValidDateTimeDayMonthYear(string input, out DateTime dateTime)
        {
            return DateTime.TryParseExact(
                input,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateTime
            );
        }

        public static string RemoveSpecialCharactersKeepAlphabetNumber(string input)
        {
            // Chỉ giữ lại các ký tự chữ cái, số và khoảng trắng
            string result = Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
            return result;
        }

        public static string RemoveSpecialCharacters(string input)
        {
            // Chỉ giữ lại các ký tự chữ cái (kể cả tiếng Việt), số và khoảng trắng
            string pattern = @"[^a-zA-Z0-9\s\u00C0-\u1EF9]";
            return Regex.Replace(input, pattern, "");
        }
        /// <summary>
        /// Lấy tháng cuối cùng của Quý
        /// </summary>
        /// <param name="quarter">Quý</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int GetLastMonthOfQuarterByQuarter(int quarter)
        {
            if (quarter < 1 || quarter > 4)
                return 0;

            return quarter * 3;
        }
        /// <summary>
        /// Lấy tháng cuối cùng của kỳ 6 tháng
        /// </summary>
        /// <param name="half"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int GetLastMonthOfHalfYear(int half)
        {
            if (half < 1 || half > 2)
                return 0;

            return half * 6;
        }

        public static NameParts SplitVietnameseFullName(this string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return new NameParts { FirstName = "", LastName = "" };

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                return new NameParts
                {
                    FirstName = "",
                    LastName = parts[0]
                };
            }

            return new NameParts
            {
                FirstName = string.Join(" ", parts.Take(parts.Length - 1)), // Tất cả trừ phần cuối
                LastName = parts.Last() // Tên gọi
            };
        }
        public class NameParts
        {
            public string FirstName { get; set; } // Họ + Tên lót
            public string LastName { get; set; }  // Tên gọi
        }

    }
}
