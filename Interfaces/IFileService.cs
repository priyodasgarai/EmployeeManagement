namespace Employee.Interfaces
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        public void DeleteImage(string imageFileName);
        public Task<bool> CheckImage(string imageFileName);
    }
}
