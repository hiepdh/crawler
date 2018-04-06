using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crawler
{
    public class pageInfo
    {
        public pageInfo()
        {

        }
        private string pageId = "";
        private string firstPageUrl = "https://dichvubds.vn/nha-dat-ban-ha-noi/page/1";
        private string pageURL = "https://dichvubds.vn/nha-dat-ban-ha-noi/page/{0}"; 
        private int maxPage = 0;
        private int pageIdx = 0;
        private List<postInfo> postList;

        public string PageURL
        {
            get
            {
                return pageURL;
            }

            set
            {
                pageURL = value;
            }
        }

        public int MaxPage
        {
            get
            {
                return maxPage;
            }

            set
            {
                maxPage = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }

            set
            {
                pageIdx = value;
            }
        }

        public List<postInfo> PostList
        {
            get
            {
                return postList;
            }

            set
            {
                postList = value;
            }
        }

        public string FirstPageUrl
        {
            get
            {
                return firstPageUrl;
            }

            set
            {
                firstPageUrl = value;
            }
        }

        public string PageId
        {
            get
            {
                return pageId;
            }

            set
            {
                pageId = value;
            }
        }
    }

    public class postInfo
    {
        private string _url;
        private string _title;
        private string _price;
        private string _phone;
        private string _district;
        private string _province;
        private string _content;
        private string _contentReply;
        private string _categoryId;
        private string _postId;
        private string _siteId;

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }
        public string Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
            }
        }
        public string Phone
        {
            get
            {
                return _phone;
            }

            set
            {
                _phone = value;
            }
        }
        public string District
        {
            get
            {
                return _district;
            }

            set
            {
                _district = value;
            }
        }
        public string Province
        {
            get
            {
                return _province;
            }

            set
            {
                _province = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }

        public string ContentReply
        {
            get
            {
                return _contentReply;
            }

            set
            {
                _contentReply = value;
            }
        }

        public string CategoryId
        {
            get
            {
                return _categoryId;
            }

            set
            {
                _categoryId = value;
            }
        }

        public string PostId
        {
            get
            {
                return _postId;
            }

            set
            {
                _postId = value;
            }
        }

        public string SiteId
        {
            get
            {
                return _siteId;
            }

            set
            {
                _siteId = value;
            }
        }

        public postInfo()
        {

        }
    }

    public class userInfo
    {
        public userInfo()
        {
            _username = _fullname = _address = _birthday = _address = _token = _uid = _sex = "";
        }

        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }

        public string Fullname
        {
            get
            {
                return _fullname;
            }

            set
            {
                _fullname = value;
            }
        }

        public string Birthday
        {
            get
            {
                return _birthday;
            }

            set
            {
                _birthday = value;
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }

        public string Sex
        {
            get
            {
                return _sex;
            }

            set
            {
                _sex = value;
            }
        }

        public string Token
        {
            get
            {
                return _token;
            }

            set
            {
                _token = value;
            }
        }

        public string Uid
        {
            get
            {
                return _uid;
            }

            set
            {
                _uid = value;
            }
        }

        private string _fullname;
        private string _birthday;
        private string _address;
        private string _sex;
        private string _token;
        private string _uid;
    }
}
