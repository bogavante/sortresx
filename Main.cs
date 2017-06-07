using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;


namespace ResourceFileSorterTrigger
{

    public class ResourceFileSorter
    {
	private FileProcessor mFileProcessor;        
        
	[STAThread]
	static int Main(string[] args)
	{                    
            ResourceFileSorter fileSorter = null;
            if (args.Length <= 0)
                return 1;
                                            
            if (CheckArgs(args[0]) == 0)
            {
                fileSorter = new ResourceFileSorter(args[0]);   
            }
            else			
                return 1;
            return 0;
        }

        private static int CheckArgs(string filepath)
        {			
            if ( !( File.Exists(filepath)) )
                return 1;            
            return 0;
        }

	public ResourceFileSorter(string path)
	{
	    mFileProcessor = new FileProcessor(path);
            mFileProcessor.Process();
	}

	private static void PrintUsage(string argument)
	{
	    System.Console.WriteLine("Invalid argument:"+ argument + "\nUsage: sortresx file_to_sort");
	}
}

    
}