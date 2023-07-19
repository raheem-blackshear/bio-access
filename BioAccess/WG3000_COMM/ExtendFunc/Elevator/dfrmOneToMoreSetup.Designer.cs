namespace WG3000_COMM.ExtendFunc.Elevator
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmOneToMoreSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(dfrmOneToMoreSetup));
            this.btnCancel = new ImageButton();
            this.btnOK = new ImageButton();
            this.radioButton0 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.textBox0 = new TextBox();
            this.Label1 = new Label();
            this.textBox1 = new TextBox();
            this.label2 = new Label();
            this.textBox3 = new TextBox();
            this.textBox2 = new TextBox();
            this.textBox5 = new TextBox();
            this.textBox4 = new TextBox();
            this.numericUpDown20 = new NumericUpDown();
            this.label141 = new Label();
            this.numericUpDown21 = new NumericUpDown();
            this.label142 = new Label();
            this.numericUpDown20.BeginInit();
            this.numericUpDown21.BeginInit();
            base.SuspendLayout();
            this.btnCancel.BackColor = Color.Transparent;
            this.btnCancel.BackgroundImage = Resources.pMain_button_normal;
            manager.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.BackColor = Color.Transparent;
            this.btnOK.BackgroundImage = Resources.pMain_button_normal;
            manager.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            manager.ApplyResources(this.radioButton0, "radioButton0");
            this.radioButton0.BackColor = Color.Transparent;
            this.radioButton0.Checked = true;
            this.radioButton0.ForeColor = Color.White;
            this.radioButton0.Name = "radioButton0";
            this.radioButton0.TabStop = true;
            this.radioButton0.UseVisualStyleBackColor = false;
            this.radioButton0.CheckedChanged += new EventHandler(this.radioButton0_CheckedChanged);
            manager.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.BackColor = Color.Transparent;
            this.radioButton2.ForeColor = Color.White;
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = false;
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton0_CheckedChanged);
            manager.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.BackColor = Color.Transparent;
            this.radioButton1.ForeColor = Color.White;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton0_CheckedChanged);
            manager.ApplyResources(this.textBox0, "textBox0");
            this.textBox0.Name = "textBox0";
            this.textBox0.ReadOnly = true;
            this.Label1.BackColor = Color.Transparent;
            this.Label1.ForeColor = Color.White;
            manager.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            manager.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.label2.BackColor = Color.Transparent;
            this.label2.ForeColor = Color.White;
            manager.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            manager.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            manager.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            manager.ApplyResources(this.textBox5, "textBox5");
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            manager.ApplyResources(this.textBox4, "textBox4");
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            manager.ApplyResources(this.numericUpDown20, "numericUpDown20");
            int[] bits = new int[4];
            bits[0] = 0x19;
            this.numericUpDown20.Maximum = new decimal(bits);
            this.numericUpDown20.Name = "numericUpDown20";
            this.numericUpDown20.ReadOnly = true;
            int[] numArray2 = new int[4];
            numArray2[0] = 5;
            this.numericUpDown20.Value = new decimal(numArray2);
            manager.ApplyResources(this.label141, "label141");
            this.label141.ForeColor = Color.White;
            this.label141.Name = "label141";
            this.numericUpDown21.DecimalPlaces = 1;
            int[] numArray3 = new int[4];
            numArray3[0] = 1;
            numArray3[3] = 0x10000;
            this.numericUpDown21.Increment = new decimal(numArray3);
            manager.ApplyResources(this.numericUpDown21, "numericUpDown21");
            int[] numArray4 = new int[4];
            numArray4[0] = 0x19;
            this.numericUpDown21.Maximum = new decimal(numArray4);
            int[] numArray5 = new int[4];
            numArray5[0] = 3;
            numArray5[3] = 0x10000;
            this.numericUpDown21.Minimum = new decimal(numArray5);
            this.numericUpDown21.Name = "numericUpDown21";
            this.numericUpDown21.ReadOnly = true;
            int[] numArray6 = new int[4];
            numArray6[0] = 4;
            numArray6[3] = 0x10000;
            this.numericUpDown21.Value = new decimal(numArray6);
            manager.ApplyResources(this.label142, "label142");
            this.label142.ForeColor = Color.White;
            this.label142.Name = "label142";
            manager.ApplyResources(this, "$this");
            base.ControlBox = false;
            base.Controls.Add(this.numericUpDown20);
            base.Controls.Add(this.label141);
            base.Controls.Add(this.numericUpDown21);
            base.Controls.Add(this.label142);
            base.Controls.Add(this.textBox5);
            base.Controls.Add(this.textBox4);
            base.Controls.Add(this.textBox3);
            base.Controls.Add(this.textBox2);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBox0);
            base.Controls.Add(this.Label1);
            base.Controls.Add(this.radioButton0);
            base.Controls.Add(this.radioButton2);
            base.Controls.Add(this.radioButton1);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Name = "dfrmOneToMoreSetup";
            base.Load += new EventHandler(this.dfrmOneToMoreSetup_Load);
            base.KeyDown += new KeyEventHandler(this.dfrmOneToMoreSetup_KeyDown);
            this.numericUpDown20.EndInit();
            this.numericUpDown21.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        #endregion

        private ImageButton btnCancel;
        private ImageButton btnOK;
        internal Label Label1;
        private Label label141;
        private Label label142;
        internal Label label2;
        public NumericUpDown numericUpDown20;
        public NumericUpDown numericUpDown21;
        public RadioButton radioButton0;
        public RadioButton radioButton1;
        public RadioButton radioButton2;
        internal TextBox textBox0;
        internal TextBox textBox1;
        internal TextBox textBox2;
        internal TextBox textBox3;
        internal TextBox textBox4;
        internal TextBox textBox5;
    }
}

