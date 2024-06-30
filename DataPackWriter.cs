using System.Runtime.InteropServices;
using System.Text;

namespace EIV_DataPack
{
    public class DataPackWriter : IDataPackManipulator
    {
        public delegate void OnFileAddedDelegate(string FileName);
        public delegate void OnDataAddedDelegate(string DataName);
        public delegate void OnDirectoryAddedDelegate(string DirectoryName);
        public delegate void OnSavedDelegate();

        public event OnFileAddedDelegate? OnFileAdded;
        public event OnDataAddedDelegate? OnDataAdded;
        public event OnDirectoryAddedDelegate? OnDirectoryAdded;
        public event OnSavedDelegate? OnSaved;

        BinaryWriter Writer;
        public DataPack Pack { get; set; }

        public DataPackWriter(BinaryWriter writer, DataPack dataPack)
        {
            Pack = dataPack;
            Writer = writer;
        }

        public void Open()
        {

        }

        public void Close()
        {
            Writer.Close();
            Writer.Dispose();
        }

        public void Save()
        {
            long metadata_starter_post = 0;
            Writer.BaseStream.Position = 7;
            Writer.Write(Pack.FileNames.Count);
            foreach (var filename in Pack.FileNames)
            {
                var name = Encoding.UTF8.GetBytes(filename);
                Writer.Write(name.Length);
                Writer.Write(name);
                var pos_bef = Writer.BaseStream.Position;
                Writer.Write((long)0);
                Pack.FileNameToData.Add(filename, pos_bef);
            }

            if (Pack.Version == 2)
            {
                // Why is this here? probably for filler
                Writer.Write((ushort)0);
            }
            else
            {
                metadata_starter_post = Writer.BaseStream.Position;
                Writer.Write((long)-1);
            }

            foreach (var item in Pack.FileNameToData)
            {
                var data = Pack.FileNameToBytes[item.Key];
                var pos_start = Writer.BaseStream.Position;
                Writer.Write(data.Length);
                Writer.Write(data);
                var pos_end = Writer.BaseStream.Position;
                Writer.BaseStream.Position = item.Value;
                Writer.Write(pos_start);
                Writer.BaseStream.Position = pos_end;
            }
            Writer.Write((long)-1); // this is here if we add more stuff (next indicator for reader!)
            var tmp_pos = Writer.BaseStream.Position;
            Console.WriteLine("posbefore_next_blob: " + tmp_pos);
            Writer.BaseStream.Position = metadata_starter_post;
            Writer.Write(tmp_pos);
            Writer.BaseStream.Position = tmp_pos;
            Writer.Write(Pack.FileNameToMetadata.Count);
            foreach (var item in Pack.FileNameToMetadata)
            {
                var name = Encoding.UTF8.GetBytes(item.Key);
                Writer.Write(name.Length);
                Writer.Write(name);
                Console.WriteLine(item.Value.ToString());
                item.Value.WriteMetadata(Writer);
            }

            // next data blob if needed

            Writer.Flush();
            Pack.FileNameToData.Clear();
            OnSaved?.Invoke();
        }

        public void AddFile(string path, string RemovePath = "")
        {
            // We normalize
            string filename = path;
            if (RemovePath != "")
                filename = path.Replace(RemovePath, "").Replace('\\', '/');
            OnFileAdded?.Invoke(filename);
            Pack.FileNames.Add(filename);
            var data = File.ReadAllBytes(path);
            FileMetadata fileMetadata = new()
            { 
                UnixMode = 0,
                CreationTimeUtc = File.GetCreationTimeUtc(path).ToFileTimeUtc(),
                LastAccessTimeUtc = File.GetLastAccessTimeUtc(path).ToFileTimeUtc(),
                LastWriteTimeUtc = File.GetLastWriteTimeUtc(path).ToFileTimeUtc()
            };
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileMetadata.UnixMode = (byte)File.GetUnixFileMode(path);
            }
            Pack.FileNameToMetadata.Add(filename, fileMetadata);
            data = Pack.Compress(data);
            Pack.FileNameToBytes.Add(filename, data);
        }

        public void AddDirectory(string path, bool Recursive = false)
        {
            var files = Directory.GetFiles(path,"*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var item in files)
            {
                AddFile(item, path + Path.DirectorySeparatorChar);
            }
            OnDirectoryAdded?.Invoke(path);
        }

        public void AddData(string dataname, string data)
        {
            AddData(dataname, Encoding.UTF8.GetBytes(data));
        }

        public void AddData(string dataname, byte[] data)
        {
            OnDataAdded?.Invoke(dataname);
            Pack.FileNames.Add(dataname);
            data = Pack.Compress(data);
            Pack.FileNameToBytes.Add(dataname, data);
        }
    }
}
