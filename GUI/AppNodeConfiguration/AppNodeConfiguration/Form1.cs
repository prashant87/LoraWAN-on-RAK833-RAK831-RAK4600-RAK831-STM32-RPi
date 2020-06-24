﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AppNodeConfiguration
{
   
    public partial class Form1 : Form
    {   
        
        string dataIn;
        string dataOut = "NTT \r\n";
        bool isWaitingRx = false;
        string[] ProfileTable = { "NC", "NO", "Per Second", "Per Minimute", "Per Hour", "Light Sensor", "Temp Sensor", "Humid Sensor", "DHT11", "DHT22", "DS18B20" };
        int timeOut = 0;
        int theWaitingVal = 0;



        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudrate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDatabits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cBoxStopbits.Text);

                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cBoxParity.Text);
                serialPort1.Open();


                
                serialPort1.WriteLine("get cf");
                theWaitingVal = 255;
                prgBarLoad.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                await Task.Delay(2000);
                if (theWaitingVal == 0)
                {
                   
                    
                    prgBarLoad.Value = 100;

                }
                else
                {
                    prgBarLoad.SetState(2);
                    prgBarLoad.Value = 10;
                    
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPort.Items.AddRange(ports);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                prgBarLoad.Value = 0;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                dataOut = tBoxSend.Text;
                serialPort1.Write(dataOut);
                theWaitingVal = 1;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = serialPort1.ReadLine();
            dataIn = dataIn.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            this.Invoke(new EventHandler(showdata));
            this.Invoke(new EventHandler(processData));
        }
        private void processData(object sender, EventArgs e)
        {
            Console.WriteLine("Recved text:");
            Console.WriteLine("Recved text:");
            if (theWaitingVal == 1)
            {

                theWaitingVal = 0;
                Console.WriteLine("MB Mode");
                if (dataIn == "0")
                {
                    cBoxMbMode.Text = "RTU";
                }
                else
                {
                    cBoxMbMode.Text = "ASCII";
                }
              
            }

            if (theWaitingVal == 2 )
            {

                theWaitingVal = 0;
                Console.WriteLine("Baud");
                try
                {
                    cBoxMbBaud.Text = (1200 * Int16.Parse(dataIn)).ToString();
                }
                catch

                {
                    cBoxMbBaud.Text =9600.ToString();
                }
            }

            if (theWaitingVal == 3)
            {

                theWaitingVal = 0; 
                Console.WriteLine("Databits");
                if (dataIn == "0")
                {
                    cBoxDb.Text = "8";
                }
                else
                {
                    cBoxDb.Text = "7";
                }

            }

            if (theWaitingVal == 4)
            {

                theWaitingVal = 0;
                Console.WriteLine("Stopbits");
                if (dataIn == "0")
                {
                    cBoxSB.Text = "1";
                }
                else
                {
                    cBoxSB.Text = "2";
                }

            }
            if (theWaitingVal ==5)
            {

                theWaitingVal = 0; 
                Console.WriteLine("Parity");
                if (dataIn == "0")
                {
                    cBoxPari.Text = "None";
                }
                else if (dataIn == "1")
                {
                    cBoxPari.Text = "Odd";
                }
                else
                {
                    cBoxPari.Text = "Even";
                }

            }
            if (theWaitingVal == 6)
            {

                theWaitingVal = 0;
                Console.WriteLine("ID");
                tBoxSid.Text = dataIn;
            }
            if (theWaitingVal == 7)
            {

                theWaitingVal = 0;
                Console.WriteLine("join mode");
                if (dataIn == "0" )
                {
                    cBoxLrMode.Text = "OTAA";
                }
                else
                {
                    cBoxLrMode.Text = "ABP";
                }

            }
            if (theWaitingVal == 8)
            {

                theWaitingVal = 0;
                Console.WriteLine("deveui");
                tBoxDevEUI.Text = dataIn;

            }
            if (theWaitingVal == 9)
            {

                theWaitingVal = 0;
                Console.WriteLine("appeui");
                tBoxAppEUI.Text = dataIn;

            }
            if (theWaitingVal == 10)
            {

                theWaitingVal = 0;
                Console.WriteLine("appkey");
                tBoxAppKey.Text = dataIn;

            }

            if (theWaitingVal == 11)
            {

                theWaitingVal = 0;
                Console.WriteLine("adr");
                if (dataIn == "0")
                {
                    cBoxAdr.Text = "Adapted";
                }
                else
                {
                    cBoxAdr.Text = "SF"+ dataIn;
                }

            }

            if (theWaitingVal == 12)
            {

                theWaitingVal = 0;
                Console.WriteLine("fr");
                if (dataIn == "0")
                {
                    cBoxAdr.Text = "AS923";
                }
                else if (dataIn == "1")
                {
                    cBoxAdr.Text = "AU915";
                }
                else if (dataIn == "2")
                {
                    cBoxAdr.Text = "CN470";
                }
                else if (dataIn == "3")
                {
                    cBoxAdr.Text = "CN779";
                }
                else if (dataIn == "4")
                {
                    cBoxAdr.Text = "EU433";
                }
                else if (dataIn == "5")
                {
                    cBoxAdr.Text = "EU868";
                }
                else if (dataIn == "6")
                {
                    cBoxAdr.Text = "KR920";
                }
                else if (dataIn == "7")
                {
                    cBoxAdr.Text = "IN865";
                }
                else if (dataIn == "8")
                {
                    cBoxAdr.Text = "AU915";
                }

                else if (dataIn == "9")
                {
                    cBoxAdr.Text = "AU915";
                }


            }
            if (theWaitingVal == 13)
            {

                theWaitingVal = 0;
                Console.WriteLine("isConfirmed");
                if (dataIn == "0")
                {
                    cBoxAdr.Text = "No";
                }
                else
                {
                    cBoxAdr.Text = "YES";
                }

            }
            if (theWaitingVal == 14)
            {

                theWaitingVal = 0;
                Console.WriteLine("Class");
                if (dataIn == "0")
                {
                    cBoxAdr.Text = "A";
                }
                else
                {
                    cBoxAdr.Text = "C";
                }

            }
            if (theWaitingVal == 15)
            {

                theWaitingVal = 0;
                Console.WriteLine("SW1");
                cBoxSW1.Text = ProfileTable[Int16.Parse(dataIn)];

            }
            if (theWaitingVal == 16)
            {

                theWaitingVal = 0;
                Console.WriteLine("SW2");
                cBoxSW2.Text = ProfileTable[Int16.Parse(dataIn)];

            }

            if (theWaitingVal == 17)
            {

                theWaitingVal = 0;
                Console.WriteLine("cBoxADC1");
                cBoxADC1.Text = ProfileTable[Int16.Parse(dataIn)];

            }
            if (theWaitingVal == 18)
            {

                theWaitingVal = 0;
                Console.WriteLine("cBoxADC2");
                cBoxADC2.Text = ProfileTable[Int16.Parse(dataIn)];

            }
            if (theWaitingVal == 19)
            {

                theWaitingVal = 0;
                Console.WriteLine("cBoxPul");
                cBoxPul.Text = ProfileTable[Int16.Parse(dataIn)];

            }

            if (theWaitingVal == 20)
            {

                theWaitingVal = 0;
                Console.WriteLine("cBoxOneW");
                cBoxOneW.Text = ProfileTable[Int16.Parse(dataIn)];

            }


            Console.WriteLine(dataIn);

            if (theWaitingVal == 255)
            {
                Console.WriteLine("cf");
                theWaitingVal = 0;
                if (dataIn == "0")
                {
                    prgBarLoad.ForeColor = Color.FromArgb(255, 0, 0);
                    //prgBarLoad.SetState(2);
                }
                else
                {
                    prgBarLoad.ForeColor = Color.FromArgb(0, 255, 0);
                }
                
            }
            /*   if (theWaitingVal == 10)
               {

                   theWaitingVal = 0;
                   Console.WriteLine(dataIn);
                   tBoxAppKey.Text = dataIn;

               }*/


            /*        if (dataIn.IndexOf("mb") > 0)
                    {
                        if (dataIn.IndexOf("id") >0 )
                        {
                            if (dataIn[dataIn.IndexOf("id") +3] == 0)
                            {
                                cBoxMbMode.Text = "RTU";
                            }
                            else
                            {
                                cBoxMbMode.Text = "ASCII";
                            }
                        }
                    }*/
        }

        private void showdata(object sender, EventArgs e)
        {
            tBoxRecv.Text +=dataIn;
        }

        private void tBoxSend_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tBoxSend_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (serialPort1.IsOpen)
                {
                    dataOut = tBoxSend.Text;
                    serialPort1.WriteLine(dataOut);

                    tBoxSend.Text = tBoxSend.Text.Remove(0);
                }
            }
        }

        private void tBoxRecv_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            isWaitingRx = true;
            //serialPort1.Write("get mb id ");
            serialPort1.WriteLine("get mb mode");
            theWaitingVal = 1;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }
            Console.WriteLine("Tao da den day roi");
            serialPort1.WriteLine("get mb baud");
            theWaitingVal = 2;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get mb db");
            theWaitingVal = 3;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get mb stbi");
            theWaitingVal = 4;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get mb pari");
            theWaitingVal = 5;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }
            serialPort1.WriteLine("get mb id");
            theWaitingVal = 6;
           
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }
            serialPort1.WriteLine("get lr mode");
            theWaitingVal = 7;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get lr deveui");
            theWaitingVal = 8;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get lr appeui");
            theWaitingVal = 9;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }
            serialPort1.WriteLine("get lr appkey");
            theWaitingVal = 10;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get lr adr");
            theWaitingVal = 11;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }


            serialPort1.WriteLine("get lr fr");
            theWaitingVal = 12;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }



            serialPort1.WriteLine("get lr cf");
            theWaitingVal = 13;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }




            serialPort1.WriteLine("get lr class");
            theWaitingVal = 14;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }


            serialPort1.WriteLine("get p_pf 0");
            theWaitingVal = 15;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get p_pf 1");
            theWaitingVal = 16;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }

            serialPort1.WriteLine("get p_pf 2");
            theWaitingVal = 17;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }


            serialPort1.WriteLine("get p_pf 3");
            theWaitingVal = 18;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }
            serialPort1.WriteLine("get p_pf 4");
            theWaitingVal = 19;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }
            serialPort1.WriteLine("get p_pf 5");
            theWaitingVal = 20;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }








            serialPort1.WriteLine("get cf");
            theWaitingVal = 255;
            /***/
            timeOut = 0;
            while (theWaitingVal != 0)
            {
                await Task.Delay(200);
                timeOut++;
                if (timeOut > 10) break;
            }





            Console.WriteLine("Tao da den day roi");


            /*  this.Invoke(new EventHandler(waitRx));*/

        }
        private void waitRx(object sender, EventArgs e)
        {
            
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button5_Click(object sender, EventArgs e)
        {


            if (cBoxMbMode.Text == "RTU")
            {
                serialPort1.WriteLine("set mb mode 0");
            }
            else
            {
                serialPort1.WriteLine("set mb mode 1");
            }
            await Task.Delay(200);

            serialPort1.Write("set mb baud ");
            serialPort1.WriteLine((Convert.ToInt32(cBoxMbBaud.Text) / 1200).ToString());
            await Task.Delay(200);

            if (cBoxMbMode.Text == "8")
            {
                serialPort1.WriteLine("set mb db 0");
            }
            else
            {
                serialPort1.WriteLine("set mb db 1");
            }
            await Task.Delay(200);


            if (cBoxSB.Text == "1")
            {
                serialPort1.WriteLine("set mb stbi 0");
            }
            else
            {
                serialPort1.WriteLine("set mb stbi 1");
            }
            await Task.Delay(200);

            if (cBoxPari.Text == "None")
            {
                serialPort1.WriteLine("set mb pari 0");
            }
            else if (cBoxPari.Text == "Odd")
            {
                serialPort1.WriteLine("set mb pari 1");
            }
            else
            {
                serialPort1.WriteLine("set mb db 2");
            }
            await Task.Delay(200);

            serialPort1.Write("set mb id ");
            serialPort1.WriteLine(tBoxSid.Text);
            await Task.Delay(200);

            serialPort1.Write("set lr deveui ");
            serialPort1.WriteLine(tBoxDevEUI.Text);
            await Task.Delay(200);
            serialPort1.Write("set lr appeui ");
            serialPort1.WriteLine(tBoxAppEUI.Text);
            await Task.Delay(200);
            serialPort1.Write("set lr appkey ");
            serialPort1.WriteLine(tBoxAppKey.Text);
            await Task.Delay(200);

            serialPort1.Write("set lr adr ");
            if (cBoxAdr.Text == "Adapted")
            {
                serialPort1.WriteLine("0");
            }
            else if (cBoxAdr.Text == "SF7" )
            {
                serialPort1.WriteLine("7");
            }
            else if (cBoxAdr.Text == "SF8")
            {
                serialPort1.WriteLine("8");
            }
            else if (cBoxAdr.Text == "SF9")
            {
                serialPort1.WriteLine("9");
            }
            else if (cBoxAdr.Text == "SF10")
            {
                serialPort1.WriteLine("10");
            }
            else if (cBoxAdr.Text == "SF11")
            {
                serialPort1.WriteLine("11");
            }
            else if (cBoxAdr.Text == "SF12")
            {
                serialPort1.WriteLine("12");
            }
            await Task.Delay(200);

            serialPort1.Write("set lr adr ");
            if (cBoxFreq.Text == "AS923")
            {
                serialPort1.WriteLine("0");
            }
            else if (cBoxFreq.Text == "AU915")
            {
                serialPort1.WriteLine("1");
            }
            else if (cBoxFreq.Text == "CN470")
            {
                serialPort1.WriteLine("2");
            }
            else if (cBoxFreq.Text == "CN779")
            {
                serialPort1.WriteLine("3");
            }
            else if (cBoxFreq.Text == "EU433")
            {
                serialPort1.WriteLine("4");
            }
            else if (cBoxFreq.Text == "EU868")
            {
                serialPort1.WriteLine("5");
            }
            else if (cBoxFreq.Text == "KR920")
            {
                serialPort1.WriteLine("6");
            }
            else if (cBoxFreq.Text == "IN865")
            {
                serialPort1.WriteLine("7");
            }
            else if (cBoxFreq.Text == "AU915")
            {
                serialPort1.WriteLine("8");
            }
            await Task.Delay(200);

            if (cBoxCfm.Text == "No")
            {
                serialPort1.WriteLine("set lr cf 0");


            }
            else
            {
                serialPort1.WriteLine("set lr cf 1");
            }

            await Task.Delay(200);
            if (cBoxClass.Text == "A")
            {
                serialPort1.WriteLine("set lr class 0");

            }
            else
            {
                serialPort1.WriteLine("set lr class 2");
            }


            await Task.Delay(200);



            /* if (tBoxSid.Text == "")
             {
                 serialPort1.WriteLine("set mb db 0");
             }
             else
             {
                 serialPort1.WriteLine("set mb db 1");
             }*/





        }

        private void cBoxPari_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void prgBarLoad_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cBoxOneW_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
namespace QuantumConcepts.Common.Forms.UI.Controls
{
    public class ProgressBarEx : ProgressBar
    {
        public ProgressBarEx()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = null;
            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
            double scaleFactor = (((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum));

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);

            rec.Width = (int)((rec.Width * scaleFactor) - 4);
            rec.Height -= 4;
            brush = new LinearGradientBrush(rec, this.ForeColor, this.BackColor, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
        }
    }
}