using System;
using System.Collections;
using System.Drawing.Printing;
using System.Threading;
using System.Drawing;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
namespace JCSLA.Reports{
	public abstract class Report {
		object m = Type.Missing;
		Thread _reportThread;
		Sheets _sheets;
		object _tag;
		string _ranBy;
		string[] _printText;
		int _printTextIx = 0;
		System.Drawing.Font _printFont;
		string n = System.Environment.NewLine;
		public delegate void AddRowHandler(Report report, ReportEventArgs reportEventArgs);
		public AddRowHandler OnAddRow;

		public delegate void CompletedHandler(Report report, ReportEventArgs reportEventArgs);
		public CompletedHandler OnCompleted;
		public void RaiseOnComplete(){
			this.OnCompleted(this, new ReportEventArgs(this, null));
		}
		public void RaiseOnAddRow(Row latestRow){
			this.OnAddRow(this, new ReportEventArgs(this, latestRow));
		}
		public abstract string Title{
			get;
		}
		public Report(string ranBy) {
			 _sheets = new Sheets(this);
			_ranBy = ranBy;
		}
		public string RanBy{
			get{
				return _ranBy;
			} 
			set{
				_ranBy = value;
			}
		}
		public object Tag{
			get{
				return _tag;
			}
			set{
				_tag = value;
			}
		}
		protected abstract void run();
		public  abstract bool IsInProgress{
			get;
			set;
		}
	
		public void Run(bool inNewThread){
			if (inNewThread){
				_reportThread = new Thread(new ThreadStart(run));
				_reportThread.Name = this.ToString();
				_reportThread.Start();
			}else{
				this.run();
			}
		}
		public string Text{
			get{
				string ret = "";
				ret = this.Banner;
				foreach(Sheet sheet in _sheets){
					ret += sheet.Text;
				}
				return ret;
			}
		}
		public void DeleteDefaultWorksheets(Excel.Workbook wb, Sheet sheet){
			Excel.Sheets excelSheets = wb.Sheets;
			foreach(Excel.Worksheet wsToDel in excelSheets){
				if (wb.Name != sheet.Title){
					wsToDel.Delete();
				}
			}
			Marshal.ReleaseComObject(excelSheets); excelSheets = null;
		}
		public Excel.Range SetCell(Excel.Worksheet ws, int row, int col, string val){
			Excel.Range cells = (Excel.Range)ws.Cells;
			Excel.Range cell = (Excel.Range)cells[row, col];
			Marshal.ReleaseComObject(cells); cells = null;
			Excel.Hyperlinks hls;
			val = val.Replace("\r", "");
			int ix = val.IndexOf("://");
			if (ix != -1){
				hls = cell.Hyperlinks;
				hls.Add(cell, val, m, m, val);
				Marshal.ReleaseComObject(hls); hls = null;
			}else{
				cell.Value2 = val;
			}
			return cell;
		}
		public Excel.Application ExcelApp{
			get{
				/* Excel has some issue you need to be aware of to work with it from dotnet. Basically, you need release all COM
				 * references of any Excel object you work with and set the dotnet references to null. There is plenty of info on the net
				 * on how to do this. See this page first: http://support.microsoft.com/?kbid=317109 */

				Excel.Application app = new Excel.Application(); 
				Excel.Sheets wss;
				app.Caption = this.Title;
				
				Excel.Workbooks wbs = app.Workbooks;
				Excel.Workbook wb = wbs.Add(m);
				Marshal.ReleaseComObject(wbs); wbs = null;

				Excel.Worksheet ws = null;
				Field fld;
				Excel.Range range;
				bool defaultWorksheetsDeleted = false;
				int exRow =1; int exCol =1;
				Sheet sheet;
				for(int i=_sheets.Count-1; i>=0; i--){
					sheet = _sheets[i];
					if (ws != null) {Marshal.ReleaseComObject(ws); ws = null;};
					wss = (Excel.Sheets) wb.Worksheets;
					ws = (Excel.Worksheet)wss.Add(m, m, m, m);
					Marshal.ReleaseComObject(wss); wss =null;
					range = SetCell(ws, 1, 1, this.Banner);
					range.Font.Bold = true;
					Marshal.ReleaseComObject(range); range = null;
					ws.Name = (sheet.Title.Trim() == "") ? i.ToString() : sheet.Title;
					if (!defaultWorksheetsDeleted){
						DeleteDefaultWorksheets(wb, sheet);
						defaultWorksheetsDeleted = true;
					}
					exRow = 2;
					foreach(Table table in sheet.Tables){
						range = SetCell(ws, exRow, 1, table.Title);
						range.Font.Bold = true;
						Marshal.ReleaseComObject(range); range = null;
						for(int j = 0; j<table.Rows.Count; j++){
							exRow++;
							for(int k = 0; k< table.Columns.Count; k++){
								exCol = k+1;
								fld = table.Fields[k, j];
								range = SetCell(ws, exRow, exCol, fld.Value);
								if (fld.IsHeader) range.Font.Bold = true;
								Marshal.ReleaseComObject(range); range = null;
							}
						}
						SetCell(ws, ++exRow, exCol, " ");
					}
					 ws.Rows.AutoFit(); ws.Columns.AutoFit();
					Marshal.ReleaseComObject(ws); ws = null;
				}
				Marshal.ReleaseComObject(wb); wb =null;
				return app;
			}
		}
		public string Truncate(string str, int maxCharsToReturn, bool withEllipse){
			string tail = "";
			if (withEllipse)
				tail = "...";
			if (str.Length >= maxCharsToReturn)
				return str.Substring(0, maxCharsToReturn - 1 - tail.Length) + tail;
			else
				return str;
		}
		/*public string[] PrintText{
			get{
				//TODO: This isn't done. I might can be used to shink the width of a report to make it fit on a page.
				int colWidth;
				string rowString = "";
				ArrayList printText = new ArrayList();
				foreach(Sheet sheet in _sheets){
					foreach(Table table in sheet.Tables){
						colWidth = (93 / table.Columns.Count) - Table.COLUMN_PAD;
						foreach(Row row in table.Rows){
							foreach(Field fld in row.Fields){
								if (fld.Value.Length > colWidth)
									fldVal = fld.Value.Substring(1, colWidth);
								else
									fldVal = fld.Value.PadRight(
										//rowString = fld.Value.Substring(1, colWidth) + " ".PadRight(Table.COLUMN_PAD);
										//printText.Add(rowString);
						}
					}
				}
				return (string[])printText.ToArray();
			}
		}*/
		protected Table CreateDefaultTable(string title, params string[] columnHeaders){
			Table table;
			Sheet sheet;
			if (_sheets[0] == null){
				sheet = new Sheet(this,title);
				_sheets.Add(sheet);
				table = new Table(sheet, title, columnHeaders);
				sheet.Add(table);
			}else{
				table = _sheets[0][0];
			}
			return table;
		}
		public Sheet CreateSheet(string title){
			Sheet sheet = new Sheet(this, title);
			_sheets.Add(sheet);
			return sheet;
		}
		public override string ToString(){
			return this.GetType().Name;
		}
		public void Print(PrintDocument pd){
			if (this.IsInProgress) throw new Exception("Can't print. Report is in progress");
			_printText = System.Text.RegularExpressions.Regex.Split(this.Text, @"\r\n");
			_printFont = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			_printTextIx = 0;
			pd.PrintPage += new PrintPageEventHandler(PD_PrintPage);
			pd.Print();
		}
		private void PD_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			float yPos = 0f;
			int count = 0;
			float leftMargin = e.MarginBounds.Left;
			float topMargin = e.MarginBounds.Top;
			string line = null;
			bool done = false;
			float linesPerPage = e.MarginBounds.Height/_printFont.GetHeight(e.Graphics);
			while (count < linesPerPage) {
				if (_printTextIx == _printText.Length ) {
					done = true;
					break;
				}
				line = _printText[_printTextIx++];
				yPos = topMargin + count * _printFont.GetHeight(e.Graphics);
				e.Graphics.DrawString(line, _printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
				count++;
			}
			e.HasMorePages = !done;
		}
		public void Clear(){
			_sheets.Clear();
		}
		public string Banner{
			get{
				string ret;
				ret  = "Report: " + this.Title+ n;
				ret += "Date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + n;
				ret += "Run By: " + this.RanBy+ n;
				return ret;
			}
		}
	}
	public class Sheets: CollectionBase{
		Report _report;
		public Sheets(Report report){
			_report = report;
		}
		public Sheet Add(Sheet sheet){
			List.Add(sheet);
			return sheet;
		}
		public Report Report{
			get{
				return _report;
			}
		}
		public Sheet this[int id]{
			get{
				if (List.Count > 0)
					return (Sheet)List[id];
				else 
					return null;
			}
			set{
				List[id] = value;
			}
		}
	}
	public class Sheet{
		Tables _tables = new Tables();
		Report _report;
		string n = System.Environment.NewLine;
		string _title;
		public Sheet(Report report, string title){
			_report = report; _title = title;
		}
		public Table Add(Table table){
			_tables.Add(table);
			return table;
		}
		public Report Report{
			get{
				return _report;
			}
		}
		public  Tables Tables{
			get{
				return _tables;
			}
		}
		public string Text{
			get{
				string ret = "";
				foreach(Table table in _tables){
					ret += table.Text;
				}
				return ret;
			}
		}
		public string Title{
			get{
				return _title;
			}
			set{
				_title = value;
			}
		}
		public Table this[int id]{
			get{
				return _tables[id];
			}
		}

	}
	public class Tables : CollectionBase{
		Sheet _sheet;
		public void Table(Sheet sheet){
			_sheet = sheet;
		}
		public Table Add(Table table){
			List.Add(table);
			return table;
		}
		public Table this[int id]{
			get{
				return (Table)List[id];
			}
		}
	}
	public class Table{
		Sheet _sheet;
		Rows _rows;
		Columns _cols;
		Fields _fields;
		public const int COLUMN_PAD = 4;
		string _title; 
		string n = System.Environment.NewLine;
		public Table(Sheet sheet, string title, params string[] columnHeaders){
			_cols = new Columns(this);
			_rows = new Rows(this);
			_title = title;
			_fields = new Fields(this);
			_sheet = sheet;
			_cols.Add(columnHeaders);
		}
		public Table(Sheet sheet, int numberOfColumnsWithNoHeader){
			_sheet = sheet;
			_cols = new Columns(this);
			_rows = new Rows(this);
			_title = "";
			_fields = new Fields(this);
			_cols.Add(numberOfColumnsWithNoHeader);
		}
		public Row AddRow(params string[] vals){
			Row row = _rows.Add(vals);
			this._sheet.Report.RaiseOnAddRow(row);
			return row;

		}
		public Row AddRow(bool raiseAddRowEvent, params string[] vals){
			Row row = _rows.Add(vals);
			if (raiseAddRowEvent)
				this._sheet.Report.RaiseOnAddRow(row);
			return row;
		}
		public void AddHeader(params string[] columnHeaders){
			_cols.Add(columnHeaders);

		}
		public Row AddRow(){
			return _rows.Add();
		}
		public Field this[int row, int col]{
			get{
				return this.Fields[row, col];
			}
		}
		public Columns Columns{
			get{
				return _cols;
			}
		}
		public Rows Rows{
			get{
				return _rows;
			}
		}
		public Fields Fields{
			get{
				return _fields;
			}
		}
		public string Title{
			get{
				return _title;
			}
			set{
				_title = value;
			}
		}
		public string Text{
			get{
				string ret;
				ret = this.Title + n;
				foreach (Row row in this.Rows){
					if (row.IsHeader){
						ret += "".PadRight(this.RowWidth, '-') + n;
						ret += row.Text + n;
						ret += "".PadRight(this.RowWidth, '-') + n;
					}else{
						ret += row.Text + n;
					}
				}
				return ret;
			}
		}
		public int RowWidth{
			get{
				int width = 0;
				if (this.Columns.Count > 0){
					foreach(Column col in this.Columns){
						width += col.MaxLength;
					}
					if (this.Columns.Count > 1)
						width += (COLUMN_PAD * (this.Columns.Count-1));
				}
				return width;
			}
		}
	}
	public class Rows : CollectionBase{
		Table _table;
		public Rows(Table table){
			_table = table;
		}
		public Row this[int row]{
			get{
				int lstCnt = List.Count;
				return (lstCnt > row) ? (Row)List[row] : null;
			}
		}
		public Row Add(params string[] vals){
			Row ret = new Row(_table, vals, List.Count);
			List.Add(ret);
			return ret;
		}
		public Row Add(){
			Row ret = new Row(_table, List.Count);
			List.Add(ret);
			return ret;
		}
	}
	public class Row{
		Table _table;
		int _id;
		public Row(Table table, string[] vals, int id){
			_id = id;
			_table = table;
			Field[] flds = _table.Fields[this];
			for(int i=0; i<vals.Length; i++){
				flds[i].Value = vals[i];
			}
		}
		public Row(Table table, int id){
			_id = id;
			_table = table;
			Field[] flds = _table.Fields[this];
			for(int i=0; i<flds.Length; i++){
				flds[i].Value = ""; //Blank row
			}
		}
		public Fields Fields{
			get{
				Fields ret = new Fields(_table);
				foreach(Field fld in _table.Fields){ 
					if (fld.Row == this){
						ret.Add(fld);
					}
				}
				return ret;
			}
		}
		public int Id{
			get{
				return _id;
			}
		}
		public string Text{
			get{
				string ret = "";
				foreach(Field fld in this.Fields){
					ret +=  Pad(fld) + " ".PadLeft(Table.COLUMN_PAD);
				}
				return ret;
			}
		}
		public string Pad(Field fld){
			int colMaxLen = fld.Column.MaxLength;
			return fld.Value.PadRight(colMaxLen);
		}
		public bool IsHeader{
			get{
				return this.Fields[0, this.Id].IsHeader;
			}
		}
	}
	public class Columns : CollectionBase{
		Table _table;
		public Columns(Table table){
			_table = table;
		}
		public void Add(int numberOfColumnsWithNoHeader){
			Column col;
			for(int i=0; i<numberOfColumnsWithNoHeader; i++){
				col = new Column(_table, "", i);
				List.Add(col);
			}
		}
		public void Add(params string[] vals){
			Column col;
			int lstCnt;
			foreach(string val in vals){
				lstCnt = List.Count;
				 col = new Column(_table, val, lstCnt);
				List.Add(col);
			}
		}
		public Column this[int col]{
			get{
				return (Column) List[col];
			}
		}
	}
	public class Column{
		Table _table; int _id;
		public Column(Table table, string header, int id){
			Field headerFld;
			_table = table; _id = id; 
			if (header.Trim() != ""){
				headerFld = _table.Fields[this, 0]; 
				headerFld.Value = header;
				headerFld.IsHeader = true;
			}
		}
		public int Id{
			get{
				return _id;
			}
		}
		public Fields Fields{
			get{
				Fields ret = new Fields(_table);
				foreach(Field fld in _table.Fields){ 
					if (fld.Column == this){
						ret.Add(fld);
					}
				}
				return ret;
			}
		}
		public int MaxLength{
			get{
				int maxLen = 0;
				foreach(Field fld in Fields){
					if (fld.Value.Length > maxLen){
						maxLen = fld.Value.Length;
					}
				}
				return maxLen;
			}
		}
	}
	public class Fields : CollectionBase{
		Table _table;
		public Fields(Table table){
			_table = table;
		}
		public Field Add(Field fld){
			List.Add(fld);
			return fld;
		}
		public Field this[int col, int row]{
			get{
				foreach(Field fld in _table.Fields){
					if (fld.Column.Id == col && fld.Row.Id == row){
						return fld;
					}
				}
				Field ret = new Field(_table, col, row);
				List.Add(ret);
				return ret;
			}
		}
		public Field this[Column col, int row]{
			get{
				foreach(Field fld in _table.Fields){
					if (fld.Column.Id == col.Id && fld.Row.Id == row){
						return fld;
					}
				}
				Field ret = new Field(_table, row, col);
				List.Add(ret);
				return ret;
			}
		}
		public Field this[Column col, Row row]{
			get{
				return this[col.Id, row.Id];
			}
		}
		public Field this[int col, Row row]{
			get{
				return this[col, row.Id];
			}
		}
		public Field[] this[Row row]{
			get{
				Field[]  flds = new Field[this._table.Columns.Count];
				int i = 0;
				bool filled = false;
				foreach(Field fld in _table.Fields){
					if (fld.Row == row){
						flds[i++] = fld;
						filled = true;
					}
				}
				if (!filled){
					foreach(Column col in _table.Columns){
						Field fld= new Field(_table, row, col);
						flds[i++] = fld;
						List.Add(fld);
					}
				}
 				return flds;
			}
		}
	}
	public class Field{
		Row _row;
		Column _column;
		Table _table;
		bool _isExcelFunction;
		string _value = "";
		bool _isHeader;
		public Field(Table table, Row row, Column col){
			_table = table; _row = row; _column = col;
		}
		public Field(Table table, int row, Column col){
			_table = table; 
			_row = _table.Rows[row];
			if (_row == null)
				_row = _table.AddRow();
			_column = col;
		}

		public Field(Table table, int row, int col){
			_table = table; 
			_column = _table.Columns[col];
			_row = _table.AddRow();
		}
		public Row Row{
			get{
				return _row;
			}
			set{
				_row = value;
			}
		}
		public Column Column{
			get{
				return _column;
			}
			set{
				_column = value;
			}
		}
		public bool  IsExcelFunction{
			get{
				return _isExcelFunction;
			}
			set{
				_isExcelFunction = value;
			}
		}
		public string Value{
			get{
				return _value;
			}
			set{
				_value = value;
			}
		}
		public bool IsHeader{
			get{
				return _isHeader;
			}
			set{
				_isHeader = value;
			}
		}
	}
	public class ReportEventArgs{
		Report _report;
		Row _lastestRow;
		public ReportEventArgs(Report report, Row lastestRow){
			_report = report; _lastestRow = lastestRow;
		}
		public string RowText{
			get{
				return _lastestRow.Text;
			}
		}
		public Row Row{
			get{
				return _lastestRow;
			}
		}
		public string ReportText{
			get{
				return _report.Text;
			}
		}

	}
}
