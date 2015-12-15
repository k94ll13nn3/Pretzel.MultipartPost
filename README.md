# Pretzel.MultipartPost

[![Build status](https://ci.appveyor.com/api/projects/status/45og7v5fqd795d0b?svg=true)](https://ci.appveyor.com/project/k94ll13nn3/pretzel-multipartpost) 
[![Release](https://img.shields.io/github/release/k94ll13nn3/Pretzel.MultipartPost.svg)](https://img.shields.io/github/release/k94ll13nn3/Pretzel.MultipartPost.svg)

This plugin aims at creating a list of all posts that are parts of the same series in HTML.

Posts that are in the same series are posts that are in a subfolder of the `_posts` folder.

### Usage

The tag has the following syntax:

```
{% multipart_post [asc|desc|wasc|wdesc] %}
```

`asc` and `desc` represents the order in which the posts are displayed (ascending and descending), `asc` is the default value when no order is specified. If prefixed by `w`, it means that the current post will not be included in the list.

This plugin also provide a block that only render its content when used in a multipart post :

```
{% is_multipart_post %}
...
{% endis_multipart_post %}
```

This can be used in addition to `multipart_post` to display a title or some text before the list.

Finally, the `multipart_post_link` tag can be used to refer to the previous or next post in the series from another post. It has the following syntax:
```
{% multipart_post_link [prev|next] %}
```

### Example

For the following file tree :

```
_posts
    series
        post1.1.md
        post1.2.md
    post2.md
    post3.md

```

The tag will have no effect for `post2` and `post3` and in `post1.1` and `post1.2`, it will render the following HTML (here is the example for `post1.1`, for `post1.2` the `current-post` class is on the second line):

```
<ul class="multipart-post-list">
    <li><a class="current-post" href="url">post1.1</a></li>
    <li><a href="url">post2.2</a></li>
</ul>

```

### Installation

Download the latest [release](https://github.com/k94ll13nn3/Pretzel.MultipartPost/releases) and copy `Pretzel.MultipartPost.dll` to the `_plugins` folder at the root of your site folder (you may have to **unblock** the file).

