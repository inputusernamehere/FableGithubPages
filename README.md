
# Publish a Fable application to Github Pages

This repo is meant to show you an example of how you can set up automatic publishing of fable applications to Github Pages every time the application changes.

Take a look at the result: https://inputusernamehere.github.io/FableGithubPages/

## 1. Create a new fable project
Using this template: https://github.com/fable-compiler/fable-templates,

In an empty folder, run these commands:
```
dotnet new -i Fable.Template
dotnet new fable
```

## 2. Create a new git repository and upload it to github
Don't forget a `.gitignore` file with at least these entries:
```
.vs/
node_modules/
bin/
obj/
*.fs.js
.local-chromium/
```

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

# Add tests
For (frontend) tests we are going to be using the Fable.Mocha library, see the documentation here: https://github.com/Zaid-Ajaj/Fable.Mocha

We will set up two test projects. The first project will have all our tests and will run them inside your browser. The second project will simply tell puppeteer to run the tests in our first project through a headless chrome/chromium instance.

## 0. Add .local-chromium to your .gitignore file
We will be using a headless version of chromium to run our frontend tests, which you probably do not want to commit to your repo as it is over 300mb. Make sure `.local-chromium/` is added to your `.gitignore` file.

## 1. Set up the test project
Our test project will be another Fable project, so re-do the very first step of this readme but this time inside of a new folder at the root of our repository: `./tests`

```
dotnet new -i Fable.Template
dotnet new fable -n <name-of-your-project>
```

## 2. Make sure the test project runs in your browser
Follow the instructions for Fable.Mocha's documentation, under the section "Running the tests using the browser". I will duplicate them here, but use the link above in case these steps change in the future:

```
Trying to use mocha to run tests in the browser will give you headaches as you have to include the compiled individual test files by yourself along with mocha specific dependencies. That's why Fable.Mocha includes a built-in test runner for the browser. You don't need to change anything in the existing code, it just works!

Compile your test project using default fable/webpack as follows.

First, install fable-loader along with with webpack if you haven't already:

npm install fable-loader webpack webpack-cli webpack-dev-server

Add an index.html page inside directory called public that contains:

<!DOCTYPE html>
<html>
    <head>
        <title>Mocha tests</title>
    </head>
    <body>
        <script src="bundle.js"></script>
    </body>
</html>

Create a webpack config file that compiles your Tests.fsproj

var path = require("path");

module.exports = {
    entry: "./tests/Tests.fsproj",
    output: {
        path: path.join(__dirname, "./public"),
        filename: "bundle.js",
    },
    devServer: {
        contentBase: "./public",
        port: 8080,
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: "fable-loader"
        }]
    }
}

Now you can run your tests live using webpack-dev-server or compile the tests and run them by yourself. Add these scripts to your package.json

"start": "webpack-dev-server",
"build-for-browser": "webpack"

Now if you run npm start you can navigate to http://localhost:8080 to see the results of your tests.
```

# 3. Set up the headless test project
Again, follow the instructions in Fable.Mocha's documentation under the section "Running the tests using headless browser". However we are going to make a few minor adjustment:

The Fable.Mocha documentation has a diagram of the folder structure that starts with `{repo}` as the root. Our root is going to be our `tests` folder.

We are going to add the same `postinstall` script into `.tests/package.json` that we have in `./package.json` to automatically run the command `dotnet tool restore` after `npm install`:
```
    "postinstall": "dotnet tool restore",
```

We are also going to change the command for running the headless tests slightly, so that it compiles our project with fable before running the tests. We could have also told our CI to compile first before running the test, but I will do it like this so that you can't accidentally run the tests without compiling a new version first. The npm script to run your tests will be this instead:
```
    "test-headless": "dotnet fable Client.Tests && webpack && dotnet run --project ./HeadlessTests/HeadlessTests.fsproj"
```

To see the final version of `package.json` with these modified scripts, take a look at `./tests/package.json` in this repo.

Again I will duplicate the instructions here for posterity and make the above-mentioned adjustments.

```
The tests you write in the browser can be easily executed inside a headless browser such you can run them in your CI server. Using a simple console application, install the package Fable.MochaPuppeteerRunner:

mkdir HeadlessTests
cd HeadlessTests
dotnet new console -lang F#
dotnet add package Fable.MochaPuppeteerRunner

Then change the contents of Program.fs into:

[<EntryPoint>]
let main argv =
    "../public"
    |> System.IO.Path.GetFullPath
    |> Puppeteer.runTests
    |> Async.RunSynchronously

Where public is the directory where the compiled tests, the bundle.js file next to index.html:

tests
   |
   |-- HeadlessTests
        |-- HeadlessTests.fsproj
        |-- Program.fs

   |-- public
        |-- index.html
        |-- bundle.js

   |-- Client.Tests
        | -- Client.Tests.fs
        | -- Client.Tests.fsproj

Also you need to add the RunWorkingDirectory property to HeadlessTests.fsproj as follows below:

<PropertyGroup>
  <OutputType>Exe</OutputType>
  <TargetFramework>netcoreapp3.0</TargetFramework>
  <RunWorkingDirectory>$(MSBuildProjectDirectory)</RunWorkingDirectory>
</PropertyGroup>

Now simply dotnet run and the tests will run inside the headless browser after the download finishes.

You can also add a npm build script to run the headless tests after compiling the Tests project:

"test-headless": "dotnet fable Client.Tests && webpack && dotnet run --project ./HeadlessTests/HeadlessTests.fsproj"

Remember to gitignore the directory of the downloaded chromium add .local-chromium to your gitignore file.
```

## 4. Add the tests to your github actions workflow
Before the `Build` step we want to add another `Run tests` step. For the complete workflow see the file `./github/workflows/build_and_publish.yml` 

```
    - name: Run tests
      run: |
        cd ./tests
        npm install
        npm run test-headless
        cd ..

    - name: Build
      run: |
        npm install
        npm run build
        git add .
        git config --local user.email "github-actions[bot]@users.noreply.github.com"
        git config --local user.name "github-actions[bot]"
        git commit -m "CI: Automated build" -a | exit 0
```

## 5. Add more tests
You can now add tests to your heart's content.