using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MessageServer.Domain
{

    public class Validation
    {
        private Message _message;
        public string messageText = string.Empty;
        public static Boolean URL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return (reg.IsMatch(url));
            }
            else
                throw new Exception("url is null or empty");
        }

        public Validation()
        {

        }
        public Validation(Message messaage)
        {
            if (messaage != null)
                this._message = messaage;
            else
                throw new Exception("message is null or empty");
        }

        public Boolean isValid()
        {

            Boolean result = false;
            string URLpattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            if (!string.IsNullOrEmpty(this._message.DestinationURL))
            {
                Regex reg = new Regex(URLpattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                result = result | reg.IsMatch(this._message.DestinationURL);
            }
            else
                messageText += "destiantion address is invalid ";
            /*
            if (!string.IsNullOrEmpty(this._message.DeptURL))
            {
                string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                result = result | reg.IsMatch(this._message.DeptURL);
            }
            else
                messageText += "deptURL address is invalid ";
                */


            if (!string.IsNullOrEmpty(_message.DeptURL))
            {
                Regex reg = new Regex(URLpattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                result = result | reg.IsMatch(this._message.DeptURL);

            }
            else
                messageText += "departure URL is nul valid, ";


            if (string.IsNullOrEmpty(_message.Content))
            {
                messageText += "content is null or empty";
                result = result | false;
            }

            return (result);

        }
        public Boolean validURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return (reg.IsMatch(url));
            }
            else
                throw new Exception("url is null or empty");
        }
    }
}