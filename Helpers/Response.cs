namespace Employee.Helpers
{
    public class Response
    {
        public Boolean Status { get; set; } = false;
        public string? Message { get; set; }
        public string[]? Data { get; set; } = null;
    }
}
