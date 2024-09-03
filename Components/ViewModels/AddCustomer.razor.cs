using System.Data;
using BlazorApp.Exceptions;
using BlazorApp.Models;

namespace BlazorApp.Components.Pages{
    public partial class AddCustomer{
        private string firstname = "";
        private string lastname = "";
        private string email = "";
        private string phone = "";
        private string address = "";
        private string postcode = "";
        private string country = "";

        private string message = "";
        private Database database = Database.Instance;

        private void HandleValidSubmit(){
            try{
                var id = database.InsertCustomer(
                    firstName: firstname,
                    lastName: lastname,
                    email: email,
                    phone: phone,
                    customerAddress: address,
                    postcode: postcode.Trim().Replace(" ", ""),
                    country: country
                );
                message = "Customer added successfully";
            
            }catch(InvalidInputException e){
                message = e.Message;
            }catch(UnexpectedException e){
                message = e.Message;
            }
        }

        
    }
}
