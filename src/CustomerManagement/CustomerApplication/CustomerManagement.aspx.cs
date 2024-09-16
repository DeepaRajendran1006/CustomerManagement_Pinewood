using CustomerWebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerApplication
{
	public partial class CustomerManagement : System.Web.UI.Page
	{
		#region private variables

		private static readonly string apiUrl = "https://localhost:44348/api/Customer";

		#endregion

		#region Protected Methods

		/// <summary>
		/// Load the list of existing customers when the page is loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected async void Page_Load(object sender, EventArgs e)
		{
			lblError.Visible = false;
			if (!IsPostBack)
			{
				btnUpdate.Enabled = false;
				await LoadCustomers();
			}
		}

		/// <summary>
		/// Load the existing list of customers and bind to grid view
		/// </summary>
		/// <returns></returns>
		protected async Task LoadCustomers()
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("https://localhost:44348/"); // Web API URL
				HttpResponseMessage responseMessage = await client.GetAsync(apiUrl);
				if (responseMessage.IsSuccessStatusCode)
				{
					var customerData = await responseMessage.Content.ReadAsStringAsync();
					var customers = JsonConvert.DeserializeObject<List<Customer>>(customerData);
					gvCustomers.DataSource = customers;
					gvCustomers.DataBind();
				}
				else
				{
					// Log error details if the response is not successful
					var errorContent = await responseMessage.Content.ReadAsStringAsync();
					Console.WriteLine("Error: " + errorContent);
				}
			}
		}

		/// <summary>
		/// GridView Command (Edit or Delete the customer)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
			clearData();
			int customerId = Convert.ToInt32(e.CommandArgument);
			if (e.CommandName == "EditCustomer")
			{
				btnUpdate.Enabled = true;
				EditCustomer(customerId);
			}
			else if (e.CommandName == "DeleteCustomer")
			{
				btnUpdate.Enabled = false;
				DeleteCustomer(customerId);
			}
		}

		/// <summary>
		///  Add a new customer (POST)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected async void btnAdd_Click(object sender, EventArgs e)
		{
			lblError.Visible = false;
			if (Page.IsValid)
			{
				// Validate the customer before add whether if any existis with the same name and email / phone number
				if (!validateCustomer(txtName.Text, txtEmailId.Text, txtPhoneNumber.Text))
				{
					var newCustomer = new Customer
					{
						Name = txtName.Text,
						Email = txtEmailId.Text,
						PhoneNumber = txtPhoneNumber.Text
					};

					using (HttpClient client = new HttpClient())
					{
						var content = new StringContent(JsonConvert.SerializeObject(newCustomer), System.Text.Encoding.UTF8, "application/json");
						HttpResponseMessage response = await client.PostAsync(apiUrl, content);
						if (response.IsSuccessStatusCode)
						{
							await LoadCustomers();  // Reload the customers list
							clearData();
						}
					}
				}
				else
				{
					lblError.Visible = true;
				}
			}
		}

		/// <summary>
		/// Clear the customer list(POST)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnClear_Click(object sender, EventArgs e)
		{
			btnUpdate.Enabled = false;
			//clear the form data
			clearData();
		}

		/// <summary>
		/// Update the selected customer (PUT)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected async void btnUpdate_Click(object sender, EventArgs e)
		{
			lblError.Visible = false;
			if (Page.IsValid)
			{
				// Get the customer id
				var customerId = Convert.ToInt32(ViewState["CustomerId"]);

				// Validate the customer
				if (!validateCustomer(txtName.Text, txtEmailId.Text, txtPhoneNumber.Text, customerId))
				{					
					var updatedCustomer = new Customer
					{
						Id = customerId,
						Name = txtName.Text,
						Email = txtEmailId.Text,
						PhoneNumber = txtPhoneNumber.Text
					};

					using (HttpClient client = new HttpClient())
					{
						var content = new StringContent(JsonConvert.SerializeObject(updatedCustomer), System.Text.Encoding.UTF8, "application/json");
						HttpResponseMessage response = await client.PutAsync($"{apiUrl}/{customerId}", content);
						if (response.IsSuccessStatusCode)
						{
							await LoadCustomers();
							clearData();
						}
					}
				}
				else
				{
					lblError.Visible = true;
				}
			}
			btnUpdate.Enabled = false;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Clear the form data
		/// </summary>
		private void clearData()
		{
			txtName.Text = "";
			txtEmailId.Text = "";
			txtPhoneNumber.Text = "";
			lblError.Text = "";
			lblError.Visible = false;
		}

		/// <summary>
		/// Validate the customer while adding / updating
		/// </summary>
		/// <param name="name">Customer name</param>
		/// <param name="email">Email Id</param>
		/// <param name="phoneNumber">Phone number</param>
		/// <param name="customerId">Customer Id</param>
		/// <returns>true / false</returns>
		private bool validateCustomer(string name, string email, string phoneNumber, int customerId = 0)
		{
			foreach (GridViewRow row in gvCustomers.Rows)
			{
				int rowIndex = row.RowIndex;
				// Get the Id, Name, Email and Phone from the grid view
				int existingId = int.Parse(gvCustomers.DataKeys[rowIndex].Values[0].ToString()); //int.Parse(row.Cells[0].Text);
				string existingName = row.Cells[1].Text;
				string existingEmail = row.Cells[2].Text;
				string existingPhone = row.Cells[3].Text;

				if ((customerId == 0) || (existingId != customerId))
				{
					if (existingName.Equals(name, StringComparison.OrdinalIgnoreCase) && existingEmail.Equals(email, StringComparison.OrdinalIgnoreCase))
					{
						lblError.Text = "Customer with the same name and email already exists.";
						return true;
					}
					else if (existingName.Equals(name, StringComparison.OrdinalIgnoreCase) && existingPhone.Equals(phoneNumber, StringComparison.OrdinalIgnoreCase))
					{
						lblError.Text = "Customer with the same name and phone number already exists.";
						return true;
					}
				}				
			}

			return false;
		}

		/// <summary>
		/// Edit the existing customer (GET by ID)
		/// </summary>
		/// <param name="customerId">Customer Id</param>
		private async void EditCustomer(int customerId)
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync($"{apiUrl}/{customerId}");
				if (response.IsSuccessStatusCode)
				{
					var customerData = await response.Content.ReadAsStringAsync();
					var customer = JsonConvert.DeserializeObject<Customer>(customerData);
					txtName.Text = customer.Name;
					txtEmailId.Text = customer.Email;
					txtPhoneNumber.Text = customer.PhoneNumber;

					ViewState["CustomerId"] = customer.Id;  // Store the customer ID for updates
				}
			}
		}

		/// <summary>
		/// Delete the customer (DELETE)
		/// </summary>
		/// <param name="customerId">Customer Id</param>
		private async void DeleteCustomer(int customerId)
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/{customerId}");
				if (response.IsSuccessStatusCode)
				{
					await LoadCustomers();  // Reload the customers list
					clearData();
				}
			}
		}

		#endregion 
	}
}