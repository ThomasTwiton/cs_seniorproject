//class PostContainer extends React.Component {

//    constructor(props) {
//        super(props);
//        this.state = {
//            error: null,
//            isLoaded: false,
//            posts: []
//        };
//        /* The following calculation will determine where the 
//         * api call should go to get the proper posts.
//         * 
//         * Presently, the api calls all lead to the same place
//         * for testing. This may or may not be what happens in
//         * the final version since the database does specify 
//         * different ids depending on whether a profile is an
//         * ensemble, venue or profile.
//        */
        
//        switch (profileType) {
//            case "ensemble":
//                this.getPostLink = "api/getAllPosts"
//                break;
//            case "profile":
//                this.getPostLink = "api/getAllPosts"
//                break;
//            case "venue":
//                this.getPostLink = "api/getAllPosts"
//                break;
//        }
//    }

//    componentDidMount() {
//        fetch(this.getPostLink)
//            .then(res => res.json())
//            .then(
//                (result) => {
//                    this.setState({
//                        isLoaded: true,
//                        posts: result.posts
//                    });
//                    console.log(result)
//                },
//                // Note: it's important to handle errors here
//                // instead of a catch() block so that we don't swallow
//                // exceptions from actual bugs in components.
//                (error) => {
//                    this.setState({
//                        isLoaded: true,
//                        error
//                    });
//                }
//            )
//    }

//    render() {
//        const { error, isLoaded, posts } = this.state;

//        // If there was an error loading the posts
//        if (error) {
//            return <div>Error: {error.message}</div>;

//        // If the component is loading the posts
//        } else if (!isLoaded) {
//            return <div>Loading...</div>;

//        // Else we should have the posts
//        } else {
//            const postsList = posts;
//            const postItems = postsList.map((p) => <Post author={p.author} post={p.post} />);

//            return (
//                <div id="postContainer" className="container">
//                    {postItems}
//                </div>
//            );
//        }
//    }
//}

function PostContainer(props) {
    // The container that holds all posts inside of it.

    const postsList = props.posts;
    const postItems = postsList.map((p) => <Post author={p.author} post={p.post} />);
    return (
        <div id="postContainer" className="container">
            {postItems}
        </div>
    );
}

function Post(props) {
    // The base component for a post
    return (
        <div className="row">
            <Avatar user={props.author} />
            <PostBody post={props.post} />
        </div>
     );
}

function Avatar(props) {
    // The displayed avatar of a post
    return (
        <div className="col-xs-1">
            <img
                className="tinyPic"
                src={props.user.avatarURL}
                alt={props.user.name}
            />
        </div>
    );
}

function BasicBody(props) {
    // The body of a type='post' component which must
    //  have text (even if it's "") and can have media.
    if (props.post.hasOwnProperty('media')) {
        return (
            <div className="col-xs-11">
                <div className="textContainer">
                    {props.post.text}
                </div>
                <hr />
                <PostMedia media={props.post.media} />
            </div>
        );
    } else {
        return (
            <div className="col-xs-11">
                <div className="textContainer">
                    {props.post.text}
                </div>
                <hr />
            </div>
        );
    }
}

function GigBody(props) {
    // The body of a type='gig' component.
    const link = "Gig/" + props.post.info.id.toString();
    return (
        <div className="col-xs-11">
            <div className="textContainer">
                {props.post.info.poster}: We're looking some entertainment!
            </div>
            <hr />
            <div className="postWButton">
                <table className="postTable">
                    <tbody><tr>
                        <td>Start:</td>
                        <td>{props.post.info.start}</td>
                    </tr>
                        <tr>
                            <td>End:</td>
                            <td>{props.post.info.end}</td>
                        </tr>
                        <tr>
                            <td>Time:</td>
                            <td>{props.post.info.time}</td>
                        </tr>
                        <tr>
                            <td>Looking for:</td>
                            <td>{props.post.info.seeking}</td>
                        </tr></tbody>
                </table>
                <a href={link} className="btn btn-danger">Plug-In</a>
            </div>
        </div>
    );
}

function AuditionBody(props) {
    // The body of a type='aud' component.
    const link = "/Home/Audition/" + props.post.info.id.toString();
    return (
        <div className="col-xs-11">
            <div className="textContainer">
                {props.post.info.poster}: We're looking for a new member!
            </div>
            <hr />
            <div className="postWButton">
                <table className="postTable">
                    <tbody><tr>
                        <td>Position:</td>
                        <td>{props.post.info.seeking}</td>
                    </tr>
                        <tr>
                            <td>Time:</td>
                            <td>{props.post.info.time}</td>
                        </tr>
                        <tr>
                            <td>Location:</td>
                            <td>{props.post.info.place}</td>
                        </tr></tbody>
                </table>
                <a href={link} className="btn btn-danger">Plug-In</a>
            </div>
        </div>
    );
}

function PostBody(props) {
    /* The PostBody component is container for the various types of posts
        that could be created.
    */
    console.log(props);

    switch (props.post.type) {
        case "gig":
            return (<GigBody post={props.post} />);
        case "aud":
            return (<AuditionBody post={props.post} />);
        default: // should be post
            return (<BasicBody post={props.post} />);
    }
}

function PostMedia(props) {
    const mediaType = props.media.type;
    let media;

    if (mediaType === "img") {
        media = (
            <img
                src={props.media.url}
                alt={props.media.alt}
            />
        );
    } else if (mediaType === "audio") {
        media = (
            <audio
                src={props.media.url}
            />
        );
    }
    return (
        <div className="mediaContainer">
            {media}
        </div>
    );
}

function aggPosts() {
    /* The way that the post data is being passed into
     * the view requires the javascript to find all of the 
     * member tags and assemble the appropriate props to be
     * passed to the EnsembleContainer. */

    let eList = document.getElementsByTagName("Post");
    console.log(eList);
    let propList = [];
    for (let m of eList) {
        let props = {
            author: {
                name: m.dataset.username,
                avatarURL: m.dataset.avatar
            },
            post: {
                text: m.dataset.text,
                type: m.dataset.type,
                
            }
        };
        let info
        switch (m.dataset.type) {
            case "gig":
                info = {
                    id: m.dataset.gigid,
                    poster: m.dataset.username,
                    start: m.dataset.start,
                    end: m.dataset.end,
                    time: m.dataset.time,
                    seeking: m.dataset.pos
                };
                props.post.info = info;
                break;

            case "aud":
                info = {
                    id: m.dataset.audid,
                    poster: m.dataset.username,
                    time: m.dataset.time,
                    seeking: m.dataset.pos,
                    place: m.dataset.place
                };
                props.post.info = info;
                break;

            default:
                console.log(m.dataset.hasmedia);
                if (m.dataset.hasmedia) {
                    let media = {
                        url: m.dataset.mediaurl,
                        type: m.dataset.mediatype
                    };
                    props.post.media = media
                };
                break;
        };

        propList.push(props)
    }
    //console.log(propList);
    return propList;
}



const p = {
    author: {
        name: 'Hello Kitty',
        avatarURL: 'https://placekitten.com/g/64/64',
    },
    post: {
        text: 'The text for a post goes here. When the text is really, really long, then this is what happens.',
        type: 'gig',   // Possible types: post, aud, gig
        info: {         // Optional property that is present for type='aud' or type='gig'
            id: 1,
            poster: "Marty's Grill",
            start: new Date().toLocaleDateString(), // type='gig'
            end: new Date().toLocaleDateString(),   // type='gig'
            time: new Date().toLocaleTimeString(),
            seeking: "Small Jazz Group",
        }
    },
};

const o = {
    author: {
        name: 'Hello Kitty',
        avatarURL: 'https://placekitten.com/g/64/64',
    },
    post: {
        text: 'The text for a post goes here. When the text is really, really long, then this is what happens.',
        type: 'post',   // Possible types: post, aud, gig
        media: {        // Optional property that only is present in type='post'
            url: '/images/banner1.svg',
            alt: 'Banner #1',
            type: 'img',
        },
    },
};

const s = {
    author: {
        name: 'Hello Kitty',
        avatarURL: 'https://placekitten.com/g/64/64',
    },
    post: {
        text: 'The text for a post goes here. When the text is really, really long, then this is what happens.',
        type: 'aud',   // Possible types: post, aud, gig
        info: {         // Optional property that is present for type='aud' or type='gig'
            id: 1,
            poster: "Chicago Symphony Orchestra",
            time: new Date().toLocaleTimeString(),
            seeking: "Vocalist",
            place: "Marty's Grill",                 // type='aud'
        }
    },
};

const t = {
    author: {
        name: 'Hello Kitty',
        avatarURL: 'https://placekitten.com/g/64/64',
    },
    post: {
        text: 'The text for a post goes here. When the text is really, really long, then this is what happens.',
        type: 'post',   // Possible types: post, aud, gig
    },
};

const e = {
    author: {
        name: 'Hello Kitty',
        avatarURL: 'https://placekitten.com/g/64/64',
    },
    post: {
        text: '',
        type: 'post',   // Possible types: post, aud, gig
        media: {        // Optional property that only is present in type='post'
            url: '/images/banner1.svg',
            alt: 'Banner #1',
            type: 'img',
        },
    },
};


const pList = aggPosts();
console.log(pList);

ReactDOM.render(
    <PostContainer posts={pList} />,
    document.getElementById('postCC')
);