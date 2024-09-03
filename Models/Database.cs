using BlazorApp.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace BlazorApp.Models
{
    /// <summary>
    /// A singleton class to connect and interact with the database
    /// The sealed class and lazy instance is used to ensure thread safety
    /// </summary>
    public sealed partial class Database
    {
        private static readonly Lazy<Database> _instance = new Lazy<Database>(() => new Database());

        private readonly string _connectionString = "Server=localhost;Database=Sales;User Id=sa;Password=Godson.123;Encrypt=True;TrustServerCertificate=True;";
        private SqlConnection _sqlConnection;

        /// <summary>
        /// return the instance of this object
        /// </summary>
        public static Database Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        /// constructor
        /// connects to the sql server database and opens the connection
        /// </summary>
        private Database(){
            _sqlConnection = new SqlConnection(_connectionString);
            OpenConnection();
        }

        /// <summary>
        /// opens the connection to the sql server database
        /// this method is called in the constructor
        /// </summary>
        void OpenConnection(){
            try{
                if(_sqlConnection.State == ConnectionState.Closed){
                    _sqlConnection.Open();
                }
            }catch(Exception e){
                System.Console.WriteLine($"Error opening connection: {e.Message}");
            }
        }

        /// <summary>
        /// closes the connection to the database
        /// this method is called in the destructor
        /// </summary>
        void CloseConnection(){
            try
            {
                if (_sqlConnection.State == ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing connection: {ex.Message}");
            }
        }

        private bool ValidatePhone(string input){
            input = input.Trim().Replace(" ", "");
            return long.TryParse(input, out _) &&(input.Count() == 11);
        }

        /// <summary>
        /// Inserts a new customer to the db and returns the id of the inserted customer
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="customerAddress"></param>
        /// <param name="postCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public int InsertCustomer(string firstName, string lastName, 
                string email, string phone, string customerAddress, string postcode, string country)
        {
            if(!ValidatePhone(phone)){
                throw new InvalidInputException("The syntax of the phone number is invalid");
            }
            int newCustomerId = 0;
            try{
                // using var sqlConn = new SqlConnection(_connectionString);
                
                using var cmd = new SqlCommand("InsertIntoCustomer", _sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure; // specify that the command is a procedure

                // add paramaters
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                cmd.Parameters.AddWithValue("@Postcode", postcode);
                cmd.Parameters.AddWithValue("@Country", country);

                // update the customer id
                newCustomerId = Convert.ToInt32((decimal)cmd.ExecuteScalar()); // returns the identity column in the result set
            }
            catch(SqlException){
                throw new UnexpectedException(" Check if customer already exists");
            }

            // System.Console.WriteLine($"new customer id = {newCustomerId}");
            return newCustomerId;
        }

        /// <summary>
        /// Returns a list of custumer items that matches the given postcode and id
        /// </summary>
        /// <param name="postcode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<Customer.CustomerItem> GetCustomersByPostCodeOrId(string? postcode = null, int? id = null){
            if((postcode == null && id == null) || (postcode!.Count() == 0 && id == null)) throw new InvalidInputException("Input a value for postcode");
            if(Regex.IsMatch(postcode!, @"[^a-zA-Z0-9\s]")) throw new InvalidInputException("The postcode contains invalid character(s)");
            
            var customerList = new List<Customer.CustomerItem>();

            try{

                using SqlCommand cmd = new("SelectCustomer", _sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure; // specify that the command is a procedure
                // add parameters
                cmd.Parameters.AddWithValue("@CustomerID", (object?)id ?? DBNull.Value); // To ensure @CustomerID is treated as NULL
                cmd.Parameters.AddWithValue("@Postcode", (object?)postcode ?? DBNull.Value);

                using SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read()){
                    var customer = new Customer.CustomerItem{
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        FirstName = reader["FirstName"].ToString() ?? "",
                        LastName = reader["LastName"].ToString() ?? "",
                        Email = reader["Email"].ToString() ?? "",
                        Phone = reader["Phone"].ToString() ?? "",
                        Address = reader["CustomerAddress"].ToString() ?? "",
                        Postcode = reader["Postcode"].ToString() ?? "",
                        Country = reader["Country"].ToString() ?? ""
                    };
                    // Console.WriteLine($"Found: {customer}");
                    customerList.Add(customer);
                }
            }
            catch (SqlException)
            {
                throw new UnexpectedException("Error uploading the customer to the database");
            }

            
            return customerList;
        }

        /// <summary>
        /// Connection closed in the destructor
        /// </summary>
        ~Database(){
            CloseConnection();
        }
    }
}