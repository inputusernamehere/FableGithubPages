module App

open Browser.Dom

open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Feliz
open Feliz.Bulma
open Feliz.Bulma.Operators
open Feliz.Router
open Fss

type AppModel = {
  Value : int
}

let App = FunctionComponent.Of<AppModel> (fun model ->
  let state = Hooks.useState model

  let changeValue n =
    let newValue =
      match n with
      | x when x < 0 -> 0
      | x when x > 10 -> 10
      | x -> x

    state.update { state.current with Value = newValue }

  Bulma.section [
    prop.children [
      Bulma.container [
        prop.style [
          style.paddingBottom (length.px 20)
        ]

        prop.children [
          Bulma.title "Hello from Fable!"
          Bulma.subtitle "This page is automatically rebuilt every time changes are pushed to master"
          Html.p [
            size.isSize5
            prop.text "Check out the repository to see how: "
          ]
          Html.a [
            size.isSize5
            prop.href "https://github.com/inputusernamehere/FableGithubPages"
            prop.text "https://github.com/inputusernamehere/FableGithubPages"
          ]
        ]
      ]

      Bulma.container [
        Bulma.card [
          Bulma.cardContent [
            Html.p [
              text.hasTextCentered ++ size.isSize3
              prop.text $"{state.current.Value}"
            ]

            Bulma.progress [
              prop.max 10
              prop.value state.current.Value
            ]
          ]
          Bulma.cardFooter [
            Bulma.cardFooterItem.a [
              prop.style [
                style.userSelect.none
              ]

              prop.text "+"
              size.isSize3
              prop.onClick <| fun _ -> changeValue (state.current.Value + 1)
            ]

            Bulma.cardFooterItem.a [
              prop.style [
                style.userSelect.none
              ]

              prop.text "-"
              size.isSize3
              prop.onClick <| fun _ -> changeValue (state.current.Value - 1)
            ]
          ]
        ]
      ]
    ]
  ]
)

let render() =
    ReactDom.render(
        App { Value = 0},
        document.getElementById("ReactEntryPoint"))

render()