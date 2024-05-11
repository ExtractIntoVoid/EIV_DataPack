using System.Text;

namespace EIV_DataPack
{
    public class DataPackReader : IDataPackManipulator
    {
        BinaryReader Reader;
        public DataPack Pack { get; set; }

        public int FileNameCount { get; internal set; } = 0;
        internal long ReadedFilesPos = -1;

        public DataPackReader(BinaryReader reader, DataPack dataPack) 
        {
            Pack = dataPack;
            Reader = reader;
        }

        public void Open()
        {
            FileNameCount = Reader.ReadInt32();
            Reader.BaseStream.Position = 7;
        }

        public void Close()
        {
            Reader.Close();
            Reader.Dispose();
        }

        public void ReadFileNames()
        {
            Pack.FileNameToData.Clear();
            Pack.FileNames.Clear();
            Reader.BaseStream.Position = 7;
            FileNameCount = Reader.ReadInt32();
            for (int i = 0; i < FileNameCount; i++)
            {
                var filename_len = Reader.ReadInt32();
                var filename = Encoding.UTF8.GetString(Reader.ReadBytes(filename_len));
                Pack.FileNames.Add(filename);
                var start_len = Reader.ReadInt64();
                Pack.FileNameToData.Add(filename, start_len);
            }
            ReadedFilesPos = Reader.BaseStream.Position;
        }

        public byte[] GetFileData(string filename)
        {
            Reader.BaseStream.Position = ReadedFilesPos;
            if (!Pack.FileNameToData.TryGetValue(filename, out var data))
            {
                throw new Exception("File not found inside the Archive!");
            }
            Reader.BaseStream.Seek(data, SeekOrigin.Begin);
            var datalen = Reader.ReadInt32();
            var databytes = Reader.ReadBytes(datalen);
            return Pack.DeCompress(databytes);
        }

        public int GetFileLen(string filename)
        {
            Reader.BaseStream.Position = ReadedFilesPos;
            if (!Pack.FileNameToData.TryGetValue(filename, out var data))
            {
                throw new Exception("File not found inside the Archive!");
            }
            Reader.BaseStream.Seek(data, SeekOrigin.Begin);
            return Reader.ReadInt32();
        }

        public byte[] GetFileData(int FileIndex)
        {
            Reader.BaseStream.Position = 11;
            long len_to_Start = -1;
            if (FileIndex > FileNameCount)
                throw new Exception("Your file Index is much larger than the files inside!");
            for (int i = 0; i <= FileIndex; i++)
            {
                var filename_len = Reader.ReadInt32();
                Reader.ReadBytes(filename_len);
                len_to_Start = Reader.ReadInt64();
            }
            if (len_to_Start == -1)
            {
                throw new Exception("File Positon is invalid!");
            }
            Reader.BaseStream.Seek(len_to_Start, SeekOrigin.Begin);
            var datalen = Reader.ReadInt32();
            var databytes = Reader.ReadBytes(datalen);
            return Pack.DeCompress(databytes);
        }

        public int GetFileLen(int FileIndex)
        {
            Reader.BaseStream.Position = 11;
            long len_to_Start = -1;
            if (FileIndex > FileNameCount)
                throw new Exception("Your file Index is much larger than the files inside!");
            for (int i = 0; i <= FileIndex; i++)
            {
                var filename_len = Reader.ReadInt32();
                Reader.ReadBytes(filename_len);
                len_to_Start = Reader.ReadInt64();
            }
            if (len_to_Start == -1)
            {
                throw new Exception("File Positon is invalid!");
            }
            Reader.BaseStream.Seek(len_to_Start, SeekOrigin.Begin);
            return Reader.ReadInt32();
        }
    }
}
