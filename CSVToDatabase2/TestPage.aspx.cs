using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

namespace CSVToDatabase2
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void importMainFileButton_Click(object sender, EventArgs e)
        {
            string fileName = @"~/App_Data/" + mainFileTextBox.Text;

            if (File.Exists(Server.MapPath(fileName)))
            {
                DataTable csvDataTable = new DataTable();
                using (TextFieldParser csvReader = new TextFieldParser(Server.MapPath(fileName)))
                {



                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = false;
                    string[] colFields = csvReader.ReadFields();
                    //foreach (string column in colFields)
                    //{
                    //    DataColumn datacolumn = new DataColumn(column);
                    //    datacolumn.AllowDBNull = true;
                    //    csvDataTable.Columns.Add(datacolumn);
                    //}
                    csvDataTable.Columns.Add("Code", typeof(string));
                    csvDataTable.Columns.Add("Type", typeof(string));
                    csvDataTable.Columns.Add("Year", typeof(string));
                    csvDataTable.Columns.Add("Make", typeof(string));
                    csvDataTable.Columns.Add("Model", typeof(string));
                    csvDataTable.Columns.Add("Trim", typeof(string));
                    csvDataTable.Columns.Add("Drive", typeof(string));
                    csvDataTable.Columns.Add("Doors", typeof(string));
                    csvDataTable.Columns.Add("Body", typeof(string));
                    csvDataTable.Columns.Add("Wholesale", typeof(string));
                    csvDataTable.Columns.Add("Retail", typeof(string));

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvDataTable.Rows.Add(fieldData);
                    }

                }
                if (SaveMainCSVFileData(csvDataTable)>0)
                {
                    mainFileStatusLabel.Text = "Imported to data table";
                }                               
            }
            else
            {
                mainFileStatusLabel.Text = "File not found.";
            }
        }

        public int SaveMainCSVFileData(DataTable tableCSV)
        {
            int rowsInserted = 0;

            for (int i = 0; i < tableCSV.Rows.Count; i++)
            {
                string code = tableCSV.Rows[i].ItemArray.GetValue(0).ToString();
                string type = tableCSV.Rows[i].ItemArray.GetValue(1).ToString();
                string year = tableCSV.Rows[i].ItemArray.GetValue(2).ToString();
                string make = tableCSV.Rows[i].ItemArray.GetValue(3).ToString();
                string model = tableCSV.Rows[i].ItemArray.GetValue(4).ToString();
                string trim = tableCSV.Rows[i].ItemArray.GetValue(5).ToString();
                string drive = tableCSV.Rows[i].ItemArray.GetValue(6).ToString();
                string doors = tableCSV.Rows[i].ItemArray.GetValue(7).ToString();
                string body = tableCSV.Rows[i].ItemArray.GetValue(8).ToString();
                decimal wholesale = Convert.ToDecimal(tableCSV.Rows[i].ItemArray.GetValue(9));
                decimal retail = Convert.ToDecimal(tableCSV.Rows[i].ItemArray.GetValue(10));

                string query = String.Format(@"Insert into tblTest values(@code,@type,@year,@make,@model,@trim,@drive,@doors,@body,@wholesale,@retail)");
                                                 
                                                // tableCSV.Rows[i].ItemArray.GetValue(2).ToString(), 
                                                // tableCSV.Rows[i].ItemArray.GetValue(3).ToString(),
                                                // tableCSV.Rows[i].ItemArray.GetValue(4).ToString(),
                                                //tableCSV.Rows[i].ItemArray.GetValue(5).ToString(),
                                                //tableCSV.Rows[i].ItemArray.GetValue(6).ToString(),
                                                //tableCSV.Rows[i].ItemArray.GetValue(7).ToString(),
                                                //tableCSV.Rows[i].ItemArray.GetValue(8).ToString(),
                                                //tableCSV.Rows[i].ItemArray.GetValue(9).ToString(),
                                                //tableCSV.Rows[i].ItemArray.GetValue(10).ToString()                                         
                                              
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@code",code);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@year", year);
                        command.Parameters.AddWithValue("@make", model);
                        command.Parameters.AddWithValue("@model", model);
                        command.Parameters.AddWithValue("@trim", trim);
                        command.Parameters.AddWithValue("@drive", drive);
                        command.Parameters.AddWithValue("@doors", doors);
                        command.Parameters.AddWithValue("@body", body);
                        command.Parameters.AddWithValue("@wholesale", wholesale);
                        command.Parameters.AddWithValue("@retail", retail);
                        connection.Open();
                        rowsInserted = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            return rowsInserted;

        }
    }
}