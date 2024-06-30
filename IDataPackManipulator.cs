namespace EIV_DataPack
{
    public interface IDataPackManipulator
    {
        public DataPack Pack { get; internal set; }
        public void Open();
        public void Close();
    }
}
