using Employee.Interfaces;

namespace Employee.Repository
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void DeleteImage(string imageFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(imageFileName))
                {
                    throw new ArgumentNullException(nameof(imageFileName));
                }
                var ContentPath = _environment.ContentRootPath;
                var path = Path.Combine(ContentPath, "Uploads\\", imageFileName);
                /*   if (File.Exists(path))
                   {
                       File.Delete(path);
                   }
                   */

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Invalid file path");
                }
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckImage(string imageFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(imageFileName))
                {
                    return false;
                }
                var ContentPath = _environment.ContentRootPath;
                var path = Path.Combine(ContentPath, "Uploads\\", imageFileName);
                if (File.Exists(path))
                {
                    return true;
                }
                else
                {
                    return false;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null)
                {
                    throw new ArgumentNullException(nameof(imageFile));
                }
                var contentPath = _environment.ContentRootPath;

                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".JPG", ".png", ".jpeg", ".pdf", ".doc" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();

                //We are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);

            }
            catch (Exception e)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }
    }
}
