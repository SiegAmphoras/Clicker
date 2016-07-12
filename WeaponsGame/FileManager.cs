using System;
using System.Collections.Generic;
using System.IO;

namespace WeaponsGame
{
	internal class FileManager
	{
		public static string[] GetFiles(string directory)
		{
			return System.IO.Directory.GetFiles(directory);
		}

        public static string[] GetFiles(string directory, bool includeSubfolders)
        {
            List<string> list = new List<string>();
            string[] directories = Directory.GetDirectories(directory);
            list.AddRange(Directory.GetFiles(directory));
            if (includeSubfolders)
            {
                string[] array = directories;
                for (int i = 0; i < array.Length; i++)
                {
                    string path = array[i];
                    list.AddRange(Directory.GetFiles(path));
                }
            }
            return list.ToArray();
        }
	}
}
