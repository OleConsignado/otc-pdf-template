using System.Collections.Generic;

namespace Otc.PdfTemplate.Abstractions
{
    public interface IPdfGenerator
    {

        /// <summary>
        /// Add item to bind to fill the template.
        /// </summary>
        /// <param name="key">Key to fill in template.</param>
        /// <param name="value">Value to add in template.</param>
        IPdfGenerator Add(string key, string value);

        /// <summary>
        /// Add a list of keys and values to fill the template.
        /// </summary>
        /// <param name="values">List of key and values to add the template.</param>
        IPdfGenerator AddRange(IEnumerable<KeyValuePair<string, string>> values);

        /// <summary>
        /// Add a image based on a position in the template.
        /// </summary>
        /// <param name="Image">Image to add in the template.</param>
        /// <param name="HorizontalPosition"></param>
        /// <param name="VerticalPosition"></param>
        IPdfGenerator AddImage(System.Drawing.Image image, float horizontalPosition, float verticalPosition);

        /// <summary>
        /// Set the path of the template file
        /// </summary>
        /// <param name="path">Path to the template file.</param>
        /// <returns></returns>
        IPdfGenerator PathFile(string path);

        /// <summary>
        /// Fill the template based on all parameter seted
        /// </summary>
        /// <returns></returns>
        byte[] Generate();
    }
}
