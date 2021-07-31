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
      Html.text "Hello from Fable!"
    ]
  ]
)

let render() =
    ReactDom.render(
        App (),
        document.getElementById("ReactEntryPoint"))

render()