using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace DreamGitBlogMaker
{
    class Program
    {
        public static List<Article> ArticleList { get; set; }
        public static List<Music> MusicList { get; set; }
        public static List<string> ArticleTypeList { get; set; }

        static void Main(string[] args)
        {
            try
            {
                initialData();
                initialIndex();
                Console.WriteLine("生成网页成功");
                Console.WriteLine("按任意键退出");
                Console.Read();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("生成网页失败");
                Console.WriteLine("按任意键退出");
                Console.Read();
            }
        }

        public static void initialData()
        {
            #region 初始化文章列表
            ArticleList = new List<Article>();
            FileInfo[] articlieFileArray = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Resource\\Article").GetFiles();
            foreach (FileInfo articlieFile in articlieFileArray)
            {
                string fileExtension = articlieFile.Name.Substring(articlieFile.Name.Length - 2, 2);
                string fileName = articlieFile.Name.Substring(0, articlieFile.Name.Length - 3);
                if ((fileExtension == "md" || fileExtension == "MD") && articlieFile.Name.Contains("-"))
                {
                    string[] splitNameArray = fileName.Split('-');
                    string articleType = splitNameArray[0];
                    string articleName = "";
                    for (int i = 1; i < splitNameArray.Length; i++)
                    {
                        articleName += splitNameArray[i];
                    }
                    StreamReader streamReader = articlieFile.OpenText();
                    string articleContent = streamReader.ReadToEnd();
                    streamReader.Dispose();
                    ArticleList.Add(new Article(articleType, articleName, articleContent));
                }
            }
            #endregion
            #region 初始化文章类别列表
            ArticleTypeList = new List<string>();
            ArticleTypeList.Add("全部");
            foreach (Article article in ArticleList)
            {
                if (!ArticleTypeList.Contains(article.ArticleType))
                {
                    ArticleTypeList.Add(article.ArticleType);
                }
            }
            if (ArticleTypeList.Contains("其它"))
            {
                ArticleTypeList.Remove("其它");
                ArticleTypeList.Add("其它");
            }
            #endregion
            #region 初始化歌曲列表
            MusicList = new List<Music>();
            FileInfo[] musicFileArray = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Resource\\Music").GetFiles();
            foreach (FileInfo musicFile in musicFileArray)
            {
                string fileExtension = musicFile.Name.Substring(musicFile.Name.Length - 3, 3);
                if (fileExtension == "mp3" || fileExtension == "MP3")
                {
                    string musicName = musicFile.Name.Remove(musicFile.Name.Length - 4, 4);
                    string musicPath =$"Resource\\Music\\{musicFile.Name}";
                    string lyricPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Resource\\Music\\{musicName}.lrc" ;
                    string lyricContent;
                    if (File.Exists(lyricPath) == false)
                    {
                        lyricContent = "";
                    }
                    else 
                    {
                        FileInfo lyricFile = new FileInfo(lyricPath);
                        StreamReader streamReader = lyricFile.OpenText();
                        lyricContent = streamReader.ReadToEnd();
                        streamReader.Dispose();
                    }
                    Music music = new Music(musicName, musicPath, lyricContent);
                    MusicList.Add(music);
                }
            }
            #endregion
        }

        public static void initialIndex()
        {
            #region 初始化head标签
            string headHtml = "<head>" +
                "<meta http-equiv=\"Content-Type\" content=\"width=device-width,initial-scale=1.0,minimum-scale=1,maximum-scale=1.0,user-scalable=no \"/>" +
                "<title>DepartedDream's Blog</title>" +
                "<link rel=\"Shortcut Icon\" href= \"Resource/Image/Pen.png \" type=\"image/x-icon\"/>" +
                "<link href=\"Resource/CSS/Main.css?v=1.0 \" rel=\"stylesheet \"/>" +
                "<link href =\"Resource/CSS/VS2015.css?v=1.0\" rel=\"stylesheet\"/>" +
                "<link href =\"Resource/CSS/DdMusicPlayer.css?v=1.0\" rel=\"stylesheet\"/>" +
                "<link href =\"Resource/CSS/MarkDown.css?v=1.0\" rel=\"stylesheet\"/>" +
                "<script src=\"Resource/JS/Highlight.pack.js?v=1.0\"></script>" +
                "<script src=\"Resource/JS/JQuery.min.js?v=1.0\"></script>" +
                "<script src=\"Resource/JS/DdMusicPlayer.js?v=1.0\"></script>" +
                "<script src=\"Resource/JS/Main.js?v=1.0\"></script>" +
                 "<script src=\"Resource/JS/Marked.min.js?v=1.0\"></script>" +
                "</head>";
            #endregion

            string videoHtml = "<video id=\"background\" autoplay=\"autoplay\" muted=\"muted\" loop=\"loop\" src=\"Resource/Video/LunarTears.mp4\"></video>";
            string titleHtml = "<div id=\"title\">DepartedDream's Blog</div>";

            #region 初始化文章分类导航
            string articleTypeListHtml = "";
            foreach (string articleType in ArticleTypeList)
            {
                articleTypeListHtml += $"<div class=\"article_type\">{articleType}</div>";
            }
            articleTypeListHtml = $"<div id=\"article_type_list\">{articleTypeListHtml}</div>";
            #endregion

            #region 初始化文章列表
            string articleListHtml = " ";
            foreach (Article article in ArticleList)
            {
                articleListHtml += $"<div class=\"article\"  article_type=\"{article.ArticleType}\" article_content=\"{Uri.EscapeUriString(article.ArticleContent)}\">{article.ArticleName}</div>";
            }
            articleListHtml = $"<div id=\"article_list\" >{articleListHtml}</div>";
            #endregion

            string artilceOutline = $"<div id=\"article_outline_list\"> </div>";

            #region 初始化音乐列表数据
            string musicListDataHtml = "";
            for (int i = 0; i < MusicList.Count; i++)
            {
                musicListDataHtml += $"<div class=\"music\" music_path=\"{MusicList[i].MusicPath}\" lyric_content=\"{Uri.EscapeUriString(MusicList[i].LyricContent)}\" music_id=\"{i + 1}\">{MusicList[i].MusicName}</div>";
            }
            musicListDataHtml = $"<div id=\"music_list_data\">{musicListDataHtml}</div>";
            #endregion

            string ddMusicPlayerHtml = "<div id=\"ddMusicPlayer\"></div>";
            string articleContentHtml = "<div id = \"article_content\"></div>";
            string bodyHtml = $"<body>{videoHtml}{titleHtml}<div id=\"center_content\">{articleTypeListHtml}{articleListHtml}{articleContentHtml}{artilceOutline}</div>{musicListDataHtml}{ddMusicPlayerHtml}</body>";
            string indexHtml = $"<!DOCTYPE html ><html xmlns = \"http://www.w3.org/1999/xhtml\">{headHtml}{bodyHtml}</html>";
            #region 创建首页文件
            FileInfo indexFile = new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}/index.html");
            StreamWriter streamWriter = indexFile.CreateText();
            streamWriter.Write(indexHtml);
            streamWriter.Dispose();
            #endregion
        }
    }
}