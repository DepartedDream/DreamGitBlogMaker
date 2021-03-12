using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.IO;

namespace DreamGitBlogMaker
{
    public class Article
    {
        public string ArticleContent {get; set; }
        public string ArticleName { get; set; }
        public string ArticleType { get; set; }

        public Article(string articleType, string articleName,string articleContent) 
        {
            ArticleType = articleType;
            ArticleName = articleName;
            ArticleContent = articleContent;
        }
    }
}