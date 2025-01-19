using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace My_store.Pages.clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public ClientInfo NewClient = new ClientInfo();
        public string ErrorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=(localdb)\\Local;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo
                                {
                                    id = reader.GetInt32(0).ToString(),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    address = reader.GetString(4),
                                    created_at = reader.GetDateTime(5).ToString()
                                };
                                listClients.Add(clientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public void OnPost()
        {
            NewClient.name = Request.Form["name"];
            NewClient.email = Request.Form["email"];
            NewClient.phone = Request.Form["phone"];
            NewClient.address = Request.Form["address"];

            if (string.IsNullOrEmpty(NewClient.name) || string.IsNullOrEmpty(NewClient.email))
            {
                ErrorMessage = "Name and Email are required!";
                return;
            }

            try
            {
                string connectionString = "Data Source=(localdb)\\Local;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients (name, email, phone, address, created_at) VALUES (@name, @email, @phone, @address, GETDATE())";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", NewClient.name);
                        command.Parameters.AddWithValue("@email", NewClient.email);
                        command.Parameters.AddWithValue("@phone", NewClient.phone);
                        command.Parameters.AddWithValue("@address", NewClient.address);

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "New client added successfully!";
                NewClient = new ClientInfo(); 
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }
        }
    }

    public class ClientInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
}
