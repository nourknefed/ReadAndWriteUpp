using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using CsvHelper;
using System.IO;
using Windows.UI.WindowManagement;
using CsvParse;
using System.Collections.ObjectModel;

namespace SharedLibrary.Helpers
{
    public class ReadWriteHelp
    {
       
        public static async Task<string> ReadfromFileAsync(string fileName)
        {
            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
            StorageFile file = await storageFolder.GetFileAsync(fileName);


            return await FileIO.ReadTextAsync(file);

        }



       

    }
}
