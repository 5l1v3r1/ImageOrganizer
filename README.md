# ImageOrganizer
Organizes a folder full of images by copying or moving them into a date-based directory structure.

## Usage

Download from the [**Releases tab**](https://github.com/bp2008/ImageOrganizer/releases).

This program is built for .NET Framework 3.5.

```
Copies files from the source directory and child directories,
consolidating them in the target directory and renaming the
files to the format yyyyMMddHHmmssfff + .extension where the
timestamp is the last modified date's year, month, day, hour,
minute, second, millisecond.

        Note: Files will not be overwritten.  If a file
        already exists, the new file will have a space and an
        identifying number will be appended to the timestamp
        in the file name (e.g. 20140408010101000.jpg and
        20140408010101000 2.jpg and 20140408010101000 3.jpg)

Usage: ImageOrganizer source target [/C] [/M] [/E:Extension1[/Extension2][/Extension3]...]

        /C      Names the files based on creation time instead of modified time.
        /M      Moves the files instead of only copying them.
        /E:     Specify the extension(s) of the files to move or copy.
                If unspecified, only jpg files will be affected.

                Example: /E:jpg/jpeg/bmp/png/gif/webp/raw

```
