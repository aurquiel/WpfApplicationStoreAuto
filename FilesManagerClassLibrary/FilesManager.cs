using LogLibraryClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FilesManagerClassLibrary
{
    public class FilesManager
    {
        public static List<string> ListOfArticlesGlobalDiscountPerArticle = new List<string>();
        public static List<string> ListOfArticlesDiscountSectorExcludingArticle = new List<string>();
        public static List<Tuple<string,string>> ListOfArticleCommaSectorUpdateSectorByArticle = new List<Tuple<string, string>>();
        public static List<Tuple<string,string>> ListUpdateArticleCommaDiscountUpdateArticlesDiscounts = new List<Tuple<string, string>>();
        public static List<Tuple<string,string>> ListUpdateArticleCommaGenderUpdateArticlesGenders = new List<Tuple<string, string>>();
        public static List<Tuple<string,string>> ListUpdateArticleCommaCategoryUpdateArticlesCategories = new List<Tuple<string, string>>();
        public static List<string> ListOfArticlesSetIva = new List<string>();
        public static List<string> ListOfArticlesSetExento = new List<string>();

        public static string GetPathFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"text|*.txt";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }

        public static Tuple<bool, string> GetArticlesFromFileArticleByLine(string pathFile, List<string> list)
        {
            try
            {
                list.Clear();
                string[] lines = File.ReadAllLines(pathFile);

                if (lines.Length <= 0)
                {
                    return new Tuple<bool, string>(false, "Error archivo de texto vacio.");
                }

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        list.Add(line.Trim());
                    }
                }

                return new Tuple<bool, string>(true, "Archivos cargados con exito.");
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                list.Clear();
                return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
            }
        }

        public static Tuple<bool, string> GetArticlesCommaField(string pathFile, List<Tuple<string, string>> listToFill)
        {
            try
            {
                listToFill.Clear();
                string[] lines = File.ReadAllLines(pathFile);

                if(lines.Length <= 0)
                {
                    return new Tuple<bool, string>(false, "Error archivo de texto vacio.");
                }

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string trimLine = line.Trim();
                        string[] splitData = trimLine.Split(',');
                        if(splitData.Length != 2)
                        {
                            listToFill.Clear();
                            return new Tuple<bool, string>(false, "Error de formato en el articulo: " + splitData[0] + ".");
                        }
                        else
                        {
                            listToFill.Add(new Tuple<string, string>(splitData[0], splitData[1]));
                        }
                    }
                }

                return new Tuple<bool, string>(true, "Archivos cargados con exito.");
            }
            catch(Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                listToFill.Clear();
                return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
            }
        }

        
        public static string GetArticlesQuotesAndCommas(List<string> articles)
        {
            string formatted = string.Empty;
            foreach(string article in articles)
            {
                formatted += "'" + article + "',";
            }
            return formatted.TrimEnd(',');
        }

    }
}
