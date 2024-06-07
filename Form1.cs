using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO.Ports;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static PROFILUS.Entry;
using static PROFILUS.IShowEntries;
using static PROFILUS.WProtocolParser;
using System.Collections.Generic;



namespace PROFILUS
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            
            InitializeComponent();
            BtnSettings.Image = Image.FromFile("C:\\SJAW\\Visual Studio\\saves\\PROFILUS\\pictures and images\\button.ico");
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false; // предотвращает множетсвенный доступ к элементу из нескольких потоков. Отключает проверку конкурентного доступа у разных поток. Мы попытались работать с свойствами обьекта не из потока создателя.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            string[] ports = SerialPort.GetPortNames(); // Получаем список имен последовательных портов.
            cBoxComPort.Items.AddRange(ports); // отображение всех портов в выподающем списке cBoxComPort
            
            // Настройки поумолчанию:
            //Parser = new WProtocolParser(this);
            
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            DateTime timNow = DateTime.Now;
            Int32 ticks = (Int32)timNow.Ticks;
            WProtocolParser Parser = new WProtocolParser();

            int sinusoida(double t, double A, double W)
            {

                double X = A * Math.Sin(W * t);
                return Convert.ToInt32(X);
            }
            string Znachenie = Convert.ToString(sinusoida(Convert.ToDouble(ticks) / 10000000, 100, 0.5))
                + "," + Convert.ToString(sinusoida(Convert.ToDouble(ticks) / 10000000, 80, 0.5))
                + "," + Convert.ToString(sinusoida(Convert.ToDouble(ticks) / 10000000, 120, 0.5));

            List<string> funcia (string Data)
            {
                List<string> entries = new List<string>();
                for (int i = 0; i < Znachenie.Length; i++)
                {
                    Parser.WorkComPort(Znachenie, entries);
                }
                return entries;
            }

        }


        private void btnConnect_Click(object sender, EventArgs e)// Обработчик событий при нажатии конпки "Connect".
                                                                 // C Помощью конструкции (try...catch..finally) обрабатываем исключения для перехвата ошибок. Вначале выполняются все инструкции в блоке try.
                                                                 //Если в этом блоке не возникло исключений, то после его выполнения начинает выполняться блок finally. И затем конструкция try..catch..finally завершает свою работу.
        {
            try
            {   // Ниже задаём параметры Порта: PortName -Имя последовательного порта; BaudRate -Скорость передачи в бодах; DataBits - Количество битов, чтобыпредставлять один символ данных;
                // StopBits -Шаблон битов, который указывает на конец символа; Parity -Четность; 
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text); // Convert.'тип данных'(конвертируемый объект) - Конвертация переменных в другой тип данных.
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopBits.Text); //Enum.Parse: преобразование строки в перечисление. Смотреть описание - нихрена не понятно.
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParityBits.Text);
                serialPort1.Open();
                LabelConnectStatus.Text = "Статус: Подключено.";
                btnConnect.Enabled = false;
                btnStop.Enabled = true;
                timer.Enabled = true;
                if (!serialPort1.IsOpen | !timer.Enabled)
                {
                    System.Windows.Forms.Application.Exit();
                }

            }
            // Если в блоке try возникает исключение, то порядок выполнения останавливается, и среда CLR начинает искать блок catch, который может обработать данное исключение.
            // Если нужный блок catch найден, то он выполняется, и после его завершения выполняется блок finally.
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelConnectStatus.Text = "Статус: Ошибка подключения.";

                btnConnect.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e) // Обработчик событий при нажатии конпки "Stop".
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                LabelConnectStatus.BackColor = Color.White;
                LabelConnectStatus.Text = "Статус: Неподключено.";
                btnConnect.Enabled = true;
                btnStop.Enabled = false;
            }
        }
        private void roundedButton1_Click(object sender, EventArgs e)
        {

        }

    }
}
