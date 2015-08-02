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
    public partial class Home : System.Web.UI.Page
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
                   


                    csvReader.SetDelimiters(new string[]{","});
                    csvReader.HasFieldsEnclosedInQuotes = false;
                    string[] colFields = csvReader.ReadFields();                   
                    
                    csvDataTable.Columns.Add("Code", typeof (string));
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
                if (InsertMainFile(csvDataTable) > 0)
                {
                    //mainFileStatusLabel.Text = "Imported to data base";
                    if (renameMainFileTable() == -1)
                    {
                        mainFileStatusLabel.Text = "Imported to data base";
                    }
                    else
                    {
                        mainFileStatusLabel.Text = "Not renaimed";
                    }
                }
                else
                {
                    mainFileStatusLabel.Text = "Not imported";
                }
               
            }
            else
            {
                mainFileStatusLabel.Text = "File not found.";
            }
        }


        protected void importAddFileButton_Click(object sender, EventArgs e)
        {
            string fileName = @"~/App_Data/" + addFileTextBox.Text;

            if (File.Exists(Server.MapPath(fileName)))
            {
                DataTable csvDataTable = new DataTable();
                using (TextFieldParser csvReader = new TextFieldParser(Server.MapPath(fileName)))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;                   
                    csvDataTable.Columns.Add("Code", typeof(string));
                    csvDataTable.Columns.Add("Description", typeof(string));
                    csvDataTable.Columns.Add("Value", typeof(string));
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        string code = null;
                        string description = null;
                        string value = null;                       
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            code = fieldData[0];
                            if (fieldData[i] == String.Empty)
                            {
                                break;
                            }
                            else
                            {
                                if (i != 0 && i % 2 == 1)
                                {
                                    description = fieldData[i];
                                }
                                else if (i != 0 && i % 2 == 0)
                                {
                                    value = fieldData[i];
                                    csvDataTable.Rows.Add(code, description, value);
                                }
                            }
                        }
                    }
                }
                if (InsertAddFile(csvDataTable) > 0)
                {
                   
                    if (renameAddFileTable() == -1)
                    {
                        addFileStatusLabel.Text = "Imported to data base";
                    }
                    else
                    {
                        addFileStatusLabel.Text = "Not renaimed";
                    }
                }
                else
                {
                    addFileStatusLabel.Text = "Not imported";
                }
               
               
            }
            else
            {
                addFileStatusLabel.Text = "File not found.";
            }
        }

        protected void importDeductFileButton_Click(object sender, EventArgs e)
        {
            string fileName = @"~/App_Data/" + deductFileTextBox.Text;

            if (File.Exists(Server.MapPath(fileName)))
            {
                DataTable csvDataTable = new DataTable();
                using (TextFieldParser csvReader = new TextFieldParser(Server.MapPath(fileName)))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    csvDataTable.Columns.Add("Code", typeof(string));
                    csvDataTable.Columns.Add("Description", typeof(string));
                    csvDataTable.Columns.Add("Value", typeof(string));
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        string code = null;
                        string description = null;
                        string value = null;
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            code = fieldData[0];
                            if (fieldData[i] == String.Empty)
                            {
                                break;
                            }
                            else
                            {
                                if (i != 0 && i % 2 == 1)
                                {
                                    description = fieldData[i];
                                }
                                else if (i != 0 && i % 2 == 0)
                                {
                                    value = fieldData[i];
                                    csvDataTable.Rows.Add(code, description, value);
                                }
                            }
                        }
                    }
                }
                if (InsertDeductFile(csvDataTable) > 0)
                {

                    if (renameDeductFileTable() == -1)
                    {
                        deductFileStatusLabel.Text = "Imported to data base";
                    }
                    else
                    {
                        deductFileStatusLabel.Text = "Not renaimed";
                    }
                }
                else
                {
                    deductFileStatusLabel.Text = "Not imported";
                }


            }
            else
            {
                deductFileStatusLabel.Text = "File not found.";
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

                string query = String.Format(@"Insert into tblMain values(@code,@type,@year,@make,@model,@trim,@drive,@doors,@body,@wholesale,@retail)");

                                                

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@code", code);
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

        //// for add file
        public int InsertAddFile(DataTable addDataTable)
        {
            string addFileTableName = "tblAddFile_temp";

            if (CreateAddTable() == -1)
            {
                using (SqlConnection connection1 = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
                {
                    using (SqlBulkCopy cpyBulkCopy = new SqlBulkCopy(connection1))
                    {
                        connection1.Open();
                        cpyBulkCopy.DestinationTableName = addFileTableName;
                        cpyBulkCopy.WriteToServer(addDataTable);
                        connection1.Close();
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }


        }


        public int CreateAddTable()
        {
            int tableCreated = 0;
            string query = String.Format(@"
                                            CREATE TABLE [dbo].[tblAddFile_temp](
	                                        [code] [varchar](max) NULL,
	                                        [description] [varchar](max) NULL,
	                                        [value] [varchar](max) NULL
                                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                                         ");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    tableCreated = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return tableCreated;
        }

        public int renameAddFileTable()
        {
            int renameStatus = 0;
            string query = String.Format(@"EXEC sp_rename 'tblAddFile', 'tblAddFile_DROP';
                                            EXEC sp_rename 'tblAddFile_temp', 'tblAddFile';
                                            DROP TABLE [dbo].[tblAddFile_DROP];
                                            ");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    renameStatus = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return renameStatus;
        }
        //// for deduct file

        public int InsertDeductFile(DataTable deductDataTable)
        {
            string deductTableName = "tblDeductFile_temp";

            if (CreateDeductTable() == -1)
            {
                using (SqlConnection connection1 = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
                {
                    using (SqlBulkCopy cpyBulkCopy = new SqlBulkCopy(connection1))
                    {
                        connection1.Open();
                        cpyBulkCopy.DestinationTableName = deductTableName;
                        cpyBulkCopy.WriteToServer(deductDataTable);
                        connection1.Close();
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }


        }

        public int CreateDeductTable()
        {
            int tableCreated = 0;
            string query = String.Format(@"
                                            CREATE TABLE [dbo].[tblDeductFile_temp](
	                                        [code] [varchar](max) NULL,
	                                        [description] [varchar](max) NULL,
	                                        [value] [varchar](max) NULL
                                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]                                        
                                            ");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    tableCreated = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return tableCreated;
        }

        public int renameDeductFileTable()
        {
            int renameStatus = 0;
            string query = String.Format(@"EXEC sp_rename 'tblDeductFile', 'tblDeductFile_DROP';
                                            EXEC sp_rename 'tblDeductFile_temp', 'tblDeductFile';
                                            DROP TABLE [dbo].[tblDeductFile_DROP];
                                            ");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    renameStatus = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return renameStatus;
        }

        //// main file saving.....
        public int InsertMainFile(DataTable mainDataTable)
        {
            string mainTableName = "tblMainFile_temp";

            if (CreateMainTable() == -1)
            {
                using (SqlConnection connection1 = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
                {
                    using (SqlBulkCopy cpyBulkCopy = new SqlBulkCopy(connection1))
                    {
                        connection1.Open();
                        cpyBulkCopy.DestinationTableName = mainTableName;
                        cpyBulkCopy.WriteToServer(mainDataTable);
                        connection1.Close();
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }


        }

        //SET ANSI_NULLS ON
        //                                    GO

        //                                    SET QUOTED_IDENTIFIER ON
        //                                    GO

        //                                    SET ANSI_PADDING ON
        //                                    GO


         //GO

         //                                   SET ANSI_PADDING OFF
         //                                   GO
        public int CreateMainTable()
        {
            int tableCreated = 0;
            string query = String.Format(@"
                                            CREATE TABLE [dbo].[tblMainFile_temp](
	                                            [Code] [varchar](300) NULL,
	                                            [Type] [varchar](100) NULL,
	                                            [Year] [varchar](100) NULL,
	                                            [Make] [varchar](100) NULL,
	                                            [Model] [varchar](100) NULL,
	                                            [Trim] [varchar](100) NULL,
	                                            [Drive] [varchar](50) NULL,
	                                            [Doors] [varchar](50) NULL,
	                                            [Body] [varchar](50) NULL,
	                                            [Wholesale] [decimal](12, 2) NULL,
	                                            [Retail] [decimal](12, 2) NULL
                                            ) ON [PRIMARY]                                          
                                            ");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    tableCreated = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return tableCreated;
        }

        public int renameMainFileTable()
        {
            int renameStatus = 0;
            string query = String.Format(@"EXEC sp_rename 'tblMainFile', 'tblMainFile_DROP';
                                            EXEC sp_rename 'tblMainFile_temp', 'tblMainFile';
                                            DROP TABLE [dbo].[tblMainFile_DROP];
                                            ");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    connection.Open();
                    renameStatus = command.ExecuteNonQuery(); 
                    connection.Close();
                }
            }
            return renameStatus;
        }
    }
}