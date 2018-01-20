using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoryCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string responseFromServer = HTTPUtility.GetRequest(data[2]+data[3]);

            var regex = new Regex("(<td class=\"pcatHead\"><img class=\"picon\")([\\s\\S]*?)(><\\/table>)");

            foreach (Match match in regex.Matches(responseFromServer)) {
                regex = new Regex("(title=\")(.*?)(\">)(\\2)");
                var regexLink = new Regex("(<a class=\"pgenmed\" href=\")(.*?)(\")");
                var regexImage = new Regex("(<var class=\"portalImg\" title=\")(.*?)(\">)");
                var item = new Item {
                    Name = regex.Match(match.Value).Groups[2].Value,
                    ImageLink = regexImage.Match(match.Value).Groups[2].Value,
                    Link =
                        $"pagefilm{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/{regexLink.Match(match.Value).Groups[2].Value}"
                };

                item.Description = FormatDescription(match.Value, item.ImageLink);

                items.Add(item);
            }

            regex = new Regex("(<a href=\")((portal\\.php\\?c=\\d+&)(?:amp;)(start=\\d+))(\">([а-яА-Я]*?.?)<\\/a><\\/span>)");
            if (regex.IsMatch(responseFromServer)) {
                var matchGroups = regex.Match(responseFromServer).Groups;
                NnmClub.NextPageUrl =
                    $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/{matchGroups[3].Value + matchGroups[4].Value}";
            }

            NnmClub.IsIptv = false;
            return items;
        }

        private static string FormatDescription(string html, string imagePath) {
            string title = string.Empty;
            string infoFile = string.Empty;
            string infoFilms = string.Empty;
            string infoPro = string.Empty;

            var regex = new Regex("(title=\\\")(.*?)(\\\">)(\\2)(<\\/a)");
            if (regex.IsMatch(html)) {
                title = regex.Match(html).Groups[2].Value;
            }
            regex = new Regex("(<img class=\"tit-b pims\" src=\")(.*?)(\")");
            if (regex.IsMatch(html)) {
                infoFile = regex.Match(html).Groups[2].Value;
            }
            regex = new Regex("(</var></a>)(.*?)(<br />)");
            if (regex.IsMatch(html)) {
                infoFilms = regex.Match(html).Groups[2].Value;
            }
            regex = new Regex("(<br \\/>)(<b>.*)(<\\/span>)");
            if (regex.IsMatch(html)) {
                infoPro = regex.Match(html).Groups[2].Value;
            }

            return
                "<div id=\"poster\" style=\"float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;\">" +
                "<img src=\"" + imagePath +
                "\" style=\"width:240px;float:left;\" /></div><span style=\"color:#3090F0\">" + title + "</span><br>" +
                infoFile + infoPro + "<br><span style=\"color:#3090F0\">Описание: </span>" + infoFilms;
        }
    }
}
