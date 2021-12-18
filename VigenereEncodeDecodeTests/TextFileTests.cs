using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using VigenereEncodeDecode;

namespace VigenereEncodeDecodeTests
{
    [TestClass]
    public class Vigenere
    {
        [TestMethod]
        public void Vigenere_Encode()
        {
            //arrange
            string text = "Это простая строка!!!";
            string key = "Ключ";
            string expected = "зюм жыъпйкк пйыъич!!!";

            //act
            string actual = TextFile.Vigenere(text, key, true);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Vigenere_Decode()
        {
            //arrange
            string text = "зюм жыъпйкк пйыъич!!!";
            string key = "Ключ";
            string expected = "это простая строка!!!";

            //act
            string actual = TextFile.Vigenere(text, key, false);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Vigenere_NullText_NullReturned() // Здесь окно выбрасывается, и его надо нажать. Умесно ли здесь проводить такой тест?
        {
            //arrange
            string text = null;
            string key = "Ключ";

            //act
            string actual = TextFile.Vigenere(text, key, true);

            //assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Vigenere_NullKey_NullReturned() // здесь то же самое
        {
            //arrange
            string text = "Это простая строка";
            string key = null;

            //act
            string actual = TextFile.Vigenere(text, key, true);

            //assert
            Assert.IsNull(actual);
        }
    }

    [TestClass]
    public class Read
    {
        [TestMethod]
        public void ReadTxtFile_Test()
        {
            //arrange
            string expected = "это простая строка";
            string path = ".txt";
            //act
            File.WriteAllText(path, expected);
            string actual = TextFile.ReadTxtFile(path, "UTF-8");
            File.Delete(path);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadDocxFile_Text()
        {
            //arrange
            string expected = "это простая строка";
            string path = ".txt";
            //act
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(path, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text(expected));
            }
            string actual = TextFile.ReadDocxFile(path);
            File.Delete(path);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class Save
    {
        [TestMethod]
        public void SaveTxtFile_Test()
        {
            //arrange
            string expected = "это простая строка";
            string path = ".txt";
            //act
            
            TextFile.SaveTxtFile(path, expected);
            string actual = File.ReadAllText(path);
            File.Delete(path);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SaveDocxFile_Text()
        {
            //arrange
            string expected = "это простая строка";
            string path = ".txt";
            //act
            TextFile.SaveDocxFile(path, expected);
            string actual;
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                Body body = wordDoc.MainDocumentPart.Document.Body;
                actual = body.InnerText.ToLower();
            }
            File.Delete(path);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
