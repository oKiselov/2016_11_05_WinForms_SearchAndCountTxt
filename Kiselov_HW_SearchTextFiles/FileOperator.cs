using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kiselov_HW_SearchTextFiles
{
    class FileOperator
    {
        /// <summary>
        /// Target word for search 
        /// </summary>
        string strSearchedWord;

        /// <summary>
        /// Word which will change searched words 
        /// </summary>
        string strNewWord;

        /// <summary>
        /// counter of found .txt files 
        /// </summary>
        int iFoundFilesCounter;
        public int Files
        { get { return iFoundFilesCounter; } }

        /// <summary>
        /// counter of changed words 
        /// </summary>
        int iChangedWordsCounter;
        public int Words { get { return iChangedWordsCounter; } }

        /// <summary>
        /// collection of edited files
        /// </summary>
        List<string> lstChanged;
        public List<string> ListOfEditedFiles
        { get { return lstChanged; } }

        /// <summary>
        /// Constructor with parameters 
        /// </summary>
        /// <param name="SearchedWord"></param>
        /// <param name="ChangedWord"></param>
        public FileOperator(string SearchedWord, string ChangedWord)
        {
            strSearchedWord = SearchedWord;
            strNewWord = ChangedWord;
            iFoundFilesCounter = 0;
            iChangedWordsCounter = 0; 
            lstChanged=new List<string>();
        }

        /// <summary>
        /// Method which returns pathes to all *.txt files in current directory
        /// return value - collection of strings - pathes 
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public List<string> GetPathes(string strPath)
        {
            List<string> listPathes = new List<string>(Directory.EnumerateFiles(strPath, "*.txt", SearchOption.AllDirectories));
            IncreaseFilesAmount(listPathes.Count);
            return listPathes; 
        }

        /// <summary>
        /// Method sets an amount of files *.txt 
        /// which were found in current directory 
        /// </summary>
        /// <param name="iCount"></param>
        private void IncreaseFilesAmount(int iCount)
        {
            iFoundFilesCounter += iCount; 
        }

        /// <summary>
        /// Method sets an amount of found words in *.txt files 
        /// </summary>
        /// <param name="iCount"></param>
        private void IncreaseWordsAmount(int iCount)
        {
            iChangedWordsCounter += iCount; 
        }

        /// <summary>
        /// Method returns amount (number) of searched words in current row  
        /// </summary>
        /// <param name="strBuf"></param>
        /// <returns></returns>
        private int GetCountWordsInFile(string strBuf)
        {
            // amount of words in current file  
            int iRet = 0;
            // position for searching 
            int iPos = 0;
            // loop of search 
            while (iPos != -1)
            {
                // gets first index of searched word in all text 
                iPos = strBuf.IndexOf(strSearchedWord, iPos);
                // if searched word was found 
                if (iPos != -1)
                {
                    iPos += strSearchedWord.Length; 
                    iRet++;
                }
            }
            return iRet;
        }

        /// <summary>
        /// Method receives the path to current file 
        /// Reads this file into collection of lines of text 
        /// Returns collection of rows  
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private List<string> ReadFile(string strPath)
        {
            // collection of rows 
            List<string> lstLines = new List<string>();

            // Counter for comparsion 
            int iCounterOfChangedWords = Words; 

            // reading process 
            using (StreamReader sr = new StreamReader(strPath))
            {
                string strTmp = sr.ReadLine();
                while (strTmp != null)
                {
                    // gets amount of searched words in current row of current file 
                    int localNumWords = GetCountWordsInFile(strTmp); 
                    // sets amount of found words into field value of class FileOperator 
                    IncreaseWordsAmount(localNumWords);
                    // fills new row - string - into collection 
                    lstLines.Add(strTmp);
                    // continue of reading process 
                    strTmp = sr.ReadLine(); 
                }
            }

            // if two compareble values are not the same 
            // method will add new path to collection of edited files
            if (iCounterOfChangedWords < Words)
            {
                lstChanged.Add(strPath);
            }
            return lstLines; 
        }

        /// <summary>
        /// Method writes edited collection of rows into file 
        /// </summary>
        /// <param name="lstLines"></param>
        /// <param name="strPath"></param>
        private void WriteFIle(List<string> lstLines, string strPath)
        {
            using (StreamWriter sw = new StreamWriter(strPath, false, Encoding.Unicode))
            {
                foreach (string row in lstLines)
                {
                    sw.WriteLine(row);
                }
            }
        }

        /// <summary>
        /// Method replaces all target-strings in common string 
        /// </summary>
        /// <param name="strBuf"></param>
        /// <returns></returns>
        private string ReplaceWords(string strBuf)
        {
            return strBuf.Replace(strSearchedWord, strNewWord);
        }

        /// <summary>
        /// Method receives collecton of pathes to *.txt files 
        /// Searchs all target-words in each file , replaces them 
        /// Writes edited rows into files with pathes from collection 
        /// 
        /// </summary>
        /// <param name="strPath"></param>
        public void ChangeWordsInFile(string strPath)
        {
            // gets collection of rows - for edition 
            List<string> lstRows = ReadFile(strPath);

            // loop moves on collection of rows which were read from current file 
            for (int i=0; i<lstRows.Count; i++)
            {
                lstRows[i]=ReplaceWords(lstRows[i]);
            }

            // writes collection of rows into current file form collection of pathes 
            WriteFIle(lstRows, strPath);
        }


    }
}
