using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace CashRegisterSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int fakevar = 0;
        double totalsum = 0.0;
        double total1st = 0.0;
        double total2st = 0.0;
        int a = 1;
        string b = "";
        double fulltotal = 0.0;
        string exceptword = "Not Availabale";

        DataTable table = new DataTable();
        DataTable ordertable = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {

            FormClosing += new FormClosingEventHandler(Form1_Closing);
            button1.Click += new EventHandler(button1_Click);

            table.Columns.AddRange(new DataColumn[4] { new DataColumn("Name", typeof(string)),
                        new DataColumn("Price", typeof(string)),
                        new DataColumn("Amount",typeof(string)),
                        new DataColumn("Description", typeof(string))});

            dataEditWindow.DataSource = table;
            this.dataEditWindow.DataSource = table;
            this.dataEditWindow.AllowUserToAddRows = false;
            this.dataEditWindow.Columns["Description"].Visible = false;

            ordertable.Columns.AddRange(new DataColumn[4] { new DataColumn("Name", typeof(string)),
                        new DataColumn("Price", typeof(string)),
                        new DataColumn("Amount", typeof(string)),
                        new DataColumn("RowNum", typeof(string))});
            dataOrderWindow.DataSource = ordertable;
            this.dataOrderWindow.DataSource = ordertable;
            this.dataOrderWindow.AllowUserToAddRows = false;

            this.dataOrderWindow.Columns["RowNum"].Visible = false;

            foreach (DataGridViewColumn col in dataEditWindow.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn col in dataOrderWindow.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (new FileInfo(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt").Length == 0)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(a != 0)
            {
                button3.Text = "REFRESH";
            }
            if(table != null)
            {
                table.Clear();
            }



            string[] exceptlines = File.ReadAllLines(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\OldProductList.txt");
            string[] exceptvalues;
            
            for(int i = 0; i < exceptlines.Length; i++)
            {

                exceptvalues = exceptlines[i].ToString().Split('/');
                int intexval = 0;

                for (int j = 0; j < exceptvalues.Length; j++)
                {
                    string exceptionvalue = exceptvalues[2];
                    
                    bool exceptbool = String.Equals(exceptionvalue, exceptword, StringComparison.InvariantCulture);
                    if (exceptbool)
                    {

                    } else { 
                    intexval = Int32.Parse(exceptionvalue);
                    }
                    if (intexval <= 0)
                    {
                        exceptvalues[2] = "Not Availabale";
                        string exceptresult = string.Join("/", exceptvalues);

                        exceptlines[i] = exceptresult;
                        File.WriteAllText(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\OldProductList.txt", string.Join("\n", exceptlines));
                    }
                }
            }
            




            string[] lines = File.ReadAllLines(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\OldProductList.txt");
            string[] values;

            for (int i = 0; i < lines.Length; i++)
            {
                values = lines[i].ToString().Split('/');
                string[] row = new string[values.Length];

                for (int j = 0; j < values.Length; j++)
                {
                    row[j] = values[j].Trim();
                }
                table.Rows.Add(row);
                a++;
            }
        }

        private void dataEditWindow_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Enabled = true;
                button2.Enabled = true;
                DataGridViewRow row = dataEditWindow.Rows[e.RowIndex];
                txtName.Text = row.Cells[0].Value.ToString();
                b = row.Cells[1].Value.ToString();
                txtPrice.Text = row.Cells[1].Value.ToString();
                txtAmount.Text = row.Cells[2].Value.ToString();
                bool CMCExCheck = String.Equals(exceptword, txtAmount.Text, StringComparison.InvariantCulture);
                if (CMCExCheck)
                {
                    textBox1.Enabled = false;
                    button2.Enabled = false;
                    button1.Enabled = false;
                }
                string totam = textBox1.Text;
                bool b1 = string.IsNullOrEmpty(totam);
                if (b1)
                {
                    total.Text = "0";
                    button2.Enabled = false;
                }
                else
                {
                    total1st = Int32.Parse(textBox1.Text);
                    total2st = Double.Parse(txtPrice.Text, CultureInfo.InvariantCulture);
                    totalsum = total1st * total2st;
                    total.Text = $"{totalsum}";
                }
            }
        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

            string totam = textBox1.Text;
            bool b1 = string.IsNullOrEmpty(totam);
           
            if (b1)
            {
                total.Text = "0";
                button2.Enabled = false;

            }
            else
            {
                if (Int32.Parse(textBox1.Text) > Int32.Parse(txtAmount.Text))
                {
                    MessageBox.Show("Your value is out of the range of available items!");
                    button2.Enabled = false;
                }
                else
                {
                    button2.Enabled = true;
                    total1st = Int32.Parse(textBox1.Text);
                    total2st = Double.Parse(txtPrice.Text, CultureInfo.InvariantCulture); 
                    totalsum = total1st * total2st;
                    total.Text = $"{totalsum}";
                }
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        private string GetTheOrder()
        {
            var order = txtName.Text + '/' + txtPrice.Text + '/' + textBox1.Text + '/' + dataEditWindow.CurrentCell.RowIndex;
            return order;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var columnIndexR = dataEditWindow.CurrentCell.ColumnIndex;
            var rowIndexR = dataEditWindow.CurrentCell.RowIndex;


            if (ordertable != null)
            {
                ordertable.Clear();
            }
            string fileName = @"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt";
            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    fakevar = 2;
                }
            }
            string Path = Environment.CurrentDirectory + "\\ram.txt";
            using (var OrderList = File.AppendText(fileName))
            {
                OrderList.WriteLine(GetTheOrder());
            }

            string[] lines2 = File.ReadAllLines(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt");
            string[] values2;

            for (int i = 0; i < lines2.Length; i++)
            {
                values2 = lines2[i].ToString().Split('/');
                string[] row = new string[values2.Length];

                for (int j = 0; j < values2.Length; j++)
                {
                    row[j] = values2[j].Trim();
                }
                ordertable.Rows.Add(row);
                a++;
            }

            fulltotal = fulltotal + totalsum;
            FullTotal.Text = $"{fulltotal}";
            textBox1.Clear();
            total.Text = "0";

            if (new FileInfo(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt").Length == 0)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.WriteAllText(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt", string.Empty);
            if (ordertable != null)
            {
                ordertable.Clear();
            }
            fulltotal = 0;
            FullTotal.Clear();
            if (new FileInfo(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt").Length == 0)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "My Application", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            } else
            {
                File.WriteAllText(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt", string.Empty);
                if (ordertable != null)
                {
                    ordertable.Clear();
                }
                fulltotal = 0;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string dir = @"C:\Users\Stanislav\source\repos\CashRegisterSystem\CashRegisterSystem\Receipts";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                button1_Click(sender, e);
            } else
            {
                string nameofthefile = dir + "\\" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss", new System.Globalization.CultureInfo("en-US")) + ".txt";
                using (TextWriter tw = new StreamWriter(nameofthefile))
                {
                    string grandtotal = "Total: " + FullTotal.Text;
                    tw.WriteLine(grandtotal);
                    string instruction = "The information below is NAME | PRICE | AMOUNT | ID NUMBER IN THE TABLE";
                    tw.WriteLine(instruction);
                    for (int i = 0; i < dataOrderWindow.Rows.Count; i++)
                        {
                          for (int j = 0; j < dataOrderWindow.Columns.Count; j++)
                          {
                              tw.Write($"{dataOrderWindow.Rows[i].Cells[j].Value.ToString()}");

                              if (j != dataOrderWindow.Columns.Count - 1)
                              {
                                  tw.Write("/");
                              }
                          }
                          tw.WriteLine();
                    }
                }
                string[] OrderListFileLines = File.ReadAllLines(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\ram.txt");
                string PathOldProductFile = @"C:\Users\Stanislav\source\repos\CashRegisterSystem\OldProductList.txt";
                string PathNewProductFile = @"C:\Users\Stanislav\source\repos\CashRegisterSystem\NewProductList.txt";

                File.Copy(PathOldProductFile, PathNewProductFile, true);
                
               if(fakevar != 1) { 
                    for (int i = 0; i <= OrderListFileLines.Length - 1; i++)
                    {
                        
                        string[] OrderListFileValues = OrderListFileLines[i].ToString().Split('/');
                        string currentamount = OrderListFileValues[2];
                        string currentstringID = OrderListFileValues[3];

                        int ID = Int32.Parse(currentstringID);

                        string[] PathNewProductFileLines = File.ReadAllLines(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\NewProductList.txt");

                        string[] PathNewProductFileValues = PathNewProductFileLines[ID].ToString().Split('/');
                        string oldamount = PathNewProductFileValues[2];
                        int intoldamount = Int32.Parse(oldamount);
                        int intcurrentamount = Int32.Parse(currentamount);
                        int inttotal = intoldamount - intcurrentamount;

                        PathNewProductFileValues[2] = inttotal.ToString();

                        string result = string.Join("/", PathNewProductFileValues);

                        PathNewProductFileLines[ID] = result;
                        File.WriteAllText(@"C:\Users\Stanislav\source\repos\CashRegisterSystem\NewProductList.txt", string.Join("\n", PathNewProductFileLines));
                       
                        
                    }
                }
                File.Copy(PathNewProductFile, PathOldProductFile, true);
                fakevar++;
                if (fakevar == 2)
                {
                    fakevar = 0;
                    if (ordertable != null)
                    {
                        ordertable.Clear();
                    }
                    fulltotal = 0;
                    FullTotal.Clear();
                }
                button3_Click(sender, e);
            }
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) //Checking if only number and delete.
            {
                e.Handled = true;
            }
           
        }
    }
}
