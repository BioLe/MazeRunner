using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class SaveData {

	private List<string[]> rowData = new List<string[]>();

	private string fileName;
    private int numberOfColumns;
    private string[] rowDataTemp;
	//rowDataTemp = new string[rowDataTempSize];

	public SaveData(string[] headerRow, int numberOfColumns, string fileName){
		this.fileName = fileName;
		this.numberOfColumns = numberOfColumns;
		if(headerRow.Length == numberOfColumns){
			rowData.Add(headerRow);
		} else{
			throw new Exception("The number of elements in the header are different than the number of columns.");
		}
	}

	public bool addRow(string[] row){
		if(row.Length == numberOfColumns){
			rowData.Add(row);
			return true;
		}
		return false;
	}

	public void exportCSV() {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++) {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        DateTime dt = System.DateTime.Now;
        string datetime = dt.ToString("yyyy-MM-dd_HH-mm-ss");

        String filePath = Application.dataPath + "/" + fileName + "_" + datetime + ".csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
}
