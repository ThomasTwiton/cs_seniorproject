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
    const link = "Audition/" + props.post.info.id.toString();
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

    switch (props.post.type) {
        case "post":
            return (<BasicBody post={props.post} />);
        case "gig":
            return (<GigBody post={props.post} />);
        case "aud":
            return (<AuditionBody post={props.post} />);
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

const pList = [p,o,s,t,e];
console.log(pList);

ReactDOM.render(
    <PostContainer posts={pList} />,
    document.getElementById('postCC')
);