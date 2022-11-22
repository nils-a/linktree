# My personal LinkTree

> A simple redirect database.

see https://nils-a.me/

## How this works.

- Domain (DNS) is setup on Cloudflare.
- Page is hosted using Cloudflare pages
- every redirect is written in `_redirects` as a "page". E.g. `https://nils-a.me/forward/github` redirects to the GitHub.
- one page-rule is set up to redirect `*.nils-a.me` to `https://nils-a.me/forward/$1`

So.. 

* a page request on `https://github.nils-a.me/` triggers the page-rule and redirects to `https://nils-a.me/forward/github`
* the page request on `https://nils-a.me/forward/github` triggers a redirect from `_redirects` to GitHub.