using RemoteFork.Tools;

namespace RemoteFork.Plugins {
    public class IconFile {
        public static string GetIconFile(string name) {
            string type = MimeTypes.Get(name);
            if (type.Contains("audio")) {
                return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597283musicfile.png";
            } else if (type.Contains("video")) {
                return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png";
            } else if (type.Contains("image")) {
                return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278jpgfile.png";
            } else {
                return "http://s1.iconbird.com/ico/2013/6/364/w256h2561372348486helpfile256.png";
            }
        }
    }
}
