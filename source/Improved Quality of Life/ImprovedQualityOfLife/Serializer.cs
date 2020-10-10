/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/demiacle/QualityOfLifeMods
**
*************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Demiacle.ImprovedQualityOfLife {
    /// <summary>
    /// This class handles file saving and loading for ModData.cs
    /// </summary>
    class Serializer {

        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>( T objectToWrite, string playerName, bool append = false) where T : new() {
            TextWriter writer = null;

            try {
                var serializer = new XmlSerializer( typeof( T ) );
                writer = new StreamWriter( ModEntry.modDirectory + playerName + ModEntry.saveFilePostfix, append);
                serializer.Serialize( writer, objectToWrite );
            } finally {
                if ( writer != null )
                    writer.Close();
            }
        }
        
    /// <summary>
    /// Reads an object instance from an XML file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The object to read from the file to.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the XML file.</returns>
    public static void ReadFromXmlFile<T>( out T objectTypeToRead, string playerName ) where T : new() {
            TextReader reader = null;
            try {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader( ModEntry.modDirectory + playerName + ModEntry.saveFilePostfix );
                objectTypeToRead = ( T)serializer.Deserialize(reader);
            } finally {
                if (reader != null)
                    reader.Close();
            }
        }

    }
}
