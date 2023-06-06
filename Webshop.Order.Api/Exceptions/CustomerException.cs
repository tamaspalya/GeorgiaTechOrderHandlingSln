namespace Webshop.Order.Api.Exceptions
{
    public class CustomerException : Exception
    {
        private string result;

        public CustomerException(string result) : base($"Customer Exception: {result}")
        {
            this.result = result;
        }

        public string GetResult()
        {
            return result;
        }
    }
}