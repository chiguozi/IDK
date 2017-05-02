using System.Collections;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using UnityEngine;
using System.IO;

public class ExcelCell
{
    public object value;
    public int index;


}

public class Excel 
{
    IWorkbook _workBook;
    ISheet _sheet;
    string _fileName;
    string _filePath;
    
    public Excel(string fullpath)
    {
        _fileName = Path.GetFileNameWithoutExtension(fullpath);
        _filePath = fullpath;
        LoadData();
    }

    public void LoadData()
    {
        using (FileStream fs = File.Open(_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            _workBook = WorkbookFactory.Create(fs);
            _sheet = _workBook.GetSheetAt(0);
        }

        for(int i = _sheet.FirstRowNum; i < _sheet.LastRowNum; i++)
        {
            var row = _sheet.GetRow(i);
            if (row == null)
                Debug.LogError("row" + i);
            else
            ProcessRow(row);
        }
    }

    void ProcessRow(IRow row)
    {
        for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
        {
            var cell = row.GetCell(i);
            if (cell == null)
                Debug.LogError(i);
            else
            {
                var color = cell.CellStyle.FillForegroundColorColor;
                //Debug.LogError(color.GetType().Name);
                //Debug.LogError(color.RGB);
            }
         
        }
    }

}
