using System.Reflection.PortableExecutable;
using System.Text;

namespace EIV_DataPack
{
    public class DataPackWriter : IDataPackManipulator
    {
        public delegate void OnFileAddedDelegate(string Filename);
        public delegate void OnDirectoryAddedDelegate(string DirectoryName);
        public delegate void OnSavedDelegate();

        public event OnFileAddedDelegate? OnFileAdded;
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
            Writer.Write((ushort)0);
            foreach (var item in Pack.FileNameToData)
            {
                var data = Pack.FileNameToMetadata[item.Key];
                var pos_start = Writer.BaseStream.Position;
                Writer.Write(data.Length);
                Writer.Write(data);
                var pos_end = Writer.BaseStream.Position;
                Writer.BaseStream.Position = item.Value;
                Writer.Write(pos_start);
                Writer.BaseStream.Position = pos_end;
            }
            Writer.Flush();
            Pack.FileNameToData.Clear();
            OnSaved?.Invoke();
        }

        public void AddFile(string path, string RemovePath = "")
        {
            // We normalize 
            string filename = path.Replace(RemovePath, "").Replace('\\' ,'/');
            OnFileAdded?.Invoke(filename);
            Pack.FileNames.Add(filename);
            var data = File.ReadAllBytes(path);
            data = Pack.Compress(data);
            Pack.FileNameToMetadata.Add(filename, data);
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
            Pack.FileNames.Add(dataname);
            data = Pack.Compress(data);
            Pack.FileNameToMetadata.Add(dataname, data);
        }
    }
}
