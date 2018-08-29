using System.Collections.Generic;

namespace Otc.PdfTemplate.Abstractions
{
    public interface IBinder
    {
        /// <summary>
        /// Make a binding of data to pdf template
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="templatePath">Path to pdf template file</param>
        /// <returns>byte array</returns>
        byte[] Bind(Dictionary<string, string> data, string templatePath);

        /// <summary>
        /// Make a binding of data and images to pdf template
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="templatePath">Path to pdf template file</param>
        /// <param name="images">List of images to binding to pdf</param>
        /// <returns>byte array</returns>
        byte[] Bind(Dictionary<string, string> data, string templatePath, List<ImageData> images);
    }
}
