module App

open Browser.Dom

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Feliz
open Feliz.Router
open Fss

// Mutable variable to count the number of times we clicked the button
let mutable count = 0

// Get a reference to our button and cast the Element to an HTMLButtonElement
let myButton = document.querySelector(".my-button") :?> Browser.Types.HTMLButtonElement

// Register our listener
myButton.onclick <- fun _ ->
    count <- count + 1
    myButton.innerText <- sprintf "You clicked: %i time(s)" count
