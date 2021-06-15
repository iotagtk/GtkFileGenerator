using System;
using System.Collections.Generic;
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
        
  
       
    }
}