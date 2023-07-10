using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace EBookStore.Site.Models.Infra
{
    public class UploadExecelToDataBase
    {
        private readonly string _connStr;
        public UploadExecelToDataBase()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public void Upload(string fullName, string OrderId)
        {
            var dt = ExecToDataBase(fullName, OrderId);

            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(_connStr, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = "OrderItems";//資料庫中的表名
            sqlbulkcopy.ColumnMappings.Add("Id", "OrderId");
            sqlbulkcopy.ColumnMappings.Add("品名", "BookId");
            sqlbulkcopy.ColumnMappings.Add("金額", "Price");
            sqlbulkcopy.ColumnMappings.Add("個數", "Qty");
            sqlbulkcopy.WriteToServer(dt);
        }

        private DataTable ExecToDataBase(string fullName, string OrderId)
        {
            DataTable dt = new DataTable();
            FileStream file = null;
            IWorkbook Workbook = null;

            try
            {
                using (file = new FileStream(fullName, FileMode.Open, FileAccess.Read)) // C#文件流讀取文件
                {
                    if (fullName.IndexOf(".xlsx") > 0)
                        //把xlsx文件中的數據寫入Workbook中
                        Workbook = new XSSFWorkbook(file);

                    else if (fullName.IndexOf(".xls") > 0)
                        //把xls文件中的數據寫入Workbook中
                        Workbook = new HSSFWorkbook(file);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0); //讀取第一個sheet 
                        //System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

                        //得到Excel工作表標題的欄位
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的總列數  
                        int cellCount = headerRow.LastCellNum;

                        // 新增 "ID" 欄位
                        dt.Columns.Add("ID", typeof(long));

                        for (int j = 0; j < cellCount; j++)
                        {
                            //得到Excel工作表指定行的單元格  
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();

                            dataRow["ID"] = Convert.ToInt64(OrderId);

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                int k = j + 1;
                                if (row.GetCell(j) != null)
                                    dataRow[k] = row.GetCell(j).ToString();
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }
                    return dt;
                }
            }

            catch (Exception)
            {
                if (file != null)
                {
                    file.Close(); //關閉當前流並釋放資源
                }
                return null;
            }
        }

        ///  <summary>    
        ///從Excel中獲取數據到DataTable   
        ///  </summary>    
        ///  <param name="strFileName"> Excel文件全路徑(服務器路徑) </param>    
        ///  <param name="SheetName">要獲取數據的工作表名稱</param>    
        ///  <param name="HeaderRowIndex">工作表標題行所在行號(從0開始) </param>    
        ///  <returns></returns>    
        public static DataTable RenderDataTableFromExcel(string strFileName, string SheetName, int HeaderRowIndex)
        {
            IWorkbook Workbook = null;

            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(" .xlsx ") > 0)

                    Workbook = new XSSFWorkbook(file);

                else if (strFileName.IndexOf(" .xls ") > 0)

                    Workbook = new HSSFWorkbook(file);
                ISheet sheet = Workbook.GetSheet(SheetName);
                return RenderDataTableFromExcel(Workbook, SheetName, HeaderRowIndex);
            }
        }

        ///  <summary>    
        ///從Excel中獲取數據到DataTable   
        ///  </summary>    
        ///  <param name="workbook">要處理的工作薄</param>    
        ///  <param name="SheetName">要獲取數據的工作表名稱</param>    
        ///  <param name="HeaderRowIndex">工作表標題行所在行號(從0開始) </param>    
        ///  <returns></returns>    
        public static DataTable RenderDataTableFromExcel(IWorkbook workbook, string SheetName, int HeaderRowIndex)
        {
            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                #region 循環各行各列,寫入數據到DataTable
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j] = null;
                        }
                        else
                        {
                            // dataRow[j] = cell.ToString();    
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    dataRow[j] = null;
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                    dataRow[j] = cell.ToString();
                                    break;
                                case CellType.String:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    dataRow[j] = cell.ErrorCellValue;
                                    break;
                                case CellType.Formula:
                                default:
                                    dataRow[j] = " = " + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                    // dataRow[j] = row.GetCell(j).ToString();    
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出錯了");
                DataRow dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally
            {
                // sheet.Dispose();    
                workbook = null;
                sheet = null;
            }
            #region 清除最後的空行
            for (int i = table.Rows.Count - 1; i > 0; i--)
            {
                bool isnull = true;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j] != null)
                    {
                        if (table.Rows[i][j].ToString() != "")
                        {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull)
                {
                    table.Rows[i].Delete();
                }
            }
            #endregion
            return table;
        }


    }
}