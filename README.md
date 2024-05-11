# EIV_DataPack
Compressed file (.eivp) for storing files, datas inside.


# How the data readed/writed into a files:

4 byte magic\
eivp aka 65 69 76 70\

2 byte version\
ushort 2 AKA 02 00 

1 byte for compressions - Version 2 - Default is 1 (Deflate)\
None = 0,\
Deflate = 1,\
GZip = 2,\
ZLib = 3,\
Brotli = 4

4 byte lenght of files\
int

65 69 76 70 | 01 00 | 01 | - filesname - | - data -


Also please dont use over 4 GB for storing in this format. NOT recommended! (maybe not even handled!)