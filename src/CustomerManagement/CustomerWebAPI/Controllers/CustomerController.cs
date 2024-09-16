using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerWebAPI.Models;

namespace CustomerWebAPI.Controllers
{
    public class CustomerController : ApiController
    {
        List<Customer> customers;

		/// <summary>
		/// Get the list of customer from SQL Database
		/// </summary>
		/// <returns></returns>
		[HttpGet]
	    public IHttpActionResult GetCustomers()
        {
            customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString()
                    });
                }
            }
            return Ok(customers);
        }

		/// <summary>
		/// Get the customer using : api/Customer/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult GetCustomer(int id)
		{
			Customer customer = null;

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				SqlCommand cmd = new SqlCommand("SELECT * FROM Customers WHERE Id = @Id", conn);
				cmd.Parameters.AddWithValue("@Id", id);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					customer = new Customer
					{
						Id = (int)reader["Id"],
						Name = reader["Name"].ToString(),
						Email = reader["Email"].ToString(),
						PhoneNumber = reader["PhoneNumber"].ToString()
					};
				}
			}

			if (customer == null)
			{
				// Create a custom response message when customer is not found
				var response = new
				{
					Message = "Customer not found. Please check the ID and try again."
				};

				return Content(HttpStatusCode.NotFound, response);  // Return 404 with custom message
			}

			return Ok(customer);
		}

		/// <summary>
		/// Add a new customer into the database
		/// </summary>
		/// <param name="customer">Customer object</param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult AddCustomer([FromBody] Customer customer)
		{
			if (customer == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid data.");
			}

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				SqlCommand cmd = new SqlCommand("INSERT INTO Customers (Name, Email, PhoneNumber) VALUES (@Name, @Email, @PhoneNumber)", conn);
				cmd.Parameters.AddWithValue("@Name", customer.Name);
				cmd.Parameters.AddWithValue("@Email", customer.Email);
				cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);

				conn.Open();
				cmd.ExecuteNonQuery();
			}

			return Ok();  // Return success status
		}

		/// <summary>
		/// Update the existing custromer
		/// </summary>
		/// <param name="id">Customer Id</param>
		/// <param name="customer"></param>
		/// <returns></returns>
		[HttpPut]
		public IHttpActionResult UpdateCustomer(int id, [FromBody] Customer customer)
		{			
			if (customer == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid data or customer not found.");
			}

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				SqlCommand cmd = new SqlCommand("UPDATE Customers SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @Id", conn);
				cmd.Parameters.AddWithValue("@Id", id);
				cmd.Parameters.AddWithValue("@Name", customer.Name);
				cmd.Parameters.AddWithValue("@Email", customer.Email);
				cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);

				conn.Open();
				int rowsAffected = cmd.ExecuteNonQuery();
				if (rowsAffected == 0)
				{
					// Create a custom response message when customer is not found
					var response = new
					{
						Message = "Customer not found. Please check the ID and try again."
					};

					return Content(HttpStatusCode.NotFound, response);  // Return 404 with custom message
				}
			}

			return Ok();  // Return success status
		}

		/// <summary>
		/// Delete an existing cutomer
		/// </summary>
		/// <param name="id">Customer Id</param>
		/// <returns></returns>
		[HttpDelete]
		public IHttpActionResult DeleteCustomer(int id)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
			{
				SqlCommand cmd = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", conn);
				cmd.Parameters.AddWithValue("@Id", id);

				conn.Open();
				int rowsAffected = cmd.ExecuteNonQuery();
				if (rowsAffected == 0)
				{
					// Create a custom response message when customer is not found
					var response = new
					{
						Message = "Customer not found. Please check the ID and try again."
					};

					return Content(HttpStatusCode.NotFound, response);  // Return 404 with custom message
				}
			}

			return Ok();  // Return success status
		}
	}
}
