namespace WG3000_COMM.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Globalization;
    using System.Windows.Forms;

    internal class DGVPrinter : IDisposable
    {
        protected Form _Owner;
        protected double _PrintPreviewZoom = 1.0;
        private RowHeightSetting _rowheight;
        private StringAlignment cellalignment;
        private StringFormat cellformat;
        private StringFormatFlags cellformatflags;
        private float colheaderheight;
        private IList colstoprint;
        private Dictionary<string, DataGridViewCellStyle> colstyles = new Dictionary<string, DataGridViewCellStyle>();
        private List<float> colwidths;
        private List<float> colwidthsoverride = new List<float>();
        private int CurrentPage;
        private int currentpageset;
        private DataGridView dgv;
        private string docName;
        private string footer;
        private Color footercolor;
        private Font footerfont;
        private StringFormat footerformat;
        private float footerHeight;
        private float footerspacing;
        private int fromPage;
        private StringAlignment headercellalignment;
        private StringFormat headercellformat;
        private StringFormatFlags headercellformatflags;
        private float headerHeight;
        private int lastrowprinted = -1;
        private Pen lines;
        private bool overridefooterformat;
        private bool overridepagenumberformat;
        private bool overridesubtitleformat;
        private bool overridetitleformat;
        private int pageHeight;
        private bool pageno = true;
        private Color pagenocolor;
        private Font pagenofont;
        private StringFormat pagenumberformat;
        private float pagenumberHeight;
        private bool pagenumberonseparateline;
        private bool pagenumberontop;
        private string pageseparator = " of ";
        private IList<PageDef> pagesets;
        private string pagetext = "Page ";
        private int pageWidth;
        private string parttext = " - Part ";
        private bool porportionalcolumns;
        private Icon ppvIcon;
        private bool printColumnHeaders = true;
        private PrintDialogSettingsClass printDialogSettings = new PrintDialogSettingsClass();
        private PrintDocument printDoc = new PrintDocument();
        private string printerName;
        private bool printFooter = true;
        private bool printHeader = true;
        private PrintRange printRange;
        private int printWidth;
        private Dictionary<string, float> publicwidthoverrides = new Dictionary<string, float>();
        private float rowheaderwidth;
        private List<float> rowheights;
        private IList rowstoprint;
        private bool showtotalpagenumber;
        private SolidBrush SolidBrush1;
        private string subtitle;
        private Color subtitlecolor;
        private Font subtitlefont;
        private StringFormat subtitleformat;
        private Alignment tablealignment;
        private string title;
        private Color titlecolor;
        private Font titlefont;
        private StringFormat titleformat;
        private int toPage = -1;
        private int totalpages;

        public DGVPrinter()
        {
            this.printDoc.PrintPage += new PrintPageEventHandler(this.printDoc_PrintPage);
            this.printDoc.BeginPrint += new PrintEventHandler(this.printDoc_BeginPrint);
            this.PrintMargins = new Margins(60, 60, 40, 40);
            this.pagenofont = new Font("Tahoma", 8f, FontStyle.Regular, GraphicsUnit.Point);
            this.pagenocolor = Color.Black;
            this.titlefont = new Font("Tahoma", 18f, FontStyle.Bold, GraphicsUnit.Point);
            this.titlecolor = Color.Black;
            this.subtitlefont = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point);
            this.subtitlecolor = Color.Black;
            this.footerfont = new Font("Tahoma", 10f, FontStyle.Bold, GraphicsUnit.Point);
            this.footercolor = Color.Black;
            this.buildstringformat(ref this.titleformat, null, StringAlignment.Center, StringAlignment.Center, StringFormatFlags.NoClip | StringFormatFlags.LineLimit | StringFormatFlags.NoWrap, StringTrimming.Word);
            this.buildstringformat(ref this.subtitleformat, null, StringAlignment.Center, StringAlignment.Center, StringFormatFlags.NoClip | StringFormatFlags.LineLimit | StringFormatFlags.NoWrap, StringTrimming.Word);
            this.buildstringformat(ref this.footerformat, null, StringAlignment.Center, StringAlignment.Center, StringFormatFlags.NoClip | StringFormatFlags.LineLimit | StringFormatFlags.NoWrap, StringTrimming.Word);
            this.buildstringformat(ref this.pagenumberformat, null, StringAlignment.Far, StringAlignment.Center, StringFormatFlags.NoClip | StringFormatFlags.LineLimit | StringFormatFlags.NoWrap, StringTrimming.Word);
            this.headercellformat = null;
            this.cellformat = null;
            this.Owner = null;
            this.PrintPreviewZoom = 1.0;
            this.headercellalignment = StringAlignment.Near;
            this.headercellformatflags = StringFormatFlags.NoClip | StringFormatFlags.LineLimit;
            this.cellalignment = StringAlignment.Near;
            this.cellformatflags = StringFormatFlags.NoClip | StringFormatFlags.LineLimit;
        }

        private void AdjustPageSets(Graphics g, PageDef pageset)
        {
            int num;
            float num4;
            float rowheaderwidth = this.rowheaderwidth;
            float num3 = 0f;
            for (num = 0; num < pageset.colwidthsoverride.Count; num++)
            {
                if (pageset.colwidthsoverride[num] >= 0f)
                {
                    rowheaderwidth += pageset.colwidthsoverride[num];
                }
            }
            for (num = 0; num < pageset.colwidths.Count; num++)
            {
                if (pageset.colwidthsoverride[num] < 0f)
                {
                    num3 += pageset.colwidths[num];
                }
            }
            if (this.porportionalcolumns && (0f < num3))
            {
                num4 = (this.printWidth - rowheaderwidth) / num3;
            }
            else
            {
                num4 = 1f;
            }
            pageset.coltotalwidth = this.rowheaderwidth;
            for (num = 0; num < pageset.colwidths.Count; num++)
            {
                if (pageset.colwidthsoverride[num] >= 0f)
                {
                    pageset.colwidths[num] = pageset.colwidthsoverride[num];
                }
                else
                {
                    pageset.colwidths[num] *= num4;
                }
                pageset.coltotalwidth += pageset.colwidths[num];
            }
            if (Alignment.Left == this.tablealignment)
            {
                pageset.margins.Right = (this.pageWidth - pageset.margins.Left) - ((int) pageset.coltotalwidth);
                if (0 > pageset.margins.Right)
                {
                    pageset.margins.Right = 0;
                }
            }
            else if (Alignment.Right == this.tablealignment)
            {
                pageset.margins.Left = (this.pageWidth - pageset.margins.Right) - ((int) pageset.coltotalwidth);
                if (0 > pageset.margins.Left)
                {
                    pageset.margins.Left = 0;
                }
            }
            else if (Alignment.Center == this.tablealignment)
            {
                pageset.margins.Left = (this.pageWidth - ((int) pageset.coltotalwidth)) / 2;
                if (0 > pageset.margins.Left)
                {
                    pageset.margins.Left = 0;
                }
                pageset.margins.Right = pageset.margins.Left;
            }
        }

        private void buildstringformat(ref StringFormat format, DataGridViewCellStyle controlstyle, StringAlignment alignment, StringAlignment linealignment, StringFormatFlags flags, StringTrimming trim)
        {
            if (format == null)
            {
                format = new StringFormat();
            }
            format.Alignment = alignment;
            format.LineAlignment = linealignment;
            format.FormatFlags = flags;
            format.Trimming = trim;
            if (controlstyle != null)
            {
                DataGridViewContentAlignment alignment2 = controlstyle.Alignment;
                if (alignment2.ToString().Contains("Center"))
                {
                    format.Alignment = StringAlignment.Center;
                }
                else if (alignment2.ToString().Contains("Left"))
                {
                    format.Alignment = StringAlignment.Near;
                }
                else if (alignment2.ToString().Contains("Right"))
                {
                    format.Alignment = StringAlignment.Far;
                }
                if (alignment2.ToString().Contains("Top"))
                {
                    format.LineAlignment = StringAlignment.Near;
                }
                else if (alignment2.ToString().Contains("Middle"))
                {
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (alignment2.ToString().Contains("Bottom"))
                {
                    format.LineAlignment = StringAlignment.Far;
                }
            }
        }

        private bool DetermineHasMorePages()
        {
            this.currentpageset++;
            return (this.currentpageset < this.pagesets.Count);
        }

        public DialogResult DisplayPrintDialog()
        {
            using (PrintDialog dialog = new PrintDialog())
            {
                dialog.UseEXDialog = this.printDialogSettings.UseEXDialog;
                dialog.AllowSelection = this.printDialogSettings.AllowSelection;
                dialog.AllowSomePages = this.printDialogSettings.AllowSomePages;
                dialog.AllowCurrentPage = this.printDialogSettings.AllowCurrentPage;
                dialog.AllowPrintToFile = this.printDialogSettings.AllowPrintToFile;
                dialog.ShowHelp = this.printDialogSettings.ShowHelp;
                dialog.ShowNetwork = this.printDialogSettings.ShowNetwork;
                dialog.Document = this.printDoc;
                if (!string.IsNullOrEmpty(this.printerName))
                {
                    this.printDoc.PrinterSettings.PrinterName = this.printerName;
                }
                this.printDoc.DefaultPageSettings.Landscape = dialog.PrinterSettings.DefaultPageSettings.Landscape;
                this.printDoc.DefaultPageSettings.PaperSize = new PaperSize(dialog.PrinterSettings.DefaultPageSettings.PaperSize.PaperName, dialog.PrinterSettings.DefaultPageSettings.PaperSize.Width, dialog.PrinterSettings.DefaultPageSettings.PaperSize.Height);
                return dialog.ShowDialog();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.SolidBrush1 != null)
                {
                    this.SolidBrush1.Dispose();
                }
                if (this.lines != null)
                {
                    this.lines.Dispose();
                }
                if (this.headercellformat != null)
                {
                    this.headercellformat.Dispose();
                }
                if (this.cellformat != null)
                {
                    this.cellformat.Dispose();
                }
                if (this.footerfont != null)
                {
                    this.footerfont.Dispose();
                }
                if (this.pagenofont != null)
                {
                    this.pagenofont.Dispose();
                }
                if (this.printDoc != null)
                {
                    this.printDoc.Dispose();
                }
                if (this.subtitlefont != null)
                {
                    this.subtitlefont.Dispose();
                }
                if (this.titlefont != null)
                {
                    this.titlefont.Dispose();
                }
            }
        }

        private void DrawImageCell(Graphics g, DataGridViewImageCell imagecell, RectangleF rectf)
        {
            Image image = (Image) imagecell.Value;
            Rectangle srcRect = new Rectangle();
            int num = 0;
            int num2 = 0;
            if ((DataGridViewImageCellLayout.Normal == imagecell.ImageLayout) || (imagecell.ImageLayout == DataGridViewImageCellLayout.NotSet))
            {
                num = image.Width - ((int) rectf.Width);
                num2 = image.Height - ((int) rectf.Height);
                if (0 > num)
                {
                    rectf.Width = srcRect.Width = image.Width;
                }
                else
                {
                    srcRect.Width = (int) rectf.Width;
                }
                if (0 > num2)
                {
                    rectf.Height = srcRect.Height = image.Height;
                }
                else
                {
                    srcRect.Height = (int) rectf.Height;
                }
            }
            else if (DataGridViewImageCellLayout.Stretch == imagecell.ImageLayout)
            {
                srcRect.Width = image.Width;
                srcRect.Height = image.Height;
                num = 0;
                num2 = 0;
            }
            else
            {
                float num5;
                srcRect.Width = image.Width;
                srcRect.Height = image.Height;
                float num3 = rectf.Height / ((float) srcRect.Height);
                float num4 = rectf.Width / ((float) srcRect.Width);
                if (num3 > num4)
                {
                    num5 = num4;
                    num = 0;
                    num2 = (int) ((srcRect.Height * num5) - rectf.Height);
                }
                else
                {
                    num5 = num3;
                    num2 = 0;
                    num = (int) ((srcRect.Width * num5) - rectf.Width);
                }
                rectf.Width = srcRect.Width * num5;
                rectf.Height = srcRect.Height * num5;
            }
            switch (imagecell.InheritedStyle.Alignment)
            {
                case DataGridViewContentAlignment.NotSet:
                    if (0 <= num2)
                    {
                        srcRect.Y = num2 / 2;
                    }
                    else
                    {
                        rectf.Y -= num2 / 2;
                    }
                    if (0 > num)
                    {
                        rectf.X -= num / 2;
                    }
                    else
                    {
                        srcRect.X = num / 2;
                    }
                    break;

                case DataGridViewContentAlignment.TopLeft:
                    srcRect.Y = 0;
                    srcRect.X = 0;
                    break;

                case DataGridViewContentAlignment.TopCenter:
                    srcRect.Y = 0;
                    if (0 <= num)
                    {
                        srcRect.X = num / 2;
                        break;
                    }
                    rectf.X -= num / 2;
                    break;

                case DataGridViewContentAlignment.TopRight:
                    srcRect.Y = 0;
                    if (0 <= num)
                    {
                        srcRect.X = num;
                        break;
                    }
                    rectf.X -= num;
                    break;

                case DataGridViewContentAlignment.MiddleLeft:
                    if (0 > num2)
                    {
                        rectf.Y -= num2 / 2;
                    }
                    else
                    {
                        srcRect.Y = num2 / 2;
                    }
                    srcRect.X = 0;
                    break;

                case DataGridViewContentAlignment.MiddleCenter:
                    if (0 > num2)
                    {
                        rectf.Y -= num2 / 2;
                    }
                    else
                    {
                        srcRect.Y = num2 / 2;
                    }
                    if (0 > num)
                    {
                        rectf.X -= num / 2;
                    }
                    else
                    {
                        srcRect.X = num / 2;
                    }
                    break;

                case DataGridViewContentAlignment.MiddleRight:
                    if (0 > num2)
                    {
                        rectf.Y -= num2 / 2;
                    }
                    else
                    {
                        srcRect.Y = num2 / 2;
                    }
                    if (0 > num)
                    {
                        rectf.X -= num;
                    }
                    else
                    {
                        srcRect.X = num;
                    }
                    break;

                case DataGridViewContentAlignment.BottomLeft:
                    if (0 > num2)
                    {
                        rectf.Y -= num2;
                    }
                    else
                    {
                        srcRect.Y = num2;
                    }
                    srcRect.X = 0;
                    break;

                case DataGridViewContentAlignment.BottomCenter:
                    if (0 > num2)
                    {
                        rectf.Y -= num2;
                    }
                    else
                    {
                        srcRect.Y = num2;
                    }
                    if (0 > num)
                    {
                        rectf.X -= num / 2;
                    }
                    else
                    {
                        srcRect.X = num / 2;
                    }
                    break;

                case DataGridViewContentAlignment.BottomRight:
                    if (0 > num2)
                    {
                        rectf.Y -= num2;
                    }
                    else
                    {
                        srcRect.Y = num2;
                    }
                    if (0 > num)
                    {
                        rectf.X -= num;
                    }
                    else
                    {
                        srcRect.X = num;
                    }
                    break;
            }
            g.DrawImage(image, rectf, srcRect, GraphicsUnit.Pixel);
        }

        public bool EmbeddedPrint(DataGridView dgv, Graphics g, Rectangle area)
        {
            if ((dgv == null) || (g == null))
            {
                throw new Exception("Null Parameter passed to DGVPrinter.");
            }
            this.dgv = dgv;
            Margins printMargins = this.PrintMargins;
            this.PrintMargins.Top = area.Top;
            this.PrintMargins.Bottom = 0;
            this.PrintMargins.Left = area.Left;
            this.PrintMargins.Right = 0;
            this.pageHeight = area.Height + area.Top;
            this.printWidth = area.Width;
            this.pageWidth = area.Width + area.Left;
            this.fromPage = 0;
            this.toPage = 0x7fffffff;
            this.PrintHeader = false;
            this.PrintFooter = false;
            if (this.cellformat == null)
            {
                this.buildstringformat(ref this.cellformat, dgv.DefaultCellStyle, this.cellalignment, StringAlignment.Near, this.cellformatflags, StringTrimming.Word);
            }
            this.rowstoprint = new List<object>(dgv.Rows.Count);
            foreach (DataGridViewRow row in (IEnumerable) dgv.Rows)
            {
                if (row.Visible)
                {
                    this.rowstoprint.Add(row);
                }
            }
            this.colstoprint = new List<object>(dgv.Columns.Count);
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    this.colstoprint.Add(column);
                }
            }
            SortedList list = new SortedList(this.colstoprint.Count);
            foreach (DataGridViewColumn column2 in this.colstoprint)
            {
                list.Add(column2.DisplayIndex, column2);
            }
            this.colstoprint.Clear();
            foreach (object obj2 in list.Values)
            {
                this.colstoprint.Add(obj2);
            }
            foreach (DataGridViewColumn column3 in this.colstoprint)
            {
                if (this.publicwidthoverrides.ContainsKey(column3.Name))
                {
                    this.colwidthsoverride.Add(this.publicwidthoverrides[column3.Name]);
                }
                else
                {
                    this.colwidthsoverride.Add(-1f);
                }
            }
            this.measureprintarea(g);
            this.totalpages = this.TotalPages();
            this.currentpageset = 0;
            this.lastrowprinted = -1;
            this.CurrentPage = 0;
            return this.PrintPage(g);
        }

        public StringFormat GetCellFormat(DataGridView grid)
        {
            if ((grid != null) && (this.cellformat == null))
            {
                this.buildstringformat(ref this.cellformat, grid.Rows[0].Cells[0].InheritedStyle, this.cellalignment, StringAlignment.Near, this.cellformatflags, StringTrimming.Word);
            }
            if (this.cellformat == null)
            {
                this.cellformat = new StringFormat(this.cellformatflags);
            }
            return this.cellformat;
        }

        public StringFormat GetHeaderCellFormat(DataGridView grid)
        {
            if ((grid != null) && (this.headercellformat == null))
            {
                this.buildstringformat(ref this.headercellformat, grid.Columns[0].HeaderCell.InheritedStyle, this.headercellalignment, StringAlignment.Near, this.headercellformatflags, StringTrimming.Word);
            }
            if (this.headercellformat == null)
            {
                this.headercellformat = new StringFormat(this.headercellformatflags);
            }
            return this.headercellformat;
        }

        private void measureprintarea(Graphics g)
        {
            int num;
            DataGridViewColumn column;
            this.rowheights = new List<float>(this.rowstoprint.Count);
            this.colwidths = new List<float>(this.colstoprint.Count);
            this.headerHeight = 0f;
            this.footerHeight = 0f;
            Font font = this.dgv.ColumnHeadersDefaultCellStyle.Font;
            if (font == null)
            {
                font = this.dgv.DefaultCellStyle.Font;
            }
            for (num = 0; num < this.colstoprint.Count; num++)
            {
                column = (DataGridViewColumn) this.colstoprint[num];
                float width = 0f;
                if (0f < this.colwidthsoverride[num])
                {
                    width = this.colwidthsoverride[num];
                }
                else
                {
                    width = this.printWidth;
                }
                SizeF ef = g.MeasureString(column.HeaderText, font, new SizeF(width, 2.147484E+09f), this.headercellformat);
                this.colwidths.Add(ef.Width);
                this.colheaderheight = (this.colheaderheight < ef.Height) ? ef.Height : this.colheaderheight;
            }
            if (this.pageno)
            {
                this.pagenumberHeight = g.MeasureString("Page", this.pagenofont, this.printWidth, this.pagenumberformat).Height;
            }
            if (this.PrintHeader)
            {
                if (this.pagenumberontop && !this.pagenumberonseparateline)
                {
                    this.headerHeight += this.pagenumberHeight;
                }
                if (!string.IsNullOrEmpty(this.title))
                {
                    this.headerHeight += g.MeasureString(this.title, this.titlefont, this.printWidth, this.titleformat).Height;
                }
                if (!string.IsNullOrEmpty(this.subtitle))
                {
                    this.headerHeight += g.MeasureString(this.subtitle, this.subtitlefont, this.printWidth, this.subtitleformat).Height;
                }
                this.headerHeight += this.colheaderheight;
            }
            if (this.PrintFooter)
            {
                if (!string.IsNullOrEmpty(this.footer))
                {
                    this.footerHeight += g.MeasureString(this.footer, this.footerfont, this.printWidth, this.footerformat).Height;
                }
                if (!this.pagenumberontop && this.pagenumberonseparateline)
                {
                    this.footerHeight += this.pagenumberHeight;
                }
                this.footerHeight += this.footerspacing;
            }
            num = 0;
            while (num < this.rowstoprint.Count)
            {
                DataGridViewRow row = (DataGridViewRow) this.rowstoprint[num];
                this.rowheights.Add(0f);
                if (this.dgv.RowHeadersVisible)
                {
                    SizeF ef2 = g.MeasureString(row.HeaderCell.EditedFormattedValue.ToString(), font);
                    this.rowheaderwidth = (this.rowheaderwidth < ef2.Width) ? ef2.Width : this.rowheaderwidth;
                }
                for (int i = 0; i < this.colstoprint.Count; i++)
                {
                    SizeF size;
                    column = (DataGridViewColumn) this.colstoprint[i];
                    string text = row.Cells[column.Index].EditedFormattedValue.ToString();
                    StringFormat cellformat = null;
                    DataGridViewCellStyle controlstyle = null;
                    if (this.ColumnStyles.ContainsKey(column.Name))
                    {
                        controlstyle = this.colstyles[column.Name];
                        this.buildstringformat(ref cellformat, controlstyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
                    }
                    else if (column.HasDefaultCellStyle || row.Cells[column.Index].HasStyle)
                    {
                        controlstyle = row.Cells[column.Index].InheritedStyle;
                        this.buildstringformat(ref cellformat, controlstyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
                    }
                    else
                    {
                        cellformat = this.cellformat;
                        controlstyle = this.dgv.DefaultCellStyle;
                    }
                    if (RowHeightSetting.CellHeight == this.RowHeight)
                    {
                        size = (SizeF) row.Cells[column.Index].Size;
                    }
                    else
                    {
                        size = g.MeasureString(text, controlstyle.Font);
                    }
                    if ((0f < this.colwidthsoverride[i]) || (size.Width > this.printWidth))
                    {
                        int num4;
                        int num5;
                        if (0f < this.colwidthsoverride[i])
                        {
                            this.colwidths[i] = this.colwidthsoverride[i];
                        }
                        else if (size.Width > this.printWidth)
                        {
                            this.colwidths[i] = this.printWidth;
                        }
                        float height = g.MeasureString(text, controlstyle.Font, new SizeF(this.colwidths[i], 2.147484E+09f), cellformat, out num4, out num5).Height;
                        this.rowheights[num] = (this.rowheights[num] < height) ? height : this.rowheights[num];
                    }
                    else
                    {
                        this.colwidths[i] = (this.colwidths[i] < size.Width) ? size.Width : this.colwidths[i];
                        this.rowheights[num] = (this.rowheights[num] < size.Height) ? size.Height : this.rowheights[num];
                    }
                }
                num++;
            }
            this.pagesets = new List<PageDef>();
            this.pagesets.Add(new PageDef(this.PrintMargins, this.colstoprint.Count));
            int num7 = 0;
            this.pagesets[num7].coltotalwidth = this.rowheaderwidth;
            for (num = 0; num < this.colstoprint.Count; num++)
            {
                float num8 = (this.colwidthsoverride[num] >= 0f) ? this.colwidthsoverride[num] : this.colwidths[num];
                if ((this.printWidth < (this.pagesets[num7].coltotalwidth + num8)) && (num != 0))
                {
                    this.pagesets.Add(new PageDef(this.PrintMargins, this.colstoprint.Count));
                    num7++;
                    this.pagesets[num7].coltotalwidth = this.rowheaderwidth;
                }
                this.pagesets[num7].colstoprint.Add(this.colstoprint[num]);
                this.pagesets[num7].colwidths.Add(this.colwidths[num]);
                this.pagesets[num7].colwidthsoverride.Add(this.colwidthsoverride[num]);
                PageDef local1 = this.pagesets[num7];
                local1.coltotalwidth += num8;
            }
            for (num = 0; num < this.pagesets.Count; num++)
            {
                this.AdjustPageSets(g, this.pagesets[num]);
            }
        }

        private int PreviewDisplayHeight()
        {
            double num = this.printDoc.DefaultPageSettings.Bounds.Height + (3f * this.printDoc.DefaultPageSettings.HardMarginX);
            return (int) (num * this.PrintPreviewZoom);
        }

        private int PreviewDisplayWidth()
        {
            double num = this.printDoc.DefaultPageSettings.Bounds.Width + (3f * this.printDoc.DefaultPageSettings.HardMarginY);
            return (int) (num * this.PrintPreviewZoom);
        }

        private void printcolumnheaders(Graphics g, ref float pos, PageDef pageset)
        {
            float x = pageset.margins.Left + this.rowheaderwidth;
            this.lines = new Pen(this.dgv.GridColor, 1f);
            for (int i = 0; i < pageset.colstoprint.Count; i++)
            {
                DataGridViewColumn column = (DataGridViewColumn) pageset.colstoprint[i];
                float width = (pageset.colwidths[i] > (this.printWidth - this.rowheaderwidth)) ? (this.printWidth - this.rowheaderwidth) : pageset.colwidths[i];
                DataGridViewCellStyle inheritedStyle = column.HeaderCell.InheritedStyle;
                RectangleF rect = new RectangleF(x, pos, width, this.colheaderheight);
                g.FillRectangle(this.SolidBrush1 = new SolidBrush(inheritedStyle.BackColor), rect);
                g.DrawString(column.HeaderText, inheritedStyle.Font, this.SolidBrush1 = new SolidBrush(inheritedStyle.ForeColor), rect, this.headercellformat);
                if (this.dgv.ColumnHeadersBorderStyle != DataGridViewHeaderBorderStyle.None)
                {
                    g.DrawRectangle(this.lines, x, pos, width, this.colheaderheight);
                }
                x += pageset.colwidths[i];
            }
            pos += this.colheaderheight + ((this.dgv.ColumnHeadersBorderStyle != DataGridViewHeaderBorderStyle.None) ? this.lines.Width : 0f);
        }

        public void PrintDataGridView(DataGridView dgv)
        {
            if (dgv == null)
            {
                throw new Exception("Null Parameter passed to DGVPrinter.");
            }
            if (typeof(DataGridView) != dgv.GetType())
            {
                throw new Exception("Invalid Parameter passed to DGVPrinter.");
            }
            this.dgv = dgv;
            if (DialogResult.OK == this.DisplayPrintDialog())
            {
                this.SetupPrint();
                this.printDoc.Print();
            }
        }

        private void printDoc_BeginPrint(object sender, PrintEventArgs e)
        {
            this.currentpageset = 0;
            this.lastrowprinted = -1;
            this.CurrentPage = 0;
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.HasMorePages = this.PrintPage(e.Graphics);
        }

        private void printfooter(Graphics g, ref float pos, Margins margins)
        {
            pos = (this.pageHeight - this.footerHeight) - margins.Bottom;
            pos += this.footerspacing;
            this.printsection(g, ref pos, this.footer, this.footerfont, this.footercolor, this.footerformat, this.overridefooterformat, margins);
            if (!this.pagenumberontop && this.pageno)
            {
                string text = this.pagetext + this.CurrentPage.ToString(CultureInfo.CurrentCulture);
                if (this.showtotalpagenumber)
                {
                    text = text + this.pageseparator + this.totalpages.ToString();
                }
                if (1 < this.pagesets.Count)
                {
                    text = text + this.parttext + ((this.currentpageset + 1)).ToString(CultureInfo.CurrentCulture);
                }
                if (!this.pagenumberonseparateline)
                {
                    pos -= this.pagenumberHeight;
                }
                this.printsection(g, ref pos, text, this.pagenofont, this.pagenocolor, this.pagenumberformat, this.overridepagenumberformat, margins);
            }
        }

        public void PrintNoDisplay(DataGridView dgv)
        {
            if (dgv == null)
            {
                throw new Exception("Null Parameter passed to DGVPrinter.");
            }
            if (typeof(DataGridView) != dgv.GetType())
            {
                throw new Exception("Invalid Parameter passed to DGVPrinter.");
            }
            this.dgv = dgv;
            this.SetupPrint();
            this.printDoc.Print();
        }

        private bool PrintPage(Graphics g)
        {
            float num3;
            bool flag = false;
            bool flag2 = false;
            float top = this.pagesets[this.currentpageset].margins.Top;
            this.CurrentPage++;
            if ((this.CurrentPage >= this.fromPage) && (this.CurrentPage <= this.toPage))
            {
                flag2 = true;
            }
            float num2 = (this.pageHeight - this.footerHeight) - this.pagesets[this.currentpageset].margins.Bottom;
            while (!flag2)
            {
                top = this.pagesets[this.currentpageset].margins.Top + this.headerHeight;
                bool flag3 = false;
                num3 = (this.lastrowprinted < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f;
                while (!flag3)
                {
                    if (this.lastrowprinted >= (this.rowstoprint.Count - 1))
                    {
                        flag3 = true;
                    }
                    else
                    {
                        if ((top + num3) >= num2)
                        {
                            flag3 = true;
                            continue;
                        }
                        this.lastrowprinted++;
                        top += this.rowheights[this.lastrowprinted];
                        num3 = ((this.lastrowprinted + 1) < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f;
                    }
                }
                this.CurrentPage++;
                if ((this.CurrentPage >= this.fromPage) && (this.CurrentPage <= this.toPage))
                {
                    flag2 = true;
                }
                if ((this.lastrowprinted >= (this.rowstoprint.Count - 1)) || (this.CurrentPage > this.toPage))
                {
                    flag = this.DetermineHasMorePages();
                    this.lastrowprinted = -1;
                    this.CurrentPage = 0;
                    return flag;
                }
            }
            top = this.pagesets[this.currentpageset].margins.Top;
            if (this.PrintHeader)
            {
                if (this.pagenumberontop && this.pageno)
                {
                    string text = this.pagetext + this.CurrentPage.ToString(CultureInfo.CurrentCulture);
                    if (this.showtotalpagenumber)
                    {
                        text = text + this.pageseparator + this.totalpages.ToString();
                    }
                    if (1 < this.pagesets.Count)
                    {
                        text = text + this.parttext + ((this.currentpageset + 1)).ToString(CultureInfo.CurrentCulture);
                    }
                    this.printsection(g, ref top, text, this.pagenofont, this.pagenocolor, this.pagenumberformat, this.overridepagenumberformat, this.pagesets[this.currentpageset].margins);
                    if (!this.pagenumberonseparateline)
                    {
                        top -= this.pagenumberHeight;
                    }
                }
                if (!string.IsNullOrEmpty(this.title))
                {
                    this.printsection(g, ref top, this.title, this.titlefont, this.titlecolor, this.titleformat, this.overridetitleformat, this.pagesets[this.currentpageset].margins);
                }
                if (!string.IsNullOrEmpty(this.subtitle))
                {
                    this.printsection(g, ref top, this.subtitle, this.subtitlefont, this.subtitlecolor, this.subtitleformat, this.overridesubtitleformat, this.pagesets[this.currentpageset].margins);
                }
            }
            if (this.PrintColumnHeaders)
            {
                this.printcolumnheaders(g, ref top, this.pagesets[this.currentpageset]);
            }
            for (num3 = (this.lastrowprinted < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f; (top + num3) < num2; num3 = (this.lastrowprinted < this.rowheights.Count) ? this.rowheights[this.lastrowprinted + 1] : 0f)
            {
                this.lastrowprinted++;
                this.printrow(g, ref top, (DataGridViewRow) this.rowstoprint[this.lastrowprinted], this.pagesets[this.currentpageset]);
                if (this.lastrowprinted >= (this.rowstoprint.Count - 1))
                {
                    this.printfooter(g, ref top, this.pagesets[this.currentpageset].margins);
                    flag = this.DetermineHasMorePages();
                    this.lastrowprinted = -1;
                    this.CurrentPage = 0;
                    return flag;
                }
            }
            if (this.PrintFooter)
            {
                this.printfooter(g, ref top, this.pagesets[this.currentpageset].margins);
            }
            if (this.CurrentPage >= this.toPage)
            {
                flag = this.DetermineHasMorePages();
                this.lastrowprinted = -1;
                this.CurrentPage = 0;
                return flag;
            }
            return true;
        }

        public void PrintPreviewDataGridView(DataGridView dgv)
        {
            if (dgv == null)
            {
                throw new Exception("Null Parameter passed to DGVPrinter.");
            }
            if (typeof(DataGridView) != dgv.GetType())
            {
                throw new Exception("Invalid Parameter passed to DGVPrinter.");
            }
            this.dgv = dgv;
            if (DialogResult.OK == this.DisplayPrintDialog())
            {
                this.PrintPreviewNoDisplay(dgv);
            }
        }

        public void PrintPreviewNoDisplay(DataGridView dgv)
        {
            if (dgv == null)
            {
                throw new Exception("Null Parameter passed to DGVPrinter.");
            }
            if (typeof(DataGridView) != dgv.GetType())
            {
                throw new Exception("Invalid Parameter passed to DGVPrinter.");
            }
            this.dgv = dgv;
            this.SetupPrint();
            using (PrintPreviewDialog dialog = new PrintPreviewDialog())
            {
                dialog.Document = this.printDoc;
                dialog.UseAntiAlias = true;
                dialog.Owner = this.Owner;
                dialog.PrintPreviewControl.Zoom = this.PrintPreviewZoom;
                dialog.Width = this.PreviewDisplayWidth();
                dialog.Height = this.PreviewDisplayHeight();
                if (this.ppvIcon != null)
                {
                    dialog.Icon = this.ppvIcon;
                }
                dialog.ShowDialog();
            }
        }

        private void printrow(Graphics g, ref float pos, DataGridViewRow row, PageDef pageset)
        {
            float left = pageset.margins.Left;
            this.lines = new Pen(this.dgv.GridColor, 1f);
            DataGridViewCellStyle inheritedStyle = row.InheritedStyle;
            float width = (pageset.coltotalwidth > this.printWidth) ? ((float) this.printWidth) : pageset.coltotalwidth;
            RectangleF rect = new RectangleF(left, pos, width, this.rowheights[this.lastrowprinted]);
            this.SolidBrush1 = new SolidBrush(inheritedStyle.BackColor);
            g.FillRectangle(this.SolidBrush1, rect);
            if (this.dgv.RowHeadersVisible)
            {
                DataGridViewCellStyle style2 = row.HeaderCell.InheritedStyle;
                RectangleF ef2 = new RectangleF(left, pos, this.rowheaderwidth, this.rowheights[this.lastrowprinted]);
                this.SolidBrush1 = new SolidBrush(style2.BackColor);
                g.FillRectangle(this.SolidBrush1, ef2);
                g.DrawString(row.HeaderCell.EditedFormattedValue.ToString(), style2.Font, this.SolidBrush1 = new SolidBrush(style2.ForeColor), ef2, this.headercellformat);
                if (this.dgv.RowHeadersBorderStyle != DataGridViewHeaderBorderStyle.None)
                {
                    g.DrawRectangle(this.lines, left, pos, this.rowheaderwidth, this.rowheights[this.lastrowprinted]);
                }
                left += this.rowheaderwidth;
            }
            for (int i = 0; i < pageset.colstoprint.Count; i++)
            {
                DataGridViewColumn column = (DataGridViewColumn) pageset.colstoprint[i];
                string s = row.Cells[column.Index].EditedFormattedValue.ToString();
                float num4 = (pageset.colwidths[i] > (this.printWidth - this.rowheaderwidth)) ? (this.printWidth - this.rowheaderwidth) : pageset.colwidths[i];
                StringFormat cellformat = null;
                DataGridViewCellStyle controlstyle = null;
                if (this.ColumnStyles.ContainsKey(column.Name))
                {
                    controlstyle = this.colstyles[column.Name];
                    this.buildstringformat(ref cellformat, controlstyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
                    Font font = controlstyle.Font;
                }
                else if (column.HasDefaultCellStyle || row.Cells[column.Index].HasStyle)
                {
                    controlstyle = row.Cells[column.Index].InheritedStyle;
                    this.buildstringformat(ref cellformat, controlstyle, this.cellformat.Alignment, this.cellformat.LineAlignment, this.cellformat.FormatFlags, this.cellformat.Trimming);
                    Font font2 = controlstyle.Font;
                }
                else
                {
                    cellformat = this.cellformat;
                    controlstyle = row.Cells[column.Index].InheritedStyle;
                }
                RectangleF ef3 = new RectangleF(left, pos, num4, this.rowheights[this.lastrowprinted]);
                g.FillRectangle(this.SolidBrush1 = new SolidBrush(controlstyle.BackColor), ef3);
                if ("DataGridViewImageCell" == column.CellType.Name)
                {
                    this.DrawImageCell(g, (DataGridViewImageCell) row.Cells[column.Index], ef3);
                }
                else
                {
                    if ("DataGridViewCheckBoxCell" == column.CellType.Name)
                    {
                        if (s == "True")
                        {
                            s = "√";
                        }
                        else
                        {
                            s = " ";
                        }
                    }
                    g.DrawString(s, controlstyle.Font, this.SolidBrush1 = new SolidBrush(controlstyle.ForeColor), ef3, cellformat);
                }
                if (this.dgv.CellBorderStyle != DataGridViewCellBorderStyle.None)
                {
                    g.DrawRectangle(this.lines, left, pos, num4, this.rowheights[this.lastrowprinted]);
                }
                left += pageset.colwidths[i];
            }
            pos += this.rowheights[this.lastrowprinted];
        }

        private void printsection(Graphics g, ref float pos, string text, Font font, Color color, StringFormat format, bool useroverride, Margins margins)
        {
            SizeF ef = g.MeasureString(text, font, this.printWidth, format);
            RectangleF layoutRectangle = new RectangleF((float) margins.Left, pos, (float) this.printWidth, ef.Height);
            this.SolidBrush1 = new SolidBrush(color);
            g.DrawString(text, font, this.SolidBrush1, layoutRectangle, format);
            pos += ef.Height;
        }

        private void SetupPrint()
        {
            int num3;
            if (this.headercellformat == null)
            {
                this.buildstringformat(ref this.headercellformat, this.dgv.Columns[0].HeaderCell.InheritedStyle, this.headercellalignment, StringAlignment.Near, this.headercellformatflags, StringTrimming.Word);
            }
            if (this.cellformat == null)
            {
                this.buildstringformat(ref this.cellformat, this.dgv.DefaultCellStyle, this.cellalignment, StringAlignment.Near, this.cellformatflags, StringTrimming.Word);
            }
            int num = (int) Math.Round((double) this.printDoc.DefaultPageSettings.HardMarginX);
            int num2 = (int) Math.Round((double) this.printDoc.DefaultPageSettings.HardMarginY);
            if (this.printDoc.DefaultPageSettings.Landscape)
            {
                num3 = (int) Math.Round((double) this.printDoc.DefaultPageSettings.PrintableArea.Height);
            }
            else
            {
                num3 = (int) Math.Round((double) this.printDoc.DefaultPageSettings.PrintableArea.Width);
            }
            this.pageHeight = this.printDoc.DefaultPageSettings.Bounds.Height;
            this.pageWidth = this.printDoc.DefaultPageSettings.Bounds.Width;
            this.PrintMargins = this.printDoc.DefaultPageSettings.Margins;
            this.PrintMargins.Right = (num > this.PrintMargins.Right) ? num : this.PrintMargins.Right;
            this.PrintMargins.Left = (num > this.PrintMargins.Left) ? num : this.PrintMargins.Left;
            this.PrintMargins.Top = (num2 > this.PrintMargins.Top) ? num2 : this.PrintMargins.Top;
            this.PrintMargins.Bottom = (num2 > this.PrintMargins.Bottom) ? num2 : this.PrintMargins.Bottom;
            this.printWidth = (this.pageWidth - this.PrintMargins.Left) - this.PrintMargins.Right;
            this.printWidth = (this.printWidth > num3) ? num3 : this.printWidth;
            this.printRange = this.printDoc.PrinterSettings.PrintRange;
            if (PrintRange.SomePages == this.printRange)
            {
                this.fromPage = this.printDoc.PrinterSettings.FromPage;
                this.toPage = this.printDoc.PrinterSettings.ToPage;
            }
            else
            {
                this.fromPage = 0;
                this.toPage = 0x7fffffff;
            }
            if (PrintRange.Selection == this.printRange)
            {
                SortedList list;
                if (this.dgv.SelectedRows.Count != 0)
                {
                    list = new SortedList(this.dgv.SelectedRows.Count);
                    foreach (DataGridViewRow row in this.dgv.SelectedRows)
                    {
                        list.Add(row.Index, row);
                    }
                    list.Values.GetEnumerator();
                    this.rowstoprint = new List<object>(list.Count);
                    foreach (object obj2 in list.Values)
                    {
                        this.rowstoprint.Add(obj2);
                    }
                    this.colstoprint = new List<object>(this.dgv.Columns.Count);
                    foreach (DataGridViewColumn column in this.dgv.Columns)
                    {
                        if (column.Visible)
                        {
                            this.colstoprint.Add(column);
                        }
                    }
                }
                else
                {
                    SortedList list2;
                    if (this.dgv.SelectedColumns.Count != 0)
                    {
                        this.rowstoprint = this.dgv.Rows;
                        list2 = new SortedList(this.dgv.SelectedColumns.Count);
                        foreach (DataGridViewRow row2 in this.dgv.SelectedColumns)
                        {
                            list2.Add(row2.Index, row2);
                        }
                        this.colstoprint = new List<object>(list2.Count);
                        foreach (object obj3 in list2.Values)
                        {
                            this.colstoprint.Add(obj3);
                        }
                    }
                    else
                    {
                        list = new SortedList(this.dgv.SelectedCells.Count);
                        list2 = new SortedList(this.dgv.SelectedCells.Count);
                        foreach (DataGridViewCell cell in this.dgv.SelectedCells)
                        {
                            int columnIndex = cell.ColumnIndex;
                            int rowIndex = cell.RowIndex;
                            if (!list.Contains(rowIndex))
                            {
                                list.Add(rowIndex, this.dgv.Rows[rowIndex]);
                            }
                            if (!list2.Contains(columnIndex))
                            {
                                list2.Add(columnIndex, this.dgv.Columns[columnIndex]);
                            }
                        }
                        this.rowstoprint = new List<object>(list.Count);
                        foreach (object obj4 in list.Values)
                        {
                            this.rowstoprint.Add(obj4);
                        }
                        this.colstoprint = new List<object>(list2.Count);
                        foreach (object obj5 in list2.Values)
                        {
                            this.colstoprint.Add(obj5);
                        }
                    }
                }
            }
            else if (PrintRange.CurrentPage == this.printRange)
            {
                this.rowstoprint = new List<object>(this.dgv.DisplayedRowCount(true));
                this.colstoprint = new List<object>(this.dgv.Columns.Count);
                for (int i = this.dgv.FirstDisplayedScrollingRowIndex; i < (this.dgv.FirstDisplayedScrollingRowIndex + this.dgv.DisplayedRowCount(true)); i++)
                {
                    DataGridViewRow row3 = this.dgv.Rows[i];
                    if (row3.Visible)
                    {
                        this.rowstoprint.Add(row3);
                    }
                }
                this.colstoprint = new List<object>(this.dgv.Columns.Count);
                foreach (DataGridViewColumn column2 in this.dgv.Columns)
                {
                    if (column2.Visible)
                    {
                        this.colstoprint.Add(column2);
                    }
                }
            }
            else
            {
                this.rowstoprint = new List<object>(this.dgv.Rows.Count);
                foreach (DataGridViewRow row4 in (IEnumerable) this.dgv.Rows)
                {
                    if (row4.Visible)
                    {
                        this.rowstoprint.Add(row4);
                    }
                }
                this.colstoprint = new List<object>(this.dgv.Columns.Count);
                foreach (DataGridViewColumn column3 in this.dgv.Columns)
                {
                    if (column3.Visible)
                    {
                        this.colstoprint.Add(column3);
                    }
                }
            }
            SortedList list3 = new SortedList(this.colstoprint.Count);
            foreach (DataGridViewColumn column4 in this.colstoprint)
            {
                list3.Add(column4.DisplayIndex, column4);
            }
            this.colstoprint.Clear();
            foreach (object obj6 in list3.Values)
            {
                this.colstoprint.Add(obj6);
            }
            foreach (DataGridViewColumn column5 in this.colstoprint)
            {
                if (this.publicwidthoverrides.ContainsKey(column5.Name))
                {
                    this.colwidthsoverride.Add(this.publicwidthoverrides[column5.Name]);
                }
                else
                {
                    this.colwidthsoverride.Add(-1f);
                }
            }
            this.measureprintarea(this.printDoc.PrinterSettings.CreateMeasurementGraphics());
            this.totalpages = this.TotalPages();
        }

        private int TotalPages()
        {
            int num = 1;
            float num2 = 0f;
            float num3 = (((this.pageHeight - this.headerHeight) - this.footerHeight) - this.PrintMargins.Top) - this.PrintMargins.Bottom;
            for (int i = 0; i < this.rowheights.Count; i++)
            {
                if ((num2 + this.rowheights[i]) > num3)
                {
                    num++;
                    num2 = 0f;
                }
                num2 += this.rowheights[i];
            }
            return num;
        }

        public StringAlignment CellAlignment
        {
            get
            {
                return this.cellalignment;
            }
            set
            {
                this.cellalignment = value;
            }
        }

        public StringFormatFlags CellFormatFlags
        {
            get
            {
                return this.cellformatflags;
            }
            set
            {
                this.cellformatflags = value;
            }
        }

        public Dictionary<string, DataGridViewCellStyle> ColumnStyles
        {
            get
            {
                return this.colstyles;
            }
        }

        public Dictionary<string, float> ColumnWidths
        {
            get
            {
                return this.publicwidthoverrides;
            }
        }

        public string DocName
        {
            get
            {
                return this.docName;
            }
            set
            {
                this.printDoc.DocumentName = value;
                this.docName = value;
            }
        }

        public string Footer
        {
            get
            {
                return this.footer;
            }
            set
            {
                this.footer = value;
            }
        }

        public StringAlignment FooterAlignment
        {
            get
            {
                return this.footerformat.Alignment;
            }
            set
            {
                this.footerformat.Alignment = value;
                this.overridefooterformat = true;
            }
        }

        public Color FooterColor
        {
            get
            {
                return this.footercolor;
            }
            set
            {
                this.footercolor = value;
            }
        }

        public Font FooterFont
        {
            get
            {
                return this.footerfont;
            }
            set
            {
                this.footerfont = value;
            }
        }

        public StringFormat FooterFormat
        {
            get
            {
                return this.footerformat;
            }
            set
            {
                this.footerformat = value;
                this.overridefooterformat = true;
            }
        }

        public StringFormatFlags FooterFormatFlags
        {
            get
            {
                return this.footerformat.FormatFlags;
            }
            set
            {
                this.footerformat.FormatFlags = value;
                this.overridefooterformat = true;
            }
        }

        public float FooterSpacing
        {
            get
            {
                return this.footerspacing;
            }
            set
            {
                this.footerspacing = value;
            }
        }

        public StringAlignment HeaderCellAlignment
        {
            get
            {
                return this.headercellalignment;
            }
            set
            {
                this.headercellalignment = value;
            }
        }

        public StringFormatFlags HeaderCellFormatFlags
        {
            get
            {
                return this.headercellformatflags;
            }
            set
            {
                this.headercellformatflags = value;
            }
        }

        public Form Owner
        {
            get
            {
                return this._Owner;
            }
            set
            {
                this._Owner = value;
            }
        }

        public StringAlignment PageNumberAlignment
        {
            get
            {
                return this.pagenumberformat.Alignment;
            }
            set
            {
                this.pagenumberformat.Alignment = value;
                this.overridepagenumberformat = true;
            }
        }

        public Color PageNumberColor
        {
            get
            {
                return this.pagenocolor;
            }
            set
            {
                this.pagenocolor = value;
            }
        }

        public Font PageNumberFont
        {
            get
            {
                return this.pagenofont;
            }
            set
            {
                this.pagenofont = value;
            }
        }

        public StringFormat PageNumberFormat
        {
            get
            {
                return this.pagenumberformat;
            }
            set
            {
                this.pagenumberformat = value;
                this.overridepagenumberformat = true;
            }
        }

        public StringFormatFlags PageNumberFormatFlags
        {
            get
            {
                return this.pagenumberformat.FormatFlags;
            }
            set
            {
                this.pagenumberformat.FormatFlags = value;
                this.overridepagenumberformat = true;
            }
        }

        public bool PageNumberInHeader
        {
            get
            {
                return this.pagenumberontop;
            }
            set
            {
                this.pagenumberontop = value;
            }
        }

        public bool PageNumberOnSeparateLine
        {
            get
            {
                return this.pagenumberonseparateline;
            }
            set
            {
                this.pagenumberonseparateline = value;
            }
        }

        public bool PageNumbers
        {
            get
            {
                return this.pageno;
            }
            set
            {
                this.pageno = value;
            }
        }

        public string PageSeparator
        {
            get
            {
                return this.pageseparator;
            }
            set
            {
                this.pageseparator = value;
            }
        }

        public System.Drawing.Printing.PageSettings PageSettings
        {
            get
            {
                return this.printDoc.DefaultPageSettings;
            }
        }

        public string PageText
        {
            get
            {
                return this.pagetext;
            }
            set
            {
                this.pagetext = value;
            }
        }

        public string PartText
        {
            get
            {
                return this.parttext;
            }
            set
            {
                this.parttext = value;
            }
        }

        public bool PorportionalColumns
        {
            get
            {
                return this.porportionalcolumns;
            }
            set
            {
                this.porportionalcolumns = value;
            }
        }

        public Icon PreviewDialogIcon
        {
            get
            {
                return this.ppvIcon;
            }
            set
            {
                this.ppvIcon = value;
            }
        }

        public bool PrintColumnHeaders
        {
            get
            {
                return this.printColumnHeaders;
            }
            set
            {
                this.printColumnHeaders = value;
            }
        }

        public PrintDialogSettingsClass PrintDialogSettings
        {
            get
            {
                return this.printDialogSettings;
            }
        }

        public PrintDocument printDocument
        {
            get
            {
                return this.printDoc;
            }
            set
            {
                this.printDoc = value;
            }
        }

        public string PrinterName
        {
            get
            {
                return this.printerName;
            }
            set
            {
                this.printerName = value;
            }
        }

        public bool PrintFooter
        {
            get
            {
                return this.printFooter;
            }
            set
            {
                this.printFooter = value;
            }
        }

        public bool PrintHeader
        {
            get
            {
                return this.printHeader;
            }
            set
            {
                this.printHeader = value;
            }
        }

        public Margins PrintMargins
        {
            get
            {
                return this.PageSettings.Margins;
            }
            set
            {
                this.PageSettings.Margins = value;
            }
        }

        public double PrintPreviewZoom
        {
            get
            {
                return this._PrintPreviewZoom;
            }
            set
            {
                this._PrintPreviewZoom = value;
            }
        }

        public PrinterSettings PrintSettings
        {
            get
            {
                return this.printDoc.PrinterSettings;
            }
        }

        public RowHeightSetting RowHeight
        {
            get
            {
                return this._rowheight;
            }
            set
            {
                this._rowheight = value;
            }
        }

        public bool ShowTotalPageNumber
        {
            get
            {
                return this.showtotalpagenumber;
            }
            set
            {
                this.showtotalpagenumber = value;
            }
        }

        public string SubTitle
        {
            get
            {
                return this.subtitle;
            }
            set
            {
                this.subtitle = value;
            }
        }

        public StringAlignment SubTitleAlignment
        {
            get
            {
                return this.subtitleformat.Alignment;
            }
            set
            {
                this.subtitleformat.Alignment = value;
                this.overridesubtitleformat = true;
            }
        }

        public Color SubTitleColor
        {
            get
            {
                return this.subtitlecolor;
            }
            set
            {
                this.subtitlecolor = value;
            }
        }

        public Font SubTitleFont
        {
            get
            {
                return this.subtitlefont;
            }
            set
            {
                this.subtitlefont = value;
            }
        }

        public StringFormat SubTitleFormat
        {
            get
            {
                return this.subtitleformat;
            }
            set
            {
                this.subtitleformat = value;
                this.overridesubtitleformat = true;
            }
        }

        public StringFormatFlags SubTitleFormatFlags
        {
            get
            {
                return this.subtitleformat.FormatFlags;
            }
            set
            {
                this.subtitleformat.FormatFlags = value;
                this.overridesubtitleformat = true;
            }
        }

        public Alignment TableAlignment
        {
            get
            {
                return this.tablealignment;
            }
            set
            {
                this.tablealignment = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                if (this.docName == null)
                {
                    this.printDoc.DocumentName = value;
                }
            }
        }

        public StringAlignment TitleAlignment
        {
            get
            {
                return this.titleformat.Alignment;
            }
            set
            {
                this.titleformat.Alignment = value;
                this.overridetitleformat = true;
            }
        }

        public Color TitleColor
        {
            get
            {
                return this.titlecolor;
            }
            set
            {
                this.titlecolor = value;
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.titlefont;
            }
            set
            {
                this.titlefont = value;
            }
        }

        public StringFormat TitleFormat
        {
            get
            {
                return this.titleformat;
            }
            set
            {
                this.titleformat = value;
                this.overridetitleformat = true;
            }
        }

        public StringFormatFlags TitleFormatFlags
        {
            get
            {
                return this.titleformat.FormatFlags;
            }
            set
            {
                this.titleformat.FormatFlags = value;
                this.overridetitleformat = true;
            }
        }

        public enum Alignment
        {
            NotSet,
            Left,
            Right,
            Center
        }

        private class PageDef
        {
            public IList colstoprint;
            public float coltotalwidth;
            public List<float> colwidths;
            public List<float> colwidthsoverride;
            public Margins margins;

            public PageDef(Margins m, int count)
            {
                this.colstoprint = new List<object>(count);
                this.colwidths = new List<float>(count);
                this.colwidthsoverride = new List<float>(count);
                this.coltotalwidth = 0f;
                this.margins = (Margins) m.Clone();
            }
        }

        public class PrintDialogSettingsClass
        {
            public bool AllowCurrentPage = true;
            public bool AllowPrintToFile = true;
            public bool AllowSelection = true;
            public bool AllowSomePages = true;
            public bool ShowHelp = true;
            public bool ShowNetwork = true;
            public bool UseEXDialog = true;
        }

        public enum RowHeightSetting
        {
            StringHeight,
            CellHeight
        }
    }
}

