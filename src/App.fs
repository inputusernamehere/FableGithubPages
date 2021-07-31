module App

open Browser.Dom

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Feliz
open Feliz.Router
open Fss

let App = FunctionComponent.Of<unit> (fun _ ->
  Html.div [
    prop.children [
      Html.div "Hello from Fable!"
      Html.div "This page is automatically rebuilt every time changes are pushed to master"
    ]
  ]
)

let render() =
    ReactDom.render(
        App (),
        document.getElementById("ReactEntryPoint"))

render()