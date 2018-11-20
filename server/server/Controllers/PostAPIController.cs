using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostAPIController : ControllerBase
    {
        public class Author
        {
            public string name { get; set; }
            public string avatarURL { get; set; }
        }

        public class MediaInfo
        {
            public string url { get; set; }
            public string alt { get; set; }
            public string type { get; set; }
        }

        public class Post
        {
            public string text { get; set; }
            public string type { get; set; }
            public MediaInfo media { get; set; }
        }

        public class PostInstance
        {
            public int Id { get; set; }

            public int ProfileId { get; set; }
            public Profile Profile { get; set; }

            public Author author { get; set; }
            public Post post { get; set; }

        }

        readonly PostInstance[] posts = new PostInstance[]
        {
            new PostInstance
            {
                Id = 1,
                ProfileId = 1,
                author =
                {
                    name = "Hello Kitty",
                    avatarURL = "https://placekitten.com/g/64/64"
                },
                post =
                {
                    text = "The text for a post goes here. When the text is really, really long, then this is what happens.",
                    type = "post",
                    media =
                    {
                        url = "/images/banner1.svg",
                        alt = "Banner #1",
                        type = "img"
                    }
                }

            },
            new PostInstance
            {
                Id = 1,
                ProfileId = 1,
                author =
                {
                    name = "Hello Kitty",
                    avatarURL = "https://placekitten.com/g/64/64"
                },
                post =
                {
                    text = "The text for a post goes here. When the text is really, really long, then this is what happens.",
                    type = "post",
                    media =
                    {
                        url = "/images/banner1.svg",
                        alt = "Banner #1",
                        type = "img"
                    }
                }

            },
            new PostInstance
            {
                Id = 1,
                ProfileId = 1,
                author =
                {
                    name = "Hello Kitty",
                    avatarURL = "https://placekitten.com/g/64/64"
                },
                post =
                {
                    text = "The text for a post goes here. When the text is really, really long, then this is what happens.",
                    type = "post",
                    media =
                    {
                        url = "/images/banner1.svg",
                        alt = "Banner #1",
                        type = "img"
                    }
                }

            },
            new PostInstance
            {
                Id = 1,
                ProfileId = 1,
                author =
                {
                    name = "Hello Kitty",
                    avatarURL = "https://placekitten.com/g/64/64"
                },
                post =
                {
                    text = "The text for a post goes here. When the text is really, really long, then this is what happens.",
                    type = "post",
                    media =
                    {
                        url = "/images/banner1.svg",
                        alt = "Banner #1",
                        type = "img"
                    }
                }

            },
        };

        [HttpGet]
        public IEnumerable<PostInstance> GetAllPosts()
        {
            return posts;
        }
        
    }
}