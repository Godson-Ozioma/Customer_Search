
using BlazorApp.Exceptions;
using BlazorApp.Models;

namespace BlazorApp.Components.Pages
{
    public partial class Home
    {
        // reference to the object that interacts with the database
        private Database database = Database.Instance;
        private string query = "";
        private string message = "";
        private List<Customer.CustomerItem> customers = [];

        /// <summary>
        /// Updates the list 'customers' with users that match the inputed postcode
        /// </summary>
        public void GetUserByPostCode(){
            query = query.Trim().Replace(" ", ""); // remove whitespaces
            try{
                customers = database.GetCustomersByPostCodeOrId(query);
                if(customers == null || customers.Count() == 0){
                    message = "No customer lives around there. Kindly try another post code...";
                }else{
                    message = "";
                }
            }catch(InvalidInputException e){
                message = e.Message;
            }catch(UnexpectedException e){
                message = e.Message;
            }
        }
    }

}
