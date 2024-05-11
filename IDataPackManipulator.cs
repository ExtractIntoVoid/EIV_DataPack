namespace EIV_DataPack
{
    internal interface IDataPackManipulator
    {
        public DataPack Pack { get; internal set; }
        public void Open();
        public void Close();
    }
}
