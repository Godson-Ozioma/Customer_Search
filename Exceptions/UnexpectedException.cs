using System;
namespace BlazorApp.Exceptions
{
    public class UnexpectedException : Exception
    {
        public UnexpectedException():base(message: "An unexpected error has occured")
        {
            
        }
        // Constructor with a custom message
        public UnexpectedException(string message) 
            : base($"An unexpected error has occured: {message}")
        {
        }
    }
}