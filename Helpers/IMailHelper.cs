namespace SuperShop.Helpers
{
    using Azure;

    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}