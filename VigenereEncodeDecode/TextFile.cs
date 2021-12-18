using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Windows;

namespace VigenereEncodeDecode
{
    public class TextFile
    {
        public static Dictionary<string, int> codeDia = new Dictionary<string, int>(){
        {"UTF-8", 65001},
        {"Windows-1251", 1251},
        {"ISO-8859-5", 28595},
        {"ISO-8859-1", 28591},
        {"KOI8-R", 20866},
        {"KOI8-U", 21866}};

        static string openPath = "";
        public static string Vigenere(string inText, string key, bool encrypting)
        {
            string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            if (string.IsNullOrEmpty(inText) || string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Поле текста/ключа для шифровки пустое!");
                return null;
            }
            key = key.ToLower();
            inText = inText.ToLower();
            foreach (var c in key)
            {
                if (!alphabet.Contains(c))
                {
                    MessageBox.Show("В ключе имеются недопустимые символы!");
                    return inText;
                }
            }
            int keyCount = 0;
            var outText = "";
            var q = alphabet.Length;
            for (int i = 0; i < inText.Length; i++)
            {
                var letterIndex = alphabet.IndexOf(inText[i]);
                var codeIndex = alphabet.IndexOf(key[keyCount % key.Length]); // тут ведётся отсчет символа ключа
                if (letterIndex < 0)
                {
                    outText += inText[i].ToString();
                }
                else
                {
                    outText += alphabet[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                    keyCount++; // и если символ является буквой русского алфавита, то увеличиваем счетчик
                }
            }
            return outText;
        }
        public static string GetFilePath(string text)
        {
            if (text != "")
            {
                var mb = MessageBox.Show("В поле ввода данных уже присутствует текст. При открытии файла он будет удален. Продолжить?", "Текст уже имеется", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.No)
                {
                    return text;
                }
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files(*.txt;*.docx)|*.txt;*.docx";
            if (openFileDialog.ShowDialog() != true)
                MessageBox.Show("Ошибка! Вы не выбрали файла!");
            openPath = openFileDialog.FileName;
            return null;
        }
        public static void SaveFile(string outText)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt|Word|*.docx";
            saveFileDialog.FileName = "Text";
            if (saveFileDialog.ShowDialog() != true)
                MessageBox.Show("Ошибка! Вы не указали, куда сохранять файл!!!");
            string path = saveFileDialog.FileName;

            if (Path.GetExtension(path) == ".txt")
            {
                SaveTxtFile(path, outText);
                MessageBox.Show("Файл успешно сохранён");
            }
            else if (Path.GetExtension(path) == ".docx")
            {
                SaveDocxFile(path, outText);
                MessageBox.Show("Файл успешно сохранён");
            }
        }
        public static string ReadFile(string item, string text)
        {
            if (Path.GetExtension(openPath) == ".txt")
            {
                return ReadTxtFile(openPath, item);
            }
            else if (Path.GetExtension(openPath) == ".docx")
            {
                return ReadDocxFile(openPath);
            }
            return text;

        }
        public static string ReadTxtFile(string path, string item)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(codeDia[item])))
            {
                return sr.ReadToEnd().ToLower();
            }
        }
        public static string ReadDocxFile(string path)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                Body body = wordDoc.MainDocumentPart.Document.Body;
                return body.InnerText.ToLower();
            }
        }
        public static void SaveTxtFile(string path, string outText)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding(65001)))
            {
                sw.Write(outText);
            }
        }
        public static void SaveDocxFile(string path, string outText)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(path, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text(outText));
            }
        }
    }
}
