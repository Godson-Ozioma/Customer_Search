
namespace BlazorApp.Models
{
    public class Customer
    {
        public class CustomerItem
    {
        public required int CustomerID {get; set;}
        public required string FirstName {get; set;}
        public required string LastName {get; set;}
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string Postcode {get; set;}
        public required string Country {get; set;}


        public override string ToString(){
            return $"CustomerID => {CustomerID} "+
            $"FirstName => {FirstName} "+
            $"LastName => {LastName} "+
            $"Email => {Email} "+
            $"Phone => {Phone} "+
            $"Address => {Address} "+
            $"Postcode => {Postcode} "+
            $"Country => {Country}";
        }
    }

    public class CustomerList
    {
        private List<CustomerItem> customers = new List<CustomerItem>();

        public List<CustomerItem> GetAllCustomers()
        {
            return customers;
        }

        public void AddCustomer(CustomerItem customer)
        {
            customers.Add(customer);
        }
    }
    }
}