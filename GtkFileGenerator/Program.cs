using System;
using System.Collections.Generic;
using gladeGenerator;
using Gtk;

namespace GtkFileGenerator
{
    class Program
    {
        [STAThread]
        
        public static void Main(string[] args)
        {

            Application.Init();

            try
            {
                clsArgsConfig.Instance();
              
                clsArgsConfig.Instance()._setArgs(args);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            if (!clsArgsConfig.Instance()._validateCommandKey())
            {
                return;
            }
            
            try
            {
                var className = clsClipboard._getText();

                if (className == "")
                {
                    Console.WriteLine("Class name is missing. Copy and paste the class name");
                    return;
                }

                if (className != "")
                {
                    string classStr = clsFile._load_static( clsFile._getExePath_replace("classTemplate.txt"));
                    string gladeStr = clsFile._load_static( clsFile._getExePath_replace("gladeTemplate.txt"));

                    classStr = classStr._replaceReplaceStr("{$ProjectName}", clsArgsConfig.Instance().ProjectName);
                    classStr = classStr._replaceReplaceStr("{$className}",className);
                
                    gladeStr = gladeStr._replaceReplaceStr("{$ProjectName}", clsArgsConfig.Instance().ProjectName);
                    gladeStr = gladeStr._replaceReplaceStr("{$className}", className);

                    string classFilePath = clsArgsConfig.Instance().ProjectFolder + "/" + className + ".cs";
                    string gladeFilePath = clsArgsConfig.Instance().ProjectFolder + "/" + className + ".glade";

                    clsFile._saveFilePath(classStr,classFilePath);
                    clsFile._saveFilePath(gladeStr,gladeFilePath);
                    
                    Console.WriteLine("I wrote it out " + classFilePath);
                    Console.WriteLine("I wrote it out " + gladeFilePath);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}