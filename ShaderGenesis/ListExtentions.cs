using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShaderGenesis
{
    public static class ListExtensions
    {
        public static void ReadLines(this List<String> thisList, string fileName)
        {
            thisList.Clear();
            //FileInfo info = new FileInfo(fileName);

            //if (info.Exists)
            //{
            thisList.AddRange(File.ReadAllLines(fileName));
            //}
        }

        public static void WriteLines(this List<String> thisList, string fileName)
        {
            File.WriteAllLines(fileName, thisList);
        }

        public static void AppendLines(this List<String> thisList, string fileName)
        {
            File.AppendAllLines(fileName, thisList);
        }

        public static void MakeUniqueList(this List<String> thisList)
        {
            List<string> uniqueList = new List<string>();
            for (int loop = 0; loop < thisList.Count; ++loop)
            {
                uniqueList.Add(thisList[loop]);
            }

            thisList.Clear();

            for (int loop = 0; loop < uniqueList.Count; ++loop)
            {
                if (thisList.Contains(uniqueList[loop]) == false)
                {
                    thisList.Add(uniqueList[loop]);
                }

            }

        }

        public static string ToCSV(this List<String> thisList)
        {
            string ret = "";

            StringBuilder sb = new StringBuilder();

            for (int loop = 0; loop < thisList.Count; ++loop)
            {
                sb.Append("\"");
                sb.Append(thisList[loop]);
                sb.Append("\"");
                if (loop != thisList.Count - 1)
                {
                    sb.Append(",");
                }
            }

            ret = sb.ToString();

            return ret;
        }

        public static void FromCSV(this List<String> thisList, string incsv)
        {
            string tempString = string.Empty;

            //Incase user did not alloc the list
            if (thisList == null)
            {
                thisList = new List<string>();
            }
            else
            {
                thisList.Clear();
            }

            byte[] buffer4 = ASCIIEncoding.UTF8.GetBytes(incsv);

            int QUOTEOPEN = 0;

            for (int lc = 0; lc < buffer4.Length; ++lc)
            {
                if (buffer4[lc] == ',')
                {
                    if (QUOTEOPEN == 0)
                    {

                        thisList.Add(tempString);

                        tempString = string.Empty;

                    }
                    else if (QUOTEOPEN == 1)
                    {
                        //pass the comma thru since its inside of qoutes
                        tempString += ASCIIEncoding.UTF8.GetString(buffer4, lc, 1);
                    }
                }
                else if (buffer4[lc] == '\"' && QUOTEOPEN == 0)
                {
                    QUOTEOPEN = 1;
                }
                else if (buffer4[lc] == '\"' && QUOTEOPEN == 1)
                {
                    QUOTEOPEN = 0;
                }
                else if (buffer4[lc] == 0)
                {
                    //here is the trim error removerd
                    thisList.Add(tempString.TrimEnd());
                    lc = buffer4.Length;
                }
                else
                {
                    tempString += ASCIIEncoding.UTF8.GetString(buffer4, lc, 1);
                }

            }//lc

            thisList.Add(tempString);

        }

        public static void FromCSV(this List<String> thisList, string incsv, char delimter)
        {
            string tempString = string.Empty;

            //Incase user did not alloc the list
            if (thisList == null)
            {
                thisList = new List<string>();
            }
            else
            {
                thisList.Clear();
            }

            byte[] buffer4 = ASCIIEncoding.UTF8.GetBytes(incsv);

            int QUOTEOPEN = 0;

            for (int lc = 0; lc < buffer4.Length; ++lc)
            {
                if (buffer4[lc] == delimter)
                {
                    if (QUOTEOPEN == 0)
                    {

                        thisList.Add(tempString);

                        tempString = string.Empty;

                    }
                    else if (QUOTEOPEN == 1)
                    {
                        //pass the comma thru since its inside of qoutes
                        tempString += ASCIIEncoding.UTF8.GetString(buffer4, lc, 1);
                    }
                }
                else if (buffer4[lc] == '\"' && QUOTEOPEN == 0)
                {
                    QUOTEOPEN = 1;
                }
                else if (buffer4[lc] == '\"' && QUOTEOPEN == 1)
                {
                    QUOTEOPEN = 0;
                }
                else if (buffer4[lc] == 0)
                {
                    //here is the trim error removerd
                    thisList.Add(tempString.TrimEnd());
                    lc = buffer4.Length;
                }
                else
                {
                    tempString += ASCIIEncoding.UTF8.GetString(buffer4, lc, 1);
                }

            }//lc

            thisList.Add(tempString);

        }

        public static void GetFileNamesInDirectory(this List<String> thisList, string path, string filter)
        {
            DirectoryInfo objdir = new DirectoryInfo(path);

            FileInfo[] FilesNames;
            FilesNames = objdir.GetFiles(filter);

            foreach (FileInfo item in FilesNames)
            {
                thisList.Add(item.ToString());
            }

        }
    }

    public static class StringExtensions
    {
        public static string SubstringSafe(this string thisString, int start, int length)
        {
            string ret = string.Empty;

            if (start > thisString.Length - 1)
            {
                ret = "";
                return ret;
            }

            ret = thisString.Substring(start, (start + length) > thisString.Length ? thisString.Length - start : length);

            return ret;
        }
    }

}
