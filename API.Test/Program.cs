using System;
using System.Windows.Forms;

namespace API.Test
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Эта строка включит визуальные стили для элементов управления.
            Application.EnableVisualStyles();
            
            // Настройка для использования совместимого рендеринга текста
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Запуск главной формы приложения.
            Application.Run(new frmTest());
        }
    }

    public partial class frmTest : Form
    {
    public frmTest()
    {
        InitializeComponent();
        }
    }

}
