using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Написать программу, 
 * которая просит ввести путь поиска текстовых файлов, 
 * слово, которое необходимо найти 
 * и слово на которое необходимо заменить. Осуществляет поиск и замену. 
 * 
 * В окне должна выводиться информация после работы( или во время работы по желанию ):
 * 1) Сколько файлов было найдено
 * 2) Сколько слов было найдено и заменено
 * 3) Пути к файлам, в которых была произведена замена.
 * Обязательно проверки и комментарии.
 */

namespace Kiselov_HW_SearchTextFiles
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
