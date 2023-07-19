namespace WG3000_COMM.Core
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Properties;

    public class InfoRow
    {
        public int category = 100;
        public string desc = "";
        public string detail = "";
        public string information = "";
        public string MjRecStr = "";
        private static DataGridViewCellStyle styleGreen = new DataGridViewCellStyle();
        private static DataGridViewCellStyle styleOrange = new DataGridViewCellStyle();
        private static DataGridViewCellStyle styleRed = null;
        private static DataGridViewCellStyle styleYellow = new DataGridViewCellStyle();

        public static object getImage(string stringValue, ref DataGridViewRow dgvr)
        {
            object obj2;
            if (styleRed == null)
            {
                loadStyle();
            }
            switch (stringValue)
            {
                case "0":
                case "2":
                    obj2 = Resources.Rec1Pass;
                    dgvr.DefaultCellStyle = styleGreen;
                    return obj2;

                case "1":
                case "3":
                    obj2 = Resources.Rec2NoPass;
                    dgvr.DefaultCellStyle = styleOrange;
                    return obj2;

                case "4":
                case "6":
                    obj2 = Resources.Rec3Warn;
                    dgvr.DefaultCellStyle = styleYellow;
                    return obj2;

                case "5":
                    obj2 = Resources.Rec4Falt;
                    dgvr.DefaultCellStyle = styleRed;
                    return obj2;

                case "101":
                    return Resources.Rec4Falt;

                case "501":
                    obj2 = Resources.Rec3Warn;
                    dgvr.DefaultCellStyle = styleYellow;
                    return obj2;
            }
            return Resources.eventlogInfo;
        }

        private static void loadStyle()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = Color.Red;
            style.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 0x86);
            style.ForeColor = Color.White;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            styleRed = style;
            style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = Color.Green;
            style.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 0x86);
            style.ForeColor = Color.White;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            styleGreen = style;
            style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = Color.Yellow;
            style.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 0x86);
            style.ForeColor = Color.Blue;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            styleYellow = style;
            style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = Color.Orange;
            style.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Pixel, 0x86);
            style.ForeColor = Color.Blue;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            styleOrange = style;
        }
    }
}

