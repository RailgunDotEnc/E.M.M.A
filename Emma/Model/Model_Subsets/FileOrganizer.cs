using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Emma.Model.Model_Subsets
{
    public class FileOrganizer
    {
        private string DownloadOrigin;
        private string userDirectory=Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private string PictureDirectory=Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private string desktopDirectory=Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string DownloadNew;
        private string[] downloadfiles;
        private string[] folder_in;
        private Memory memory;
        private settings settings;
        private string[] imageTypes = { "jpg","png","gif","webp", "tiff", "psd", "raw", "bmp", "heif", "indd",
                                        "jpeg 2000", "svg", "ai", "krita", "pdf", "ico","kra","pdf"};


        //Constructor
        public FileOrganizer(Memory mem, settings set)
        {
            memory = mem;
            settings = set;
            updateDirectries();
        }

        //Organize files
        public void organize() {
            string type;
            string cur;
            string loc;
            //If setting is turned off
            if (settings.get_organizeFolders() == false)
                return;
            folder_in = Directory.GetFiles(DownloadNew);
            //Move all files in emmasdownloads/new to correct location
            for (int i = 0;i<folder_in.Length;i++) {
                cur = folder_in[i];
                type = filetype(cur);
                loc = DownloadNew+"/"+type;
                //Move to picture folder
                if(isImg(type))
                    loc = PictureDirectory + "/"+type;
                MoveFile(loc, cur,type);
            }

            downloadfiles = Directory.GetFiles(DownloadOrigin);
            //Move all files in download to emmasdownloads/new
            for (int i = 0; i < downloadfiles.Length; i++) {
                cur = downloadfiles[i];
                loc = DownloadNew+"/new";
                type = filetype(cur);
                if (isFile(cur))               
                    MoveFile(loc,cur,type);
            }
            
        }

        //Find users download folder
        private void updateDirectries() {
            //Assign locations
            DownloadOrigin = userDirectory+"/Downloads";
            try
            {
                MakeFolder(desktopDirectory, "/EmmaDownloads");
                MakeFolder(desktopDirectory, "/EmmaDownloads/new");
                DownloadNew = desktopDirectory + "/EmmaDownloads";
            }
            catch {
                DownloadNew = desktopDirectory + "/EmmaDownloads";
            }
            try
            {
                //Check to make sure directories exist and work
                folder_in = Directory.GetFiles(DownloadOrigin);
                folder_in = Directory.GetFiles(DownloadNew);
                folder_in=Directory.GetFiles(PictureDirectory);
                folder_in = Directory.GetFiles(DownloadNew + "/new");
                organize();
            }
            catch(Exception e) {
                memory.SaveData("File location error: "+e.Message);
            }
        }



        //Delete files outside of persondal download folder
        private void deleteFiles() { 
            
        }

        //Check File type
        private string filetype(string file) {
            int size=file.Length;
            string extention = "";
            file = file.ToLower();
            for (int i = size-1; i > 0; i--) {
                if (file[i] == '.')
                    break;
                extention = file[i]+extention;
            }
            return extention;
        }

        //Check if File or file directory
        private bool isFile(String value) {
            FileAttributes type = File.GetAttributes(value);
            //detect whether its a directory or file
            if ((type & FileAttributes.Directory) == FileAttributes.Directory)
                return false;
            else
                return true;
        }

        //Check if File is an image
        private bool isImg(string type) {
            if(imageTypes.Contains(type.ToLower()))
                return true;
            return false;
        }

        //Make folder
        private void MakeFolder(string dir, string type) {
            Directory.CreateDirectory(dir + "/" + type);
        }

        //Move file to correct folder
        private void MoveFile(string dir, string file, string type) {
            string filename = "";
            if (type.Equals("")) {
                memory.SaveData("No File extention: " + file);
                return;
            }
            for (int i = file.Length - 1; i > 0; i--) {
                if (file[i] == '/' || file[i]=='\\')
                    break;
                filename=file[i]+filename;
            }
            string from = file;
            string to = dir+"/"+filename;
            try
            {
                File.Move(from, to);
            }
            //If file is missing a folder to be placed in
            catch (System.IO.DirectoryNotFoundException)
            {
                MakeFolder(dir,"");
                File.Move(from, to);
            }
            catch (System.IO.IOException e)
            {
                //if file exist remove and replace
                if (e.Message.Contains("already exist"))
                {
                    memory.SaveData("File Exist error: " + filename);
                    File.Delete(to);
                    File.Move(from, to);
                }
                //Unknown error
                else {
                    memory.SaveData("File error: "+e.Message);
                }
            }
        }
    }
}
