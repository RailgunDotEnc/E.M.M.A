using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Emma.Model.Model_Subsets
{
    public class FileOrganizer
    {
        private string CurDirectory;
        private string DownloadOrigin;
        private string userfile;
        private string DownloadNew;
        private string imageloc;
        private string[] downloadfiles;
        private string[] folder_in;
        private Memory memory;
        private settings settings;
        private string[] imageTypes = { "jpg","png","gif","webp", "tiff", "psd", "raw", "bmp", "heif", "indd",
                                        "jpeg 2000", "svg", "ai", "krita", "pdf", "ico","kra","pdf"};


        //Constructor
        public FileOrganizer(string directory, Memory mem, settings set)
        {
            CurDirectory = directory;
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
                loc = DownloadNew+"\\"+type;
                //Move to picture folder
                if(isImg(type))
                    loc = imageloc+"\\"+type;
                MoveFile(loc, cur,type);
            }

            downloadfiles = Directory.GetFiles(DownloadOrigin);
            //Move all files in download to emmasdownloads/new
            for (int i = 0; i < downloadfiles.Length; i++) {
                cur = downloadfiles[i];
                loc = DownloadNew+"\\new";
                type = filetype(cur);
                if (isFile(cur))               
                    MoveFile(loc,cur,type);
            }
            
        }

        //Find users download folder
        private void updateDirectries() {
            string temp="";
            int countBackslash = 0;
            for (int i = 0; i < CurDirectory.Length; i++) {
                if (CurDirectory[i]=='\\')
                    countBackslash++;
                if (countBackslash < 3)
                    temp = temp + CurDirectory[i];
                else
                    break;
            }
            //Assign locations
            userfile = temp;
            DownloadOrigin = userfile+"\\Downloads";
            imageloc = userfile + "\\OneDrive\\Pictures";
            try
            {
                MakeFolder(userfile, "\\OneDrive\\Desktop\\EmmaDownloads");
                MakeFolder(userfile, "\\OneDrive\\Desktop\\EmmaDownloads\\new");
                DownloadNew = userfile + "\\OneDrive\\Desktop\\EmmaDownloads";
            }
            catch {
                DownloadNew = userfile + "\\Desktop\\EmmaDownloads";
            }
            try
            {
                //Check to make sure directories exist and work
                folder_in = Directory.GetFiles(DownloadOrigin);
                folder_in = Directory.GetFiles(imageloc);
                folder_in = Directory.GetFiles(DownloadNew);
                folder_in = Directory.GetFiles(DownloadNew + "\\new");
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
            Directory.CreateDirectory(dir + "\\" + type);
        }

        //Move file to correct folder
        private void MoveFile(string dir, string file,string type) {
            string filename = "";
            if (type.Equals("")) {
                memory.SaveData("No File extention: " + file);
                return;
            }
            for (int i = file.Length - 1; i > 0; i--) {
                if (file[i] == '\\')
                    break;
                filename=file[i]+filename;
            }
            string from = file;
            string to = dir+"\\"+filename;
            try
            {
                File.Move(from, to);
            }
            //If file is missing a folder to be placed in
            catch (System.IO.DirectoryNotFoundException)
            {
                MakeFolder(dir,type);
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
