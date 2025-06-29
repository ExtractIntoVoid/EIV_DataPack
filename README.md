# EIV_DataPack
Compressed file (.eivp) for storing files, datas inside.

This can be used to store compiled assets, code, etc.\
Each DataPack version has an internal structure currently we have 4.

# Versions
Description
### Version 1
This is never released to git, not been used.
### Version 2
First ever version released in git.\
Only File Name and data stored.
### Version 3
Added File Metadata to store inside.
### Version 4
Metadata Saving is optional. (Writing a 1 Byte instead of 8+8+8+1 Byte)\
Added saving Extra Data for the files, its a Byte Array, saving as how much its there.\
Added option for saving a file Id, this can anonymize file names. \
To know FileName save yours into something like StringIdList or StringIdDict. (And ofc load your data back into it.)\
Added GUID if want, usable to load data from desired GUID file.\
Added Custom Compression, sadly only ZLib, Brotli compression only available for NET8+.

