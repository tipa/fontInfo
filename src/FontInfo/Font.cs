﻿using FontInfo.Reader;
using FontInfo.Records;
using FontInfo.Tables;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FontInfo
{
    public class Font
    {
        private readonly string filename;

        private async Task loadDataAsync()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (AsyncBinaryReader binaryReader = new AsyncBinaryReader(fs))
                {
                    List<TableRecord> tables = await TableRecord.GetAllTablesAsync(binaryReader).ConfigureAwait(false);

                    TableRecord namingTableRecord = TableRecord.GetNamesTable(tables);
                    TableRecord os2TableRecord = TableRecord.GetOS2Table(tables);
                    TableRecord headTableRecord = TableRecord.GetHeadTable(tables);

                    NamingTable namingTable = await NamingTable.CreateAsync(binaryReader, namingTableRecord).ConfigureAwait(false);
                    OS2Table os2Table = await OS2Table.CreateAsync(binaryReader, os2TableRecord).ConfigureAwait(false);
                    HeadTable headTable = await HeadTable.CreateAsync(binaryReader, headTableRecord).ConfigureAwait(false);

                    Details = new FontDetails(namingTable, headTable);
                    Metrics = new FontMetrics(os2Table, headTable);
                }
            }
        }

        public static async Task<Font> CreateAsync(string fileName)
        {
            Font font = new Font(fileName);
            await font.loadDataAsync().ConfigureAwait(false);
            return font;
        }

        public FontMetrics Metrics { get; private set; }

        public FontDetails Details { get; private set; }


        private Font(string fileName)
        {
            this.filename = fileName;
        }

    }
}
