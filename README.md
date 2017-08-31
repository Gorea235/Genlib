Genlib (Discontinued)
======

This project is no longer being used/maintained. I still have old projects that rely on this, and there are a few snippets of code that might be of use in the future, so I won't delete it, but I won't work on this any more.

The main reason for this is that it is old, messy and, for the most part, unnecessary. Parts like the hashing, logging, WPF, serialization and most of the Utilities are either not needed (or more suitable being implemented on a per-project bases rather than a library) or have standard .NET API that is better than I could ever write.

There are useful sections of the library, like the Encryption class and math/color helpers. While it might be nice to have libs for these (e.g. Genlib.Math.dll or Genlib.Crypto.dll), I feel like having a full lib for what would be a single class is overkill.

Another reason is the .NET Core/Framework fragmentation that has evolved, which is annoying (and causing weird duplicate folders & files in my Google Drive) and is not what I would want for this kind of lib (ATM I would want a single .NET Standard 2.0+ lib, which none of this is).

## Old Readme

This is a general library containing various functions, extention and classes that extend the capibility of the .NET framework.

I made this project simple as a way to "compile" code from various sources into a single usable project.

Genlib is the .NET Core port of the original version of the library, however GenlibWin is the old .NET Framework version. Both will be kept in line with each other (the win version uses links to get the source files so changes are kept accross platforms), however only GenlibWin will have the logging (due to the windows-only portion) and wpf extensions (as this is only windows).
