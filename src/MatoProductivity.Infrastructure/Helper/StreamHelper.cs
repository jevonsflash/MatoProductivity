using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MatoProductivity.Infrastructure.Helper
{
    public class StreamHelper
    {
        public static void WriteStream(Stream stream, string fileName)
        {
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                fs = new FileStream(fileName, FileMode.Create);
                bw = new BinaryWriter(fs);
                bw.Write(bytes);
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                bw.Close();
                fs.Close();
            }
        }
    }
}
