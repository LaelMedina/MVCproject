using MathNet.Numerics;

namespace MVCproyect.Services
{
    public class FileStorageService
    {
        public void createFolder(string folderName) 
        {
            string path_ = "C:\\Users\\Usuario\\Desktop\"" + folderName;

            if (!Directory.Exists(path_)) 
            {
                Directory.CreateDirectory(path_);
            }
        }
    }
}
