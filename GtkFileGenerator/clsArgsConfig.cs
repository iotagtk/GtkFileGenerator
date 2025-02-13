using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace gladeGenerator
{
    public class clsArgsConfig
    {
        private static clsArgsConfig _singleInstance = null;

        public string ProjectFolder = "";     
        public string ProjectName = "";

        public static clsArgsConfig Instance()
        {
            if (_singleInstance == null) {
                _singleInstance = new clsArgsConfig();
            }
            return _singleInstance;
        }
   
        private List<string> commndKeyArray = new List<string> {
            "-projectName","-projectDir"};
        public Boolean _validateCommandKey()
        {
   
            if (ProjectFolder == "" )
            {
                Console.WriteLine("The projectFolder is not specified.");
                return false;
            }
            if (ProjectName == "")
            {
                Console.WriteLine("ProjectName is not specified.");
                return false;
            }
            
            if (ProjectName != "")
            {
                ProjectName = _getProjectName(ProjectName);
            }

            return true;
        }
        public void _setArgs(string[] args)
        {

            int i = 0;
            foreach (var commandKey in args)
            {
                if (commandKey._indexOf("-projectDir") != -1)
                {
                    if (args._safeIndexOf(i + 1) && 
                        commndKeyArray.IndexOf(args[i+1]) == -1 && 
                        args[i+1] != ""){
                        ProjectFolder = args[i + 1];
                    }
                    i++;
                    continue;
                }

                if (commandKey._indexOf("-projectName") != -1)
                {
                    
                    if (args._safeIndexOf(i + 1) && 
                        commndKeyArray.IndexOf(args[i+1]) == -1 && 
                        args[i+1] != ""){
                        ProjectName = args[i + 1];              
                    }
                    i++;
                    continue;
                }
             
                i++;
            }
        }
        
        /// <summary>
        /// プロジェクトパスを取得する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string _getProjectName(string path)
        {
            path = path.TrimEnd(Path.DirectorySeparatorChar);

            var separator = Path.DirectorySeparatorChar;
            string[] pathArray = path.Split(separator);
            
            if (pathArray.Length == 0)
            {
                return path;
            }

            for (int i = pathArray.Length; i > 0; i--)
            {
                var pathArray1 = pathArray[0..i];
                string stCsvData = string.Join("/", pathArray1);
                string csprojPath = stCsvData + "/" + pathArray[i - 1] + ".csproj";
                if (File.Exists(csprojPath))
                {
                    return pathArray[i - 1];
                    break;
                }
            }
            
            return "";
        }

    }
}