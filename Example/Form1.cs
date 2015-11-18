using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    { //delcare all objects/ variables
        private string connectionString;
        private DataViewManager dataViewManager;
        private DataSet dataSet;

        //constructor
        public Form1()
        {
            InitializeComponent();

            InitializeComponent();
            //sign the connection string value
            connectionString = "data source=THINKPAD-PC;initial catalog=northwind;Integrated Security=true";
            //create a new sql connection and put he connection string on it
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            //create the DataSet
            dataSet = new DataSet("custOrders");

            /*************************************************customers*******************************************/
            //Fill the dataset with customers, map the default table "Table" to "customers"
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter("select * from Customers", connection);

            //map
            dataAdapter1.TableMappings.Add("Table", "Customers");

            //fill the dataAdapter
            dataAdapter1.Fill(dataSet);

            /*************************************************Orders*******************************************/
            //Fill the dataset with Orders, map the default table "Table" to "Orders"
            SqlDataAdapter dataAdapter2 = new SqlDataAdapter("select * from Orders", connection);

            //map
            dataAdapter2.TableMappings.Add("Table", "Orders");

            //fill the dataAdapter
            dataAdapter2.Fill(dataSet);

            /*********************************************Order Details***********************************************/
            //Fill the dataset with Order Details, map the default table "Table" to "Order Details"
            SqlDataAdapter dataAdapter3 = new SqlDataAdapter("select * from [Order Details]", connection);

            //map
            dataAdapter3.TableMappings.Add("Table", "OrderDetails");

            //fill the dataAdapter
            dataAdapter3.Fill(dataSet);

            //give the relationship for the 3 tables(customers, orders, order details)
            //1a.  relationship between Customers & Orders
            DataRelation relateCustomerOrder;
            DataColumn colmaster1;
            DataColumn colDetail1;


            colmaster1 = dataSet.Tables["Customers"].Columns["CustomerID"];
            colDetail1 = dataSet.Tables["Orders"].Columns["CustomerID"];

            //2a. relate the customers and orders
            relateCustomerOrder = new DataRelation("RelCustomerOrder", colmaster1, colDetail1);
            dataSet.Relations.Add(relateCustomerOrder);
            /*-------------------------------------------------------*/
            //1b. relate the orders and order details
            DataRelation relateOrderOrderDetails;
            DataColumn colmaster2;
            DataColumn colDetail2;

            colmaster2 = dataSet.Tables["Orders"].Columns["OrderID"];
            //colDetail2 = dataSet.Tables["Order Details"].Columns["OrderID"];

            //2b. relate the customers and orders
            relateOrderOrderDetails = new DataRelation("RelOrderOrdDetails", colmaster2, colDetail2);
            dataSet.Relations.Add(relateOrderOrderDetails);


            //asign the detault view manage from dataset to dsView
            dataViewManager = dataSet.DefaultViewManager;

            //grid data binding
            grdOrder.DataSource = dataViewManager;
            grdOrder.DataMember = "Customers.RelCustomerOrder";

            grdDetails.DataSource = dataViewManager;
            grdOrder.DataMember = "Customers.RelOrderOrdDetails";

            //combobox
            cbCustName.DataSource = dataViewManager;
            cbCustName.DisplayMember = "Customers.CompanyName";
            cbCustName.ValueMember = "Customers.CustomerID";

            //txtbox binding
            txtContact.DataBindings.Add("Text", dataViewManager, "Customers.ContactName");
            txtPhone.DataBindings.Add("Text", dataViewManager, "Customers.Phone");
            txtFax.DataBindings.Add("Text", dataViewManager, "Customers.Fax");

        }

        private void buttonPre_Click(object sender, EventArgs e)
        {
            //check if in the first position
            if (this.BindingContext[dataViewManager, "Customers"].Position > 0)
            {
                this.BindingContext[dataViewManager, "Customers"].Position--; //decreament by 1
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            //check if in the last position, then you move
            CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[dataViewManager, "Customers"];
            if (currencyManager.Position < currencyManager.Count - 1)
            {
                currencyManager.Position++;
            }
        }
    }
}
