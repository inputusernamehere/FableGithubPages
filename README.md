
# Publish a Fable application to Github Pages

This repo is meant to show you an example of how you can set up automatic publishing of fable applications to Github Pages every time the application changes.

Take a look at the result: https://inputusernamehere.github.io/FableGithubPages/

## 1. Create a new fable project
Using this template: https://github.com/fable-compiler/fable-templates,

In an empty folder, run these commands:
```
dotnet new -i Fable.Template
dotnet new fable -n <name-of-your-project>
```

## 2. Create a new git repository and upload it to github

## 3. Enable Github Pages
Navigate to your repository on github.
Under Settings -> Pages, select the default branch and the folder `/docs`.
Github currently only allows us to publish from `/` or `/docs`, so I chose `/docs` to keep the files more organized.

## 4. Prepare the repository for Github Pages
Rename the `/public` folder to `/docs`.
Change the mentions of `/public` to `/docs` in webpack.config.js.
In `package.json` add the following script: `"build": "dotnet fable src && webpack`.
The scripts section of `package.json` should now look like this:
```
  "scripts": {
    "postinstall": "dotnet tool restore",
    "start": "dotnet fable watch src --run webpack-dev-server",
    "build": "dotnet fable src && webpack"
  },
```

## 5. Add a Github Action to build on push to the master branch
For an example action, see `.github/workflows/build_and_publish.yml`.
This action will trigger every time there is a push to the master branch.

## 6. Make changes to your Fable application
Edit App.fs, commit your changes and push them.
After a short while you should now be able to see the changes you have made on your Github Pages page.