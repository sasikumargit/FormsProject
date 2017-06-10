using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using Jeylabs.AJG.PickeringsForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Helpers
{
    public  class FileHandler
    {
        /// <summary>
        /// Method to save file to defined path
        /// </summary>
        /// <param name="FileCollections">file collections</param>
        /// <param name="folderReferenceName"> folder name </param>
        /// <returns></returns>
        [ExceptionHandler]
        public  List<string> fileSave(List<HttpPostedFileBase> FileCollections, string folderReferenceName )
        {
            string serverPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
            List<string> RetrunFileCollections = new List<string>();
            if (!Directory.Exists(serverPath+ folderReferenceName))
            {
                Directory.CreateDirectory(serverPath + folderReferenceName);
            }
            foreach (HttpPostedFileBase File in FileCollections)
            {
                try
                {
                    string FileName = string.Empty;
                    if (File.FileName.Contains("\\"))
                    {
                        FileName = Path.GetFileName(File.FileName);   
                    }
                    else
                    {
                        FileName = File.FileName;
                    }
                    RetrunFileCollections.Add(serverPath + folderReferenceName + "\\" + FileName);
                    File.SaveAs(serverPath + folderReferenceName + "\\" + FileName);
                }    
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return RetrunFileCollections;
        }
        
        /// <summary>
        /// Read file content from file path
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
                        
        [ExceptionHandler]
        public string ReadFile(string filepath)
        {
            string fileContent = string.Empty;
            try
            {
                fileContent= File.ReadAllText(filepath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fileContent;
        }

        /// <summary>
        /// Delete files from defined folder
        /// </summary>
        /// <param name="Folder"></param>
        [ExceptionHandler]
        public void DeleteFiles(string Folder)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Folder);
                var files = di.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
                di.Delete(); 
            }
            catch ( Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}