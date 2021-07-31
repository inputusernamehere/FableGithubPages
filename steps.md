1. Create a new fable project using this template: https://github.com/fable-compiler/fable-templates
2. Create a new git repository, upload it to github
3. Change the settings of the repository to enable github pages to be served in this repo, from /docs.
4. Rename the `/public` folder to '/docs`. Change the mentions of `/public` to `/docs` in webpack.config.js
5. Add a github action to build on push to the master branch