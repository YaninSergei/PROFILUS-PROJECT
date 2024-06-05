using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace PROFILUS
{
    internal class RoundedButton : Button
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr RoundButton(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            IntPtr ptr = RoundButton(0, 0, Width, Height, 25, 25); 
            Region = Region.FromHrgn(ptr);
        }
    }
    
}
 