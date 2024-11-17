
namespace maptool
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.checkGuide = new System.Windows.Forms.CheckBox();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkMsg = new System.Windows.Forms.CheckBox();
            this.checkPictureBoxOnly = new System.Windows.Forms.CheckBox();
            this.btnEditLine = new System.Windows.Forms.Button();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.btnEditPolygon = new System.Windows.Forms.Button();
            this.btnEditCancel = new System.Windows.Forms.Button();
            this.btnEditString = new System.Windows.Forms.Button();
            this.textString = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboFontStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboSymbolAngle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.checkTransParent = new System.Windows.Forms.CheckBox();
            this.btnBrushColorFore = new System.Windows.Forms.Button();
            this.btnFontColor = new System.Windows.Forms.Button();
            this.btnBrushColorBack = new System.Windows.Forms.Button();
            this.btnLineColor = new System.Windows.Forms.Button();
            this.comboLineType = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.comboPolyHatchType = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.comboPolyBrushType = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.comboStringAngle = new System.Windows.Forms.ComboBox();
            this.comboStringType = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboStringSize = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboLineWidth = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnEditSymbol = new System.Windows.Forms.Button();
            this.btnEditFreeline = new System.Windows.Forms.Button();
            this.btnAddressSearch = new System.Windows.Forms.Button();
            this.btnGetInfo = new System.Windows.Forms.Button();
            this.bntDataList = new System.Windows.Forms.Button();
            this.btnMapLayer = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnPointSearch = new System.Windows.Forms.Button();
            this.comboScale = new System.Windows.Forms.ComboBox();
            this.textVal = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.Black;
            this.listBox1.ForeColor = System.Drawing.Color.LawnGreen;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 14;
            this.listBox1.Location = new System.Drawing.Point(389, 799);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(144, 74);
            this.listBox1.TabIndex = 1;
            this.listBox1.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(335, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1444, 944);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(1095, 13);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(77, 28);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "拡大";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(1178, 12);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(77, 28);
            this.btnDown.TabIndex = 10;
            this.btnDown.Text = "縮小";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1702, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 28);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // checkGuide
            // 
            this.checkGuide.AutoSize = true;
            this.checkGuide.Location = new System.Drawing.Point(10, 941);
            this.checkGuide.Name = "checkGuide";
            this.checkGuide.Size = new System.Drawing.Size(88, 18);
            this.checkGuide.TabIndex = 26;
            this.checkGuide.Text = "checkGuide";
            this.checkGuide.UseVisualStyleBackColor = true;
            this.checkGuide.Visible = false;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Location = new System.Drawing.Point(1301, 12);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(95, 35);
            this.btnSaveImage.TabIndex = 28;
            this.btnSaveImage.Text = "Save Image";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Visible = false;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // checkMsg
            // 
            this.checkMsg.AutoSize = true;
            this.checkMsg.Location = new System.Drawing.Point(444, 845);
            this.checkMsg.Name = "checkMsg";
            this.checkMsg.Size = new System.Drawing.Size(89, 18);
            this.checkMsg.TabIndex = 29;
            this.checkMsg.Text = "メッセージ出力";
            this.checkMsg.UseVisualStyleBackColor = true;
            this.checkMsg.Visible = false;
            // 
            // checkPictureBoxOnly
            // 
            this.checkPictureBoxOnly.AutoSize = true;
            this.checkPictureBoxOnly.Location = new System.Drawing.Point(1405, 21);
            this.checkPictureBoxOnly.Name = "checkPictureBoxOnly";
            this.checkPictureBoxOnly.Size = new System.Drawing.Size(103, 18);
            this.checkPictureBoxOnly.TabIndex = 31;
            this.checkPictureBoxOnly.Text = "PictureBoxのみ";
            this.checkPictureBoxOnly.UseVisualStyleBackColor = true;
            this.checkPictureBoxOnly.Visible = false;
            // 
            // btnEditLine
            // 
            this.btnEditLine.Location = new System.Drawing.Point(6, 35);
            this.btnEditLine.Name = "btnEditLine";
            this.btnEditLine.Size = new System.Drawing.Size(85, 40);
            this.btnEditLine.TabIndex = 42;
            this.btnEditLine.Text = "線";
            this.btnEditLine.UseVisualStyleBackColor = true;
            this.btnEditLine.Click += new System.EventHandler(this.btnEditLine_Click);
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Location = new System.Drawing.Point(6, 833);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(85, 40);
            this.btnSaveEdit.TabIndex = 43;
            this.btnSaveEdit.Text = "保存";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            // 
            // btnEditPolygon
            // 
            this.btnEditPolygon.Location = new System.Drawing.Point(7, 157);
            this.btnEditPolygon.Name = "btnEditPolygon";
            this.btnEditPolygon.Size = new System.Drawing.Size(85, 40);
            this.btnEditPolygon.TabIndex = 44;
            this.btnEditPolygon.Text = "ポリゴン";
            this.btnEditPolygon.UseVisualStyleBackColor = true;
            this.btnEditPolygon.Click += new System.EventHandler(this.btnEditPolygon_Click);
            // 
            // btnEditCancel
            // 
            this.btnEditCancel.Location = new System.Drawing.Point(211, 833);
            this.btnEditCancel.Name = "btnEditCancel";
            this.btnEditCancel.Size = new System.Drawing.Size(85, 40);
            this.btnEditCancel.TabIndex = 45;
            this.btnEditCancel.Text = "取り消し";
            this.btnEditCancel.UseVisualStyleBackColor = true;
            this.btnEditCancel.Click += new System.EventHandler(this.btnEditCancel_Click);
            // 
            // btnEditString
            // 
            this.btnEditString.Location = new System.Drawing.Point(10, 519);
            this.btnEditString.Name = "btnEditString";
            this.btnEditString.Size = new System.Drawing.Size(85, 40);
            this.btnEditString.TabIndex = 46;
            this.btnEditString.Text = "文字列";
            this.btnEditString.UseVisualStyleBackColor = true;
            this.btnEditString.Click += new System.EventHandler(this.btnEditString_Click);
            // 
            // textString
            // 
            this.textString.Location = new System.Drawing.Point(108, 562);
            this.textString.Name = "textString";
            this.textString.Size = new System.Drawing.Size(199, 21);
            this.textString.TabIndex = 47;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboFontStyle);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.comboSymbolAngle);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.pictureBox2);
            this.groupBox4.Controls.Add(this.checkTransParent);
            this.groupBox4.Controls.Add(this.btnBrushColorFore);
            this.groupBox4.Controls.Add(this.btnFontColor);
            this.groupBox4.Controls.Add(this.btnBrushColorBack);
            this.groupBox4.Controls.Add(this.btnLineColor);
            this.groupBox4.Controls.Add(this.comboLineType);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.comboPolyHatchType);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.comboPolyBrushType);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.btnDelete);
            this.groupBox4.Controls.Add(this.btnSelect);
            this.groupBox4.Controls.Add(this.comboStringAngle);
            this.groupBox4.Controls.Add(this.comboStringType);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.comboStringSize);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.comboLineWidth);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.btnEditSymbol);
            this.groupBox4.Controls.Add(this.btnEditFreeline);
            this.groupBox4.Controls.Add(this.btnEditLine);
            this.groupBox4.Controls.Add(this.btnSaveEdit);
            this.groupBox4.Controls.Add(this.textString);
            this.groupBox4.Controls.Add(this.btnEditPolygon);
            this.groupBox4.Controls.Add(this.btnEditString);
            this.groupBox4.Controls.Add(this.btnEditCancel);
            this.groupBox4.Location = new System.Drawing.Point(10, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(313, 920);
            this.groupBox4.TabIndex = 58;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "作図パネル";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // comboFontStyle
            // 
            this.comboFontStyle.FormattingEnabled = true;
            this.comboFontStyle.Location = new System.Drawing.Point(205, 707);
            this.comboFontStyle.Name = "comboFontStyle";
            this.comboFontStyle.Size = new System.Drawing.Size(99, 22);
            this.comboFontStyle.TabIndex = 98;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 705);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 14);
            this.label2.TabIndex = 97;
            this.label2.Text = "スタイル";
            // 
            // comboSymbolAngle
            // 
            this.comboSymbolAngle.FormattingEnabled = true;
            this.comboSymbolAngle.Location = new System.Drawing.Point(205, 450);
            this.comboSymbolAngle.Name = "comboSymbolAngle";
            this.comboSymbolAngle.Size = new System.Drawing.Size(99, 22);
            this.comboSymbolAngle.TabIndex = 96;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 458);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 14);
            this.label1.TabIndex = 95;
            this.label1.Text = "シンボル角度";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(134, 391);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(48, 48);
            this.pictureBox2.TabIndex = 94;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // checkTransParent
            // 
            this.checkTransParent.AutoSize = true;
            this.checkTransParent.Location = new System.Drawing.Point(6, 274);
            this.checkTransParent.Name = "checkTransParent";
            this.checkTransParent.Size = new System.Drawing.Size(48, 18);
            this.checkTransParent.TabIndex = 93;
            this.checkTransParent.Text = "透過";
            this.checkTransParent.UseVisualStyleBackColor = true;
            // 
            // btnBrushColorFore
            // 
            this.btnBrushColorFore.Location = new System.Drawing.Point(155, 304);
            this.btnBrushColorFore.Name = "btnBrushColorFore";
            this.btnBrushColorFore.Size = new System.Drawing.Size(95, 26);
            this.btnBrushColorFore.TabIndex = 89;
            this.btnBrushColorFore.UseVisualStyleBackColor = true;
            this.btnBrushColorFore.Click += new System.EventHandler(this.btnBrushColorFore_Click);
            // 
            // btnFontColor
            // 
            this.btnFontColor.Location = new System.Drawing.Point(258, 594);
            this.btnFontColor.Name = "btnFontColor";
            this.btnFontColor.Size = new System.Drawing.Size(44, 26);
            this.btnFontColor.TabIndex = 91;
            this.btnFontColor.UseVisualStyleBackColor = true;
            this.btnFontColor.Click += new System.EventHandler(this.btnFontColor_Click);
            // 
            // btnBrushColorBack
            // 
            this.btnBrushColorBack.Location = new System.Drawing.Point(155, 336);
            this.btnBrushColorBack.Name = "btnBrushColorBack";
            this.btnBrushColorBack.Size = new System.Drawing.Size(94, 26);
            this.btnBrushColorBack.TabIndex = 90;
            this.btnBrushColorBack.UseVisualStyleBackColor = true;
            this.btnBrushColorBack.Click += new System.EventHandler(this.btnBrushColorBack_Click);
            // 
            // btnLineColor
            // 
            this.btnLineColor.Location = new System.Drawing.Point(208, 63);
            this.btnLineColor.Name = "btnLineColor";
            this.btnLineColor.Size = new System.Drawing.Size(94, 26);
            this.btnLineColor.TabIndex = 88;
            this.btnLineColor.UseVisualStyleBackColor = true;
            this.btnLineColor.Click += new System.EventHandler(this.btnLineColor_Click);
            // 
            // comboLineType
            // 
            this.comboLineType.FormattingEnabled = true;
            this.comboLineType.Location = new System.Drawing.Point(208, 100);
            this.comboLineType.Name = "comboLineType";
            this.comboLineType.Size = new System.Drawing.Size(94, 22);
            this.comboLineType.TabIndex = 87;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(108, 95);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(29, 14);
            this.label20.TabIndex = 86;
            this.label20.Text = "線種";
            // 
            // comboPolyHatchType
            // 
            this.comboPolyHatchType.FormattingEnabled = true;
            this.comboPolyHatchType.Location = new System.Drawing.Point(155, 246);
            this.comboPolyHatchType.Name = "comboPolyHatchType";
            this.comboPolyHatchType.Size = new System.Drawing.Size(94, 22);
            this.comboPolyHatchType.TabIndex = 85;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 246);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(55, 14);
            this.label19.TabIndex = 84;
            this.label19.Text = "ハッチタイプ";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 335);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(93, 14);
            this.label18.TabIndex = 82;
            this.label18.Text = "ポリゴン色（背面）";
            // 
            // comboPolyBrushType
            // 
            this.comboPolyBrushType.FormattingEnabled = true;
            this.comboPolyBrushType.Location = new System.Drawing.Point(155, 212);
            this.comboPolyBrushType.Name = "comboPolyBrushType";
            this.comboPolyBrushType.Size = new System.Drawing.Size(94, 22);
            this.comboPolyBrushType.TabIndex = 81;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 215);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 14);
            this.label17.TabIndex = 80;
            this.label17.Text = "ポリゴン塗タイプ";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(97, 833);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 40);
            this.btnDelete.TabIndex = 79;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(9, 787);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(85, 40);
            this.btnSelect.TabIndex = 78;
            this.btnSelect.Text = "選択";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // comboStringAngle
            // 
            this.comboStringAngle.FormattingEnabled = true;
            this.comboStringAngle.Location = new System.Drawing.Point(205, 679);
            this.comboStringAngle.Name = "comboStringAngle";
            this.comboStringAngle.Size = new System.Drawing.Size(99, 22);
            this.comboStringAngle.TabIndex = 77;
            // 
            // comboStringType
            // 
            this.comboStringType.FormattingEnabled = true;
            this.comboStringType.Location = new System.Drawing.Point(205, 653);
            this.comboStringType.Name = "comboStringType";
            this.comboStringType.Size = new System.Drawing.Size(99, 22);
            this.comboStringType.TabIndex = 76;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 677);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(51, 14);
            this.label16.TabIndex = 75;
            this.label16.Text = "文字角度";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 650);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 14);
            this.label15.TabIndex = 74;
            this.label15.Text = "縦・横";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 569);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 14);
            this.label14.TabIndex = 73;
            this.label14.Text = "文字列";
            // 
            // comboStringSize
            // 
            this.comboStringSize.FormattingEnabled = true;
            this.comboStringSize.Location = new System.Drawing.Point(205, 627);
            this.comboStringSize.Name = "comboStringSize";
            this.comboStringSize.Size = new System.Drawing.Size(99, 22);
            this.comboStringSize.TabIndex = 72;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 623);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 14);
            this.label13.TabIndex = 71;
            this.label13.Text = "文字サイズ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 596);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 14);
            this.label12.TabIndex = 70;
            this.label12.Text = "文字色";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 304);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 14);
            this.label11.TabIndex = 67;
            this.label11.Text = "ポリゴン色（前面）";
            // 
            // comboLineWidth
            // 
            this.comboLineWidth.FormattingEnabled = true;
            this.comboLineWidth.Location = new System.Drawing.Point(208, 35);
            this.comboLineWidth.Name = "comboLineWidth";
            this.comboLineWidth.Size = new System.Drawing.Size(94, 22);
            this.comboLineWidth.TabIndex = 64;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(108, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 14);
            this.label9.TabIndex = 63;
            this.label9.Text = "線色";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(108, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 14);
            this.label8.TabIndex = 61;
            this.label8.Text = "線幅";
            // 
            // btnEditSymbol
            // 
            this.btnEditSymbol.Location = new System.Drawing.Point(6, 391);
            this.btnEditSymbol.Name = "btnEditSymbol";
            this.btnEditSymbol.Size = new System.Drawing.Size(85, 40);
            this.btnEditSymbol.TabIndex = 49;
            this.btnEditSymbol.Text = "シンボル";
            this.btnEditSymbol.UseVisualStyleBackColor = true;
            this.btnEditSymbol.Click += new System.EventHandler(this.btnEditSymbol_Click);
            // 
            // btnEditFreeline
            // 
            this.btnEditFreeline.Location = new System.Drawing.Point(7, 82);
            this.btnEditFreeline.Name = "btnEditFreeline";
            this.btnEditFreeline.Size = new System.Drawing.Size(85, 40);
            this.btnEditFreeline.TabIndex = 48;
            this.btnEditFreeline.Text = "自由線";
            this.btnEditFreeline.UseVisualStyleBackColor = true;
            this.btnEditFreeline.Click += new System.EventHandler(this.btnEditFreeline_Click);
            // 
            // btnAddressSearch
            // 
            this.btnAddressSearch.Location = new System.Drawing.Point(437, 4);
            this.btnAddressSearch.Name = "btnAddressSearch";
            this.btnAddressSearch.Size = new System.Drawing.Size(96, 35);
            this.btnAddressSearch.TabIndex = 59;
            this.btnAddressSearch.Text = "住所検索";
            this.btnAddressSearch.UseVisualStyleBackColor = true;
            this.btnAddressSearch.Click += new System.EventHandler(this.btnAddressSearch_Click);
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.Location = new System.Drawing.Point(539, 4);
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(96, 35);
            this.btnGetInfo.TabIndex = 60;
            this.btnGetInfo.Text = "地点情報";
            this.btnGetInfo.UseVisualStyleBackColor = true;
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // bntDataList
            // 
            this.bntDataList.Location = new System.Drawing.Point(335, 4);
            this.bntDataList.Name = "bntDataList";
            this.bntDataList.Size = new System.Drawing.Size(96, 35);
            this.bntDataList.TabIndex = 61;
            this.bntDataList.Text = "登録データ一覧";
            this.bntDataList.UseVisualStyleBackColor = true;
            this.bntDataList.Click += new System.EventHandler(this.bntDataList_Click);
            // 
            // btnMapLayer
            // 
            this.btnMapLayer.Location = new System.Drawing.Point(641, 4);
            this.btnMapLayer.Name = "btnMapLayer";
            this.btnMapLayer.Size = new System.Drawing.Size(96, 35);
            this.btnMapLayer.TabIndex = 62;
            this.btnMapLayer.Text = "地図レイヤ合成";
            this.btnMapLayer.UseVisualStyleBackColor = true;
            this.btnMapLayer.Click += new System.EventHandler(this.btnMapLayer_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(221, 965);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 28);
            this.button1.TabIndex = 63;
            this.button1.Text = "▼";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(261, 965);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 28);
            this.button2.TabIndex = 64;
            this.button2.Text = "▲";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnPointSearch
            // 
            this.btnPointSearch.Location = new System.Drawing.Point(743, 4);
            this.btnPointSearch.Name = "btnPointSearch";
            this.btnPointSearch.Size = new System.Drawing.Size(96, 35);
            this.btnPointSearch.TabIndex = 65;
            this.btnPointSearch.Text = "座標検索";
            this.btnPointSearch.UseVisualStyleBackColor = true;
            this.btnPointSearch.Click += new System.EventHandler(this.btnPointSearch_Click);
            // 
            // comboScale
            // 
            this.comboScale.FormattingEnabled = true;
            this.comboScale.Location = new System.Drawing.Point(955, 16);
            this.comboScale.Name = "comboScale";
            this.comboScale.Size = new System.Drawing.Size(81, 22);
            this.comboScale.TabIndex = 66;
            this.comboScale.SelectedIndexChanged += new System.EventHandler(this.comboScale_SelectedIndexChanged);
            // 
            // textVal
            // 
            this.textVal.Location = new System.Drawing.Point(1050, 16);
            this.textVal.Name = "textVal";
            this.textVal.Size = new System.Drawing.Size(39, 21);
            this.textVal.TabIndex = 19;
            this.textVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1782, 1001);
            this.Controls.Add(this.comboScale);
            this.Controls.Add(this.btnPointSearch);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMapLayer);
            this.Controls.Add(this.bntDataList);
            this.Controls.Add(this.btnGetInfo);
            this.Controls.Add(this.btnAddressSearch);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.checkGuide);
            this.Controls.Add(this.checkPictureBoxOnly);
            this.Controls.Add(this.checkMsg);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.textVal);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.listBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "Form1";
            this.Text = "地図手書き入力ツール";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox checkGuide;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkMsg;
        private System.Windows.Forms.CheckBox checkPictureBoxOnly;
        private System.Windows.Forms.Button btnEditLine;
        private System.Windows.Forms.Button btnSaveEdit;
        private System.Windows.Forms.Button btnEditPolygon;
        private System.Windows.Forms.Button btnEditCancel;
        private System.Windows.Forms.Button btnEditString;
        private System.Windows.Forms.TextBox textString;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnEditFreeline;
        private System.Windows.Forms.Button btnEditSymbol;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboLineWidth;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboStringSize;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboStringAngle;
        private System.Windows.Forms.ComboBox comboStringType;
        private System.Windows.Forms.Button btnAddressSearch;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnGetInfo;
        private System.Windows.Forms.Button bntDataList;
        private System.Windows.Forms.ComboBox comboPolyBrushType;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox comboPolyHatchType;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox comboLineType;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnMapLayer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnBrushColorBack;
        private System.Windows.Forms.Button btnLineColor;
        private System.Windows.Forms.Button btnBrushColorFore;
        private System.Windows.Forms.Button btnFontColor;
        private System.Windows.Forms.CheckBox checkTransParent;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox comboSymbolAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboFontStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPointSearch;
        private System.Windows.Forms.ComboBox comboScale;
        private System.Windows.Forms.TextBox textVal;
    }
}

