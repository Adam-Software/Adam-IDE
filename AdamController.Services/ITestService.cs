namespace AdamController.Services
{
    public interface ITestService
    {
        public string GetMessage(); 
    }


    public class TestService : ITestService
    {
        public string GetMessage()
        {
            return "Is test message";
        }
    }
}
