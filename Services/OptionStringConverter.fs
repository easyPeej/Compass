namespace Compass.Services

open System
open System.Globalization
open Avalonia.Data.Converters

type OptionStringConverter() =
    interface IValueConverter with
        member this.Convert (value: obj, targetType: Type, parameter: obj, culture: CultureInfo) =
            match value with
            | :? option<int64> as opt ->
                match opt with
                | Some v -> v.ToString()
                | None -> "n/a"
            | :? option<string> as opt ->
                match opt with
                | Some v -> v.ToString()
                | None -> "n/a"
            | _ -> $"reveived value: %A{value}" 
    
    
        member this.ConvertBack (value: obj, targetType: Type, parameter: obj, culture: CultureInfo) =
            raise (NotImplementedException("Convert back is not implemented"))
            
type BoolToYesNoConverter() =
    interface IValueConverter with
        member this.Convert(value: obj, targetType: Type, parameter: obj, culture: CultureInfo) =
            match value with
            | :? bool as b -> if b then "Yes" :> obj else "No" :> obj
            | _ -> "No" :> obj

        member this.ConvertBack(value: obj, targetType: Type, parameter: obj, culture: CultureInfo) =
            match value with
            | :? string as s -> (s.Trim().ToLower() = "yes") :> obj
            | _ -> false :> obj